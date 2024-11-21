using System;
using System.Threading.Tasks;
using MusicRequests.Core.Helpers;
using MusicRequests.Core.Messages;
using MusicRequests.Core.Models;
using MvvmCross.Localization;
using MvvmCross;
using MvvmCross.Plugin.Messenger;
using Microsoft.Maui.Networking;

namespace MusicRequests.Core.Services
{
    public class CacheService : ICacheService
    {
        private readonly IFileService _fileService;
        private readonly IMvxMessenger _messenger;
        private readonly ISessionService _sessionService;

        public CacheService(IFileService fileService,
                             IMvxMessenger messenger,
                             ISessionService sessionService)
        {
            _fileService = fileService;
            _messenger = messenger;
            _sessionService = sessionService;
        }

        #region GetData

        public async Task<TData> GetData<TData>(string fileKey,
                                                 int timeoutInminutes,
                                                 Func<Task<TData>> actionExecution,
                                                 string password = "")
        {
            // Check the headers of the WebAPI
            InitialChecksBeforeExecution();

            TData result = default(TData);
            Cache<TData> dataStored = null;
            bool executeAction = true;

            dataStored = await this.ReadFile<TData>(fileKey, password);


            if (dataStored != null)
            {
                // Check if the data is still usable
                if (dataStored.DateTimeSaved.AddMinutes(timeoutInminutes) > DateTime.Now)
                {
                    result = dataStored.Data;
                    executeAction = false;
                }
            }

            if (executeAction)
            {

                // Check connectivity
                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    if (dataStored != null)
                    {
                        _messenger.Publish(new NoConnectionMessage(this, ConnStatus.NoConnection));
                        return dataStored.Data;
                    }
                    else
                    {
                        throw new ConnMusicRequestException(ConnStatus.NoConnection);
                    }
                }

                // Call the API and save the result
                result = await actionExecution();
                if (result != null)
                {
                    try
                    {
                        await this.SaveData<TData>(result, fileKey, timeoutInminutes, password);
                    }
                    catch (Exception ex)
                    {
                        // Se ha producido un error guardando los datos en caché. Lo ignoramos y seguimos.
                        // El usuario no tendrá disponibles estos datos en caché en la siguiente llamada.
                    }
                }
                else if (dataStored != null)
                {
                    // Recuperar el contenido cacheado (Nocontent o NotModified)
                    result = dataStored.Data;
                }
            }

            return result;
        }

        #endregion

        #region SaveData

        public Task SaveData<TData>(TData input, string fileKey, int timeoutInminutes, string password)
        {
            return Task.Run(() =>
            {
                var cache = new Cache<TData>()
                {
                    CacheKey = fileKey,
                    Data = input,
                    DateTimeSaved = DateTime.Now,
                    TimeoutInMinutes = timeoutInminutes
                };

                _fileService.DeleteFile(fileKey, !string.IsNullOrEmpty(password));

                if (string.IsNullOrEmpty(password))
                {
                    _fileService.WriteTextFile<Cache<TData>>(fileKey, cache);
                }
                else
                {
                    _fileService.WriteTextFileEncrypted<Cache<TData>>(fileKey, cache, password);
                }
            });
        }

        #endregion

        #region ReadFile

        public Task<Cache<TData>> ReadFile<TData>(string fileKey, string password)
        {
            return Task.Run(() =>
            {
                Cache<TData> dataStored;
                // Normal or encrypted file
                if (string.IsNullOrEmpty(password))
                {
                    // Open the file and check if exists
                    dataStored = _fileService.ReadTextFile<Cache<TData>>(fileKey);
                }
                else
                {
                    dataStored = _fileService.ReadTextFileEncrypted<Cache<TData>>(fileKey, password);
                }

                return dataStored;
            }).LogExceptions();
        }

        public Cache<TData> ReadFileSync<TData>(string fileKey, string password)
        {
            Cache<TData> dataStored;
            // Normal or encrypted file
            if (string.IsNullOrEmpty(password))
            {
                // Open the file and check if exists
                dataStored = _fileService.ReadTextFile<Cache<TData>>(fileKey);
            }
            else
            {
                dataStored = _fileService.ReadTextFileEncrypted<Cache<TData>>(fileKey, password);
            }

            return dataStored;
        }

        #endregion

        #region GetLastModificationDateOfFile

        public DateTime GetLastModificationDateOfFile<TData>(string fileKey, string password)
        {
            try
            {
                var date = DateTime.MinValue;
                var result = this.ReadFileSync<TData>(fileKey, password);

                if (result != null)
                    return result.DateTimeSaved;

                return date;
            }
            catch
            {
                return DateTime.Now;
            }
        }

        #endregion

        #region GetDataNoCache
        public async Task<TData> GetDataNoCache<TData>(Func<Task<TData>> actionExecution)
        {
            // Check the headers of the WebAPI
            InitialChecksBeforeExecution();

            TData result = default(TData);
            try
            {
                // Check connectivity
                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    throw new ConnMusicRequestException(ConnStatus.NoConnection);
                }

                // Call the API and save the result
                result = await actionExecution();

            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }
        #endregion

        #region GetDataNoCache(Func<Task> actionExecution)
        public async Task GetDataNoCache(Func<Task> actionExecution)
        {
            try
            {
                // Check connectivity
                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    throw new ConnMusicRequestException(ConnStatus.NoConnection);
                }

                // Call the API and save the result
                await actionExecution();

            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region InitialChecksBeforeExecution
        private void InitialChecksBeforeExecution()
        {
            // Check the session timeout
            if (_sessionService.IsSessionExpired())
            {
                // Reseteamos el tiempo que ha estado el usuario sin actividad
                _sessionService.SaveUserLastAccess();

                var localization = Mvx.IoCProvider.Resolve<IMvxTextProvider>();
                //throw new BackendException(new BackendResult()
                //{
                //    Numero = 1023,
                //    Descripcion = localization.GetText(Constants.GeneralNamespace, Constants.Shared, "SessionExpired"),
                //    Origen = "MS",
                //    TipoError = "Session expired"
                //});
            }

            // Save the time the user is accesing the API
            _sessionService.SaveUserLastAccess();
        }
        #endregion

        #region DeleteCache
        public void DeleteCache(string filename, bool isprivate)
        {
            _fileService.DeleteFile(filename, isprivate);
        }
        #endregion

        #region GetDataEternalCache
        public async Task<TData> GetDataEternalCache<TData>(string fileKey, Func<Task<TData>> actionExecution, string password)
        {
            TData result = default(TData);
            Cache<TData> dataStored = null;

            dataStored = await this.ReadFile<TData>(fileKey, password);

            if (dataStored != null && dataStored.Data != null)
            {
                result = dataStored.Data;
            }
            else
            {
                result = await actionExecution.Invoke();
                await this.SaveData(result, fileKey, 0, password);
            }

            return result;
        }
        #endregion
    }
}


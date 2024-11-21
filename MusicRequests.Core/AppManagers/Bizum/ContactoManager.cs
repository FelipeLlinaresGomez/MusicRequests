//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using MusicRequests.Core.Models;
//using MusicRequests.Core.Services;
//using System.Linq;
//using MusicRequests.Core.Helpers;
//using MvvmCross;

//namespace MusicRequests.Core.Managers
//{
//    public class ContactoManager : IContactoManager
//    {
//        List<ContactoBizum> contactosFrecuentes;
//        List<ContactoItemViewModel> allContactos;

//        #region CleanCache

//        public void CleanCache()
//        {
//            contactosFrecuentes = null;
//            allContactos = null; ;
//        }
//        #endregion

//        #region GetAllContacts

//        public async Task<List<ContactoItemViewModel>> GetAllContacts()
//        {
//            var allContactosAux = new List<ContactoItemViewModel>();
//            var _contactService = Mvx.IoCProvider.Resolve<IContactService>();

//            try
//            {
//                var _cacheService = Mvx.IoCProvider.Resolve<ICacheService>();
//                var contacts = await _cacheService.GetData(
//                    Files.FILE_CONTACTOS_TELEFONO,
//                    5,
//                    () => _contactService.GetAllContacts(),
//                    string.Empty);

//                if (contacts != null)
//                {
//                    foreach (var c in contacts)
//                    {
//                        if (!string.IsNullOrEmpty(c.Telefono))
//                        {
//                            var currentContactoFormatted = c.Telefono.Replace("+34", "").Replace((char)160, ' ').Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", "");

//                            if (!allContactosAux.Any(x => x.Telefono.Contains(currentContactoFormatted)))
//                            {
//                                allContactosAux.Add(new ContactoItemViewModel
//                                {
//                                    FirstName = c.Nombre == string.Empty ? c.Nombre = " " : c.Nombre,
//                                    FullName = $"{c.Nombre} {c.Apellidos}",
//                                    SecondName = c.Apellidos,
//                                    HasBizum = c.BizumUser,
//                                    Telefono = currentContactoFormatted
//                                });
//                            }
//                        }
//                    }

//                    allContactos = allContactosAux.OrderBy(x => x.FullName).ToList();
//                }

//                return allContactos;
//            }
//            catch (Exception ex)
//            {
//                System.Diagnostics.Debug.WriteLine(ex);
//            }
//            return allContactos;
//        }

//        #endregion

//        #region GetContactosFrecuentes
//        public async Task<List<ContactoItemViewModel>> GetContactosFrecuentes()
//        {
//            if (contactosFrecuentes != null)
//            {
//                return (from c in contactosFrecuentes
//                        orderby c.Nombre
//                        select new ContactoItemViewModel()
//                        {
//                            FirstName = c.Nombre == string.Empty ? c.Nombre = " " : c.Nombre,
//                            FullName = c.Alias,
//                            SecondName = c.Apellidos,
//                            Telefono = c.Telefono,
//                            HasBizum = c.BizumUser,
//                        }).OrderBy(x => x.FullName).ToList();
//            }

//            var result = new List<ContactoItemViewModel>();
//            var _cacheService = Mvx.IoCProvider.Resolve<ICacheService>();
//            var contacts = await _cacheService.ReadFile<List<ContactoBizum>>(Files.FILE_CONTACTOS_FRECUENTES,
//                                                                             string.Empty);
//            if (contacts != null)
//            {
//                contactosFrecuentes = contacts.Data;

//                result = (from c in contacts.Data
//                          orderby c.Nombre
//                          select new ContactoItemViewModel()
//                          {
//                              FirstName = c.Nombre == string.Empty ? c.Nombre = " " : c.Nombre,
//                              FullName = c.Alias,
//                              SecondName = c.Apellidos,
//                              Telefono = c.Telefono,
//                              HasBizum = c.BizumUser,
//                          }).OrderBy(x => x.FullName).ToList();
//            }

//            return result;
//        }

//        #endregion

//        #region AddContactoFrecuente

//        public async Task AddContactoFrecuente(ContactoItemViewModel contacto)
//        {
//            var _cacheService = Mvx.IoCProvider.Resolve<ICacheService>();
//            if (contacto == null)
//            {
//                //throw new BackendException(new BackendResult()
//                //{
//                //    Descripcion = "No podemos añadir a frecuentes un contacto sin haberlo seleccionado previamente",
//                //    Numero = BackEndErrors.ERROR_ADD_CONTACTO_FRECUENTE_SIN_SELECCIONARLO,
//                //    Origen = "APP"
//                //});
//            }

//            var data = await _cacheService.ReadFile<List<ContactoBizum>>(Files.FILE_CONTACTOS_FRECUENTES,
//                                                                         string.Empty);
//            if (data != null && data.Data != null)
//                contactosFrecuentes = data.Data;

//            if (contactosFrecuentes == null)
//                contactosFrecuentes = new List<ContactoBizum>();

//            // Solo añadimos el contacto si no existe en la lista de frecuentes
//            if (contactosFrecuentes.Any((arg) => arg.Telefono.Equals(contacto.Telefono)))
//                contactosFrecuentes.RemoveAll((arg) => arg.Telefono.Equals(contacto.Telefono));

//            contactosFrecuentes.Add(new ContactoBizum()
//            {
//                Alias = contacto.FullName,
//                Apellidos = contacto.SecondName,
//                Nombre = contacto.FirstName,
//                BizumUser = contacto.HasBizum,
//                Telefono = contacto.Telefono
//            });

//            await _cacheService.SaveData(contactosFrecuentes,
//                                         Files.FILE_CONTACTOS_FRECUENTES,
//                                         0,
//                                         string.Empty);
//        }


//        #endregion
//    }
//}

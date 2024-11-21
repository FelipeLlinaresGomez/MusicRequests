using System;
using MusicRequests.Plugin.File;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using MusicRequests.Core.Helpers;
using System.Diagnostics;

namespace MusicRequests.Core.Services
{
	public class FileService : IFileService
	{
		private const string PRIVATE_FOLDER = "private";
		private const string PRIVATE_TUTORIALS = "tutorials";

		private IMvxFileStore _mvxFileStore;
		private readonly IEncryptionService _encryptedService;
		public FileService (IMvxFileStore mvxFileStore,
			IEncryptionService encryptedService)
		{
			_mvxFileStore = mvxFileStore;
			_encryptedService = encryptedService;
		}

		#region Read and Write files with no encryption

		public T ReadTextFile<T> (string filename)
		{
			string fileContent = string.Empty;

			if (_mvxFileStore.TryReadTextFile (filename, out fileContent)) {
				try {
					var output = JsonConvert.DeserializeObject<T> (fileContent);
					return output;
				} catch (Exception ex) {
					var msg = ex.Message;
				}
			}
			return default(T);
		}

		public string ReadTextFile(string filename){
			
			string fileContent = string.Empty;
			_mvxFileStore.TryReadTextFile (filename, out fileContent);
			return fileContent;
	
		}

		public void WriteTextFile<T> (string filename, T input)
		{
			if (input != null)
				_mvxFileStore.WriteFile (filename, JsonConvert.SerializeObject (input));
		}

		public void WriteTextFile(string filename, string content){
			_mvxFileStore.WriteFile (filename, content);
		}

		#endregion

		private string GetStringFromUserCode(string userCode) {
			if (string.IsNullOrEmpty(userCode)) return "123456789ABC";
			return $"{userCode}{userCode}{userCode}";
		}
			
		#region Read and Write files with encryption

		public T ReadTextFileEncrypted<T> (string filename, string password)
		{
			

			string fileContent = string.Empty;

			var newfilename = FormatPrivateFileName (filename);

			if (_mvxFileStore.TryReadTextFile (newfilename, out fileContent)) {
				// Decrypt the file
				fileContent = _encryptedService.Decrypt(fileContent, GetStringFromUserCode(password));
				if (fileContent != null) {
					var output = JsonConvert.DeserializeObject<T> (fileContent);
					return output;
				}
			}
			return default(T);
		}

		public string ReadTextFileEncrypted(string filename, string password, bool useUserCodePassword)
		{
			string fileContent = string.Empty;

			var newPassword = useUserCodePassword ? GetStringFromUserCode(password) : password;
			var newfilename = FormatPrivateFileName(filename);

			if (_mvxFileStore.TryReadTextFile(newfilename, out fileContent))
			{
				// Decrypt the file
				fileContent = _encryptedService.Decrypt(fileContent, newPassword);
				if (!string.IsNullOrEmpty(fileContent))
				{
					return fileContent;
				}
			}
			return fileContent;
		}


		public void WriteTextFileEncrypted<T> (string filename, T input, string password)
		{
			this.WriteTextFileEncrypted(filename, JsonConvert.SerializeObject(input), password, true);
		}

		public void WriteTextFileEncrypted(string filename, string content, string password, bool useUserCodePassword)
		{
			if (!string.IsNullOrEmpty(content))
			{
				_mvxFileStore.EnsureFolderExists(PRIVATE_FOLDER);

				filename = FormatPrivateFileName(filename);

				var newPassword = useUserCodePassword ? GetStringFromUserCode(password) : password;

				_mvxFileStore.WriteFile(filename,
										_encryptedService.Encrypt(content, newPassword));
			}    
			
		}

		#endregion

		#region DeleteFile

		public void DeleteFile (string filename, bool isPrivate = false)
		{
			if (isPrivate)
				filename = FormatPrivateFileName (filename);

			if (_mvxFileStore.Exists(filename))
				_mvxFileStore.DeleteFile (filename);
		}

		#endregion

		public string SaveDocument (Stream stream, string filename)
		{
			var memoryStream = new MemoryStream ();
			stream.CopyTo (memoryStream);
			byte [] pictureBytes = memoryStream.ToArray ();

			return SaveDocument (pictureBytes, filename);
		}

		public string SaveDocument (byte [] bytes, string filename)
		{
			_mvxFileStore.EnsureFolderExists (PRIVATE_FOLDER);

			filename = FormatPrivateFileName (filename);
			
			_mvxFileStore.WriteFile (filename, bytes);
			return _mvxFileStore.NativePath (filename);
		}

		public void DeletePrivateData ()
		{
			if (_mvxFileStore.FolderExists (PRIVATE_FOLDER))
				_mvxFileStore.DeleteFolder (PRIVATE_FOLDER, true);
		}


		private string FormatPrivateFileName (string filename)
		{
			//if (App.User != null)
			//{
			//	if ((App.Os == Models.TipoDispositivo.WINP) || (App.Os == Models.TipoDispositivo.WINT))
			//	{
			//		if (filename.Contains(App.User.CodigoUsuario))
			//			return $"{PRIVATE_FOLDER}\\{filename}";
			//		else
			//			return $"{PRIVATE_FOLDER}\\{App.User.CodigoUsuario}_{filename}";
			//	}
			//	if (filename.Contains(App.User.CodigoUsuario))
			//		return $"{PRIVATE_FOLDER}/{filename}";
			//	return $"{PRIVATE_FOLDER}/{App.User.CodigoUsuario}_{filename}";
			//}
			return filename;
		}

		public void DeleteAllUserData ()
		{
			DeletePrivateData ();
		}

		#region Exists
		public bool Exists(string filename, bool isPrivate = false)
		{
			var newfilename = filename;
			if (isPrivate)
				newfilename = FormatPrivateFileName(filename);

			return _mvxFileStore.Exists(newfilename);
		}
		#endregion


		public FileDocument GetDocument(string filename, bool isPrivate = false)
		{
			FileDocument document = null;
			var newfilename = filename;
			if (isPrivate)
				newfilename = FormatPrivateFileName(filename);
			
			if (_mvxFileStore.Exists(newfilename))
			{
				document = new FileDocument()
				{
					Fullpath = _mvxFileStore.NativePath(newfilename)
				};
				byte[] documentBytes = null;
				_mvxFileStore.TryReadBinaryFile(newfilename, out documentBytes);

				if (documentBytes != null)
				{
					document.Document = documentBytes;
				}
			}

			return document;
		}
	}
}


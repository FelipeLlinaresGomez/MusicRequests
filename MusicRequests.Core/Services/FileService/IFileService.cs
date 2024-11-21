using System;
using System.Collections.Generic;
using System.IO;

namespace MusicRequests.Core.Services
{
	public interface IFileService
	{
		T ReadTextFile<T>(string filename);
		string ReadTextFile(string filename);
		T ReadTextFileEncrypted<T>(string filename, string password);
		string ReadTextFileEncrypted(string filename, string password, bool useUserCodePassword);

		void WriteTextFile<T>(string filename, T input);
		void WriteTextFile(string filename, string content);
		void WriteTextFileEncrypted<T>(string filename, T input, string password);
		void WriteTextFileEncrypted(string filename, string content, string password, bool useUserCodePassword);

		string SaveDocument (Stream stream, string filename);
		string SaveDocument (byte[] bytes, string filename);

		void DeleteFile(string filename, bool isPrivate = false);
		void DeletePrivateData ();
		void DeleteAllUserData ();


		bool Exists(string filename, bool isPrivate = false);
		FileDocument GetDocument(string filename, bool isPrivate = false);
	}

	public class FileDocument
	{
		public string Fullpath { get; set; }
		public byte[] Document { get; set; }
	}
}


using System;
namespace MusicRequests.Core
{
	public interface IDocumentService
	{
		void OpenPdfDocument(string fullpathDocument, byte[] pdfDocument);
	}
}

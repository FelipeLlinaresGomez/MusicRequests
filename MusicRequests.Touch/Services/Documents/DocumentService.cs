using System;
using System.Drawing;
using Foundation;
using MusicRequests.Core;
using QuickLook;
using UIKit;
using MusicRequests.Touch.Styles;

namespace MusicRequests.Touch
{
	public class DocumentService : IDocumentService
	{
		public void OpenPdfDocument(string fullpathDocument, byte[] pdfDocument)
		{
			string[] fullpathArray = fullpathDocument.Split("/".ToCharArray());

			var title = fullpathArray[fullpathArray.Length - 1];

			QLPreviewItemFileSystem prevItem = new QLPreviewItemFileSystem(fullpathDocument, title);
			QLPreviewController previewController = new QLPreviewController();
			previewController.DataSource = new PreviewControllerDS(prevItem);
			((UINavigationController)UIApplication.SharedApplication.KeyWindow.RootViewController).PresentModalViewController(previewController, true);
		}
	}

	public class PreviewControllerDS : QLPreviewControllerDataSource
	{
		private QLPreviewItem _item;

		public PreviewControllerDS(QLPreviewItem item)
		{
			_item = item;
		}

		public override nint PreviewItemCount(QLPreviewController controller)
		{
			return 1;
		}

		public override IQLPreviewItem GetPreviewItem(QLPreviewController controller, nint index)
		{
			return _item;
		}
	}

	public class QLPreviewItemFileSystem : QLPreviewItem
	{
		private readonly string _fileNameAndPath;
		private readonly string _title;
		public QLPreviewItemFileSystem(string fileNameAndPath, string title)
		{
			_title = title;
			_fileNameAndPath = fileNameAndPath;
		}

		public override string PreviewItemTitle
        {
			get
			{
				return _title;
			}
		}
		public override NSUrl PreviewItemUrl
        {
			get
			{
				return NSUrl.FromFilename(_fileNameAndPath);
			}
		}
	}
}

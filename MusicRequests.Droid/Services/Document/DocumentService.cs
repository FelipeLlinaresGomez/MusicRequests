using Android.Content;
using AndroidX.Core.Content;
using MusicRequests.Core;
using MvvmCross;
using MvvmCross.Platforms.Android;

namespace MusicRequests.Droid
{
    public class DocumentService : IDocumentService
	{
		public void OpenPdfDocument(string filePath, byte[] pdfDocument)
		{
            var activity = Mvx.IoCProvider.Resolve<IMvxAndroidCurrentTopActivity>().Activity;

            Java.IO.File file = new Java.IO.File(filePath);
            file.SetReadable(true);
            Android.Net.Uri uri = FileProvider.GetUriForFile(activity,
                                                             activity.ApplicationContext.PackageName + ".fileprovider", file);
            Intent intent = new Intent(Intent.ActionView);
            intent.SetDataAndType(uri, "application/pdf");
            intent.SetFlags(ActivityFlags.GrantReadUriPermission | ActivityFlags.ClearWhenTaskReset | ActivityFlags.NewTask);

            activity.StartActivity(intent);
		}
	}
}

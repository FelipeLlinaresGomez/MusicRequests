using System.Threading.Tasks;
using Microsoft.Maui.Storage;


namespace MusicRequests.Core.Services.Image
{
    public interface IMediaFileExtensions
    {
        /// <summary>
        ///  Does nothing
        /// </summary>
        /// <param name="file">The file image</param>
        /// <returns>True if rotation occured, else false</returns>
        Task<bool> FixOrientationAsync(FileResult file);
    }
}


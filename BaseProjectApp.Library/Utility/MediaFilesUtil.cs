using Microsoft.AspNetCore.Http;
using BaseProjectApp.Library.Templates.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseProjectApp.Library.Utility
{
    public class MediaUtil
    { 
        static public int? GetMediaTypeFromExtension(IFormFile? file)
        {
            if (file == null)
                return null;

            string extension = Path.GetExtension(file.FileName).ToLower();

            if (IsImageExtension(extension))
            {
                return (int)MediaType.Image;
            }
            else if (IsVideoExtension(extension))
            {
                return (int)MediaType.Video;
            }
            else
            {
                return (int)MediaType.File;
            }
        }

        static private bool IsImageExtension(string extension)
        {
            string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
            return imageExtensions.Contains(extension);
        }

        static private bool IsVideoExtension(string extension)
        {
            string[] videoExtensions = { ".mp4", ".avi", ".mov", ".mkv", ".wmv" };
            return videoExtensions.Contains(extension);
        }
    }
}

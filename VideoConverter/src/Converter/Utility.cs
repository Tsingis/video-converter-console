using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;

namespace VideoConverter
{
    public static class Utility
    {
        public static string DownloadVideo(string fileUrl)
        {
            var desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var url = new Uri(fileUrl);
            var file = Path.GetFileName(url.LocalPath);
            var downloadPath = Path.Join(desktop, file);

            try
            {
                using (var client = new WebClient())
                {
                    client.DownloadFile(url, downloadPath);
                }

                return downloadPath;
            }
            catch (Exception ex)
            {
                throw new WebException("Download failed.", ex);
            }
        }

        public static List<T> GetAllPublicConstantsValues<T>(this Type type)
        {
            return type
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(x => x.IsLiteral && !x.IsInitOnly && x.FieldType == typeof(T))
                .Select(x => (T)x.GetRawConstantValue())
                .ToList();
        }
    }
}
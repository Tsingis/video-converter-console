using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace VideoConverter
{
    public static class Utility
    {
        public static async Task<string> DownloadFileAsync(string fileUrl)
        {
            try
            {
                var url = new Uri(fileUrl);
                var path = Path.GetTempPath();
                var file = Path.GetFileName(url.LocalPath);
                var downloadPath = Path.Join(path, file);

                using (var client = new HttpClient())
                {
                    var res = await client.GetAsync(url);
                    {
                        if (res.IsSuccessStatusCode)
                        {
                            using (var fs = new FileStream(downloadPath, FileMode.Create))
                            {
                                await res.Content.CopyToAsync(fs);
                                return downloadPath;
                            }
                        }

                        throw new HttpRequestException($"Status code: " + res.StatusCode);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Download failed. " + ex.Message);
            }
        }

        public static string GetOutputFilepath(string inputFilepath, string outputFolder, string outputFormat)
        {
            var inputFile = Path.GetFileName(inputFilepath);
            var outputFilepath = Path.Join(outputFolder, inputFile);
            return Path.ChangeExtension(outputFilepath, outputFormat);
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
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
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

        public static string GetOutputFilepath(string inputFilePath, string outputDir, string outputFormat)
        {
            var inputFile = Path.GetFileName(inputFilePath);
            var outputFilepath = Path.Join(outputDir, inputFile);
            return Path.ChangeExtension(outputFilepath, outputFormat);
        }

        public static bool FFmpegExecutablesExist(string targetDirectory)
        {
            var execs = new string[] { "ffmpeg.exe", "ffprobe.exe", "ffplay.exe" };
            var files = Directory.GetFiles(targetDirectory).Select(Path.GetFileName);
            return execs.All(x => files.Contains(x));
        }
    }
}
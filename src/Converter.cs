using System;
using System.Net;
using System.IO;
using Xabe.FFmpeg;
using System.Threading.Tasks;

namespace VideoConverter {
    public static class Converter
    {
        public async static Task ConvertVideoFromFile(string inputFilePath, string outputFormat)
        {
            var outputFilePath = Path.ChangeExtension(inputFilePath, outputFormat);

            try
            {
                IConversion conversion;
                switch (outputFormat)
                {
                    case VideoFormats.mp4:
                        conversion = await FFmpeg.Conversions.FromSnippet.ToMp4(inputFilePath, outputFilePath);
                        break;
                    case VideoFormats.webm:
                        conversion = await FFmpeg.Conversions.FromSnippet.ToWebM(inputFilePath, outputFilePath);
                        break;
                    case VideoFormats.gif:
                        conversion = await FFmpeg.Conversions.FromSnippet.ToGif(inputFilePath, outputFilePath, 1);
                        break;
                    default:
                        throw new VideoFormatException("Unsupported video format.");
                }
                
                await conversion.Start();
            }
            catch (Exception ex)
            {
                throw new ConversionException("Conversion failed.", ex);
            }
            finally {
                File.Delete(inputFilePath);
            }
            
        }

        public static async Task ConvertVideoFromUrl(string fileUrl, string outputFormat)
        {
            var downloadPath = DownloadVideo(fileUrl);
            
            if (File.Exists(downloadPath))
            {   
                await ConvertVideoFromFile(downloadPath, outputFormat);
                
                if (File.Exists(downloadPath))
                {
                    File.Delete(downloadPath);
                }
            }
            else
            {
                throw new FileNotFoundException("File was not found. File: " + downloadPath);
            } 
        }

        private static string DownloadVideo(string fileUrl)
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

        private class VideoFormats
        {
            public const string mp4 = "mp4";
            public const string webm = "webm";
            public const string gif = "gif";
        }

    }
}
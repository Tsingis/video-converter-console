using System;
using System.Net;
using System.IO;
using MediaToolkit;
using MediaToolkit.Model;

namespace VideoConverter {
    public static class Converter {

        public static void ConvertVideoFromFile(string inputFilePath, string outputFormat)
        {
            var inputFile = new MediaFile {
                Filename = inputFilePath
            };

            var outputFile = new MediaFile {
                Filename = Path.ChangeExtension(inputFilePath, outputFormat)
            };

            try
            {
                using (var engine = new Engine())
                {
                    engine.Convert(inputFile, outputFile);
                }
            }
            catch (Exception ex)
            {
                throw new ConversionException("Conversion failed.", ex);
            }
        }

        public static void ConvertVideoFromUrl(string fileUrl, string outputFormat)
        {
            var downloadPath = DownloadVideo(fileUrl);
            
            if (File.Exists(downloadPath))
            {   
                ConvertVideoFromFile(downloadPath, outputFormat);
                
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
            }
            catch (Exception ex)
            {
                throw new WebException("Download failed.", ex);
            }
            
            return downloadPath;
        }
    }
}
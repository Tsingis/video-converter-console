using System.Net;
using System;
using System.IO;
using MediaToolkit;
using MediaToolkit.Model;

namespace VideoConverter {
    public static class Converter {

        public static void ConvertVideoFromFile(string inputFilePath, string outputFormat)
        {
            var filename = Path.GetFileNameWithoutExtension(inputFilePath);
            var folder = Path.GetDirectoryName(inputFilePath);

            var inputFile = new MediaFile {
                Filename = inputFilePath
            };

            var outputFile = new MediaFile {
                Filename = Path.Join(folder, filename) + $".{outputFormat}"
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
                DeleteVideo(downloadPath);
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

        private static void DeleteVideo(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

    }
}
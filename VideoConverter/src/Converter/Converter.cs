using System;
using System.IO;
using Xabe.FFmpeg;
using System.Threading.Tasks;

namespace VideoConverter
{
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
                    case VideoFormats.Mp4:
                        conversion = await FFmpeg.Conversions.FromSnippet.ToMp4(inputFilePath, outputFilePath);
                        break;
                    case VideoFormats.Webm:
                        conversion = await FFmpeg.Conversions.FromSnippet.ToWebM(inputFilePath, outputFilePath);
                        break;
                    case VideoFormats.Gif:
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
            finally
            {
                File.Delete(inputFilePath);
            }

        }

        public static async Task ConvertVideoFromUrl(string fileUrl, string outputFormat)
        {
            var downloadPath = Utility.DownloadVideo(fileUrl);

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
    }
}
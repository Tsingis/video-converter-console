using System;
using System.IO;
using System.Threading.Tasks;
using Xabe.FFmpeg;
using Xabe.FFmpeg.Exceptions;

namespace VideoConverter
{
    public static class Converter
    {
        private static readonly string _alternativeFFmpegPath = Environment.CurrentDirectory;

        public static async Task<string> ConvertVideoAsync(string inputFilePath, string outputFileDir, string outputFormat)
        {
            var outputFilePath = Utility.GetOutputFilepath(inputFilePath, outputFileDir, outputFormat);
            if (File.Exists(outputFilePath))
            {
                File.Delete(outputFilePath);
            }

            try
            {
                IConversion conversion;
                switch (outputFormat)
                {
                    case VideoFormat.Mp4:
                        conversion = await FFmpeg.Conversions.FromSnippet.ToMp4(inputFilePath, outputFilePath);
                        break;
                    case VideoFormat.Webm:
                        conversion = await FFmpeg.Conversions.FromSnippet.ToWebM(inputFilePath, outputFilePath);
                        break;
                    case VideoFormat.Gif:
                        conversion = await FFmpeg.Conversions.FromSnippet.ToGif(inputFilePath, outputFilePath, 1);
                        break;
                    default:
                        throw new VideoFormatException("Unsupported video format.");
                }

                await conversion.Start();
                return outputFilePath;
            }
            catch (FFmpegNotFoundException)
            {
                if (Utility.FFmpegExecutablesExist(_alternativeFFmpegPath))
                {
                    FFmpeg.SetExecutablesPath(_alternativeFFmpegPath);
                }

                throw new FFmpegPathException($"FFmpeg executables not found in environment PATH or in {_alternativeFFmpegPath}.");
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
    }
}
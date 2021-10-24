using System;
using System.IO;
using Xabe.FFmpeg;
using System.Threading.Tasks;

namespace VideoConverter
{
    public class Converter
    {
        public string _outputFolder;

        public Converter(string outputFolder)
        {
            _outputFolder = outputFolder;
        }

        public async Task ConvertVideo(string inputFilePath, string outputFormat)
        {
            var outputFilePath = Utility.GetOutputFilepath(inputFilePath, _outputFolder, outputFormat);
            if (File.Exists(outputFilePath))
            {
                throw new ConversionException("Output file already exists.");
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
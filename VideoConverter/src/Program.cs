using System.IO;
using System;
using System.Threading.Tasks;

namespace VideoConverter
{
    public class Program
    {

        public static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }

        private static async Task MainAsync(string[] args)
        {
            var exitCode = ExitCode.Start;

            Console.WriteLine("Give URL or file path and output format");
            Console.WriteLine("Example: https://thissite.com/video.webm mp4");

            while (exitCode != ExitCode.Success)
            {
                Console.Write("\nInput: ");
                var input = Console.ReadLine();

                var parameters = input.Split(" ", StringSplitOptions.None);

                if (parameters.Length > 1 && parameters.Length <= 2)
                {
                    var file = parameters[0];
                    var outputFormat = VideoFormats.Mp4;

                    if (parameters.Length == 2)
                    {
                        var format = parameters[1].ToLower();
                        if (VideoFormats.IsSupportedVideoFormat(format))
                        {
                            outputFormat = format;
                        }
                        else
                        {
                            exitCode = ExitCode.Error;
                            Console.WriteLine("\nOutput format is not supported.");
                            continue;
                        }
                    }

                    var extension = Path.GetExtension(file).Replace(".", "");
                    if (extension == outputFormat)
                    {
                        exitCode = ExitCode.Error;
                        Console.WriteLine("\nOutput and input formats are the same.");
                        continue;
                    }
                    else
                    {
                        if (Uri.IsWellFormedUriString(file, UriKind.RelativeOrAbsolute))
                        {
                            await Converter.ConvertVideoFromUrl(file, outputFormat);
                        }
                        else
                        {
                            await Converter.ConvertVideoFromFile(file, outputFormat);
                        }

                        exitCode = ExitCode.Success;
                    }
                }
                else
                {
                    exitCode = ExitCode.Error;
                    Console.WriteLine("\nIncorrect input. Try again.");
                    continue;
                }
            }
        }

        private enum ExitCode
        {
            Start = 0,
            Success,
            Error
        }
    }
}

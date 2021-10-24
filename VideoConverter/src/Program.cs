using System.IO;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace VideoConverter
{
    public class Program
    {
        private static IConfiguration _config;
        public static void Main(string[] args)
        {
            _config = ProgramConfig();
            MainAsync(args).GetAwaiter().GetResult();
        }

        private static async Task MainAsync(string[] args)
        {
            var exitCode = ExitCode.Start;

            var defaultOutputFormat = _config.GetValue<string>("defaultOutputFormat").ToLower();
            var outputFolder = _config.GetValue<string>("outputFolder");

            if (!Directory.Exists(outputFolder))
            {
                var path = Path.Join(Environment.CurrentDirectory, "Output");
                Directory.CreateDirectory(path);
                outputFolder = path;
            }

            if (String.IsNullOrEmpty(defaultOutputFormat))
            {
                defaultOutputFormat = VideoFormat.Mp4;
            }

            Console.WriteLine("Give URL or file path and output format");
            Console.WriteLine("Example: https://thissite.com/video.webm mp4");

            while (exitCode != ExitCode.End)
            {
                var outputFormat = defaultOutputFormat;

                Console.Write("\nInput (q to quit): ");
                var input = Console.ReadLine();

                var parameters = input.Split(" ", StringSplitOptions.RemoveEmptyEntries);

                if (!String.IsNullOrEmpty(input) && parameters.Length <= 2)
                {
                    if (parameters[0].ToLower().Equals("q"))
                    {
                        exitCode = ExitCode.End;
                        break;
                    }

                    if (parameters.Length == 2)
                    {
                        var format = parameters[1].ToLower();
                        if (VideoFormat.IsSupportedVideoFormat(format))
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

                    var file = parameters[0];
                    var inputFormat = Path.GetExtension(file).Replace(".", "");
                    if (inputFormat.Equals(outputFormat))
                    {
                        exitCode = ExitCode.Error;
                        Console.WriteLine("\nOutput and input formats are the same.");
                        continue;
                    }
                    else
                    {
                        try
                        {
                            var converter = new Converter(outputFolder);
                            if (Uri.IsWellFormedUriString(file, UriKind.RelativeOrAbsolute))
                            {
                                var downloadPath = Utility.DownloadVideo(file);
                                if (File.Exists(downloadPath))
                                {
                                    await converter.ConvertVideo(downloadPath, outputFormat);

                                    if (File.Exists(downloadPath))
                                    {
                                        File.Delete(downloadPath);
                                    }
                                }
                                else
                                {
                                    await converter.ConvertVideo(file, outputFormat);
                                }
                                exitCode = ExitCode.Success;
                                Console.WriteLine("Successfully conversed file to " + outputFolder);
                            }
                        }
                        catch (Exception ex)
                        {
                            exitCode = ExitCode.Error;
                            Console.WriteLine("Error in conversion. " + ex.Message);
                        }
                    }
                }
                else
                {
                    exitCode = ExitCode.Error;
                    Console.WriteLine("\nIncorrect input. Try again.");
                }
            }
        }

        private static IConfiguration ProgramConfig()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config.json", false);

            return builder.Build();
        }

        private enum ExitCode
        {
            Start = 0,
            End,
            Success,
            Error
        }
    }
}

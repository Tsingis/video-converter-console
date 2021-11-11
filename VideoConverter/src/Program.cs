using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace VideoConverter
{
    public class Program
    {
        private static ExitCode _exitCode;
        private static string _outputFolder;
        private static string _outputFormat;
        private static string _defaultOutputFormat;
        private static IConfiguration _config;

        public static void Main()
        {
            _exitCode = ExitCode.Start;

            SetupProgram();

            Console.WriteLine("Give URL or file path and output format");
            Console.WriteLine("Example: https://thissite.com/video.webm mp4");

            while (_exitCode != ExitCode.End)
            {
                _outputFormat = _defaultOutputFormat;

                Console.Write("\nInput (q to quit): ");
                var input = Console.ReadLine();

                var parameters = input.Split(" ", StringSplitOptions.RemoveEmptyEntries);

                if (!String.IsNullOrEmpty(input) && parameters.Length <= 2)
                {
                    if (parameters[0].ToLower().Equals("q"))
                    {
                        _exitCode = ExitCode.End;
                        Environment.Exit((int)_exitCode);
                    }

                    if (parameters.Length == 2)
                    {
                        var format = parameters[1].ToLower();
                        if (VideoFormat.IsSupportedVideoFormat(format))
                        {
                            _outputFormat = format;
                        }
                        else
                        {
                            _exitCode = ExitCode.Error;
                            Console.WriteLine("\nOutput format is not supported.");
                            continue;
                        }
                    }

                    var inputFile = parameters[0];
                    var inputFormat = Path.GetExtension(inputFile).Replace(".", "");
                    if (inputFormat.Equals(_outputFormat))
                    {
                        _exitCode = ExitCode.Error;
                        Console.WriteLine("\nOutput and input formats are the same.");
                        continue;
                    }

                    ConvertVideo(inputFile);
                }
                else
                {
                    _exitCode = ExitCode.Error;
                    Console.WriteLine("\nIncorrect input. Try again.");
                }
            }
        }

        private static void ConvertVideo(string inputFile)
        {
            try
            {
                var converter = new Converter(_outputFolder);
                if (Uri.IsWellFormedUriString(inputFile, UriKind.RelativeOrAbsolute))
                {
                    var downloadPath = Utility.DownloadFileAsync(inputFile).Result;

                    if (File.Exists(downloadPath))
                    {
                        converter.ConvertVideoAsync(downloadPath, _outputFormat).Wait();
                    }
                }
                else
                {
                    converter.ConvertVideoAsync(inputFile, _outputFormat).Wait();
                }

                _exitCode = ExitCode.Success;
                Console.WriteLine("Successfully conversed file to " + _outputFolder);
            }
            catch (Exception ex)
            {
                _exitCode = ExitCode.Error;
                Console.WriteLine("Error in conversion. " + ex.Message);
            }
        }

        private static void SetupProgram()
        {
            try
            {
                _config = SetupConfig();
                _defaultOutputFormat = _config.GetValue<string>("defaultOutputFormat").ToLower();
                _outputFolder = _config.GetValue<string>("outputFolder");

                if (!Directory.Exists(_outputFolder))
                {
                    var path = Path.Join(Environment.CurrentDirectory, "Output");
                    Directory.CreateDirectory(path);
                    _outputFolder = path;
                }

                if (String.IsNullOrEmpty(_defaultOutputFormat))
                {
                    _defaultOutputFormat = VideoFormat.Mp4;
                }
            }
            catch (Exception ex)
            {
                _exitCode = ExitCode.End;
                Console.WriteLine("Error in config. " + ex.Message);
                Console.WriteLine("Press any key to quit");
                Console.ReadKey();
                Environment.Exit((int)_exitCode);
            }
        }

        private static IConfiguration SetupConfig()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config.json", false);

            return builder.Build();
        }

        private enum ExitCode
        {
            End = 0,
            Start = 1,
            Success,
            Error
        }
    }
}

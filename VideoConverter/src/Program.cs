using System;
using System.IO;
using CommandLine;
using CommandLine.Text;
using Microsoft.Extensions.Configuration;

namespace VideoConverter
{
    public class Program
    {
        private static ExitCode _exitCode;
        private static string _inputFile;
        private static string _outputDir;
        private static string _outputFormat;
        private static string _defaultOutputDir;
        private static string _defaultOutputFormat;

        public static void Main()
        {
            SetupProgram();

            Console.WriteLine("Give URL or file path and optional output format and/or output path.");
            Console.WriteLine("Default parameters can be set with config.json file.\n");
            Console.WriteLine("Usage: -i <input url or path> -f <output format> -o <output path>");
            Console.WriteLine("Example: -i https://video.com/video1.mp4 -f webm");

            while (true)
            {
                _outputDir = _defaultOutputDir;
                _outputFormat = _defaultOutputFormat;

                Console.Write("\nType input (q to quit): ");

                var input = Console.ReadLine();
                if (input.ToLower().Equals("q"))
                {
                    Environment.Exit((int)ExitCode.OK);
                }

                var parameters = input.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                var parsed = Parser.Default.ParseArguments<Options>(parameters);

                parsed.WithParsed<Options>(opt =>
                {
                    _exitCode = HandleOptions(opt);
                    if (_exitCode != ExitCode.Error)
                    {
                        _exitCode = Convert();
                    }
                });
            }
        }

        private static ExitCode HandleOptions(Options options)
        {
            if (options.OutputFormat != null)
            {
                var isSupported = VideoFormat.IsSupportedVideoFormat(options.OutputFormat);
                if (!isSupported)
                {
                    Console.WriteLine($"Output format {options.OutputFormat} is not supported.");
                    return ExitCode.Error;
                }
                _outputFormat = options.OutputFormat;
            }

            if (options.OutputPath != null)
            {
                _outputDir = options.OutputPath;
            }

            if (!String.IsNullOrEmpty(options.InputFile))
            {
                var inputFormat = Path.GetExtension(options.InputFile).Replace(".", "");
                if (inputFormat.Equals(_outputFormat))
                {
                    Console.WriteLine("Output and input formats are the same.");
                    return ExitCode.Error;
                }

                _inputFile = options.InputFile;
            }

            return ExitCode.OK;
        }

        private static ExitCode Convert()
        {
            try
            {
                string output = String.Empty;
                var converter = new Converter(_outputDir);
                if (Uri.IsWellFormedUriString(_inputFile, UriKind.RelativeOrAbsolute))
                {
                    var downloadPath = Utility.DownloadFileAsync(_inputFile).Result;

                    if (File.Exists(downloadPath))
                    {
                        output = converter.ConvertVideoAsync(downloadPath, _outputFormat).Result;
                    }
                }
                else
                {
                    output = converter.ConvertVideoAsync(_inputFile, _outputFormat).Result;
                }

                Console.WriteLine($"Successfully conversed file {output}");
                return ExitCode.OK;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in conversion. {ex.Message}");
                return ExitCode.Error;
            }
        }

        private static void SetupProgram()
        {
            try
            {
                var config = SetupConfig();
                _defaultOutputFormat = config.GetValue<string>("defaultOutputFormat").ToLower();
                _defaultOutputDir = config.GetValue<string>("defaultOutputDir");

                if (!Directory.Exists(_defaultOutputDir))
                {
                    var path = Path.Join(Environment.CurrentDirectory, "Output");
                    Directory.CreateDirectory(path);
                    _defaultOutputDir = path;
                }

                if (String.IsNullOrEmpty(_defaultOutputFormat) ||
                    !VideoFormat.IsSupportedVideoFormat(_defaultOutputFormat))
                {
                    _defaultOutputFormat = VideoFormat.Mp4;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in config. " + ex.Message);
                Console.WriteLine("Press any key to quit");
                Console.ReadKey();
                Environment.Exit((int)ExitCode.Error);
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
            OK = 0,
            Error,
        }
    }
}

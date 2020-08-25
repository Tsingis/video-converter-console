using System;

namespace VideoConverter
{
    public class Program
    {
        static void Main(string[] args)
        {   
            var exitCode = ExitCode.Start;

            Console.WriteLine("Give URL or file path and output format");
            Console.WriteLine("Example: https://thissite.com/video.webm mp4");
        
            while (exitCode != ExitCode.Success)
            {

                Console.Write("\nInput: ");
                var input = Console.ReadLine();

                var parameters = input.Split(" ", StringSplitOptions.None);

                if (parameters.Length == 2)
                {
                    var file = parameters[0];
                    var outputFormat = parameters[1];
                    
                    if (Uri.IsWellFormedUriString(file, UriKind.RelativeOrAbsolute))
                    {
                        Converter.ConvertVideoFromUrl(file, outputFormat);
                    }
                    else
                    {
                        Converter.ConvertVideoFromFile(file, outputFormat);
                    }
      
                    exitCode = ExitCode.Success;
                }
                else
                {
                    exitCode = ExitCode.Error;
                    Console.WriteLine("\nIncorrect input. Try again.");
                }
            }
        }

        private enum ExitCode {
            Start = 0,
            Success,
            Error
        }
    }
}

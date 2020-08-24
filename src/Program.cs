using System;

namespace VideoConverter
{
    public class Program
    {
        static void Main(string[] args)
        {   
            Console.WriteLine("Give URL or file path and output format");
            Console.WriteLine("Example: https://thissite.com/video.webm mp4");
            Console.Write("\nInput: ");
            var input = Console.ReadLine();

            var parameters = input.Split(" ", StringSplitOptions.None);

            if (parameters.Length > 0 && parameters.Length <= 2)
            {
                var file = parameters[0];
                var outputFormat = parameters[1];
                
                if (new Uri(file).IsFile)
                {
                    Converter.ConvertVideoFromFile(file, outputFormat);
                }
                else
                {
                    Converter.ConvertVideoFromUrl(file, outputFormat);
                }
            }
        }
    }
}

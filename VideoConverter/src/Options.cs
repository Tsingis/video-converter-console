using CommandLine;

public class Options
{
    [Option('i', "input", Required = true, HelpText = "The input file to convert.")]
    public string InputFile { get; set; }

    [Option('f', "format", Required = false, HelpText = "Format for the converted output file.")]
    public string OutputFormat { get; set; }

    [Option('o', "output", Required = false, HelpText = "Output path for the converted file.")]
    public string OutputPath { get; set; }
}
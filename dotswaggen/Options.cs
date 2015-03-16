using System.Reflection;
using CommandLine;
using CommandLine.Text;

namespace dotswaggen
{
    public class Options
    {
        [Option('s', "swagger", Required = true,
            HelpText = "Input files to be processed.")]
        public string InputFile { get; set; }

        [Option('n', "namespace", Required = true,
            HelpText = "The namespace to use for generated code")]
        public string Namespace { get; set; }

        [Option('o', "output", Required = true,
            HelpText = "The folder to output rendered code to")]
        public string OutputFolder { get; set; }

        [Option('m', "model", Required = false, DefaultValue = "c#",
            HelpText = "The language model to use (Default 'c#')")]
        public string Model { get; set; }

        [Option("t-prefix", Required = false,
            HelpText = "Prefix the template filename for each template type")]
        public string TemplatePrefix { get; set; }

        [Option("o-prefix", Required = false,
            HelpText = "Prefix the output filename for each file generated")]
        public string OutputPrefix { get; set; }

        [Option("o-single-name", Required = false,
            HelpText = "The filename to write all output to", MutuallyExclusiveSet = "o-prefix")]
        public string WriteSingleFileName { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            var help = new HelpText
            {
                Heading = new HeadingInfo("DotSwagGen", Assembly.GetEntryAssembly().GetName().Version.ToString(3)),
                AdditionalNewLineAfterOption = true,
                AddDashesToOption = true
            };
            // help.AddPreOptionsLine("<>");
            help.AddPreOptionsLine("Usage: dotswaggen -s <filename.json> -n test.namespace -o <directory>");
            help.AddOptions(this);
            return help;
        }
    }
}
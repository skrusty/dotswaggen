using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using CommandLine;
using dotswaggen.Swagger;
using DotLiquid;
using Newtonsoft.Json;

namespace dotswaggen
{
    internal class Program
    {
        private static Options _options;

        private static void Main(string[] args)
        {
            // Load command line options
            _options = new Options();
            var result = Parser.Default.ParseArgumentsStrict(args, _options, () => _options.GetUsage());
            if (!result)
            {
                return;
            }

            // TODO: Allow multiple files or input file directory
            ProcessFile(_options.InputFile);
        }

        private static void ProcessFile(string inputFile)
        {
            // Load json file
            var filename = Path.GetFileNameWithoutExtension(inputFile);
            string json;
            using (var webClient = new WebClient())
            {
                json = webClient.DownloadString(inputFile);
            }

            try
            {
                var settings = new JsonSerializerSettings
                {
                    MissingMemberHandling = MissingMemberHandling.Error,
                    Error = (sender, args) => { Console.WriteLine(args.ErrorContext.Error.Message); }
                };

                // do some nasty hacks here because Json.NET reserves '$' for internal stuff
                json = json.Replace("$ref", "ref");

                // Parse Models
                var swaggerResource = JsonConvert.DeserializeObject<ApiDeclaration>(json, settings);

                if (swaggerResource.Apis == null)
                    return;

                foreach (var model in swaggerResource.Models)
                {
                    var datatypeModel = new Model
                    {
                        Name = model.Key,
                        Resourceurl = inputFile,
                        sub_type = GetSubType(swaggerResource, model.Key),
                        Namespace = _options.Namespace,
                        Description = model.Value.Description,
                        Properties = new List<ModelProperty>()
                    };

                    datatypeModel.Properties.AddRange(model.Value.Properties.Where(p => p.Value != null).Select(prop => new ModelProperty
                    {
                        Description = prop.Value.Description,
                        Name = prop.Key,
                        Type = prop.Value.Templatetype
                    }));

                    var renderedCode = ApplyTemplate(GetTemplate("Model"), datatypeModel);

                    WriteFile(renderedCode, datatypeModel.Name);
                }

                var apiOperationModel = new ApiOperations
                {
                    Resourceurl = inputFile,
                    Namespace = _options.Namespace,
                    Name = filename ?? "OutputClass",
                    Apis = swaggerResource.Apis.ToList()
                };

                var actionRenderedCode = ApplyTemplate(GetTemplate("Action"), apiOperationModel);

                WriteFile(actionRenderedCode, apiOperationModel.Name);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void WriteFile(string renderedCode, string fileName)
        {
            using (
                var outFile =
                    File.CreateText(Path.Combine(_options.OutputFolder,
                        string.Format("{0}{1}.{2}", _options.OutputPrefix, fileName, "cs"))))
            {
                outFile.Write(renderedCode);
            }
        }

        private static string GetSubType(ApiDeclaration swaggerResource, string subTypeName)
        {
            var subType =
                swaggerResource.Models.SingleOrDefault(
                    x => x.Value.SubTypes != null && x.Value.SubTypes.Contains(subTypeName)).Key;
            return subType;
        }

        private static Template GetTemplate(string name)
        {
            var template2 =
                Template.Parse(
                    File.ReadAllText(string.Format("Templates\\{0}{1}Template.txt", _options.TemplatePrefix, name)));
            return template2;
        }

        private static string ApplyTemplate<TMODEL>(Template template, TMODEL model)
        {
            var renderedCode = template.Render(new RenderParameters()
            {
                Filters = new[] { typeof(TextFilters) },
                LocalVariables = Hash.FromAnonymousObject(new
                {
                    Model = model
                })
            });
            return renderedCode;
        }
    }

    public static class TextFilters
    {
        public static string Varname(Context context, string input)
        {
            return string.Concat(
                "@",
                System.Text.RegularExpressions.Regex.Replace(input, "[^a-zA-Z_0-9]", "_")
            );
        }
    }

    public class InspectedType : DotLiquid.Tag
    {
        public override void Initialize(string tagName, string markup, List<string> tokens)
        {
            base.Initialize(tagName, markup, tokens);
            
        }

        public override void Render(Context context, TextWriter result)
        {
            base.Render(context, result);
        }
    }

    public class TemplateProperties : Drop
    {
        public string Resourceurl { get; set; }
        public string Namespace { get; set; }
        public string Name { get; set; }
    }

    public class ApiOperations : TemplateProperties
    {
        public List<Api> Apis { get; set; }
    }

    public class Model : TemplateProperties
    {
        public string Description { get; set; }
        public List<ModelProperty> Properties { get; set; }
        public string sub_type { get; set; }
    }

    public class ModelProperty : Drop
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
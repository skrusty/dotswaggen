using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using CommandLine;
using dotswaggen.Swagger;
using DotLiquid;
using Newtonsoft.Json;
using dotswaggen.CSharpModel;

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
                var converter = LoadConverter(json);

                //Set up enums required by the CSharp view of the world
                Template.RegisterSafeType(typeof(CSharpModel.Operations.HttpMethod), o => o.ToString());
                Template.RegisterSafeType(typeof(CSharpModel.Operations.ParameterType), o => o.ToString());

                foreach (var m in converter.Models)
                {
                    var typeFileModel = new ClassFile()
                    {
                        Resourceurl = inputFile,
                        Namespace = _options.Namespace,
                        DataType = m
                    };

                    WriteFile(ApplyTemplate(GetTemplate("Model"), typeFileModel), m.Name);
                }

                var operationFileModel = new OperationsFile
                {
                    Resourceurl = inputFile,
                    Namespace = _options.Namespace,
                    Name = filename ?? "OutputClass",
                    Apis = converter.Apis
                };

                WriteFile(ApplyTemplate(GetTemplate("Action"), operationFileModel), operationFileModel.Name);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static SwaggerConverter LoadConverter(string json)
        {
            var swaggerResource = LoadSwagger(json);

            if (swaggerResource.Apis == null)
                throw new Exception("Could not load JSON as Swagger document");

            var converter = new SwaggerConverter(swaggerResource);
            return converter;
        }

        private static ApiDeclaration LoadSwagger(string json)
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
            return swaggerResource;
        }

        private static void WriteFile(string renderedCode, string fileName)
        {
            Directory.CreateDirectory(_options.OutputFolder);

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
            return template.Render(Hash.FromAnonymousObject(new
                {
                    Model = model
                }));
        }
    }

    public class TemplateProperties : Drop
    {
        public string Resourceurl { get; set; }
        public string Namespace { get; set; }
    }

    public class OperationsFile : TemplateProperties
    {
        public string Name { get; set; }
        public CSharpModel.Operations.Api[] Apis { get; set; }
    }

    public class ClassFile : TemplateProperties
    {
        public CSharpModel.DataTypes.DataType DataType { get; set; }
    }
}
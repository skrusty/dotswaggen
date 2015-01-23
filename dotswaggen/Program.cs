using System;
using System.Linq;
using CommandLine;
using DotLiquid;
using dotswaggen.Swagger;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace dotswaggen
{
    class Program
    {

        private static Options _options;

        static void Main(string[] args)
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
            string json;
            using (var webClient = new System.Net.WebClient())
            {
                json = webClient.DownloadString(inputFile);
            }

            try
            {
                var settings = new JsonSerializerSettings {MissingMemberHandling = MissingMemberHandling.Error};

                // Parse Models
                var swaggerResource = JsonConvert.DeserializeObject<ApiDeclaration>(json, settings);

                if (swaggerResource.Apis == null)
                    return;


                // Load Model template with prefix is specified
                var template =
                    Template.Parse(
                        File.ReadAllText(string.Format("Templates\\{0}ModelTemplate.txt", _options.TemapltePrefix)));

                foreach (var model in swaggerResource.Models)
                {
                    var subType =
                        swaggerResource.Models.SingleOrDefault(
                            x => x.Value.SubTypes != null && x.Value.SubTypes.Contains(model.Key)).Key;
                    var tempModel = new Model()
                    {
                        Name = model.Key,
                        Resourceurl = inputFile,
                        sub_type = subType,
                        Namespace = _options.Namespace,
                        Description = model.Value.Description,
                        Properties = new List<ModelProperty>()
                    };
                    foreach (var prop in model.Value.Properties)
                    {
                        if (prop.Value != null)
                        {
                            var newProp = new ModelProperty()
                            {
                                Description = prop.Value.Description,
                                Name = prop.Key,
                                Type = DataTypeRegistry.TypeLookup(prop.Value.Type)
                            };
                            tempModel.Properties.Add(newProp);
                        }
                    }

                    var renderedCode = template.Render(Hash.FromAnonymousObject(new
                    {
                        Model = tempModel
                    }));

                    // write code to output folder
                    using (
                        var outFile =
                            File.CreateText(Path.Combine(_options.OutputFolder,
                                string.Format("{0}{1}.{2}", _options.OutputPrefix, tempModel.Name, "cs"))))
                    {
                        outFile.Write(renderedCode);
                    }
                }

                template =
                    Template.Parse(
                        File.ReadAllText(string.Format("Templates\\{0}ActionTemplate.txt", _options.TemapltePrefix)));
                var tmpModel = new ApiOperations()
                {
                    Resourceurl = inputFile,
                    Namespace = _options.Namespace,
                    Name = "OutputClass",
                    Apis = swaggerResource.Apis.ToList()
                };

                var actionRenderedCode = template.Render(Hash.FromAnonymousObject(new
                {
                    Model = tmpModel
                }));

                // write code to output folder
                using (
                    var outFile =
                        File.CreateText(Path.Combine(_options.OutputFolder,
                            string.Format("{0}{1}.{2}", _options.OutputPrefix, tmpModel.Name, "cs"))))
                {
                    outFile.Write(actionRenderedCode);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

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

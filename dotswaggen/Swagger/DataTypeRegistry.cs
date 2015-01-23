using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace dotswaggen.Swagger
{
    public class DataTypeRegistry
    {

        public static Dictionary<string, string> SwaggerTypeMappings = new Dictionary<string, string>()
        {
            {"integer", "int"},
            {"long", "int"},
            {"float", "float"},
            {"double", "double"},
            {"string", "string"},
            {"byte", "byte"},
            {"boolean", "bool"},
            {"date", "DateTime"},
            {"dateTime", "DateTime"},
        };

        public static string TypeLookup(string type)
        {
            if (SwaggerTypeMappings.ContainsKey(type.ToLower()))
                return SwaggerTypeMappings[type.ToLower()];

            // check for complex type
            var r = new Regex(@"List\[(.*)\]");
            var match = r.Match(type);
            if (match.Success)
                return string.Format("List[{0}]", TypeLookup(match.Value));

            return type;
        }

    }
}
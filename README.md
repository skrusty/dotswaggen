# DotSwagGen
DotSwagGen is a command line code generator for the swagger specification. 

## Templates
DotSwagGen uses a template engine to create code output in any language for a given swagger specification. 

Currently there are two templates included, a C# Model and a C# Operation template. You can create your own templates for specific requirements or languages.

## Usage
Usage: dotswaggen -s \<filename.json\> -n test.namespace -o \<directory\>

  -s, --swagger      Required. Input files to be processed.

  -n, --namespace    Required. The namespace to use for generated code

  -o, --output       Required. The folder to output rendered code to

  --t-prefix         Prefix the template filename for each template type

  --o-prefix         Prefix the output filename for each file generated
  
  --o-single-name    The filename to write all output to

  --help             Display this help screen.

## Swagger Spec Support
Currently we only support Swagger 1.2. 
1.1 is planned to arrive soon.

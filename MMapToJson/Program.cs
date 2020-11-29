using System;
using CommandLine;

namespace MMapToJson
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<InputArguments>(args)
                   .WithParsed<InputArguments>(options => new JsonExporter(options).Export());
        }
    }
}

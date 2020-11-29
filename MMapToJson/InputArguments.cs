using System;
using System.Collections.Generic;
using System.Text;
using CommandLine;

namespace MMapToJson
{
    class InputArguments
    {
        [Option(Required = true)]
        public string SourcePath { get; set; }
        [Option]
        public string DestinationPath { get; set; }
        [Option]
        public bool Overwrite { get; set; }
    }
}

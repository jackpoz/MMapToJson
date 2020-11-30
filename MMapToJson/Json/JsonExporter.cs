using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace MMapToJson.Json
{
    class JsonExporter
    {
        string SourcePath { get; set; }
        string DestinationPath { get; set; }
        bool Overwrite { get; set; }

        const string MMTileExtension = ".mmtile";
        const string JsonExtension = ".json";

        public JsonExporter(InputArguments args)
        {
            var extension = Path.GetExtension(args.SourcePath);
            if (extension != MMTileExtension)
                throw new ArgumentException($"The extension '{extension}' is not supported, please specific a file with extension '{MMTileExtension}'", nameof(args.SourcePath));

            this.SourcePath = args.SourcePath;

            // Use the same folder with json extension if the destination path has not been specified
            if (string.IsNullOrEmpty(args.DestinationPath))
                this.DestinationPath = this.SourcePath.Remove(this.SourcePath.Length - MMTileExtension.Length, MMTileExtension.Length) + JsonExtension;
            else
                this.DestinationPath = args.DestinationPath;

            this.Overwrite = args.Overwrite;

            if (File.Exists(this.DestinationPath))
            {
                if (this.Overwrite)
                    File.Delete(this.DestinationPath);
                else
                    throw new ArgumentException($"File '{this.DestinationPath}' exists already, delete it or specify --{nameof(Overwrite)} parameter", nameof(args.DestinationPath));
            }
        }

        public void Export()
        {
            var tile = MMTileLoader.LoadMMTile(SourcePath);
            var json = JsonSerializer.Serialize(tile, new JsonSerializerOptions
            {
                IncludeFields = true,
                WriteIndented = true,
                MaxDepth = int.MaxValue,
                NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowNamedFloatingPointLiterals
            });
            File.WriteAllText(DestinationPath, json);
        }
    }
}

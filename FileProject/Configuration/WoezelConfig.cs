using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace FileProject.Configuration
{
    public class WoezelConfig
    {
        public string Name { get; set; } = default!;
        public string Path { get; set; } = default!;

        public void SaveTo(string path)
        {
            var configPath = System.IO.Path.Combine(path, "config.json");
            var woezelConfig = JsonSerializer.Serialize<FileProject.Configuration.WoezelConfig>(this);
            System.IO.File.WriteAllText(configPath, woezelConfig);
        }
    }
}

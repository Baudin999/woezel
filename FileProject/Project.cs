using FileProject.Configuration;
using System;
using System.IO;
using System.Collections.Generic;

namespace FileProject
{
    public class Project
    {
        public string Name { get; }
        public string BasePath { get; }
        public WoezelConfig Config { get; }
        private ProjectFilesWatcher? fileWatcher;
        public List<FileModule> Modules = new List<FileModule>();

        public Project(WoezelConfig config)
        {
            this.Name = config.Name;
            this.BasePath = config.Path;
            this.Config = config;
            Init();
        }

        private void Init()
        {
            if (!Directory.Exists(this.BasePath))
            {
                Directory.CreateDirectory(this.BasePath);
            }

            var configPath = Path.Combine(this.BasePath, "config.json");
            if (!File.Exists(configPath))
            {
                Config.SaveTo(this.BasePath);
            }


            Console.Write("Initializing files");
            foreach (string file in Directory.EnumerateFiles(this.BasePath, "*.*", SearchOption.AllDirectories))
            {
                Modules.Add(new FileModule(file, this.BasePath));
                Console.Write(".");
            }
            Console.WriteLine("\nFinished initializing files");

        }

        public void Watch()
        {
            fileWatcher = new ProjectFilesWatcher(this.BasePath);
            fileWatcher.ModuleStream.Subscribe("Logger", message =>
            {
                Console.WriteLine(message.ToString());
                Console.Out.Flush();
            });
            fileWatcher.Start();
            Console.WriteLine($"Started watching directory: '{this.BasePath}'");
            Console.WriteLine("Press 'q' to quit the application.");
        }

        public void Exit()
        {
            if (fileWatcher != null)
            {
                fileWatcher.Stop();
            }
        }
    }
}

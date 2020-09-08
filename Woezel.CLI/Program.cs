using System;
using System.Text.Json;


namespace Woezel.CLI
{
    class Program
    {
        private static void Main(string[] args)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            // load configuration file
            var jsonString = System.IO.File.ReadAllText(@"config.json");

            var woezelConfig = JsonSerializer.Deserialize<FileProject.Configuration.WoezelConfig>(jsonString, options);

            var project = new FileProject.Project(woezelConfig);
            project.Watch();

            while (Console.ReadKey().Key != ConsoleKey.Q) { }
            Console.WriteLine();

            project.Exit();
        }

    }
}

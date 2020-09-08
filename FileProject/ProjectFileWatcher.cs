﻿using System;
using System.IO;

namespace FileProject
{
    public class ProjectFilesWatcher : IDisposable
    {
        public string BasePath { get; }
        private FileSystemWatcher? watcher;
        public ModuleStream ModuleStream { get; } = new ModuleStream();

        public ProjectFilesWatcher(string basePath)
        {
            this.BasePath = basePath;
        }

        public void Start()
        {
            // Create a new FileSystemWatcher and set its properties.
            this.watcher = new FileSystemWatcher
            {
                IncludeSubdirectories = true,
                Path = this.BasePath,

                // Watch for changes in FileName and LastWrite 
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.CreationTime,

                // Only watch text files.
                //Filter = "*.car"
            };

            // Add event handlers.
            watcher.Changed += OnChanged;
            watcher.Deleted += OnDelete;
            watcher.Renamed += OnRenamed;
            watcher.Created += OnCreated;

            // Begin watching.
            watcher.EnableRaisingEvents = true;
        }

        // Define the event handlers.
        private void OnChanged(object source, FileSystemEventArgs e)
        {
            try
            {
                ModuleStream.Publish(new ModuleStreamMessage(
                    FileModule.FromPathToName(e.FullPath, this.BasePath),
                    e.FullPath,
                    MessageType.ModuleChanged
                ));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void OnCreated(object source, FileSystemEventArgs e)
        {
            try
            {
                ModuleStream.Publish(new ModuleStreamMessage(
                    FileModule.FromPathToName(e.FullPath, this.BasePath),
                    e.FullPath,
                    MessageType.ModuleChanged
                ));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void OnDelete(object source, FileSystemEventArgs e)
        {
            try
            {
                ModuleStream.Publish(new ModuleStreamMessage(
                    FileModule.FromPathToName(e.FullPath, this.BasePath),
                    e.FullPath,
                    MessageType.ModuleDeleted
                ));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void OnRenamed(object source, RenamedEventArgs e)
        {
            try
            {
                ModuleStream.Publish(new ModuleStreamMessage(
                    FileModule.FromPathToName(e.FullPath, this.BasePath),
                    e.FullPath,
                    FileModule.FromPathToName(e.OldFullPath, this.BasePath),
                    e.OldFullPath,
                    MessageType.ModuleMoved
                ));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Stop()
        {
            this.Dispose();
        }

        public void Dispose()
        {
            Console.WriteLine("Disposing of the Project Watcher");
            if (watcher != null)
            {
                watcher.EnableRaisingEvents = false;
                watcher.Dispose();
            }
        }
    }
}

using System;
using System.Linq;
using System.Collections.Generic;

namespace FileProject
{
    public class FileModule
    {
        
        public string Name { get; }
        public string FullName { get; }
        public IEnumerable<string> Parts { get; }
        public string FilePath { get; }
        public string BasePath { get; }
        public DateTime LastParsed { get; private set; }
        public bool IsCarFile => FilePath.EndsWith(".car");
        public string Namespace { get; }
        public string Code { get; private set; } = default!;


        public FileModule(string filePath, string basePath)
        {
            this.FilePath = filePath;
            this.BasePath = basePath;

            this.FullName = FileModule.FromPathToName(filePath, basePath);
            this.Parts = FullName.Split(".");
            this.Name = Parts.Last();

            if (IsCarFile)
            {
                Namespace = string.Join(".", Parts.SkipLastN());
                LoadText();
            }
            else
            {
                Namespace = "";
            }
        }

        private void LoadText()
        {
            this.Code = System.IO.File.ReadAllText(this.FilePath);
        }

        internal static string FromPathToName(string path, string basePath)
        {
            var p = path.Replace(basePath, "").Replace("/", ".").Replace("\\", ".").Replace(".car", "");
            if (p.StartsWith(".", StringComparison.Ordinal))
            {
                p = p.Substring(1);
            }
            return p;
        }

    }

    internal static class Extensions
    {
        internal static IEnumerable<T> SkipLastN<T>(this IEnumerable<T> source, int n = 1)
        {
            var it = source.GetEnumerator();
            bool hasRemainingItems = false;
            var cache = new Queue<T>(n + 1);

            do
            {
                if (hasRemainingItems = it.MoveNext())
                {
                    cache.Enqueue(it.Current);
                    if (cache.Count > n)
                        yield return cache.Dequeue();
                }
            } while (hasRemainingItems);
        }
    }
}

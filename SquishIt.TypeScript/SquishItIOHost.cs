using System;
using System.Collections.Generic;
using System.Linq;
using TypeScript.Compiler.IOHosts;

namespace SquishIt.TypeScript
{
    public class SquishItIOHost : WindowsIOHost
    {
        public const string INTERNAL_ABSOLUTE_HEAD = "internal://";

        public Dictionary<string, string> Internal { get; set; } 

        public SquishItIOHost(string basePath) : base(basePath)
        {
            Internal = new Dictionary<string, string>();
        }

        private bool IsInternal(string filename)
        {
            return filename.StartsWith(INTERNAL_ABSOLUTE_HEAD) &&
                Internal.ContainsKey(filename);
        }

        public override string ResolvePath(string path)
        {
            return IsInternal(path) ? path : base.ResolvePath(path);
        }

        public override bool IsAbsolute(string path)
        {
            return IsInternal(path) || base.IsAbsolute(path);
        }

        public override string ReadFile(string path)
        {
            return IsInternal(path) ? Internal[path] : base.ReadFile(path);
        }

        public override bool IsFile(string path)
        {
            return IsInternal(path) || base.IsFile(path);
        }
    }
}

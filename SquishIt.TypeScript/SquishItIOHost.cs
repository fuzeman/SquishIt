using System;
using System.Collections.Generic;
using System.Linq;
using TypeScript.Compiler.IOHosts;

namespace SquishIt.TypeScript
{
    public class SquishItIOHost : WindowsIOHost
    {
        public const string INTERNAL_ABSOLUTE_HEAD = "internal://";

        private readonly Dictionary<string, string> _internal;

        public SquishItIOHost(string basePath) : base(basePath)
        {
            _internal = new Dictionary<string, string>();
        }

        /// <summary>
        /// Add internal file
        /// </summary>
        /// <param name="filename">File Name</param>
        /// <param name="content">File Contents</param>
        public void Add(string filename, string content)
        {
            _internal.Add(INTERNAL_ABSOLUTE_HEAD + filename.Replace("\\", "/"), content);
        }

        private bool IsInternal(string filename)
        {
            return filename.StartsWith(INTERNAL_ABSOLUTE_HEAD) &&
                _internal.ContainsKey(filename.Replace("\\", "/"));
        }

        public override string DirectoryName(string path)
        {
            // Jump out of internal:// if we are looking up the directory name
            return base.DirectoryName(path.Replace("\\", "/").StartsWith(INTERNAL_ABSOLUTE_HEAD) ?
                path.Substring(INTERNAL_ABSOLUTE_HEAD.Length) : path);
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
            return IsInternal(path) ? _internal[path] : base.ReadFile(path);
        }

        public override bool IsFile(string path)
        {
            return IsInternal(path) || base.IsFile(path);
        }
    }
}

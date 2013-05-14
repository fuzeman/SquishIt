using System.Linq;
using SquishIt.Framework;
using SquishIt.Framework.Base;
using System.IO;
using TypeScript;
using TypeScript.Compiler;
using TypeScript.Compiler.Data;
using TypeScript.Compiler.IOHosts;

namespace SquishIt.TypeScript
{
    public class TypeScriptPreprocessor : Preprocessor
    {
        public override string[] Extensions
        {
            get { return new[] {".ts"}; }
        }

        public override IProcessResult Process(string filePath, string content)
        {
            var ioHost = new SquishItIOHost(Path.GetDirectoryName(filePath));
            ioHost.Add(filePath, content);

            var compiler = new BatchCompiler(ioHost);

            compiler.CompilationEnvironment.Code.Add(new SourceUnit("internal://" + filePath, content));
            compiler.Run();

            var dependencies = compiler.ResolvedEnvironment.Code
                .Select(c => (c.Path.StartsWith("internal:") ? c.Path.Substring(11) : c.Path).Replace("/", "\\"))
                .Where(p => !p.EndsWith(filePath.Replace("/", "\\")));

            return new ProcessResult(compiler.Result, dependencies);
        }
    }
}

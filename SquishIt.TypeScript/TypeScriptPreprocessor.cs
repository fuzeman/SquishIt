using SquishIt.Framework;
using SquishIt.Framework.Base;
using TypeScript;

namespace SquishIt.TypeScript
{
    public class TypeScriptPreprocessor : Preprocessor
    {
        public override string[] Extensions
        {
            get { return new[] {".ts"}; }
        }

        private TypeScriptCompiler _compiler;

        public override IProcessResult Process(string filePath, string content)
        {
            if (_compiler == null)
                _compiler = new TypeScriptCompiler();
            return new ProcessResult(_compiler.Compile(content));
        }
    }
}

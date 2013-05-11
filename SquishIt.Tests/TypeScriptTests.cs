using NUnit.Framework;
using SquishIt.Framework;
using SquishIt.Tests.Helpers;
using System;
namespace SquishIt.Tests
{
    [TestFixture]
    public class TypeScriptTests
    {
        JavaScriptBundleFactory javaScriptBundleFactory;

        [SetUp]
        public void Setup()
        {
            javaScriptBundleFactory = new JavaScriptBundleFactory();
        }

        [TestCase(typeof (TypeScript.TypeScriptPreprocessor)), Platform(Exclude = "Unix, Linux, Mono")]
        public void CanBundleJavascriptWithArbitraryTypeScript(Type preprocessorType)
        {
            var preprocessor = Activator.CreateInstance(preprocessorType) as IPreprocessor;
            Assert.NotNull(preprocessor);

            var typescript = @"class Greeter {
	greeting: string;
	constructor (message: string) {
		this.greeting = message;
	}
	greet() {
		return ""Hello, "" + this.greeting;
	}
}";

            var tag = javaScriptBundleFactory
                .WithDebuggingEnabled(true)
                .Create()
                .WithPreprocessor(preprocessor)
                .AddString(typescript, ".ts")
                .Render("~/brewed.js");

            Assert.AreEqual("<script type=\"text/javascript\">" +
                "var Greeter = (function () {\n" +
                    "function Greeter(message) {\n" +
                        "this.greeting = message;\n" +
                    "}\n" +
                    "Greeter.prototype.greet = function () {\n" +
                        "return \"Hello, \" + this.greeting;\n" +
                    "};\n" +
                    "return Greeter;\n" +
                "})();</script>\n\n",
                TestUtilities.TrimLines(TestUtilities.NormalizeLineEndings(tag)));
        }
    }
}

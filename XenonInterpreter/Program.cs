using System;
using XenonCore;

namespace XenonInterpreter {
    class MainClass {
        public static void Main (string[] args) {
            const string source = @"
            fn main (a, b, c) {
            }
            ";
            Console.WriteLine ("Tokenizing source...");
            var lexer = new Lexer (source);
            var lexemes = lexer.Scan ();
            Console.WriteLine ("Parsing context...");
            var pu = ParsingUnit.Create (lexemes);
            var parser = new Parser (pu);
            var ast = parser.Parse ();

            Console.WriteLine ("---- terminated");
            Console.ReadLine ();
        }
    }
}

using System;
using XenonCore;

namespace XenonInterpreter {
    class MainClass {
        public static void Main (string[] args) {
            const string source = @"
            fn main (a, b, c) {
                a = 0x1337
                b = ~1337
                c = 'hello, world'
                tpl = (1, 2, 3)
                lsta = []
                lstb = [1, 2, 3]
                test = something (tpl)
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

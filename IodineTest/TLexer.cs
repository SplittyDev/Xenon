using System;
using NUnit.Framework;
using IodineCore;
namespace XenonTest {
    
    [TestFixture]
    public class TLexer {

        [Test]
        public void TestA () {
            const string SOURCE = @"hello world";
            var lexer = new Lexer (SOURCE);
            var tokens = lexer.Scan ();
            var i = 0;
            Assert.AreEqual (tokens[i].Type, TokenClass.Identifier);
            Assert.AreEqual (tokens[i].Value, "hello");
            i++;
            Assert.AreEqual (tokens[i].Type, TokenClass.Identifier);
            Assert.AreEqual (tokens[i].Value, "world");
        }
    }
}


using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using VirtualPet.Modules.Game.Models;

namespace VirtualPet.Modules.Game.Tests.Models
{
    [TestClass]
    public class MethodsTests
    {
        [TestMethod]
        public void TestCheckS()
        {
            // Method should return "'s" if the string in question ends with a letter other than s, otherwise it should return "'"
            Assert.AreEqual("Ryan's", Methods.CheckS("Ryan"));
            Assert.AreEqual("Jess'", Methods.CheckS("Jess"));
        }

        [TestMethod]
        public void TestJoinWithAnd()
        {
            // See methods
            Assert.AreEqual("", Methods.JoinWithAnd(new List<string>()));
            Assert.AreEqual("a", Methods.JoinWithAnd(new List<string>() { "a" }));
            Assert.AreEqual("a and b", Methods.JoinWithAnd(new List<string>() { "a", "b" }));
            Assert.AreEqual("a, b, and c", Methods.JoinWithAnd(new List<string> { "a", "b", "c" }));
        }

        [TestMethod]
        public void TestCapitalise()
        {
            // Method should capitalise the first character of a string
            Assert.AreEqual("Hello world", Methods.Capitalise("hello world"));
        }

        [TestMethod]
        public void TestRandomChoice()
        {
            // Method should return a string from the list passed from it (exactly what string is randomly determined and therefore impossible to test)
            for (int i=0; i<50; i++)
            {
                Assert.IsTrue(new List<string>() { "a", "b", "c" }.Contains(Methods.RandomChoice(new List<string>() { "a", "b", "c" })));
            }
        }
    }
}

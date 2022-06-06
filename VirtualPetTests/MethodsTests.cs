using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace VirtualPetTests
{
    [TestClass]
    public class MethodsTests
    {
        [TestMethod]
        public void TestCheckS()
        {
            Assert.AreEqual("Fred's", Methods.CheckS("Fred"));
            Assert.AreEqual("Jess'", Methods.CheckS("Jess"));
        }

        [TestMethod]
        public void TestJoinWithAnd()
        {
            Assert.AreEqual("", Methods.JoinWithAnd(new List<string>()));
            Assert.AreEqual("Fred", Methods.JoinWithAnd(new List<string> { "Fred" }));
            Assert.AreEqual("Fred and Arthur", Methods.JoinWithAnd(new List<string> { "Fred", "Arthur" }));
            Assert.AreEqual("Fred, Arthur, and Mabel", Methods.JoinWithAnd(new List<string> { "Fred", "Arthur", "Mabel" }));
        }

        [TestMethod]
        public void TestCapitalise()
        {
            Assert.AreEqual("Hello world!", Methods.Capitalise("hello world!"));
        }
    }
}

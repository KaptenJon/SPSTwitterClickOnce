using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TwitterHostTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var service = new ServiceReference1.Service1Client("BasicHttpBinding_IService1");
            var t = service.GetTweets();
            Assert.IsFalse(t.Any());
        }
    }
}

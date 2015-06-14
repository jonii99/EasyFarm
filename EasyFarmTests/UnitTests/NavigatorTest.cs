using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EasyFarmTests
{
    [TestClass]
    public class NavigatorTest
    {
        [TestMethod]
        public void CreateNavigator()
        {
            Navigator navigator = new Navigator(new StubNavigtion());
            navigator.TravelNext();
        }
    }
}

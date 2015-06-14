using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EasyFarm.Navigation;

namespace EasyFarmTests.UnitTests
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

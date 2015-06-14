using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyFarmTests
{
    /// <summary>
    /// A stubbed navigation unit. 
    /// </summary>
    public class StubNavigtion : INavigation
    {
        public void Goto(Position position)
        {
            return;
        }
    }
}

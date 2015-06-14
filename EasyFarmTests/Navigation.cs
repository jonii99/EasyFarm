using FFACETools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyFarmTests
{
    /// <summary>
    /// Navigation unit that uses fface for navigation.
    /// </summary>
    public class Navigation : INavigation
    {
        /// <summary>
        /// The player's fface session. 
        /// </summary>
        private FFACE.NavigatorTools _navigator;

        /// <summary>
        /// Creates a new navigation unit. 
        /// </summary>
        /// <param name="fface"></param>
        public Navigation(FFACE.NavigatorTools navigator)
        {
            _navigator = navigator;
        }

        /// <summary>
        /// Travels to the given position. 
        /// </summary>
        /// <param name="position"></param>
        public void Goto(Position position)
        {
            _navigator.Goto((float)position.X, (float)position.Z, false);
        }
    }
}

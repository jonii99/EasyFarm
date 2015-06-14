using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyFarmTests
{
    /// <summary>
    /// Represents a route position. 
    /// </summary>
    public class Position
    {
        /// <summary>
        /// The player's x axis coordinate. 
        /// </summary>
        public double X { get; set; }
        
        /// <summary>
        /// The player's height. 
        /// </summary>
        public double Y { get; set; }
        
        /// <summary>
        /// The player's y axis coodinate. 
        /// </summary>
        public double Z { get; set; }
        /// <summary>
        /// The player's heading. 
        /// </summary>
        public double H { get; set; }
    }
}

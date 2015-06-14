using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyFarmTests
{
    public class NavigationModel
    {
        /// <summary>
        /// The route to travel. 
        /// </summary>
        public List<Position> Route { get; set; }

        /// <summary>
        /// The current position. 
        /// </summary>
        public Position Current { get; set; }

        public NavigationModel()
        {
            this.Route = new List<Position>();
        }

        /// <summary>
        /// Sets current to the next postion in the route. 
        /// </summary>
        public void MoveNext()
        {
            if (!Route.Any()) return;

            Current = Current ?? Route[0];

            // Set current to first if 
            if (Current == null)
            {
                Current = Route[0];
            }

            // Get the index for our current position. 
            var index = Route.IndexOf(Current);
            
            // Reverse route when reaching end. 
            if (index == Route.Count - 1) 
            {
                index = 0;
                Route.Reverse();
            }
            else // Move index to next position. 
            {
                index++;
            }

            // Set current to next position. 
            Current = Route[index];
        }
    }
}

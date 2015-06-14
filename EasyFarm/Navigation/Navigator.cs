using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyFarm.Navigation
{
    /// <summary>
    /// Controlls the player's navigation. 
    /// </summary>
    public class Navigator
    {    
        /// <summary>
        /// Holds the navigation data. 
        /// </summary>
        private NavigationModel _model;

        /// <summary>
        /// Navigates our charater. 
        /// </summary>
        private INavigation _navigation;

        /// <summary>
        /// Create a new navigator with the 
        /// given navigation unit. 
        /// </summary>
        /// <param name="navigation"></param>
        public Navigator(INavigation navigation)
        {
            _navigation = navigation;
            _model = new NavigationModel();
        }

        /// <summary>
        /// Travels to the next route position. 
        /// </summary>
        public void TravelNext()
        {
            _model.MoveNext();
            _navigation.Goto(_model.Current);
        }
    }
}
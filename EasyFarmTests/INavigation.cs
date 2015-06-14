using System;
namespace EasyFarmTests
{
    /// <summary>
    /// Inteface for navigating to positions. 
    /// </summary>
    public interface INavigation
    {
        /// <summary>
        /// Moves the character to this position. 
        /// </summary>
        /// <param name="position"></param>
        void Goto(Position position);
    }
}

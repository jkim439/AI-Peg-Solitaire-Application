using System;
using System.Collections.Generic;

namespace Peg_Solitaire
{
    /// <summary>
    /// Abstract class for game solving agents.
    /// All game solving agents should be derived from this class
    /// </summary>
    class Agent
    {
        private GameState gameState;
        private int totalExpandedStates;

        /// <summary>
        /// Stub for solve function of inherited classes
        /// </summary>
        /// <returns> Nested list containing a move sequence to a solution. </returns>
        public virtual List<List<List<int>>> Solve(bool isTimeout, DateTime timeout)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Stub Accessor function for expanded states
        /// </summary>
        /// <returns> Integer number of expanded states </returns>
        public virtual int getTotalExpandedStates()
        {
            throw new NotImplementedException();
        }
    }
}

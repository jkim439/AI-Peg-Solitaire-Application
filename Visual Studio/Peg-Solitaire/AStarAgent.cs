using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peg_Solitaire
{
    class AStarAgent : Agent
    {
        private GameState gameState;

        public AStarAgent(GameState startState)
        {
            gameState = startState;
        }

        public override List<List<List<int>>> Solve()
        {
            //leaving this blank for now
        }
    }
}

using System;
using System.Collections.Generic;

namespace Peg_Solitaire
{
    class QLearningAgent : Agent
    {
        private GameState gameState;
        private int totalExpandedStates; // Not used for a Q learning agent, but inherited from Agent class

        /// <summary>
        /// Constructor that sets up the agent for the given game start state
        /// </summary>
        /// <param name="startState"></param>
        public QLearningAgent(GameState startState)
        {
            gameState = startState;
            totalExpandedStates = 0;
        }

        /// <summary>
        /// Runs Q-learning until the time specified by the timeout parameter
        /// Once the time is reached, returns the best path found by using
        /// the learning sequence.
        /// </summary>
        /// <param name="isTimeout"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public override List<List<List<int>>> Solve(bool isTimeout, DateTime timeout)
        {
            GameState currentState = gameState;
            GameState nextState;
            List<List<int>> move;
            List<List<List<int>>> moveList = new List<List<List<int>>>();

            while (DateTime.Now <= timeout)
            {
                currentState = gameState;
                while(DateTime.Now <= timeout)
                {
                    move = currentState.GetMoveFromQValue();
                    nextState = currentState.NextState(move);
                    if(nextState.IsGoalState())
                    {
                        currentState.UpdateQvalue(move, 15);
                        break;
                    }
                    else if(nextState.NextMoves().Count == 0)
                    {
                        currentState.UpdateQvalue(move, 1-currentState.GetPegsLeft());
                        break;
                    }
                    else
                    {
                        currentState.UpdateQvalue(move, 1);
                    }
                    currentState = nextState;
                }
            }
            currentState = gameState;
            while(!currentState.IsGoalState() && currentState.NextMoves().Count != 0)
            {
                List<List<int>> bestMove = currentState.GetBestQMove();
                moveList.Add(bestMove);
                currentState = currentState.NextState(bestMove);
            }
            return moveList;
        }

        /// <summary>
        /// Override required for virtual function in Agent class, but not used.
        /// </summary>
        /// <returns>number of expanded states (0)</returns>
        public override int getTotalExpandedStates()
        {
            return totalExpandedStates;
        }

    }
}

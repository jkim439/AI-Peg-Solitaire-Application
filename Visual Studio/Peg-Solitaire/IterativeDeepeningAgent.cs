using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peg_Solitaire
{
    class IterativeDeepeningAgent : Agent
    {
        private GameState gameState;
        private int totalExpandedStates;

        /// <summary>
        /// Constructor that sets up the agent for the given game start state
        /// </summary>
        /// <param name="startState"></param>
        public IterativeDeepeningAgent(GameState startState)
        {
            gameState = startState;
            totalExpandedStates = 0;
        }

        /// <summary>
        /// Attempts to solve the game state using an iterative deepening algorithm.
        /// If a solution is found, the move sequence to the solution is returned.
        /// Each move is a list of coordinate pairs formatted as follows:
        /// The first pair is the row and column of the peg to be moved
        /// The second pair is the row and column of the peg to be jumped
        /// The third pair is the row and column of the new peg location
        /// Example output:
        /// [[[2,0],[1,0],[0,0]],[[2,2],[1,1],[0,0]]...]
        /// If no solution is found, a no solution exception is thrown.
        /// </summary>
        /// <returns> Nexted list containing a move sequence to a solution. </returns>
        public override List<List<List<int>>> Solve(bool isTimeout, DateTime timeout)
        {
            Stack<List<List<int>>> moveStack = new Stack<List<List<int>>>();
            Stack<GameState> stateStack = new Stack<GameState>();
            List<GameState> expandedStates = new List<GameState>();
            List<GameState> usedStates = new List<GameState>();
            List<GameState> children;
            List<List<GameState>> stateChildren = new List<List<GameState>>();
            GameState top;
            List<List<int>> moveTop;
            List<List<List<int>>> returnList = new List<List<List<int>>>();
            int depthLimit = 1;
            stateStack.Push(gameState);
            moveStack.Push(new List<List<int>>());
            bool done = false;

            while (!done)
            {
                depthLimit++;
                stateStack.Clear();
                moveStack.Clear();
                expandedStates.Clear();
                usedStates.Clear();
                stateChildren.Clear();
                returnList.Clear();
                stateStack.Push(gameState);
                moveStack.Push(new List<List<int>>());
                while (moveStack.Count < depthLimit)
                {
                    if (DateTime.Now >= timeout)
                        throw new Exception("Search for solution timed out.");

                    if(stateStack.Count <= 0)
                    {
                        done = true;
                        break;
                    }
                    top = stateStack.Pop();
                    moveTop = moveStack.Pop();

                    if (top.IsGoalState())
                    {
                        stateStack.Push(top);
                        moveStack.Push(moveTop);
                        done = true;
                        break;
                    }

                    if (!expandedStates.Contains(top))
                    {
                        children = top.GetSuccessors();
                        totalExpandedStates++;
                        expandedStates.Add(top);
                        stateChildren.Add(children);
                    }
                    else
                    {
                        children = stateChildren[expandedStates.IndexOf(top)];
                    }

                    foreach (GameState indivChild in children)
                    {
                        if (!usedStates.Contains(indivChild))
                        {
                            stateStack.Push(top);
                            moveStack.Push(moveTop);
                            stateStack.Push(indivChild);
                            moveStack.Push(top.NextMoves()[children.IndexOf(indivChild)]);
                            usedStates.Add(indivChild);
                            break;
                        }
                    }
                }
            }
            if (stateStack.Count == 0)
                throw new Exception("No solution exists for this game.");
            while (moveStack.Count > 1)
            {
                returnList.Insert(0, moveStack.Pop());
            }
            return returnList;
        }

        /// <summary>
        /// Accessor function for the number of expanded states
        /// </summary>
        /// <returns>Number of expanded states</returns>
        public override int getTotalExpandedStates()
        {
            return totalExpandedStates;
        }
    }
}

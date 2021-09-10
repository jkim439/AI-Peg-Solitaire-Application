using System;
using System.Collections.Generic;

namespace Peg_Solitaire
{
    /// <summary>
    /// Agent that uses breadth-first search to solve puzzles
    /// </summary>
    class BreadthFirstAgent : Agent
    {
        private GameState gameState;
        private int totalExpandedStates;

        /// <summary>
        /// Constructor that sets up the agent for the given game start state
        /// </summary>
        /// <param name="startState"></param>
        public BreadthFirstAgent(GameState startState)
        {
            gameState = startState;
            totalExpandedStates = 0;
        }

        /// <summary>
        /// Attempts to solve the game state using a breadth-first search algorithm.
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
            Queue<List<List<List<int>>>> moveQueue = new Queue<List<List<List<int>>>>();
            Queue<GameState> stateQueue = new Queue<GameState>();
            List<GameState> explored = new List<GameState>();
            List<GameState> frontier = new List<GameState>();
            List<List<List<int>>> returnList = new List<List<List<int>>>();
            GameState frontState;
            List<GameState> children;
            List<List<List<int>>> frontMoveList;
            List<List<List<int>>> nextMoveList;

            if (gameState.IsGoalState())
                return new List<List<List<int>>>();
            stateQueue.Enqueue(gameState);
            moveQueue.Enqueue(new List<List<List<int>>>());
            while (stateQueue.Count > 0)
            {
                if (DateTime.Now >= timeout)
                    throw new Exception("Search for solution timed out.");
                frontState = stateQueue.Dequeue();
                frontMoveList = moveQueue.Dequeue();
                explored.Add(frontState);
                if(frontState.IsGoalState())
                {
                    return frontMoveList;
                }
                children = frontState.GetSuccessors();
                totalExpandedStates++;
                foreach (GameState indivChild in children)
                {
                    if((!explored.Contains(indivChild)) && (!frontier.Contains(indivChild)))
                    {
                        stateQueue.Enqueue(indivChild);
                        nextMoveList = new List<List<List<int>>>(frontMoveList)
                        {
                            frontState.NextMoves()[children.IndexOf(indivChild)]
                        };
                        moveQueue.Enqueue(nextMoveList);
                        frontier.Add(indivChild);
                    }
                }
            }
            throw new Exception("No solution exists for this game.");
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

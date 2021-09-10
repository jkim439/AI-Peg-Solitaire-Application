using System;
using System.Collections.Generic;

namespace Peg_Solitaire
{
    /// <summary>
    /// Object containing a full representation of a peg-solitaire
    /// game in its current state with a map of peg locations,
    /// a count of the pegs, and any additional constraints.
    /// Also contains important functions to evalueate goal states
    /// and determine valid moves and next states.
    /// </summary>
    class GameState
    {
        // Number of remaining pegs
        private readonly int pegsLeft;
        // Boolean map of peg locations
        private List<List<bool>> pegLocations;
        // Flag set to true if specific final peg location required
        private readonly bool hasGoalLoc;
        // Coordinates of final peg location
        private readonly List<int> goalLoc;
        // Q-Learning rate
        double alpha = 0.4;
        // Q-Learning future discount
        double gamma = 0.9;
        // List of actions that have Q values
        private List<List<List<int>>> QMoves;
        // Q Values corresponding to actions
        private List<double> QValues;

        /// <summary>
        /// Constructor for a basic peg-solitaire game with no extra constraints
        /// Takes a peg map of starting peg locations as an argument.
        /// The peg map 2D list of rows and columns of boolean values.
        /// True indicates peg, false indicates an empty hole.
        /// </summary>
        /// <param name="startingPegs">2D list of bools indicating peg locations.</param>
        public GameState(List<List<bool>> startingPegs)
        {
            pegLocations = startingPegs;
            hasGoalLoc = false;
            goalLoc = new List<int> { 0, 0 };
            pegsLeft = 0;
            foreach (List<bool> pegRow in pegLocations)
            {
                foreach (bool peg in pegRow)
                {
                    if (peg)
                    {
                        pegsLeft++;
                    }
                }
            }
            QMoves = new List<List<List<int>>>();
            QValues = new List<double>();
        }

        /// <summary>
        /// Returns a boolean value indicating if the state is a goal state
        /// </summary>
        /// <returns> bool true if goal state, false otherwise </returns>
        public bool IsGoalState()
        {
            if(!hasGoalLoc)
            {
                return pegsLeft == 1;
            }
            else
            {
                return (pegsLeft == 1) && pegLocations[goalLoc[0] - 1][goalLoc[1] - 1];
            }
        }

        /// <summary>
        /// Accessor function for remaining pegs
        /// </summary>
        /// <returns>number of remaining pegs</returns>
        public int GetPegsLeft()
        {
            return pegsLeft;
        }

        /// <summary>
        /// Returns a list of all valid moves from the current state.
        /// Each valid move is a list of coordinate pairs formatted as follows:
        /// The first pair is the row and column of the peg to be moved
        /// The second pair is the row and column of the peg to be jumped
        /// The third pair is the row and column of the new peg location
        /// Example output of two valid moves:
        /// [[[2,0],[1,0],[0,0]],[[2,2],[1,1],[0,0]]]
        /// </summary>
        /// <returns> All possible move sets in nested lists. </returns>
        public List<List<List<int>>> NextMoves()
        {
            int row = 0;
            int col = 0;
            List<List<List<int>>> movesList = new List<List<List<int>>>();

            foreach (List<bool> pegRow in pegLocations)
            {
                foreach (bool peg in pegRow)
                {
                    if (peg)
                    {
                        // Check move up and left
                        if (row >= 2 && col >= 2)
                        {
                            if (pegLocations[row - 1][col - 1] && !pegLocations[row - 2][col - 2])
                            {
                                movesList.Add( new List<List<int>> { new List<int> { row, col },
                                    new List<int> { row - 1, col - 1 }, new List<int> { row - 2, col - 2 } });
                            }
                        }
                        // Check move up and right
                        if (row >= 2 && col < pegLocations[row-2].Count)
                        {
                            if (pegLocations[row - 1][col] && !pegLocations[row - 2][col])
                            {
                                movesList.Add(new List<List<int>> { new List<int> { row, col },
                                    new List<int> { row - 1, col }, new List<int> { row - 2, col } });
                            }
                        }
                        // Check move left
                        if (col >= 2)
                        {
                            if (pegLocations[row][col - 1] && !pegLocations[row][col - 2])
                            {
                                movesList.Add(new List<List<int>> { new List<int> { row, col },
                                    new List<int> { row, col - 1 }, new List<int> { row, col - 2 } });
                            }
                        }
                        // Check move right
                        if (col + 2 < pegRow.Count)
                        {
                            if (pegLocations[row][col + 1] && !pegLocations[row][col + 2])
                            {
                                movesList.Add(new List<List<int>> { new List<int> { row, col },
                                    new List<int> { row, col + 1 }, new List<int> { row, col + 2 } });
                            }
                        }
                        // Check moves down and left and down and right
                        if (row + 2 < pegLocations.Count)
                        {
                            if (pegLocations[row + 1][col] && !pegLocations[row + 2][col])
                            {
                                movesList.Add(new List<List<int>> { new List<int> { row, col },
                                    new List<int> { row + 1, col }, new List<int> { row +2, col } });
                            }
                            if (pegLocations[row + 1][col+1] && !pegLocations[row + 2][col+2])
                            {
                                movesList.Add(new List<List<int>> { new List<int> { row, col },
                                    new List<int> { row + 1, col +1 }, new List<int> { row +2, col +2} });
                            }
                        }
                    }
                    col++;
                }
                col = 0;
                row++;
            }
            return movesList;
        }

        /// <summary>
        /// Builds and returns the next state child that results from performing
        /// the action in the parameter next move, from the current state.
        /// </summary>
        /// <param name="nextMove"> List of coordinates for 1 move from NextMoves list</param>
        /// <returns> Gamestate object for next state</returns>
        public GameState NextState(List<List<int>> nextMove)
        {
            List<List<bool>> newPegLocations = new List<List<bool>>();
            foreach (List<bool> row in pegLocations)
            {
                newPegLocations.Add(new List<bool>(row));
            }
            string exceptString;

            // Check to make sure nextMove is valid
            if (nextMove[0][0] >= pegLocations.Count || nextMove[0][1] >= pegLocations[nextMove[0][0]].Count)
                throw new Exception("Invalid Starting Peg");
            if (nextMove[1][0] >= pegLocations.Count || nextMove[1][1] >= pegLocations[nextMove[1][0]].Count)
                throw new Exception("Invalid Middle Peg");
            if (nextMove[2][0] >= pegLocations.Count || nextMove[2][1] >= pegLocations[nextMove[2][0]].Count)
                throw new Exception("Invalid Final Peg Location");
            if (pegLocations[nextMove[0][0]][nextMove[0][1]] == false)
            {
                exceptString = string.Format("Invalid Move: No peg to move from ({0},{1})", nextMove[0][0], nextMove[0][1]);
                throw new Exception(exceptString);
            }
            if (pegLocations[nextMove[1][0]][nextMove[1][1]] == false)
            {
                exceptString = string.Format("Invalid Move: No peg to jump at ({0},{1})", nextMove[1][0], nextMove[1][1]);
                throw new Exception(exceptString);
            }
            if (pegLocations[nextMove[2][0]][nextMove[2][1]] == true)
            {
                exceptString = string.Format("Invalid Move: ({0},{1}) already occupied", nextMove[2][0], nextMove[2][1]);
                throw new Exception(exceptString);
            }

            // Apply move to peg locations
            newPegLocations[nextMove[0][0]][nextMove[0][1]] = false;
            newPegLocations[nextMove[1][0]][nextMove[1][1]] = false;
            newPegLocations[nextMove[2][0]][nextMove[2][1]] = true;

            // Return the new game state after the move
            return new GameState(newPegLocations);
        }

        /// <summary>
        /// Expands the current state, generating the children of all
        /// valid moves from the current state, returning them in a list.
        /// Combines NextMoves and NextState functions.
        /// </summary>
        /// <returns> List of Gamestates containing all possible state children. </returns>
        public List<GameState> GetSuccessors()
        {
            List<List<List<int>>> validMoves = NextMoves();
            List<GameState> successorList = new List<GameState>();

            foreach (List<List<int>> move in validMoves)
            {
                successorList.Add(NextState(move));
            }
            return successorList;
        }

        /// <summary>
        /// Returns the Q-value for the given action
        /// in the current state
        /// </summary>
        /// <param name="move"></param>
        /// <returns>Q-value</returns>
        public double GetQValue(List<List<int>> move)
        {
            int qIndex;
            bool hasValue = FindQValueIndex(move, out qIndex);
            if (hasValue)
                return QValues[qIndex];
            else return 0;
        }

        /// <summary>
        /// Returns the highest Q-value out of all
        /// possible actions from the current state
        /// </summary>
        /// <returns>Highest Q-value</returns>
        public double GetMaxQValue()
        {
            List<List<List<int>>> validMoves = NextMoves();
            bool first = true;
            double currQVal = 0, maxQVal = 0;
            foreach (List<List<int>> move in validMoves)
            {
                currQVal = GetQValue(move);
                if (first || currQVal > maxQVal)
                {
                    maxQVal = currQVal;
                    first = false;
                }
            }
            return maxQVal;
        }

        /// <summary>
        /// Updates the Q-value for the input action
        /// from the current state based on the input
        /// reward received from the action.
        /// </summary>
        /// <param name="move"></param>
        /// <param name="reward"></param>
        public void UpdateQvalue(List<List<int>> move, double reward)
        {
            int qIndex;
            bool hasValue = FindQValueIndex(move, out qIndex);
            if (hasValue)
            {
                QValues[qIndex] = QValues[qIndex] + alpha * (reward + gamma * NextState(move).GetMaxQValue() - QValues[qIndex]);
            }
            else
            {
                QMoves.Add(move);
                QValues.Add(alpha * (reward + gamma * NextState(move).GetMaxQValue()));
            }
        }

        /// <summary>
        /// Uses Q values to provide the next action to be taken.
        /// Includes a 3% exploration rate
        /// </summary>
        /// <returns>next move to be taken</returns>
        public List<List<int>> GetMoveFromQValue()
        {
            double currQVal = 0;
            double maxQVal = GetMaxQValue();
            Random random = new Random();
            List<List<List<int>>> maxMoves = new List<List<List<int>>>();

            List<List<List<int>>> validMoves = NextMoves();

            if (validMoves.Count == 0)
                return new List<List<int>>();

            if (random.Next(100) < 3)
                return validMoves[random.Next(validMoves.Count - 1)];

            foreach (List<List<int>> move in validMoves)
            {
                currQVal = GetQValue(move);
                if (currQVal >= maxQVal)
                {
                    maxMoves.Add(move);
                }
            }
            return maxMoves[random.Next(maxMoves.Count - 1)];
        }

        /// <summary>
        /// Returns the next move based on Q values without exploration
        /// </summary>
        /// <returns>next move to be taken</returns>
        public List<List<int>> GetBestQMove()
        {
            double currQVal = 0;
            double maxQVal = GetMaxQValue();
            Random random = new Random();
            List<List<List<int>>> maxMoves = new List<List<List<int>>>();

            List<List<List<int>>> validMoves = NextMoves();

            if (validMoves.Count == 0)
                return new List<List<int>>();

            foreach (List<List<int>> move in validMoves)
            {
                currQVal = GetQValue(move);
                if (currQVal >= maxQVal)
                {
                    maxMoves.Add(move);
                }
            }
            return maxMoves[random.Next(maxMoves.Count - 1)];
        }
        
        /// <summary>
        /// Finds the index of the Q value for the given move. Saves the index in the
        /// out qIndex parameter. Returns true if successful or false there is no
        /// Q value saved for the specified move.
        /// </summary>
        /// <param name="move"></param>
        /// <param name="qIndex"></param>
        /// <returns></returns>
        private bool FindQValueIndex(List<List<int>> move, out int qIndex)
        {
            int index = 0;
            foreach (List<List<int>> QMove in QMoves)
            {
                if (QMove[0][0] == move[0][0] && QMove[0][1] == move[0][1] && QMove[2][0] == move[2][0] && QMove[2][1] == move[2][1])
                {
                    qIndex = index;
                    return true;
                }
                index++;
            }
            qIndex = -1;
            return false;
        }
    }
}

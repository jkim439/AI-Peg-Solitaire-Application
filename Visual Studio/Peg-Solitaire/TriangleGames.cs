using System.Collections.Generic;

namespace Peg_Solitaire
{
    /// <summary>
    /// Class of public static functions that can be called to build
    /// GameState objects for triangle peg-solitaire games.
    /// </summary>
    class TriangleGames
    {
        /// <summary>
        /// Builds and returns a basic triangle peg-solitaire game with the open hole
        /// at the top and no extra constraints. Accepts one parameter specifying the
        /// number of rows in the triangle.
        /// </summary>
        /// <param name="numRows"> integer number of rows or triangle side length.</param>
        /// <returns>Valid starting game state of a triangle peg-solitaire game.</returns>
        public static GameState BasicTriangle(int numRows)
        {
            List<bool> firstRow = new List<bool> { false };
            List<bool> currentRow = new List<bool> { true, true };
            List<List<bool>> pegMap = new List<List<bool>> { };

            pegMap.Add(new List<bool> (firstRow));

            for(int i = 0; i < numRows-1; i++)
            {
                pegMap.Add(new List<bool> (currentRow));
                currentRow.Add(true);
            }

            return new GameState(pegMap);
        }
    }
}

using System;

namespace GameOfLife
{
    /// <summary>
    /// Represents a generation of Conway's Game of Life.
    /// </summary>
    public class GameOfLife
    {
        #region Attributes
        /// <summary>
        /// The current cell configuration.
        /// </summary>
        protected bool[,] Cells { get; }
        /// <summary>
        /// Determines whether the cells wrap around at the end of the board.
        /// </summary>
        public bool DoWrap { get; set; }
        /// <summary>
        /// The width of the board on the x axis.
        /// </summary>
        public int XWidth => Cells.GetLength(0);
        /// <summary>
        /// The width of the board on the y axis.
        /// </summary>
        public int YWidth => Cells.GetLength(1);
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new GameOfLife with the given cell configuration.
        /// </summary>
        /// <param name="cells">The cell configuration.</param>
        public GameOfLife(bool[,] cells)
        {
            Cells = cells;
            DoWrap = true;
        }
        /// <summary>
        /// Creates a new GameOfLife with the given cell configuration.
        /// </summary>
        /// <param name="cells">The cell configuration.</param>
        /// <param name="doWrap">Determines whether the cells wrap around at the end of the board.</param>
        public GameOfLife(bool[,] cells, bool doWrap)
        {
            Cells = cells;
            DoWrap = doWrap;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets the cell at the given coordinates.
        /// </summary>
        /// <param name="x">The x coordinate of the cell.</param>
        /// <param name="y">The y coordinate of the cell.</param>
        /// <returns><c>true</c>, if the cell is alive, else <c>false</c>.</returns>
        public bool GetCell(int x, int y)
        {
            // Wrap the cell coordinate
            int xMod = ((x % XWidth) + XWidth) % XWidth;
            int yMod = ((y % YWidth) + YWidth) % YWidth;

            // If the cell is outside the board and wrapping is disabled, the cell is dead.
            if (!DoWrap && ((x != xMod) || (y != yMod)))
                return false;

            return Cells[xMod, yMod];
        }

        /// <summary>
        /// Gets the neighboring cells for the cell at the given coordinates.
        /// </summary>
        /// <param name="x">The x coordinate of the cell.</param>
        /// <param name="y">The y coordinate of the cell.</param>
        /// <returns>The neighboring cells.</returns>
        public bool[] GetCellNeighbors(int x, int y)
        {
            return new bool[8]
            {
                GetCell(x - 1, y - 1),
                GetCell(x - 1, y),
                GetCell(x - 1, y + 1),

                GetCell(x, y - 1),
                GetCell(x, y + 1),

                GetCell(x + 1, y - 1),
                GetCell(x + 1, y),
                GetCell(x + 1, y + 1),
            };
        }

        /// <summary>
        /// Gets the number of living neighboring cells.
        /// </summary>
        /// <param name="x">The x coordinate of the cell.</param>
        /// <param name="y">The y coordinate of the cell.</param>
        /// <returns>The number of living neighboring cells.</returns>
        public int GetAliveNeighborCount(int x, int y)
        {
            int count = 0;

            foreach(var cell in GetCellNeighbors(x, y))
            {
                if (cell)
                    count++;
            }

            return count;
        }

        /// <summary>
        /// Gets the state of the given cell in the next generation.
        /// </summary>
        /// <param name="x">The x coordinate of the cell.</param>
        /// <param name="y">The y coordinate of the cell.</param>
        /// <returns><c>true</c>, if the cell is alive in the next generation, else <c>false</c>.</returns>
        public bool GetNextCell(int x, int y)
        {
            int aliveCount = GetAliveNeighborCount(x, y);
 
            if (GetCell(x, y))
            {
                // The cell is currently alive.
                // If it has less than 2 living neighbors, it dies through underpopulation.
                if (aliveCount < 2)
                    return false;
                // If it has more than 3 living neighbors, it dies through overpopulation.
                if (aliveCount > 3)
                    return false;
                // Otherwise, it survives.
                return true;
            }
            // The cell is currently dead.
            // If it has 3 living neighbors, it becomes alive through reproduction.
            if (aliveCount == 3)
                return true;
            // Otherwise, it remains dead.
            return false;
        }

        /// <summary>
        /// Gets the next generation of the game.
        /// </summary>
        /// <returns>The next generation of the game.</returns>
        public GameOfLife GetNextGeneration()
        {
            bool[,] nextGeneration = new bool[XWidth, YWidth];

            for (int xIndex = 0; xIndex < XWidth; xIndex++)
            {
                for (int yIndex = 0; yIndex < YWidth; yIndex++)
                {
                    nextGeneration[xIndex, yIndex] = GetNextCell(xIndex, yIndex);
                }
            }

            return new GameOfLife(nextGeneration);
        }
        #endregion
    }
}

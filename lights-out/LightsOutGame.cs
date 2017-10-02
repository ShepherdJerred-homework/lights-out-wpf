using System;

namespace lights_out {
    internal class LightsOutGame {
        private bool[,] grid; // Stores on/off state of cells in g
        private int numCells = 3; // Number of cells in grid
        private readonly Random rand; // Used to generate random numbers

        public LightsOutGame() {
            rand = new Random(); // Initialize random number generator
            grid = new bool[numCells, numCells];
            // Turn entire grid on
            for (var r = 0; r < numCells; r++) {
                for (var c = 0; c < numCells; c++) {
                    grid[r, c] = true;
                }
            }
        }

        // Returns the number of horizontal/vertical cells in the grid
        public int NumCells {
            get => numCells;
            set { numCells = value; }
        }

        // Returns the grid value at the given row and column
        public bool GetGridValue(int row, int col) {
            return grid[row, col];
        }

        // Randomizes the grid
        public void NewGame() {
            grid = new bool[numCells, numCells];
            // Fill grid with either white or black
            for (var r = 0; r < numCells; r++) {
                for (var c = 0; c < numCells; c++) {
                    grid[r, c] = rand.Next(2) == 1;
                }
            }
        }

        // Inverts the selected box and all surrounding boxes
        public void MakeMove(int row, int col) {
            for (var i = row - 1; i <= row + 1; i++) {
                for (var j = col - 1; j <= col + 1; j++) {
                    if (i >= 0 && i < numCells && j >= 0 && j < numCells) {
                        grid[i, j] = !grid[i, j];
                    }
                }
            }
        }

        // Returns true if all cells are off
        public bool PlayerWon() {
            for (var r = 0; r < numCells; r++) {
                for (var c = 0; c < numCells; c++) {
                    if (grid[r, c]) {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
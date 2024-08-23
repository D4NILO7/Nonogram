using System;

namespace NonogramCore.Core
{
    [Serializable]
    public class Field
    {
        private Tile[,] _tiles;

        private Difficulty _difficulty;
        public int RowCount { get; }
        public int ColumnCount { get; }

        private ActionState _actionState = ActionState.Exclude;

        private readonly DateTime _startTime;

        public Field(Difficulty fieldDifficulty)
        {
            _difficulty = fieldDifficulty;
            RowCount = GetFieldStructure().GetLength(0);
            ColumnCount = GetFieldStructure().GetLength(1);
            ActionState = _actionState;

            _tiles = new Tile[RowCount, ColumnCount];
            Initialize();
            _startTime = DateTime.Now;
        }


        public ActionState ActionState
        {
            get { return _actionState; }
            set { _actionState = value; }
        }

        /// <summary>
        /// Fills the 2D Array with Tiles. Creates the field.
        /// The default tile state is Plain. 
        /// </summary>
        public void Initialize()
        {
            for (var row = 0; row < RowCount; row++)
            {
                for (var column = 0; column < ColumnCount; column++)
                {
                    _tiles[row, column] = new Tile();
                    _tiles[row, column].State = TileState.Plain;
                }
            }
        }

        /// <summary>
        /// Returns Tile from a specific position defined by index of the row and column.
        /// </summary>
        /// <param name="row"> index of the row</param>
        /// <param name="column">index of the column</param>
        /// <returns>Tile</returns>
        public Tile GetTile(int row, int column)
        {
            return _tiles[row, column];
        }

        /// <summary>
        /// Changes the state of the tile on a specific position.
        /// From Plain to Colored and back or from Excluded to Colored.
        /// </summary>
        /// <param name="row">row index of the tile</param>
        /// <param name="column">column index of th tile</param>
        public void ColorTile(int row, int column)
        {
            if (row < 0 || row >= RowCount || column < 0 || column >= ColumnCount)
            {
                return;
            }

            Tile tile = GetTile(row, column);
            if (tile.State == TileState.Plain || tile.State == TileState.Excluded)
            {
                tile.State = TileState.Colored;
            }
            else
            {
                tile.State = TileState.Plain;
            }
        }

        /// <summary>
        /// Changes the state of the tile on a specific position.
        /// From Plain to Excluded and back or from Colored to Excluded
        /// </summary>
        /// <param name="row">row index of the tile</param>
        /// <param name="column">column index of th tile</param>
        public void ExcludeTile(int row, int column)
        {
            if (row < 0 || row >= RowCount || column < 0 || column >= ColumnCount)
            {
                return;
            }

            Tile tile = GetTile(row, column);
            if (tile.State == TileState.Plain || tile.State == TileState.Colored)
            {
                tile.State = TileState.Excluded;
            }
            else
            {
                tile.State = TileState.Plain;
            }
        }

        /// <summary>
        /// Checks the field based on the field structure
        /// </summary>
        /// <returns>true if the field is solved, false if not</returns>
        public bool IsSolved()
        {
            for (var row = 0; row < RowCount; row++)
            {
                for (var column = 0; column < ColumnCount; column++)
                {
                    var tile = _tiles[row, column];
                    var field = GetFieldStructure();

                    switch (field[row, column])
                    {
                        case 1 when (tile.State != TileState.Colored):
                        case 0 when (tile.State != TileState.Excluded):
                            return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Calculates the actual score based on time and difficulty.
        /// </summary>
        /// <returns>actual score</returns>
        public int GetScore()
        {
            var minutesPassed = 60 * (DateTime.Now - _startTime).Minutes;
            var secondsPassed = (DateTime.Now - _startTime).Seconds;
            return RowCount * ColumnCount * 10 - (minutesPassed + secondsPassed);
        }

        /// <summary>
        /// Chooses the array based on difficulty and returns it.
        /// Based on this array is the field initialized.
        /// </summary>
        /// <returns>2D array</returns>
        public int[,] GetFieldStructure()
        {
            if (_difficulty == Difficulty.Easy)
            {
                return _easyField;
            }

            if (_difficulty == Difficulty.Medium)
            {
                return _mediumField;
            }

            return _hardField;
        }

        public int[] CountInRow(Field field, int row)
        {
            int[] result = new int[field.ColumnCount];
            var indexInResult = 0;
            var consecutiveInRow = 0;
            var fieldStructure = field.GetFieldStructure();

            for (var column = 0; column < field.ColumnCount; column++)
            {
                if ((fieldStructure[row, column] == 0 || column == field.ColumnCount - 1))
                {
                    if (column == field.ColumnCount-1 && fieldStructure[row, column] == 1)
                    {
                        consecutiveInRow++;
                    }

                    if (consecutiveInRow > 0)
                    {
                        result[indexInResult] = consecutiveInRow;
                        consecutiveInRow = 0;
                        indexInResult++;
                    }
                }
                else if(fieldStructure[row, column] == 1)
                {
                    consecutiveInRow++;
                }
            }

            return result;
        }

        public int[] CountInColumn(Field field, int column)
        {
            int[] result = new int[field.RowCount];
            var indexInResult = 0;
            var consecutiveInCol = 0;
            var fieldStructure = field.GetFieldStructure();

            for (var row = 0; row < field.RowCount; row++)
            {
                if ((fieldStructure[row, column] == 0 || row == field.RowCount - 1))
                {
                    if (row == field.RowCount - 1 && fieldStructure[row, column] == 1)
                    {
                        consecutiveInCol++;
                    }

                    if (consecutiveInCol > 0)
                    {
                        result[indexInResult] = consecutiveInCol;
                        consecutiveInCol = 0;
                        indexInResult++;
                    }
                }
                else if (fieldStructure[row, column] == 1)
                {
                    consecutiveInCol++;
                }
            }

            return result;
        }

        /// <summary>
        /// Based on these arrays is the field initialized.
        /// </summary>
        private readonly int[,] _easyField = new int[,]
        {
            {1, 1, 1},
            {0, 0, 0},
            {1, 1, 1},
        };

        private readonly int[,] _mediumField = new int[,]
        {
            {1, 1, 0, 1, 1},
            {0, 1, 0, 1, 1},
            {1, 0, 1, 0, 1},
            {0, 1, 0, 1, 1},
            {1, 0, 1, 1, 0}
        };

        private readonly int[,] _hardField = new int[,]
        {
            {0, 1, 1, 0, 0, 0, 0, 1, 1, 0},
            {0, 0, 0, 1, 0, 0, 1, 0, 0, 0},
            {0, 0, 1, 1, 1, 1, 1, 1, 0, 0},
            {0, 1, 1, 1, 1, 1, 1, 1, 1, 0},
            {1, 1, 1, 0, 1, 1, 0, 1, 1, 1},
            {1, 0, 1, 1, 1, 1, 1, 1, 0, 1},
            {1, 0, 1, 1, 1, 1, 1, 1, 0, 1},
            {0, 0, 1, 0, 0, 0, 0, 1, 0, 0},
            {0, 0, 0, 1, 0, 0, 1, 0, 0, 0}
        };
    }
}
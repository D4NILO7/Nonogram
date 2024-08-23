using System;
using System.Text.RegularExpressions;
using NonogramCore.Core;
using NonogramCore.Entity;
using NonogramCore.Service;

namespace NonogramConsole
{
    public class ConsoleUI
    {
        private readonly Regex _regCommand = new Regex("^([CE]{1})([A-Z]{1})([0-9]{1,2})$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private readonly Regex _regDifficulty = new Regex("^([EHM]{1})$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private readonly Regex _regQuestion = new Regex("^([YN]{1})$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private Field _field;
        private string _player;
        //private readonly IScoreService _scoreService = new ScoreServiceFile();
        private readonly IScoreService _scoreService = new ScoreServiceEF();
        //private readonly IRatingsService _ratingsService = new RatingServiceFile();
        private readonly IRatingsService _ratingsService = new RatingServiceEF();
        //private readonly ICommentsService _commentsService = new CommentServiceFile();
        private readonly ICommentsService _commentsService = new CommentServiceEF();

        public void Play()
        {
            Console.WriteLine("Welcome to the game NONOGRAM. Good luck, have fun!");
            ReadPlayerName();
            ShowTopScorersAndRatings();
            CreateField();
            do
            {
                Console.Clear();
                PrintField();
                ProcessInput();

            } while (!_field.IsSolved());

            _scoreService.AddScore(new Score { Player = _player, Points = _field.GetScore(), PlayedAt = DateTime.Now });
            Console.Clear();
            PrintField();
            Console.WriteLine();
            Console.WriteLine("Congratulation...game solved. Your prize is the experience");
            Console.WriteLine();
            RateTheGame();
            WriteComment();
            ShowComments();
            Console.WriteLine("Thank you for playing!");
        }

        private void ProcessInput()
        {
            Console.WriteLine();
            Console.Write("Enter command (Color-CA1, Exclude-EB2):");
            string line = Console.ReadLine().ToUpper();
            var match = _regCommand.Match(line);

            if (match.Success)
            {
                var row = (match.Groups[2].ToString())[0] - 'A';
                var column = int.Parse(match.Groups[3].ToString()) - 1;

                if (line.StartsWith("C"))
                {
                    _field.ColorTile(row, column);
                }
                else if (line.StartsWith("E"))
                {
                    _field.ExcludeTile(row, column);
                }
            }
            else
            {
                Console.WriteLine("Wrong input:" + line);
            }

        }

        private void PrintField()
        {
            CluesInColumn();
            Console.Write("  ");
            for (var column = 0; column < 2 * _field.ColumnCount; column++)
            {
                Console.Write("==");
            }

            Console.WriteLine();
            for (var row = 0; row < _field.RowCount; row++)
            {
                Console.Write((char) (row + 'A') + " ");
                for (var column = 0; column < _field.ColumnCount; column++)
                {
                    var tile = _field.GetTile(row, column);
                    switch (tile.State)
                    {
                        case TileState.Colored:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("{0,2}", "■");
                            Console.ResetColor();
                            break;
                        case TileState.Excluded:
                            Console.Write("{0,2}", "X");
                            break;
                        case TileState.Plain:
                            Console.Write("{0,2}", "-");
                            break;
                        default:
                            throw new ArgumentOutOfRangeException($"TileState", "Unsupported tile state");
                    }

                    Console.Write("  ");
                }
                CluesInRow(row);
                Console.WriteLine();
            }

            for (var column = 0; column < _field.ColumnCount; column++)
            {
                Console.Write("  ");
                Console.Write("{0,2}", column + 1);
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Actual score: {0,2}", _field.GetScore());
        }

        /// <summary>
        /// Counts the consecutive ones in the field structure (in each column)
        /// and based on it prints the clues on the top of the table.
        /// </summary>
        private void CluesInColumn()
        {
            var fieldStructure = _field.GetFieldStructure();
            var index = 0;
            var rowIndex = _field.RowCount - 1;
            var numberOfClues = 0;
            int[,] clues = new int[(_field.RowCount + 1) / 2, _field.ColumnCount];

            for (var column = 0; column < _field.ColumnCount; column++)
            {
                while (rowIndex >= 0)
                {
                    if (fieldStructure[rowIndex, column] == 0 && numberOfClues != 0)
                    {
                        clues[index, column] = numberOfClues;
                        numberOfClues = 0;
                        index++;
                    }
                    else if (fieldStructure[rowIndex, column] == 1)
                    {
                        numberOfClues++;
                    }

                    if (rowIndex == 0 && numberOfClues != 0)
                    {
                        clues[index, column] = numberOfClues;
                        numberOfClues = 0;
                        break;
                    }

                    if (rowIndex == 0)
                    {
                        break;
                    }

                    rowIndex--;
                }

                index = 0;
                rowIndex = _field.RowCount - 1;
            }

            for (var row = (_field.RowCount + 1) / 2 - 1; row >= 0; row--)
            {
                for (var column = 0; column < _field.ColumnCount; column++)
                {
                    Console.Write("  ");
                    if (clues[row, column] != 0)
                    {
                        Console.Write("{0,2}", clues[row, column]);
                    }
                    else
                    {
                        Console.Write("  ");
                    }
                }

                Console.WriteLine();
            }
        }

        /// <summary>
        /// Counts the consecutive ones in the field structure (in each row)
        /// and prints the clues on the left side of the field.
        /// </summary>
        /// <param name="row">index of the row</param>
        private void CluesInRow(int row)
        {
            var fieldStructure = _field.GetFieldStructure();
            var index = _field.ColumnCount - 1;
            var clues = 0;

            Console.Write("||");
            while (index >= 0)
            {
                if (fieldStructure[row, index] == 0 && clues != 0)
                {
                    Console.Write(" ");
                    Console.Write(clues);
                    Console.Write(" ");
                    clues = 0;
                }
                else if (fieldStructure[row, index] == 1)
                {
                    clues++;
                }

                if (index == 0 && clues != 0)
                {
                    Console.Write(" ");
                    Console.Write(clues);
                    Console.Write(" ");
                    clues = 0;
                    return;
                }

                index--;
            }
        }

        private void CreateField()
        {
            Console.Write("Choose difficulty(E-easy, M-medium, H-hard):");
            string line = Console.ReadLine().ToUpper();
            var match = _regDifficulty.Match(line);

            if (match.Success)
            {
                if (line.Equals("E"))
                {
                    _field = new Field(Difficulty.Easy);
                }

                if (line.Equals("M"))
                {
                    _field = new Field(Difficulty.Medium);
                }

                if (line.Equals("H"))
                {
                    _field = new Field(Difficulty.Hard);
                }
            }
            else
            {
                Console.WriteLine("Wrong input: " + line);
                CreateField();
            }
        }
        
        /// <summary>
        /// Reads in the players name at the beginning
        /// </summary>
        private void ReadPlayerName()
        {
            Console.Write("Enter your username: ");
            _player = Console.ReadLine();
        }

        private void ShowTopScorersAndRatings()
        {
            Console.WriteLine();
            Console.Write("Do you want to see the Hall Of Fame and the ratings?(y/n): ");
            string line = Console.ReadLine().ToUpper();
            var match = _regQuestion.Match(line);

            if (match.Success)
            {
                if (line.Equals("Y"))
                {
                    PrintTopScoresAndRatings();
                }
            }
            else
            {
                Console.WriteLine("Wrong input " + line);
                ShowTopScorersAndRatings();
            }
        }

        private void RateTheGame()
        {
            Console.WriteLine();
            Console.Write("Do you want to rate the game ?(y/n): ");
            string line = Console.ReadLine().ToUpper();
            var match = _regQuestion.Match(line);

            if (match.Success)
            {
                if (line.Equals("Y"))
                {
                    int rating;
                    do
                    {
                        Console.Write("Add rating (0-5): ");
                        rating = Convert.ToInt32(Console.ReadLine());

                        if (rating < 0 || rating > 5)
                        {
                            Console.WriteLine("Wrong input");
                        }
                    } while (rating < 0 || rating > 5);

                    _ratingsService.AddRating(new Rating { Player = _player, RatingValue = rating, RatedAt = DateTime.Now });
                }
            }
            else
            {
                Console.WriteLine("Wrong input " + line);
                RateTheGame();
            }
        }

        private void PrintTopScoresAndRatings()
        {
            Console.WriteLine();
            Console.WriteLine("############################");
            Console.WriteLine("####### Hall Of Fame #######");
            Console.WriteLine("############################");
            Console.WriteLine();
            foreach (var score in _scoreService.GetTopScores())
            {
                Console.WriteLine("{0}   {1}", score.Player, score.Points);
            }
            Console.WriteLine();
            Console.WriteLine("############################");
            Console.WriteLine("###### Latest ratings ######");
            Console.WriteLine("############################");
            Console.WriteLine();
            foreach (var rating in _ratingsService.GetLatestRatings())
            {
                Console.WriteLine("{0}      {1}   {2}", rating.Player, rating.RatingValue, rating.RatedAt);
            }
            Console.WriteLine();
        }

        private void WriteComment()
        {
            Console.WriteLine();
            Console.Write("Do you want to write a comment ?(y/n): ");
            string line = Console.ReadLine().ToUpper();
            var match = _regQuestion.Match(line);

            if (match.Success)
            {
                if (line.Equals("Y"))
                {
                    Console.Write("Write a short comment: ");
                    string comment = Console.ReadLine();
                    _commentsService.AddComment(new Comment { Player = _player, CommentContent = comment, WroteAt = DateTime.Now });
                }
            }
            else
            {
                Console.WriteLine("Wrong input " + line);
                WriteComment();
            }
        }

        private void PrintComments()
        {
            Console.WriteLine();
            Console.WriteLine("############################");
            Console.WriteLine("######### Comments #########");
            Console.WriteLine("############################");
            Console.WriteLine();
            foreach (var comment in _commentsService.GetLatestComments())
            {
                Console.WriteLine("{0}    {1}    {2}", comment.Player, comment.CommentContent, comment.WroteAt);
            }
            Console.WriteLine();
        }

        private void ShowComments()
        {
            Console.WriteLine();
            Console.Write("Do you want to see the comments ?(y/n): ");
            string line = Console.ReadLine().ToUpper();
            var match = _regQuestion.Match(line);

            if (match.Success)
            {
                if (line.Equals("Y"))
                {
                    PrintComments();
                }
            }
            else
            {
                Console.WriteLine("Wrong input " + line);
                ShowComments();
            }
        }

    }
}
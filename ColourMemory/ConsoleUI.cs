using System;
using ColourMemory.Services;

namespace ColourMemory
{
    public class ConsoleUI
    {
        private readonly GameLogic _game;

        /// <summary>
        /// Initializes a new instance of the ConsoleUI class.
        /// </summary>
        public ConsoleUI(GameLogic game)
        {
            _game = game;
        }

        /// <summary>
        /// Runs the main game loop, handling user interaction and game progression.
        /// </summary>
        public void PrintGameLoop()
        {
            PromptStartMenu();
            _game.ResetGame(PromptDifficulty());

            bool playing = true;
            while (playing)
            {
                while (!_game.AllPairsFound())
                {
                    PrintGameBoard(_game);

                    var first = FlipCard(_game, "Select first card");

                    PrintGameBoard(_game);

                    var second = FlipCard(_game, "Select second card", first);

                    PrintGameBoard(_game);

                    bool isPair = _game.IsPair(first, second);
                    Console.WriteLine(isPair ? "\nPair found! +1 point!" : "\nNot a pair! -1 point!");

                    Thread.Sleep(2000);
                    _game.HandlePair(first, second, isPair);
                }

                PrintGameBoard(_game);

                Console.WriteLine($"\nAll pairs found! Final score: {_game.Score}");

                PromptSaveHighScore(_game);
                PrintHighScores();

                playing = PromptPlayAgain();
            }

            Console.WriteLine("GG, thanks for playing!");
        }

        /// <summary>
        /// Prompts the user to play again and resets the game if confirmed.
        /// </summary>
        private bool PromptPlayAgain()
        {
            while (true)
            {
                Console.WriteLine($"\nDo you want to play again (y/n)?");
                string input = Console.ReadLine()?.ToLower();

                if (input == "y")
                {
                    _game.ResetGame(PromptDifficulty());
                    return true;
                }
                else if (input == "n")
                    return false;
                else
                {
                    Console.WriteLine("Invalid input, please enter 'y' or 'n'");
                    Thread.Sleep(1000);        
                }
            }
        }

        /// <summary>
        /// Prompts the user for difficulty level and returns the selected value.
        /// </summary>
        private int PromptDifficulty()
        {
            while (true)
            {
                Console.WriteLine("\nSelect difficulty: (3) 3x4 or (6) 6x4");
                int difficulty = 3;
                string input = Console.ReadLine() ?? string.Empty;

                if (input == "3")
                    return difficulty;
                else if (input == "6")
                    difficulty = 6;
                else
                {
                    Console.WriteLine("Invalid input, defaulting to 3x4");
                    Thread.Sleep(2000);
                }

                return difficulty;
            }
        }

        /// <summary>
        /// Prompts the user to start the game, view high scores, or exit.
        /// </summary>
        private void PromptStartMenu()
        {
            while (true)
            {
                Console.WriteLine("\nSelect an option:");
                Console.WriteLine("1. Start Game");
                Console.WriteLine("2. View High Scores");
                Console.WriteLine("3. Exit");
                Console.Write("Enter choice (1-3): ");
                string input = Console.ReadLine() ?? string.Empty;

                if (input == "1")
                    return;
                else if (input == "2")
                {
                    PrintHighScores();
                }
                else if (input == "3")
                {
                    Console.WriteLine("Exiting game. Goodbye!");
                    Environment.Exit(0);
                }
                else
                {
                    Console.WriteLine("Invalid choice. Please try again.");
                }
            }
        }

        /// <summary>
        /// Prompts the user to enter their name and saves the high score.
        /// </summary>
        private void PromptSaveHighScore(GameLogic game)
        {
            Console.Write($"\nInput your name for the highscore: ");
            string input = Console.ReadLine() ?? string.Empty;
            input = string.IsNullOrWhiteSpace(input) ? "Player" : input;

            HighScoreSerializer.SaveHighScore(game.Score, input, game.Difficulty);
        }

        /// <summary>
        /// Displays the top 10 high scores.
        /// </summary>
        private void PrintHighScores()
        {
            var highScores = HighScoreSerializer.LoadHighScore()
                .OrderByDescending(hs => hs.Score)
                .ToList();

            Console.WriteLine($"\n--- High Scores  ---");
            for (int i = 0; i < Math.Min(10, highScores.Count); i++)
            {
                var hs = highScores[i];
                Console.WriteLine($"{i + 1}. {hs.Name} - {hs.Score} - {hs.Difficulty}x4");
            }
        }

        /// <summary>
        /// Renders the current state of the game board to the console.
        /// </summary>
        private void PrintGameBoard(GameLogic game)
        {
            Console.Clear();

            Console.WriteLine($"Current score: {game.Score}");
            Console.WriteLine("\n    0   1   2   3");
            for (int i = 0; i < game.GameBoard.Rows; i++)
            {
                Console.Write($"{i}  ");
                for (int j = 0; j < game.GameBoard.Columns; j++)
                {
                    if (game.GameBoard.GetCard(i, j).IsPaired)
                    {
                        Console.Write("    ");
                    }
                    else if (game.GameBoard.GetCard(i, j).IsViewed)
                    {
                        Console.Write($"[{game.GameBoard.GetCard(i, j).Colour}] ");
                    }
                    else
                        Console.Write("[X] ");

                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Prompts the user to select a card to flip and validates the input.
        /// </summary>
        private (int, int) FlipCard(GameLogic game, string prompt, (int, int)? isFlipped = null)
        {
            int row, col;

            while (true)
            {
                Console.WriteLine($"\n{prompt}:");

                Console.Write($"Input row (0 - {game.GameBoard.Rows - 1}): ");
                if (!int.TryParse(Console.ReadLine(), out row) || row < 0 || row > game.GameBoard.Rows - 1)
                {
                    Console.WriteLine("Invalid row!");
                    continue;
                }

                Console.Write("Input column (0 - 3): ");
                if (!int.TryParse(Console.ReadLine(), out col) || col < 0 || col > game.GameBoard.Columns - 1)
                {
                    Console.WriteLine("Invalid column!");
                    continue;
                }

                if (isFlipped != null && (row, col) == isFlipped.Value)
                {
                    Console.WriteLine("Card picked already!");
                    continue;
                }

                if (!game.TryFlipCard(row, col))
                {
                    Console.WriteLine("Invalid card. Card already picked or paired!");
                    continue;
                }

                return (row, col);
            }
        }
    }
}
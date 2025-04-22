using ColourMemory;

internal class Program
{
    static void Main(string[] args)
    {
        var game = new GameLogic();
        bool playing = true;
        //int cheats = 0;

        while (playing)
        {
            //if (cheats == 0)
            //    game.CheatCode(); cheats++;

            while (!game.AllPairsFound())
            {
                PrintGameBoard(game);

                var first = FlipCard(game, "Select first card");

                PrintGameBoard(game);

                var second = FlipCard(game, "Select second card", first);

                PrintGameBoard(game);

                bool isPair = game.IsPair(first, second);
                Console.WriteLine(isPair ? "\nPair found! +1 point!" : "\nNot a pair! -1 point!");

                Thread.Sleep(2000);
                game.HandlePair(first, second, isPair);
            }

            PrintGameBoard(game);
            Console.WriteLine($"\nAll pairs found! Final score: {game.Score}");

            
            Console.WriteLine($"\nDo you want to play again (y/n)?");
            string prompt = Console.ReadLine()?.ToLower();

            if (prompt == "n")
                playing = false;
            else
                game.ResetGame();
        }

        Console.WriteLine("GG, thanks for playing!");
    }

    // Updates gameboard visually
    static void PrintGameBoard(GameLogic game)
    {
        Console.Clear();

        Console.WriteLine($"Current score: {game.Score}");
        Console.WriteLine("\n    0   1   2   3");
        for (int i = 0; i < 4; i++)
        {
            Console.Write($"{i}  ");
            for (int j = 0; j < 4; j++)
            {
                if (game.PairedCards[i, j])
                {
                    Console.Write("    ");
                }
                else if (game.ViewedCards[i, j])
                {
                    Console.Write($"[{game.GameBoard[i, j][0]}] ");
                }
                else
                    Console.Write("[X] ");

            }
            Console.WriteLine();
        }
    }

    // Validates input and game marks card as viewed
    static (int, int) FlipCard(GameLogic game, string prompt, (int, int)? isFlipped = null)
    {
        int row, col;

        while (true)
        {
            Console.WriteLine($"\n{prompt}:");

            Console.Write("Input row (0 - 3): ");
            if (!int.TryParse(Console.ReadLine(), out row) || row < 0 || row > 3)
            {
                Console.WriteLine("Invalid row!");
                continue;
            }

            Console.Write("Input column (0 - 3): ");
            if (!int.TryParse(Console.ReadLine(), out col) || col < 0 || col > 3)
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
                Console.WriteLine("Inavlid cards. Card already picked or paired!");
                continue;
            }

            return (row, col);
        }
    }
}
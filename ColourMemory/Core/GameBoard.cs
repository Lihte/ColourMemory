using ColourMemory.Models;
using ColourMemory.Services;

namespace ColourMemory.Core;

public class GameBoard
{
    private IRandomProvider _rnd;

    private Card[,] Cards { get; }

    public int Rows { get; init; }
    public int Columns { get; init; }

    /// <summary>
    /// Initializes a new instance of the GameBoard class with the specified difficulty.
    /// </summary>
    public GameBoard(IRandomProvider randomProvider, int difficulty = 3)
    {
        _rnd = randomProvider;

        if (difficulty == 3)
        {
            Rows = 3;
            Columns = 4;
        }
        else if (difficulty == 6)
        {
            Rows = 6;
            Columns = 4;
        }

        Cards = new Card[Rows, Columns];
        Setup(difficulty);
    }

    /// <summary>
    /// Sets up the game board with shuffled cards based on difficulty.
    /// </summary>
    private void Setup(int difficulty)
    {
        List<string> colours;

        if (difficulty == 3)
        {
            colours = new List<string>
                {
                    "Red", "Red", "Blue", "Blue", "Green", "Green", "Yellow", "Yellow",
                    "Purple", "Purple", "Orange", "Orange"
                };
        }
        else if (difficulty == 6)
        {
            colours = new List<string>
                {
                    "Red", "Red", "Blue", "Blue", "Green", "Green", "Yellow", "Yellow",
                    "Purple", "Purple", "Orange", "Orange", "Magenta", "Magenta", "Violet", "Violet",
                    "Teal", "Teal", "Chartreuse", "Chartreuse", "Amber", "Amber", "Vermilion", "Vermilion"
                };
        }
        else
        {
            throw new ArgumentException("Size must be 3 or 6");
        }

        Shuffle(colours, _rnd);

        int index = 0;
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
            {
                Cards[i, j] = new Card(colours[index++]);
            }
        }
    }

    /// <summary>
    /// Gets the card at the specified row and column.
    /// </summary>
    public Card GetCard(int row, int col) => Cards[row, col];

    /// <summary>
    /// Gets the array of all cards on the board.
    /// </summary>
    public Card[,] GetCards() => Cards;

    /// <summary>
    /// Shuffles the list of items using the provided random provider.
    /// </summary>
    private void Shuffle<T>(IList<T> list, IRandomProvider rnd)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = rnd.Next(i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}
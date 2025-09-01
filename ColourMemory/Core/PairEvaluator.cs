using ColourMemory.Models;

namespace ColourMemory.Core;

public class PairEvaluator
{
    /// <summary>
    /// Determines if two cards form a pair based on their colour.
    /// </summary>
    public bool IsPair(Card first, Card second) => first.Colour == second.Colour;

    /// <summary>
    /// Evaluates the pair and returns the score adjustment.
    /// </summary>
    public int Evaluate(Card first, Card second)
    {
        return IsPair(first, second) ? 1 : -1;
    }
}

using Xunit;
using ColourMemory.Core;
using ColourMemory.Models;
using ColourMemory.Services;

namespace ColourMemory.Tests;

public class GameLogicTests
{
    [Fact]
    public void TryFlipCard_ShouldChangeViewedCards()
    {
        var rnd = new StandardRandom();
        var game = new GameLogic(rnd);
        game.ResetGame(difficulty:3);

        bool flipped = game.TryFlipCard(0, 0);
        Assert.True(flipped);
        Assert.True(game.GameBoard.GetCard(0, 0).IsViewed);
    }

    [Fact]
    public void TryFlipCard_CannotFlipPairedCard()
    {
        var rnd = new StandardRandom();
        var game = new GameLogic(rnd);
        game.ResetGame(3);

        game.GameBoard.GetCard(0, 0).IsPaired = true;

        bool flipped = game.TryFlipCard(0, 0);

        Assert.False(flipped);
    }

    [Fact]
    public void Pair_IncresesScore()
    {
        var fixedValues = Enumerable.Range(0, 12).Reverse(); // reverse list to get original order after gameboard shuffle, otherwise last card would be first
        var rnd = new FixedRandomProvider(fixedValues);
        var game = new GameLogic(rnd);
        game.ResetGame(3);

        game.TryFlipCard(0, 0);
        game.TryFlipCard(0, 1);

        bool isPair = game.IsPair((0, 0), (0, 1));
        game.HandlePair((0, 0), (0, 1), isPair);

        Assert.True(isPair);
        Assert.Equal(1, game.Score);
        Assert.True(game.GameBoard.GetCard(0, 0).IsPaired);
        Assert.True(game.GameBoard.GetCard(0, 1).IsPaired);
    }

    [Fact]
    public void NotPair_DecresesScoreAndHidesCards()
    {
        var fixedValues = Enumerable.Range(0, 12);
        var rnd = new FixedRandomProvider(fixedValues);
        var game = new GameLogic(rnd);
        game.ResetGame(3);

        game.TryFlipCard(0, 0);
        game.TryFlipCard(0, 1);

        bool isPair = game.IsPair((0, 0), (0, 1));
        game.HandlePair((0, 0), (0, 1), isPair);

        Assert.False(isPair);
        Assert.Equal(-1, game.Score);
        Assert.False(game.GameBoard.GetCard(0, 0).IsViewed);
        Assert.False(game.GameBoard.GetCard(0, 1).IsViewed);
    }

    [Fact]
    public void AllPairsFound_ReturnsTrueWhenAllCardsPaired()
    {
        StandardRandom rnd = new StandardRandom();
        var game = new GameLogic(rnd);
        game.ResetGame(6);

        for (int i = 0; i < game.GameBoard.Rows; i++)
        {
            for (int j = 0; j < game.GameBoard.Columns; j++)
            {
                game.GameBoard.GetCard(i, j).IsPaired = true;
            }
        }

        Assert.True(game.AllPairsFound());
    }

    [Fact]
    public void AllPairsFound_ReturnsFalseIfAnyPairNotFound()
    {
        StandardRandom rnd = new StandardRandom();
        var game = new GameLogic(rnd);
        game.ResetGame(6);

        game.GameBoard.GetCard(0, 0).IsPaired = true;
        game.GameBoard.GetCard(1, 1).IsPaired = false;

        Assert.False(game.AllPairsFound());
    }

    [Fact]
    public void NewGame_ScoreAtZero()
    {
        StandardRandom rnd = new StandardRandom();
        var game = new GameLogic(rnd);
        game.ResetGame(6);

        Assert.Equal(0, game.Score);
    }

    [Fact]
    public void ResetGame_ShouldResetScoreAndDifficulty()
    {
        StandardRandom rnd = new StandardRandom();
        var game = new GameLogic(rnd);
        game.ResetGame(6);
        game.CheatCode();

        Assert.Equal(12, game.Score);
        Assert.Equal(6, game.Difficulty);

        game.ResetGame(3);

        Assert.Equal(0, game.Score);
        Assert.Equal(3, game.Difficulty);
    }

    [Fact]
    public void GameBoard_ShouldInitialize_WithRandomColours()
    {
        var knownColoursOrder = new List<string>
            {
                "Red", "Red", "Blue", "Blue", "Green", "Green", "Yellow", "Yellow",
                "Purple", "Purple", "Orange", "Orange"
            };
        var rnd = new StandardRandom();
        var game = new GameLogic(rnd);
        game.ResetGame(3);

        List<string> gameColours = new List<string>();
        foreach (var card in game.GameBoard.GetCards())
            gameColours.Add(card.Colour);

        Assert.NotEmpty(gameColours);
        Assert.Equal(12, gameColours.Count);
        Assert.NotEqual(knownColoursOrder, gameColours);
    }

    [Fact]
    public void GameBoard_ShouldInitialize_WithKnownColours()
    {
        var fixedValues = Enumerable.Range(0, 12).Reverse(); // reverse list to get original order after gameboard shuffle, otherwise last card would be first
        var rnd = new FixedRandomProvider(fixedValues);
        var game = new GameLogic(rnd);
        game.ResetGame(3);

        Card firstCard = game.GameBoard.GetCard(0, 0);
        Card lastCard = game.GameBoard.GetCard(2, 3);

        Assert.Equal("Red", firstCard.Colour);
        Assert.Equal("Orange", lastCard.Colour);
    }
}

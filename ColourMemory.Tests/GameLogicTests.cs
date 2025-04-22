using Xunit;

namespace ColourMemory.Tests
{
    public class GameLogicTests
    {
        [Fact]
        public void TryFlipCard_ShouldChangeViewedCards()
        {
            var game = new GameLogic();
            bool flipped = game.TryFlipCard(0, 0);
            Assert.True(flipped);
            Assert.True(game.ViewedCards[0, 0]);
        }

        [Fact]
        public void TryFlipCard_CannotFlipPairedVard()
        {
            var game = new GameLogic();

            game.PairedCards[0, 0] = true;

            bool flipped = game.TryFlipCard(0, 0);

            Assert.False(flipped);
        }

        [Fact]
        public void Pair_IncresesScore()
        {
            var game = new GameLogic();

            game.GameBoard[0, 0] = "Blue";
            game.GameBoard[0, 1] = "Blue";

            game.TryFlipCard(0, 0);
            game.TryFlipCard(0, 1);

            bool isPair = game.IsPair((0, 0), (0, 1));
            game.HandlePair((0, 0), (0, 1), isPair);

            Assert.True(isPair);
            Assert.Equal(1, game.Score);
            Assert.True(game.PairedCards[0, 0]);
            Assert.True(game.PairedCards[0, 1]);
        }

        [Fact]
        public void NotPair_DecresesScoreAndHidesCards()
        {
            var game = new GameLogic();

            game.GameBoard[0, 0] = "Blue";
            game.GameBoard[0, 1] = "Red";

            game.TryFlipCard(0, 0);
            game.TryFlipCard(0, 1);

            bool isPair = game.IsPair((0, 0), (0, 1));
            game.HandlePair((0, 0), (0, 1), isPair);

            Assert.False(isPair);
            Assert.Equal(-1, game.Score);
            Assert.False(game.ViewedCards[0, 0]);
            Assert.False(game.ViewedCards[0, 1]);
        }

        [Fact]
        public void AllPairsFound_ReturnsTrueWhenAllCardsPaired()
        {
            var game = new GameLogic();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    game.PairedCards[i, j] = true;
                }
            }

            Assert.True(game.AllPairsFound());
        }

        [Fact]
        public void AllPairsFound_ReturnsFalseIfAnyPairNotFound()
        {
            var game = new GameLogic();

            game.PairedCards[0, 0] = true;
            game.PairedCards[1, 1] = false;

            Assert.False(game.AllPairsFound());
        }

        [Fact]
        public void NewGame_ScoreAtZero()
        {
            var game = new GameLogic();

            Assert.Equal(0, game.Score);
        }
    }
}
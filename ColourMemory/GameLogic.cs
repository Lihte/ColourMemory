namespace ColourMemory
{
    public class GameLogic
    {
        private GameBoard _gameBoard;
        public GameBoard GameBoard { get => _gameBoard; }

        private IRandomProvider _rnd;
        private PairEvaluator _pairEvaluator;

        public int Score { get; private set; } = 0;
        public int Difficulty { get; private set; }

        /// <summary>
        /// Initializes a new instance of the GameLogic class with the specified random provider and difficulty.
        /// </summary>
        public GameLogic(IRandomProvider randomProvider, int difficulty = 3)
        {
            _rnd = randomProvider;
            Difficulty = difficulty;
            _pairEvaluator = new PairEvaluator();
            _gameBoard = ResetGameBoard(Difficulty);
        }

        /// <summary>
        /// Attempts to flip a card at the specified row and column.
        /// Returns true if the card was successfully flipped; otherwise, false.
        /// </summary>
        public bool TryFlipCard(int row, int col)
        {
            var card = _gameBoard.GetCard(row, col);
            if (card.IsPaired || card.IsViewed)
                return false;
                
            card.IsViewed = true;
            return true;
        }

        /// <summary>
        /// Determines whether the two specified cards form a pair.
        /// </summary>
        public bool IsPair((int, int) first, (int, int) second)
        {
            var card1 = _gameBoard.GetCard(first.Item1, first.Item2);
            var card2 = _gameBoard.GetCard(second.Item1, second.Item2);

            return _pairEvaluator.IsPair(card1, card2);
        }

        /// <summary>
        /// Handles the result of a pair attempt, updating card states and score accordingly.
        /// </summary>
        public void HandlePair((int, int) first, (int, int) second, bool isPair)
        {
            var card1 = _gameBoard.GetCard(first.Item1, first.Item2);
            var card2 = _gameBoard.GetCard(second.Item1, second.Item2);

            if (isPair)
            {
                card1.IsPaired = true;
                card2.IsPaired = true;
                Score += _pairEvaluator.Evaluate(card1, card2);
            }
            else
            {
                card1.IsViewed = false;
                card2.IsViewed = false;
                Score += _pairEvaluator.Evaluate(card1, card2);
            }
        }

        /// <summary>
        /// Checks if all pairs have been found on the game board.
        /// </summary>
        public bool AllPairsFound()
        {
            foreach (var card in _gameBoard.GetCards())
                if (card.IsPaired == false) return false;

            return true;
        }

        /// <summary>
        /// Creates and returns a new game board with the specified difficulty.
        /// </summary>
        private GameBoard ResetGameBoard(int difficulty)
        {
            return new GameBoard(_rnd, difficulty);
        }

        /// <summary>
        /// Resets the game state and initializes a new game board with the specified difficulty.
        /// </summary>
        public void ResetGame(int difficulty)
        {
            Score = 0;
            Difficulty = difficulty;
            _gameBoard = ResetGameBoard(Difficulty);
        }

        /// <summary>
        /// Sets all cards as paired and adjusts the score for testing purposes.
        /// </summary>
        public void CheatCode()
        {
            for (int i = 0; i < GameBoard.Rows; i++)
            {
                for (int j = 0; j < GameBoard.Columns; j++)
                {
                    GameBoard.GetCard(i, j).IsPaired = true;
                    Score++;
                }
            }

            Score /= 2; // each pair is 2 cards
        }
    }
}

namespace ColourMemory
{
    public class GameLogic
    {
        public string[,] GameBoard { get; private set; } = new string[4, 4];

        public bool[,] ViewedCards { get; private set; } = new bool[4, 4];
        public bool[,] PairedCards { get; private set; } = new bool[4, 4];

        public int Score { get; private set; } = 0;

        public GameLogic()
        {
            SetupBoard();
        }

        private void SetupBoard()
        {
            List<string> colours = new List<string>
            {
                "Red", "Red", "Blue", "Blue", "Green", "Green", "Yellow", "Yellow", "Purple", "Purple", "Orange", "Orange", "White", "White", "Cyan", "Cyan"
            };

            // Inject for true low coupling?
            Random rnd = new Random();

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    int index = rnd.Next(colours.Count);
                    GameBoard[i, j] = colours[index];
                    colours.RemoveAt(index);
                }
            }
        }

        public bool TryFlipCard(int row, int col)
        {
            if (row < 0 || row > 3 || col < 0 || col > 3)
                return false;

            if (ViewedCards[row, col] || PairedCards[row, col])
                return false;

            ViewedCards[row, col] = true;
            return true;
        }

        public bool IsPair((int, int) first, (int, int) second)
        {
            var (x1, y1) = first;
            var (x2, y2) = second;

            return GameBoard[x1, y1] == GameBoard[x2, y2];
        }

        public void HandlePair((int, int) first, (int, int) second, bool isPair)
        {
            var (x1, y1) = first;
            var (x2, y2) = second;

            if (isPair)
            {
                PairedCards[x1, y1] = true;
                PairedCards[x2, y2] = true;
                Score++;
            }
            else
            {
                ViewedCards[x1, y1] = false;
                ViewedCards[x2, y2] = false;
                Score--;
            }
        }

        public bool AllPairsFound()
        {
            foreach (var pair in PairedCards)
                if (pair == false) return false;
            return true;
        }

        public void ResetGame()
        {
            GameBoard = new string[4, 4];
            
            ViewedCards = new bool[4, 4];
            PairedCards = new bool[4, 4];
            Score = 0;

            SetupBoard();
        }

        // Just for testing to get to the end of the game
        //public void CheatCode()
        //{
        //    for (int i = 0; i < 4; i++)
        //    {
        //        for (int j = 0; j < 4; j++)
        //        {
        //            PairedCards[i, j] = true;
        //        }
        //    }
        //}
    }
}

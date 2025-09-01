namespace ColourMemory.Models
{
    public class Card
    {
        public string Colour { get; }
        public bool IsPaired { get; set; }
        public bool IsViewed { get; set; }

        public Card(string colour)
        {
            Colour = colour;
            IsViewed = false;
            IsPaired = false;
        }
    }
}
public class StandardRandom : IRandomProvider
{
    private Random _rnd = new Random();
    public int Next(int max) => _rnd.Next(max);
}
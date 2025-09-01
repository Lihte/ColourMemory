namespace ColourMemory.Services;

public class FixedRandomProvider : IRandomProvider
{
    private readonly Queue<int> _values;

    public FixedRandomProvider(IEnumerable<int> values)
    {
        _values = new Queue<int>(values);
    }

    public int Next(int max)
    {
        if (_values.Count == 0)
            throw new InvalidOperationException("No more fixed random values available.");

        return _values.Dequeue();
    }
}
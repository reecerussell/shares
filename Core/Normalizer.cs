namespace Shares.Core
{
    internal class Normalizer : INormalizer
    {
        public string Normalize(string input)
        {
            return input.ToUpper();
        }
    }
}

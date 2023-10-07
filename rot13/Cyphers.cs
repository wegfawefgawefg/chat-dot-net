using System.Text;

namespace rot13
{
    public static class Rot13Encoder
    {
        public static string Encode(string input)
        {
            StringBuilder result = new StringBuilder();

            foreach (char c in input)
            {
                if (c >= 'a' && c <= 'z')
                {
                    result.Append((char)((c - 'a' + 13) % 26 + 'a'));
                }
                else if (c >= 'A' && c <= 'Z')
                {
                    result.Append((char)((c - 'A' + 13) % 26 + 'A'));
                }
                else
                {
                    result.Append(c);
                }
            }

            return result.ToString();
        }
    }
}

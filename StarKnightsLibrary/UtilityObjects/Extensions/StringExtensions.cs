using Microsoft.Xna.Framework;
using System.Linq;

namespace StarKnightsLibrary.UtilityObjects.Extensions
{
    public static class StringExtensions
    {
        public static bool TryParse(this string input, out Vector2 vector)
        {
            if (!input.StartsWith("(") || !input.EndsWith(")") || input.Count(i => i == ',') != 1)
            {
                vector = new Vector2();
                return false;
            }
            input = input.Trim('(', ')');
            var split = input.Split(',');
            if (!float.TryParse(split[0], out float x) || !float.TryParse(split[1], out float y))
            {
                vector = new Vector2();
                return false;
            }
            vector = new Vector2(x, y);
            return true;
        }
    }
}

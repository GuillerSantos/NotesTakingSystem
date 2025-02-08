
namespace NTS.Client.Utilities
{
    public class NotesCardColorUtil
    {
        public static string GetLighterOrDarkerColor(string hexColor, float factor = 1.2f)
        {
            try
            {
                if (hexColor.StartsWith("#"))
                    hexColor = hexColor[1..];

                if (hexColor.Length != 6) return hexColor;

                int r = Convert.ToInt32(hexColor[..2], 16);
                int g = Convert.ToInt32(hexColor[2..4], 16);
                int b = Convert.ToInt32(hexColor[4..], 16);

                r = Math.Min(255, (int)(r * factor));
                g = Math.Min(255, (int)(g * factor));
                b = Math.Min(255, (int)(b * factor));

                return $"#{r:X2}{g:X2}{b:X2}";
            }
            catch
            {
                return hexColor;
            }
        }
    }
}

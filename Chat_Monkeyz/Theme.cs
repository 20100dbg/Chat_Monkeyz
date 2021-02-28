using System;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;

namespace Chat_Monkeyz
{
    public class Theme
    {

        public String name;
        public Color fontcolor;
        public Color backcolor;
        public Font font;
        public Size size;

        public Theme()
        {

        }

        public Theme Clone()
        {
            return (Theme)MemberwiseClone();
        }

        public static Theme CreateThemeFromFile(String filename)
        {
            Theme t = Program.defaultTheme.Clone();
            t.name = Path.GetFileNameWithoutExtension(filename);

            String line;
            using (StreamReader sr = new StreamReader(filename))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    String[] keyValue = line.Split('=');

                    if (keyValue[0] == "fontcolor") t.fontcolor = StringToColor(keyValue[1]);
                    if (keyValue[0] == "backcolor") t.backcolor = StringToColor(keyValue[1]);

                    if (keyValue[0] == "font") t.font = new Font(keyValue[1], t.font.Size);
                    if (keyValue[0] == "fontsize")
                    {
                        int px;
                        float em;
                        if (int.TryParse(keyValue[1], out px)) em = px * 72 / 96;
                        else em = t.font.Size;

                        t.font = new Font(t.font.Name, em);
                    }

                    if (keyValue[0] == "width")
                    {
                        int width;
                        if (int.TryParse(keyValue[1], out width)) t.size.Width = width;
                    }

                    if (keyValue[0] == "height")
                    {
                        int height;
                        if (int.TryParse(keyValue[1], out height)) t.size.Height = height;
                    }
                }
            }

            return t;
        }


        public static Color StringToColor(String str)
        {
            int r = 0, g = 0, b = 0;
            
            MatchCollection mc = Regex.Matches(str,"[a-fA-F0-9]{2}");

            if (mc.Count == 3)
            {
                r = int.Parse(mc[0].Value, System.Globalization.NumberStyles.HexNumber);
                g = int.Parse(mc[1].Value, System.Globalization.NumberStyles.HexNumber);
                b = int.Parse(mc[2].Value, System.Globalization.NumberStyles.HexNumber);
            }

            return Color.FromArgb(r, g, b);
        }

    }
}

﻿using System.Globalization;
 
namespace VisioAutomation.Drawing
{
    public struct ColorRgb
    {
        private readonly byte _r;
        private readonly byte _g;
        private readonly byte _b;

        public ColorRgb(byte r, byte g, byte b)
        {
            this._r = r;
            this._g = g;
            this._b = b;
        }

        public ColorRgb(short r, short g, short b) :
            this( (byte)r, (byte)g, (byte) b)
        {
        }

        public ColorRgb(int rgb)
        {
            ColorRgb.GetRgbBytes((uint) rgb, out this._r, out this._g, out this._b);
        }

        public ColorRgb(uint rgb)
        {
            ColorRgb.GetRgbBytes(rgb, out this._r, out this._g, out this._b);
        }


        public ColorRgb(System.Drawing.Color color)
        {
            this._r = color.R;
            this._g = color.G;
            this._b = color.B;
        }

        public byte R => this._r;

        public byte G => this._g;

        public byte B => this._b;

        public override string ToString()
        {
            var invariant_culture = CultureInfo.InvariantCulture;
            var s = string.Format(invariant_culture, "{0}({1},{2},{3})", nameof(ColorRgb), this.R, this.G, this.B);
            return s;
        }

        public static explicit operator System.Drawing.Color(ColorRgb color)
        {
            return System.Drawing.Color.FromArgb(color._r, color._g, color._b);
        }

        public static explicit operator int(ColorRgb color)
        {
            return color.ToRgb();
        }

        public static explicit operator ColorRgb(int rgbint)
        {
            return new ColorRgb(rgbint);
        }

        public string ToWebColorString()
        {
            return ColorRgb.ToWebColorString(this._r, this._g, this._b);
        }

        public override bool Equals(object other)
        {
            return other is ColorRgb && this.Equals((ColorRgb) other);
        }

        public static bool operator ==(ColorRgb lhs, ColorRgb rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(ColorRgb lhs, ColorRgb rhs)
        {
            return !lhs.Equals(rhs);
        }

        private bool Equals(ColorRgb other)
        {
            return (this._r == other._r && this._g == other._g && this._b == other._b);
        }

        public override int GetHashCode()
        {
            return this.ToRgb();
        }

        public int ToRgb()
        {
            return (this._r << 16) | (this._g << 8) | (this._b);
        }

        public static ColorRgb ParseWebColor(string webcolor)
        {
            var c = ColorRgb.TryParseWebColor(webcolor);
            if (!c.HasValue)
            {
                string s = string.Format("Failed to parse color string \"{0}\"", webcolor);
                throw new System.FormatException(s);
            }

            return c.Value;
        }

        public static ColorRgb? TryParseWebColor(string webcolor)
        {
            // fail if string is null
            if (webcolor == null)
            {
                return null;
            }

            // fail if string is empty
            if (webcolor.Length < 1)
            {
                return null;
            }

            // clean any leading or trailing whitespace
            webcolor = webcolor.Trim();

            // fail if string is empty
            if (webcolor.Length < 1)
            {
                return null;
            }

            // strip leading # if it is there
            while (webcolor.StartsWith("#"))
            {
                webcolor = webcolor.Substring(1);
            }

            // clean any leading or trailing whitespace
            webcolor = webcolor.Trim();

            // fail if string is empty
            if (webcolor.Length < 1)
            {
                return null;
            }

            // fail if string doesn't have exactly 6 digits
            if (webcolor.Length != 6)
            {
                return null;
            }

            int current_color;
            bool result = int.TryParse(webcolor, NumberStyles.HexNumber, null, out current_color);

            if (!result)
            {
                // fail if parsing didn't work
                return null;
            }

            // at this point parsing worked

            // the integer value is converted directly to an rgb value

            var the_color = new ColorRgb(current_color);
            return the_color;
        }
        
        private static void GetRgbBytes(uint rgb, out byte r, out byte g, out byte b)
        {
            r = (byte)((rgb & 0x00ff0000) >> 16);
            g = (byte)((rgb & 0x0000ff00) >> 8);
            b = (byte)((rgb & 0x000000ff) >> 0);
        }

        private static string ToWebColorString(byte r, byte g, byte b)
        {
            var invariant_culture = CultureInfo.InvariantCulture;
            string color_string = string.Format(invariant_culture, "#{0:x2}{1:x2}{2:x2}", r, g, b);
            return color_string;
        }

        public string ToFormula()
        {
            string formula = string.Format("{0}({1},{2},{3})",  "RGB", this.R, this.G, this.B);
            return formula;
        }        
    }
}
﻿namespace sm
{
    public class CStyle
    {
        public List<object> Border { get; set; } = [];
        public List<object> Font { get; set; } = [];

        internal string Set(string _text, List<object> _styles)
        {
            string tmp = "";
            foreach (object o in _styles)
            {
                Type type = o.GetType();
                if (type == typeof(Styling))
                {
                    tmp += $"\u001b[{(int)o}m";
                }

                if (type == typeof(Color))
                {
                    tmp += $"\u001b[38;5;{(int)o}m";
                }
            }

            return tmp + $"{_text}\u001b[0m";
        }
    }

    internal class CStyleBuilder
    {
        private readonly CStyle Style = new();

        public CStyleBuilder AddBorder(object _style)
        {
            if (!Style.Border.Contains(_style)) Style.Border.Add(_style);
            return this;
        }

        public CStyleBuilder AddFont(object _style)
        {
            if (!Style.Font.Contains(_style)) Style.Font.Add(_style);
            return this;
        }

        public CStyleBuilder AddFonts(List<object> _styles)
        {
            foreach (object o in _styles) AddFont(o);
            return this;
        }

        public CStyleBuilder AddBorders(List<object> _styles)
        {
            foreach (object o in _styles) AddBorder(o);
            return this;
        }

        public CStyle Build()
        {
            return Style;
        }
    }

    internal class CStyleDirector
    {
        public CStyle Construct(CStyleBuilder builder)
        {
            return builder.AddFont(Color.white).AddBorder(Color.white).Build();
        }
    }

    public enum Styling
    {
        None = 0,
        Bold = 1,
        Italic = 3,
        Underline = 4,
        Reversed = 7,
        Blink = 5,
        DoubleUnderline = 21,
        Crossed = 9
    }

    public enum Color
    {
        maroon = 1,
        green = 2,
        olive = 3,
        navy = 4,
        purple = 5,
        teal = 6,
        silver = 7,
        grey = 8,
        red = 9,
        lime = 10,
        yellow = 11,
        blue = 12,
        magenta = 13,
        cyan = 14,
        white = 15,
        grey0 = 16,
        navyblue = 17,
        darkblue = 18,
        blue3 = 19,
        blue3_1 = 20,
        blue1 = 21,
        darkgreen = 22,
        deepskyblue4 = 23,
        deepskyblue4_1 = 24,
        deepskyblue4_2 = 25,
        dodgerblue3 = 26,
        dodgerblue2 = 27,
        green4 = 28,
        springgreen4 = 29,
        turquoise4 = 30,
        deepskyblue3 = 31,
        deepskyblue3_1 = 32,
        dodgerblue1 = 33,
        green3 = 34,
        springgreen3 = 35,
        darkcyan = 36,
        lightseagreen = 37,
        deepskyblue2 = 38,
        deepskyblue1 = 39,
        green3_1 = 40,
        springgreen3_1 = 41,
        springgreen2 = 42,
        cyan3 = 43,
        darkturquoise = 44,
        turquoise2 = 45,
        green1 = 46,
        springgreen2_1 = 47,
        springgreen1 = 48,
        mediumspringgreen = 49,
        cyan2 = 50,
        cyan1 = 51,
        darkred = 52,
        deeppink4 = 53,
        purple4 = 54,
        purple4_1 = 55,
        purple3 = 56,
        blueviolet = 57,
        orange4 = 58,
        grey37 = 59,
        mediumpurple4 = 60,
        slateblue3 = 61,
        slateblue3_1 = 62,
        royalblue1 = 63,
        chartreuse4 = 64,
        darkseagreen4 = 65,
        paleturquoise4 = 66,
        steelblue = 67,
        steelblue3 = 68,
        cornflowerblue = 69,
        chartreuse3 = 70,
        darkseagreen4_1 = 71,
        cadetblue = 72,
        cadetblue_1 = 73,
        skyblue3 = 74,
        steelblue1 = 75,
        chartreuse3_1 = 76,
        palegreen3 = 77,
        seagreen3 = 78,
        aquamarine3 = 79,
        mediumturquoise = 80,
        steelblue1_1 = 81,
        chartreuse2 = 82,
        seagreen2 = 83,
        seagreen1 = 84,
        seagreen1_1 = 85,
        aquamarine1 = 86,
        darkslategray2 = 87,
        darkred_1 = 88,
        deeppink4_1 = 89,
        darkmagenta = 90,
        darkmagenta_1 = 91,
        darkviolet = 92,
        purple_1 = 93,
        orange4_1 = 94,
        lightpink4 = 95,
        plum4 = 96,
        mediumpurple3 = 97,
        mediumpurple3_1 = 98,
        slateblue1 = 99,
        yellow4 = 100,
        wheat4 = 101,
        grey53 = 102,
        lightslategrey = 103,
        mediumpurple = 104,
        lightslateblue = 105,
        yellow4_1 = 106,
        darkolivegreen3 = 107,
        darkseagreen = 108,
        lightskyblue3 = 109,
        lightskyblue3_1 = 110,
        skyblue2 = 111,
        chartreuse2_1 = 112,
        darkolivegreen3_1 = 113,
        palegreen3_1 = 114,
        darkseagreen3 = 115,
        darkslategray3 = 116,
        skyblue1 = 117,
        chartreuse1 = 118,
        lightgreen = 119,
        lightgreen_1 = 120,
        palegreen1 = 121,
        aquamarine1_1 = 122,
        darkslategray1 = 123,
        red3 = 124,
        deeppink4_2 = 125,
        mediumvioletred = 126,
        magenta3 = 127,
        darkviolet_1 = 128,
        purple_2 = 129,
        darkorange3 = 130,
        indianred = 131,
        hotpink3 = 132,
        mediumorchid3 = 133,
        mediumorchid = 134,
        mediumpurple2 = 135,
        darkgoldenrod = 136,
        lightsalmon3 = 137,
        rosybrown = 138,
        grey63 = 139,
        mediumpurple2_1 = 140,
        mediumpurple1 = 141,
        gold3 = 142,
        darkkhaki = 143,
        navajowhite3 = 144,
        grey69 = 145,
        lightsteelblue3 = 146,
        lightsteelblue = 147,
        yellow3 = 148,
        darkolivegreen3_2 = 149,
        darkseagreen3_1 = 150,
        darkseagreen2 = 151,
        lightcyan3 = 152,
        lightskyblue1 = 153,
        greenyellow = 154,
        darkolivegreen2 = 155,
        palegreen1_1 = 156,
        darkseagreen2_1 = 157,
        darkseagreen1 = 158,
        paleturquoise1 = 159,
        red3_1 = 160,
        deeppink3 = 161,
        deeppink3_1 = 162,
        magenta3_1 = 163,
        magenta3_2 = 164,
        magenta2 = 165,
        darkorange3_1 = 166,
        indianred_1 = 167,
        hotpink3_1 = 168,
        hotpink2 = 169,
        orchid = 170,
        mediumorchid1 = 171,
        orange3 = 172,
        lightsalmon3_1 = 173,
        lightpink3 = 174,
        pink3 = 175,
        plum3 = 176,
        violet = 177,
        gold3_1 = 178,
        lightgoldenrod3 = 179,
        tan = 180,
        mistyrose3 = 181,
        thistle3 = 182,
        plum2 = 183,
        yellow3_1 = 184,
        khaki3 = 185,
        lightgoldenrod2 = 186,
        lightyellow3 = 187,
        grey84 = 188,
        lightsteelblue1 = 189,
        yellow2 = 190,
        darkolivegreen1 = 191,
        darkolivegreen1_1 = 192,
        darkseagreen1_1 = 193,
        honeydew2 = 194,
        lightcyan1 = 195,
        red1 = 196,
        deeppink2 = 197,
        deeppink1 = 198,
        deeppink1_1 = 199,
        magenta2_1 = 200,
        magenta1 = 201,
        orangered1 = 202,
        indianred1 = 203,
        indianred1_1 = 204,
        hotpink = 205,
        hotpink_1 = 206,
        mediumorchid1_1 = 207,
        darkorange = 208,
        salmon1 = 209,
        lightcoral = 210,
        palevioletred1 = 211,
        orchid2 = 212,
        orchid1 = 213,
        orange1 = 214,
        sandybrown = 215,
        lightsalmon1 = 216,
        lightpink1 = 217,
        pink1 = 218,
        plum1 = 219,
        gold1 = 220,
        lightgoldenrod2_1 = 221,
        lightgoldenrod2_2 = 222,
        navajowhite1 = 223,
        mistyrose1 = 224,
        thistle1 = 225,
        yellow1 = 226,
        lightgoldenrod1 = 227,
        khaki1 = 228,
        wheat1 = 229,
        cornsilk1 = 230
    }
}
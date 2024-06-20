using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sm
{
    internal class CRender
    {
        internal static int CWidth = Console.WindowWidth;
        internal static int CHeight = Console.WindowHeight;
        internal static Color ActiveColor = Color.red;

        internal static void Write(Point _pos, string _text)
        {
            if (_pos.X >= CWidth) _pos.X = CWidth - 1;
            if (_pos.Y >= CHeight) _pos.Y = CHeight - 1;

            Console.SetCursorPosition(_pos.X, _pos.Y);
            Console.Write(_text);
        }

        internal static void Remove(Point _pos, Dimensions _dim)
        {
            for (int i = 0; i < _dim.Height; i++)
            {
                int y = _pos.Y + i;
                if (y >= CHeight) y = CHeight - 1;

                Console.SetCursorPosition(_pos.X, y);
                Console.Write(new string(' ', _dim.Width));
            }
        }

        internal static string BuildString(string _text, int width)
        {
            return string.Concat(Enumerable.Repeat(_text, width));
        }

        internal static string Border(Get _part)
        {
            return _part switch
            {
                Get.TopLeft => "┌",
                Get.TopRight => "┐",
                Get.BottomLeft => "└",
                Get.BottomRight => "┘",
                Get.Horizontal => "─",
                Get.HorizontalDown => "┬",
                Get.HorizontalUp => "┴",
                Get.Vertical => "│",
                Get.VerticalLeft => "├",
                Get.VerticalRight => "┤",
                Get.Cross => "┼",
                Get.ArrowDown => "↓",
                Get.ArrowRight => "→",
                Get.ArrowLeft => "←",
                _ => throw new InvalidOperationException("Unknown Global.Border part."),
            };
        }

        internal enum Get
        {
            TopLeft,
            TopRight,
            BottomLeft,
            BottomRight,
            Horizontal,
            HorizontalDown,
            HorizontalUp,
            Vertical,
            VerticalLeft,
            VerticalRight,
            Cross,
            ArrowDown,
            ArrowRight,
            ArrowLeft
        }
    }
}

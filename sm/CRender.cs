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

        internal static void SetPos(Point _pos)
        {
            if (_pos.X >= CWidth) _pos.X = CWidth - 1;
            if (_pos.Y >= CHeight) _pos.Y = CHeight - 1;
            Console.SetCursorPosition(_pos.X, _pos.Y);
        }

        internal static void Write(Point _pos, string _text)
        {
            SetPos(_pos);
            Console.Write(_text);
        }

        internal static void Remove(Point _pos, Dimensions _dim)
        {
            for (int i = 0; i < _dim.Height; i++)
            {
                SetPos(new Point(_pos.X, _pos.Y + i));
                Console.Write(string.Concat(Enumerable.Repeat(" ", _dim.Width)));
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
            ArrowDown
        }
    }
}

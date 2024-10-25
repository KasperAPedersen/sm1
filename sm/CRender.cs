using System.Drawing;

namespace sm
{
    internal class CRender
    {
        private static int _cWidth = Console.WindowWidth;
        private static int _cHeight = Console.WindowHeight;
        internal static Color ActiveColor = Color.aquamarine1;

        internal static void Write(Point pos, string text)
        {
            if (pos.X >= _cWidth) pos.X = _cWidth - 1;
            if (pos.Y >= _cHeight) pos.Y = _cHeight - 1;

            Console.SetCursorPosition(pos.X, pos.Y);
            Console.Write(text);
        }

        internal static void Remove(Point pos, Dimensions dim)
        {
            for (int i = 0; i < dim.Height; i++)
            {
                int y = pos.Y + i;
                if (y >= _cHeight) y = _cHeight - 1;

                Console.SetCursorPosition(pos.X, y);
                Console.Write(new string(' ', dim.Width));
            }
        }

        internal static string BuildString(string text, int width)
        {
            return string.Concat(Enumerable.Repeat(text, width));
        }

        internal static string Border(Get part)
        {
            return part switch
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

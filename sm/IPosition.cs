using System.Drawing;

namespace sm
{
    struct Position(Point absolute, Point relative)
    {
        public Point Absolute { get; set; } = absolute;
        public Point Relative { get; set; } = relative;
    }
    internal interface IPosition
    {
        Position Pos { get; set; }
    }
}

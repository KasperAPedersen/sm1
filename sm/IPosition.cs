using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sm
{
    struct Position(Point _absolute, Point _relative)
    {
        public Point Absolute { get; set; } = _absolute;
        public Point Relative { get; set; } = _relative;
    }
    internal interface IPosition
    {
        Position Pos { get; set; }
    }
}

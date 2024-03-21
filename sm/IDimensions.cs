using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sm
{
    struct Dimensions(int _x, int _y)
    {
        public int Width { get; set; } = _x;
        public int Height { get; set; } = _y;
    }

    internal interface IDimensions
    {
        Dimensions Dim { get; set; } 
    }
}

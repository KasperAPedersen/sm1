using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sm
{
    internal class CInput : CObject, IController
    {
        string Text = "a";

        public CInput(CObject _parent, Point _pos) : base(_parent, _pos, new Dimensions(1, 1))
        {
            Render();
        }

        internal override void Render()
        {
            Write(Pos.Absolute, Text);
        }

        public bool Run()
        {
            SetPos(Pos.Absolute);
            return true;
        }
    }
}
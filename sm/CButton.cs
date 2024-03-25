using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sm
{
    internal class CButton : CObject
    {
        List<object> Styling = new List<object>();
        string Text = "";

        public CButton(CObject _parent, Point _pos, Dimensions _dim, Align _align, string _text, List<object> _styles = null) : base(_parent, _pos, new Dimensions(_dim.Width < _text.Length + 2 ? _text.Length + 2 : _dim.Width, _dim.Height < 3 ? 3 : _dim.Height))
        {
            Text = _text;
            Styling = _styles;

            if (newObjPos(_parent, Aligner(_align, _parent, _pos), Dim)) Render();
        }

        internal override void Render()
        {
            int currentHeight = 0;
            string tmp = "";

            // Border of box 
            tmp = $"{Border(Get.TopLeft)}{string.Concat(Enumerable.Repeat(Border(Get.Horizontal), Dim.Width - 2))}{Border(Get.TopRight)}";
            tmp = CStyling.Set(tmp, CStyling.Get([typeof(BorderBgColor), typeof(BorderColor), typeof(BorderStyling)], Styling));
            Write(new Point(Pos.Absolute.X, Pos.Absolute.Y + currentHeight++), tmp);


            int maxWidth = Dim.Width - 2;
            int diff = maxWidth - Text.Length;
            int diffRem = diff % 2;

            // Border of text
            tmp = CStyling.Set($"{Border(Get.Vertical)}", CStyling.Get([typeof(BorderBgColor), typeof(BorderColor), typeof(BorderStyling)], Styling));
            tmp += $"{string.Concat(Enumerable.Repeat(" ", diff / 2))}";

            // Text styling
            tmp += CStyling.Set(Text, CStyling.Get([typeof(FontBgColor), typeof(FontColor), typeof(FontStyling)], Styling));
            tmp += $"{string.Concat(Enumerable.Repeat(" ", diff / 2 + diffRem))}";

            // Border of text
            tmp += CStyling.Set($"{Border(Get.Vertical)}", CStyling.Get([typeof(BorderBgColor), typeof(BorderColor), typeof(BorderStyling)], Styling));
            Write(new Point(Pos.Absolute.X, Pos.Absolute.Y + currentHeight++), tmp);

            // Border of box
            tmp = $"{Border(Get.BottomLeft)}{string.Concat(Enumerable.Repeat(Border(Get.Horizontal), Dim.Width - 2))}{Border(Get.BottomRight)}";
            tmp = CStyling.Set(tmp, CStyling.Get([typeof(BorderBgColor), typeof(BorderColor), typeof(BorderStyling)], Styling));
            Write(new Point(Pos.Absolute.X, Pos.Absolute.Y + currentHeight++), tmp);
        }

        internal override void ChangeStyling(List<object> _styles)
        {
            Styling = _styles;
            Remove(Pos.Absolute, Dim);
            RenderChildren();
        }
    }
}

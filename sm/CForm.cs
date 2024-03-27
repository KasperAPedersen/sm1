using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sm
{
    internal class CForm : CRender
    {
        CControllers controller; // controller obj
        public List<CObject> Objects = new List<CObject>(); // list of objs
        public List<string> values { get; set; } = new List<string>(); // list of obj values

        public CForm(CObject _parent, string _title, string[] _labels)
        {
            // Init a new controller
            controller = new CControllers();

            // Add the form to obj list
            CBox formBox = new(_parent, new Point(0, 0), new Dimensions(35, 30), Align.Middle, [BorderColor.lime]);
            CLabel title = new(formBox, new Point(0, 0), Align.Middle, _title, []);
            Objects.Add(formBox);
            Objects.Add(title);

            // Add the input fields to the controller list
            int labelHeight = 0;
            for(int i = 0; i < _labels.Length; i++)
            {
                controller.Add(new CInput(formBox, new Point(0, labelHeight += 3), new Dimensions(20, 3), _labels[i], Align.Middle, [BorderColor.yellow3_1]));
            }

            // Add obj to obj list
            foreach(CObject obj in controller.controllerObjects)
            {
                Objects.Add(obj);
                obj.shouldRender = true;
            }

            formBox.RenderChildren(); // Render each child of form

            // Run controller
            controller.Run(this, controller.controllerIndex);
        }

        internal void Finished(List<string> _values)
        {
            // Remove each obj
            foreach(CObject obj in Objects)
            {
                obj.shouldRender = false; // stop the child from rendering

                Remove(obj.Pos.Absolute, obj.Dim); // Remove the obj
                if(obj is CInput) // check if obj is CInput
                {
                    CInput o = (CInput)obj; // typecast obj to CINput
                    o.Text = ""; // Reset text of obj
                }
            }

            // Save values
            values = _values;
        }

        internal List<string> GetValues()
        {
            return values;
        }
    }
}

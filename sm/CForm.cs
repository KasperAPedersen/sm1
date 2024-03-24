using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sm
{
    internal class CForm : CController
    {
        public List<CObject> Objects = new List<CObject>();
        public List<string> values { get; set; } = new List<string>();
        public CForm(List<CObject> _list) 
        {
            Objects = _list;
            foreach (CObject obj in Objects)
            {
                if (obj is IController)
                {
                    controllerObjects.Add(obj);
                }
            }
        }

        internal void Finished(List<string> _values)
        {
            foreach(CObject obj in Objects)
            {
                Remove(obj.Pos.Absolute, obj.Dim);
                if(obj is CInput)
                {
                    CInput o = (CInput)obj;
                    o.Text = "";
                }
            }

            values = _values;
        }

        internal void Show()
        {
            foreach (CObject obj in Objects)
            {
                obj.Render();
            }

            CController.Run(this, controllerIndex);
        }

        // Get values func
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sm
{
    internal class CControllers : CRender
    {
        public List<CObject> ControllerObjects { get; set; } = [];
        public int controllerIndex = 0;

        public CControllers() { }

        internal void Add(CObject _obj)
        {
            ControllerObjects.Add(_obj);
        }

        internal void Run(CForm _form, int _index)
        {
            if (_index >= ControllerObjects.Count) _index = ControllerObjects.Count - 1;
            if (_index < 0) _index = 0;

            CObject obj = ControllerObjects[_index];
            ControllerState result = obj.Init();

            switch (result)
            {
                case ControllerState.Next:
                    Run(_form, ++_index);
                    break;
                case ControllerState.Previous:
                    Run(_form, --_index);
                    break;
                case ControllerState.Finish:
                    _form.Finished(GetValues());
                    return;
                case ControllerState.Cancel:
                    _form.Cancelled();
                    return;
                default:
                    break;
            }
        }

        internal List<string> GetValues()
        {
            List<string> values = [];
            foreach (object obj in ControllerObjects)
            {
                if (obj is CInput)
                {
                    CInput o = (CInput)obj;
                    values.Add(o.Text);
                }

                if (obj is CComboBox)
                {
                    CComboBox o = (CComboBox)obj;
                    values.Add(o.Text);
                }
            }

            return values;
        }
    }

    enum ControllerState
    {
        Idle,
        Previous,
        Next,
        Finish,
        Cancel
    }
}

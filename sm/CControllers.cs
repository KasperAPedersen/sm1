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
        public List<CObject> controllerObjects { get; set; } = new List<CObject>();
        public int controllerIndex = 0;

        public CControllers() { }

        internal void Add(CObject _obj)
        {
            controllerObjects.Add(_obj);
        }

        internal void Run(CForm _form, int _index)
        {
            // Correcting index for first, last
            if (_index >= controllerObjects.Count) _index = controllerObjects.Count - 1;
            if (_index < 0) _index = 0;

            // Get & init obj
            CObject obj = controllerObjects[_index];
            ControllerState result = obj.Init();

            // switch for the controllerState result
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
                default:
                    break;
            }
        }

        internal List<string> GetValues()
        {
            List<string> values = new List<string>();
            foreach (CInput obj in controllerObjects)
            {
                values.Add(obj.Text);
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

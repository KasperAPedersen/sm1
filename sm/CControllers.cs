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
        public List<CObject> controllerObjects { get; set; } = new List<CObject>(); // obj list
        public int controllerIndex = 0; // obj indexer


        public CControllers() { }


        // Func to add the objects to obj array
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
                    Run(_form, ++_index); // run next obj
                    break;
                case ControllerState.Previous:
                    Run(_form, --_index); // run prev obj
                    break;
                case ControllerState.Finish:
                    _form.Finished(GetValues()); // Save values to CForm obj
                    return;
                default:
                    break;
            }
        }

        internal List<string> GetValues()
        {
            // Create new string list
            List<string> values = new List<string>();
            foreach (CInput obj in controllerObjects)
            {
                // Add every obj to string list
                values.Add(obj.Text); 
            }

            // return  string list
            return values;
        }
    }

    //  controller states
    enum ControllerState
    {
        Idle,
        Previous,
        Next,
        Finish,
        Cancel
    }
}

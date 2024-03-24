using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

namespace sm
{
    internal class CController : CRender
    {
        // List of controller objects
        public static List<CObject> controllerObjects { get; set; } = new List<CObject>();
        public static int controllerIndex = 0;

        // run controllers objects run func
        internal static void Run(CForm _form, int _index)
        {
            // correct _index for next
            if (_index >= controllerObjects.Count) _index = controllerObjects.Count - 1;

            // Correct _index for previous
            if (_index < 0 ) _index = 0;

            CObject obj = controllerObjects[_index];
            ControllerState result = obj.Init();

            switch(result)
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
                    Idle();
                    break;
            }
        }

        // Idle
        internal static void Idle()
        {
            CRender.SetPos(new Point(Console.WindowWidth - 5, Console.WindowHeight));
        }

        internal static List<string> GetValues()
        {
            List<string> values = new List<string>();
            foreach(CInput obj in controllerObjects)
            {
                values.Add(obj.Text);
            }

            Idle();
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

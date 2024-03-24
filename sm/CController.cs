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
        static public List<CObject> controllerObjects { get; set; } = new List<CObject>();
        static public List<string> controllerObjectsValues { get; set; } = new List<string>();
        static public int controllerIndex = 0;

        // run controllers objects run func
        internal static void Run(int _index)
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
                    Run(++_index);
                    break;
                case ControllerState.Previous:
                    Run(--_index);
                    break;
                case ControllerState.Finish:
                    Finished();
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

        internal static void Finished()
        {
            Idle();
            foreach(CInput obj in controllerObjects)
            {
                controllerObjectsValues.Add(obj.Text);
            }
        }

        internal static List<string> GetValues()
        {
            return controllerObjectsValues;
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

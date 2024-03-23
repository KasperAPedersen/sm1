using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sm
{
    internal class CController : CRender
    {
        // List of controller objects
        static public List<CObject> controllerObjects { get; set; } = new List<CObject>();
        int index = 0;

        // run controllers objects run func

        // if current object run func returns false, - return to previous controller object

        // if current object run func returns true, - continue to next controller object
    }
}

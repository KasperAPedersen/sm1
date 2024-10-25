namespace sm
{
    internal class CControllers : CRender
    {
        public List<CObject> ControllerObjects { get; set; } = [];
        public readonly int ControllerIndex = 0;

        internal void Add(CObject obj)
        {
            ControllerObjects.Add(obj);
        }

        internal void Run(CForm form, int index)
        {
            if (index >= ControllerObjects.Count) index = ControllerObjects.Count - 1;
            if (index < 0) index = 0;

            CObject obj = ControllerObjects[index];
            ControllerState result = obj.Init();

            switch (result)
            {
                case ControllerState.Next:
                    form.ReDrawBtns();
                    Run(form, ++index);
                    break;
                case ControllerState.Previous:
                    Run(form, --index);
                    break;
                case ControllerState.Finish:
                    form.Finished(GetValues());
                    return;
                case ControllerState.Cancel:
                    form.Cancelled();
                    return;
            }
        }

        private List<string> GetValues()
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

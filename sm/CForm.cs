using System.Drawing;

namespace sm
{
    internal class CForm : CRender
    {
        private readonly CObject _parent;
        readonly CControllers controller;
        private readonly List<CObject> _objects = [];
        public bool IsFinished { get; private set; }
        private List<string> Values { get; set; } = [];
        private readonly CBox _formBox;

        readonly int _currentComboBox, _currentInputField;

        //
        public CForm(CObject parent, string title, List<string> objects, List<Type> types, List<string>? inputLabelsValue = null, List<List<string>>? comboLabels = null, List<int>? comboLabelsValue = null)
        {
            controller = new CControllers();
            _parent = parent;

            _formBox = new(parent, new Point(0, 0), new Dimensions(35, 45), new CStyleBuilder().AddBorder(CRender.ActiveColor).Build(), Align.Middle);
            CLabel label = new(_formBox, new Point(0, 0), Align.Middle, title, new CStyleBuilder().Build());
            _objects.Add(_formBox);
            _objects.Add(label);

            int labelHeight = 0;
            for(int i = 0; i < objects.Count; i++)
            {
                if (types[i].Name == nameof(CInput))
                {
                    controller.Add(new CInput(_formBox, new Point(0, labelHeight += 3), new Dimensions(_formBox.Dim.Width, 3), objects[i], new CStyleBuilder().AddBorder(Color.white).Build(), Align.Middle));
                }

                if (types[i].Name == nameof(CComboBox) && comboLabels != null)
                {
                    List<string> tmp = [];

                    foreach (string s in comboLabels[_currentComboBox++])
                    {
                        tmp.Add(s);
                    }

                    controller.Add(new CComboBox(_formBox, new Point(0, labelHeight += 3), new Dimensions(_formBox.Dim.Width, 3), tmp, new CStyleBuilder().AddFont(Color.white).AddBorders([Color.white]).Build(), Align.Middle));
                }
            }

            _currentComboBox = 0;

            // Render children & set values to input fields
            foreach(CObject obj in controller.ControllerObjects)
            {
                _objects.Add(obj);
                obj.ShouldRender = true;


                if (obj is CInput input)
                {
                    if(inputLabelsValue?.Count > _currentInputField)
                    {
                        string tmp = inputLabelsValue[_currentInputField++];
                        CInput ob = input;
                        ob.Text = tmp;
                    }
                }

                if(obj is CComboBox combo)
                {
                    if(comboLabelsValue?.Count > _currentComboBox)
                    {
                        CComboBox ob = combo;
                        ob.SelectIndex = comboLabelsValue[_currentComboBox++];
                    }
                }
            }

            controller.Add(new CButton(_formBox, new Point(0, _formBox.Dim.Height - 4), new Dimensions(_formBox.Dim.Width / 2 - 2, 3), Align.Left, "Confirm", new CStyleBuilder().Build()));
            controller.Add(new CButton(_formBox, new Point(0, _formBox.Dim.Height - 4), new Dimensions(_formBox.Dim.Width / 2 - 2, 3), Align.Right, "Cancel", new CStyleBuilder().Build()));

            _formBox.RenderChildren();
            controller.Run(this, controller.ControllerIndex);
        }
        //

        internal void Finished(List<string> values)
        {
            foreach (CObject obj in _objects)
            {
                obj.ShouldRender = false;

                if (obj is CInput)
                {
                    CInput o = (CInput)obj;
                    o.Text = "";
                }

                if (obj is CComboBox)
                {
                    CComboBox o = (CComboBox)obj;
                    o.Text = "";
                }
            }

            Remove(_formBox.Pos.Absolute, _formBox.Dim);

            Values = values;
            _parent.RenderChildren();
            IsFinished = true;
        }

        internal void Cancelled()
        {
            foreach (CObject obj in _objects)
            {
                obj.ShouldRender = false;

                if (obj is CInput)
                {
                    CInput o = (CInput)obj;
                    o.Text = "";
                }

                if (obj is CComboBox)
                {
                    CComboBox o = (CComboBox)obj;
                    o.Text = "";
                }
            }

            _parent.RenderChildren();
            IsFinished = false;
        }

        internal void ReDrawBtns()
        {
            _ = new CButton(_formBox, new Point(0, _formBox.Dim.Height - 4), new Dimensions(_formBox.Dim.Width / 2 - 2, 3), Align.Left, "Confirm", new CStyleBuilder().Build());
            _ = new CButton(_formBox, new Point(0, _formBox.Dim.Height - 4), new Dimensions(_formBox.Dim.Width / 2 - 2, 3), Align.Right, "Cancel", new CStyleBuilder().Build());
        }

        internal List<string> GetValues()
        {
            return Values;
        }
    }
}

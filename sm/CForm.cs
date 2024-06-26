using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sm
{
    /*internal class CForm : CRender
    {
        readonly CObject Parent;
        readonly CControllers controller;
        public List<CObject> Objects = [];
        public bool IsFinished { get; set; }
        public List<string> Values { get; set; } = [];
        readonly CBox formBox;

        public CForm(CObject _parent, string _title, string[]? _inputLabels = null, List<string>? _inputLabelsValue = null, List<List<string>>? _comboLabes = null)
        {
            controller = new CControllers();
            Parent = _parent;

            formBox = new(_parent, new Point(0, 0), new Dimensions(35, 45), new CStyleBuilder().AddBorder(CRender.ActiveColor).Build(), Align.Middle);
            CLabel title = new(formBox, new Point(0, 0), Align.Middle, _title, new CStyleBuilder().Build());
            Objects.Add(formBox);
            Objects.Add(title);

            int labelHeight = 0;
            if (_inputLabels != null)
            {
                for (int i = 0; i < _inputLabels.Length; i++)
                {
                    controller.Add(new CInput(formBox, new Point(0, labelHeight += 3), new Dimensions(formBox.Dim.Width, 3), _inputLabels[i], new CStyleBuilder().AddBorder(Color.white).Build(), Align.Middle));
                }
            }

            if (_comboLabes != null)
            {
                for (int i = 0; i < _comboLabes.Count; i++)
                {
                    List<string> tmp = [];

                    foreach(string s in _comboLabes[i])
                    {
                        tmp.Add(s);
                    }

                    controller.Add(new CComboBox(formBox, new Point(0, labelHeight += 3), new Dimensions(formBox.Dim.Width, 3), tmp, new CStyleBuilder().AddFont(Color.white).AddBorders([Color.white]).Build(), Align.Middle));
                }
            }

            for (int i = 0; i < controller.ControllerObjects.Count; i++)
            {
                CObject obj = controller.ControllerObjects[i];
                Objects.Add(obj);
                obj.ShouldRender = true;

                if (obj is CInput input)
                {
                    for (int o = 0; o < _inputLabelsValue?.Count; o++)
                    {
                        if (_inputLabelsValue[o] == null) break;

                        if (o == i)
                        {

                            CInput ob = input;
                            ob.Text = _inputLabelsValue[o];
                        }
                    }
                }
            }

            controller.Add(new CButton(formBox, new Point(0, formBox.Dim.Height - 4), new Dimensions(formBox.Dim.Width / 2 - 2, 3), Align.Left, "Confirm", new CStyleBuilder().Build()));
            controller.Add(new CButton(formBox, new Point(0, formBox.Dim.Height - 4), new Dimensions(formBox.Dim.Width / 2 - 2, 3), Align.Right, "Cancel", new CStyleBuilder().Build()));

            formBox.RenderChildren();

            controller.Run(this, controller.controllerIndex);
        }

        internal void Finished(List<string> _values)
        {
            foreach (CObject obj in Objects)
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

            Remove(formBox.Pos.Absolute, formBox.Dim);

            Values = _values;
            Parent.RenderChildren();
            IsFinished = true;
        }

        internal void Cancelled()
        {
            foreach (CObject obj in Objects)
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

            Parent.RenderChildren();
            IsFinished = false;
        }

        internal void ReDrawBtns()
        {
            new CButton(formBox, new Point(0, formBox.Dim.Height - 4), new Dimensions(formBox.Dim.Width / 2 - 2, 3), Align.Left, "Confirm", new CStyleBuilder().Build());
            new CButton(formBox, new Point(0, formBox.Dim.Height - 4), new Dimensions(formBox.Dim.Width / 2 - 2, 3), Align.Right, "Cancel", new CStyleBuilder().Build());
        }

        internal List<string> GetValues()
        {
            return Values;
        }
    }*/


    // ----------------------
    internal class CForm : CRender
    {
        readonly CObject Parent;
        readonly CControllers controller;
        public List<CObject> Objects = [];
        public bool IsFinished { get; set; }
        public List<string> Values { get; set; } = [];
        readonly CBox formBox;

        readonly int currentComboBox = 0, currentInputField = 0;

        //
        public CForm(CObject _parent, string _title, List<string> _objects, List<Type> _types, List<string>? _inputLabelsValue = null, List<List<string>>? _comboLabels = null, List<int>? _comboLabelsValue = null)
        {
            controller = new CControllers();
            Parent = _parent;

            formBox = new(_parent, new Point(0, 0), new Dimensions(35, 45), new CStyleBuilder().AddBorder(CRender.ActiveColor).Build(), Align.Middle);
            CLabel title = new(formBox, new Point(0, 0), Align.Middle, _title, new CStyleBuilder().Build());
            Objects.Add(formBox);
            Objects.Add(title);

            int labelHeight = 0;
            for(int i = 0; i < _objects.Count; i++)
            {
                if (_types[i].Name == typeof(CInput).Name.ToString())
                {
                    controller.Add(new CInput(formBox, new Point(0, labelHeight += 3), new Dimensions(formBox.Dim.Width, 3), _objects[i], new CStyleBuilder().AddBorder(Color.white).Build(), Align.Middle));
                }

                if (_types[i].Name == typeof(CComboBox).Name.ToString() && _comboLabels != null)
                {
                    List<string> tmp = [];

                    foreach (string s in _comboLabels[currentComboBox++])
                    {
                        tmp.Add(s);
                    }

                    controller.Add(new CComboBox(formBox, new Point(0, labelHeight += 3), new Dimensions(formBox.Dim.Width, 3), tmp, new CStyleBuilder().AddFont(Color.white).AddBorders([Color.white]).Build(), Align.Middle));
                }
            }

            currentComboBox = 0;

            // Render children & set values to input fields
            for (int i = 0; i < controller.ControllerObjects.Count; i++)
            {
                CObject obj = controller.ControllerObjects[i];
                Objects.Add(obj);
                obj.ShouldRender = true;


                if (obj is CInput input)
                {
                    if(_inputLabelsValue?.Count > currentInputField)
                    {
                        string tmp = _inputLabelsValue[currentInputField++];
                        CInput ob = input;
                        ob.Text = tmp;
                    }
                }

                if(obj is CComboBox combo)
                {
                    if(_comboLabelsValue?.Count > currentComboBox)
                    {
                        CComboBox ob = combo;
                        ob.selectIndex = _comboLabelsValue[currentComboBox++];
                    }
                }
            }

            controller.Add(new CButton(formBox, new Point(0, formBox.Dim.Height - 4), new Dimensions(formBox.Dim.Width / 2 - 2, 3), Align.Left, "Confirm", new CStyleBuilder().Build()));
            controller.Add(new CButton(formBox, new Point(0, formBox.Dim.Height - 4), new Dimensions(formBox.Dim.Width / 2 - 2, 3), Align.Right, "Cancel", new CStyleBuilder().Build()));

            formBox.RenderChildren();

            controller.Run(this, controller.controllerIndex);
        }
        //

        internal void Finished(List<string> _values)
        {
            foreach (CObject obj in Objects)
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

            Remove(formBox.Pos.Absolute, formBox.Dim);

            Values = _values;
            Parent.RenderChildren();
            IsFinished = true;
        }

        internal void Cancelled()
        {
            foreach (CObject obj in Objects)
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

            Parent.RenderChildren();
            IsFinished = false;
        }

        internal void ReDrawBtns()
        {
            _ = new CButton(formBox, new Point(0, formBox.Dim.Height - 4), new Dimensions(formBox.Dim.Width / 2 - 2, 3), Align.Left, "Confirm", new CStyleBuilder().Build());
            _ = new CButton(formBox, new Point(0, formBox.Dim.Height - 4), new Dimensions(formBox.Dim.Width / 2 - 2, 3), Align.Right, "Cancel", new CStyleBuilder().Build());
        }

        internal List<string> GetValues()
        {
            return Values;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sm
{
    internal class CForm : CRender
    {
        CObject Parent;
        readonly CControllers controller;
        public List<CObject> Objects = [];
        public bool IsFinished {  get; set; }
        public List<string> values { get; set; } = [];
        CBox formBox;

        public CForm(CObject _parent, string _title, string[]? _inputLabels = null, List<string>? _inputLabelsValue = null, List<List<string>>? _comboLabes = null)
        {
            controller = new CControllers();
            Parent = _parent;

            formBox = new(_parent, new Point(0, 0), new Dimensions(35, 30), Align.Middle, [BorderColor.lime]);
            CLabel title = new(formBox, new Point(0, 0), Align.Middle, _title, []);
            Objects.Add(formBox);
            Objects.Add(title);
            
            int labelHeight = 0;
            if(_inputLabels != null)
            {
                for (int i = 0; i < _inputLabels.Length; i++)
                {
                    controller.Add(new CInput(formBox, new Point(0, labelHeight += 3), new Dimensions(formBox.Dim.Width, 3), _inputLabels[i], Align.Middle, [BorderColor.yellow3_1]));
                }
            }

            if (_comboLabes != null)
            {
                for (int i = 0; i < _comboLabes.Count; i++)
                {
                    controller.Add(new CComboBox(formBox, new Point(0, labelHeight += 3), new Dimensions(formBox.Dim.Width, 3), ["Mr.", "Mrs."], Align.Middle, [BorderColor.yellow3_1]));
                }
            }

            for(int i = 0; i < controller.controllerObjects.Count; i++)
            {
                CObject obj = controller.controllerObjects[i];
                Objects.Add(obj);
                obj.shouldRender = true;

                if(obj is CInput)
                {
                    for(int o = 0; o < _inputLabelsValue.Count; o++)
                    {
                        if (_inputLabelsValue[o] == null) break;

                        if (o == i)
                        {

                            CInput ob = (CInput)obj;
                            ob.Text = _inputLabelsValue[o];
                        }
                    }
                }
            }

            controller.Add(new CButton(formBox, new Point(0, formBox.Dim.Height - 4), new Dimensions(formBox.Dim.Width / 2 - 2, 3), Align.Left, "Confirm"));
            controller.Add(new CButton(formBox, new Point(0, formBox.Dim.Height - 4), new Dimensions(formBox.Dim.Width / 2 - 2, 3), Align.Right, "Cancel"));

            formBox.RenderChildren();

            controller.Run(this, controller.controllerIndex);
        }

        internal void Finished(List<string> _values)
        {
            foreach(CObject obj in Objects)
            {
                obj.shouldRender = false;

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

            values = _values;
            Parent.RenderChildren();
            IsFinished = true;
        }

        internal void Cancelled()
        {
            foreach (CObject obj in Objects)
            {
                obj.shouldRender = false;

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

        internal List<string> GetValues()
        {
            return values;
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sm
{
    internal class CInput : CObject, IController
    {
        CStyle Style;
        public string Text { get; set; } = "";
        public string Label { get; set; } = "";
        readonly int maxLength = 15;
        bool isActive = false;

        public CInput(CObject _parent, Point _pos, Dimensions _dim, CStyle _style) : this(_parent, _pos, _dim, "", _style) { }
        public CInput(CObject _parent, Point _pos, Dimensions _dim, string _label, CStyle _style, Align _align = Align.None) : base(_parent, _pos, _dim)
        {
            ShouldRender = false;
            if (_label != null) Label = _label;
            maxLength = _dim.Width - 8;

            Style = _style;
            if (ShouldRender && NewObjPos(_parent, Aligner(_align, _parent, _pos), Dim)) Render();
        }

        internal override void Render()
        {
            int currentHeight = 0;
            string tmp;

            if (Text.Length > Dim.Width - 4) Text = Text[..(Dim.Width - 4)]; 

            tmp = Border(Get.TopLeft) + BuildString(Border(Get.Horizontal), Dim.Width - 2) + Border(Get.TopRight);
            tmp = isActive ? Style.Set(tmp, [CRender.ActiveColor]) : Style.Set(tmp, Style.Border);
            Write(new Point(Pos.Absolute.X, Pos.Absolute.Y + currentHeight++), tmp);

            tmp = Border(Get.Vertical) + BuildString(" ", Dim.Width - 2) + Border(Get.Vertical);
            tmp = isActive ? Style.Set(tmp, [CRender.ActiveColor]) : Style.Set(tmp, Style.Border);
            Write(new Point(Pos.Absolute.X, Pos.Absolute.Y + currentHeight), tmp);

            tmp = (Label != "" && Text == "") ? Style.Set(Label, [Color.grey]) : Text;
            tmp = Style.Set(tmp, Style.Font);
            Write(new Point(Pos.Absolute.X + 2, Pos.Absolute.Y + currentHeight++), tmp);

            tmp = Border(Get.BottomLeft) + BuildString(Border(Get.Horizontal), Dim.Width - 2) + Border(Get.BottomRight);
            tmp = isActive ? Style.Set(tmp, [CRender.ActiveColor]) : Style.Set(tmp, Style.Border);
            Write(new Point(Pos.Absolute.X, Pos.Absolute.Y + currentHeight++), tmp);
        }

        internal override ControllerState Init()
        {
            Console.CursorVisible = true;
            updateActiveField(true);

            bool keepRunning = true;
            while (keepRunning)
            {

                Console.SetCursorPosition(Pos.Absolute.X + Text.Length + 2, Pos.Absolute.Y + 1);
                ConsoleKeyInfo key = Console.ReadKey();
                switch (key.Key)
                {
                    case ConsoleKey.Tab:
                        switch(key.Modifiers)
                        {
                            case ConsoleModifiers.Shift:
                                updateActiveField(false);
                                return ControllerState.Previous;
                            default:
                                updateActiveField(false);
                                return ControllerState.Next;
                        }
                    case ConsoleKey.DownArrow:
                    case ConsoleKey.Enter:
                        updateActiveField(false);
                        return ControllerState.Next;
                    case ConsoleKey.UpArrow:
                        updateActiveField(false);
                        return ControllerState.Previous;
                    case ConsoleKey.Escape:
                        return ControllerState.Cancel;
                    case ConsoleKey.Backspace:
                        Text = Text != "" ? Text.Remove(Text.Length - 1, 1) : "";
                        break;
                    default:
                        if (Text.Length < maxLength - 1) Text += key.KeyChar;
                        break;
                }

                Render();
            }
            return ControllerState.Idle;
        }

        internal override void ChangeStyling(CStyle _style)
        {
            Style = _style;
            Remove(Pos.Absolute, Dim);
            RenderChildren();
        }

        internal void updateActiveField(bool _active)
        {
            isActive = _active;
            Render();
        }
    }
}
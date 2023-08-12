using System;
using UnityEditor;
using UnityEngine;

namespace Smart
{
    public partial class LevelEditor
    {
        void ControlActive()
        {
            if (KeyUp(KeyCode.Escape))
            {
                active = !active;

                editor.PreviewGUIChanged();

                RepaintAll();

                e.Use();
            }
        }

        void ControlMode()
        {
            if (e.control) { return; }

            if (KeyDown(KeyCode.C))
            {
                Tools.current = Tool.None;

                Array vals = Enum.GetValues(typeof(Mode));

                int current = (int)mode;

                bool greater = (current + 1) > (vals.Length - 1);

                current = greater ? 0 : current + 1;

                mode = (Mode)current;

                editor.PreviewGUIChanged();

                RepaintAll();
            }
        }

        void ControlPaint()
        {
            if (e.control) { return; }

            if (KeyDown(KeyCode.B))
            {
                switch (paint)
                {
                    case Paint.Brush:
                        paint = Paint.Single;
                        break;
                    case Paint.Single:
                        paint = Paint.Brush;
                        break;
                }

                RepaintAll();
            }
        }

        void ControlHeight()
        {
            if (KeyDown(KeyCode.Plus) || KeyDown(KeyCode.Equals))
            {
                height += size.y;

                RepaintAll();
            }

            if (KeyDown(KeyCode.Minus))
            {
                height -= size.y;

                RepaintAll();
            }
        }

        void ControlTools()
        {
            // Exit
            if (tool == Tools.current)
            {
                return;
            }

            switch (Tools.current)
            {
                case Tool.Move:
                    if (mode == Mode.Move) { return; }
                    mode = Mode.Move;
                    break;
                case Tool.Rotate:
                    if (mode == Mode.Rotate) { return; }
                    mode = Mode.Rotate;
                    break;
                case Tool.Scale:
                    if (mode == Mode.Scale) { return; }
                    mode = Mode.Scale;
                    break;
                case Tool.Rect:
                    if (mode == Mode.Rect) { return; }
                    mode = Mode.Rect;
                    break;
            }

            editor.PreviewGUIChanged();

            tool = Tools.current;
        }
    }
}
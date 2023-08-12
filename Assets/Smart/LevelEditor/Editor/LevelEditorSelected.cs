using UnityEditor;
using UnityEngine;

namespace Smart
{
    using static EditorGUILayout;
    using GL = GUILayout;

    public partial class LevelEditor
    {
        void DRAW_SELECTED()
        {
            BeginVertical("FrameBox");
            DRAW_SELECTED_CONTENTS();
            EndVertical();
        }

        void DRAW_SELECTED_CONTENTS()
        {

            if (selected.Count == 0)
            {
                GL.Label("Nothing Selected", "CenteredLabel");
                return;
            }

            int maxColumns = (int)(position.width / imgWidth1);
            int rowElements = 0;
            bool rowOpen = false;

            for (int i = 0; i < selected.Count; i++)
            {
                if (rowElements == 0) { BeginHorizontal(); rowOpen = true; }
                BeginVertical();
                bool removed = RemoveButton(selected[i], buttonOps80);
                
                if(!removed && selected[i])
                {
                    GL.Label(selected[i].name, GL.ExpandWidth(false));
                }

                EndVertical();
                rowElements++;
                if (rowElements == maxColumns) { EndHorizontal(); rowOpen = false; rowElements = 0; }

                if (removed) { break; }
            }
            if (rowOpen) { EndHorizontal(); }
        }
    }
}

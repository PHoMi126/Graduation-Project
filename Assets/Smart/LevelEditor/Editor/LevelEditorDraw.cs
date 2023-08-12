using UnityEditor;
using UnityEngine;

namespace Smart
{
    using static EditorGUILayout;
    using static EditorGUIUtility;
    using GL = GUILayout;

    public partial class LevelEditor
    {

        GUILayoutOption[] buttonOps80 = new GUILayoutOption[]
        {
            GL.Width(imgWidth1),
            GL.Height(imgHeight1)
        };

        GUILayoutOption[] buttonOps100 = new GUILayoutOption[]
        {
            GL.Width(imgWidth2),
            GL.Height(imgHeight2)
        };

        void CREATE_DATA_GUI()
        {
            if (GL.Button("Create Data", "LargeButton"))
            {
                string path = EditorUtility.SaveFilePanelInProject("", "", "asset", "");

                if(string.IsNullOrWhiteSpace(path))
                {
                    return;
                }

                data = CreateInstance<LevelEditorData>();

                AssetDatabase.CreateAsset(data, path);

                AssetDatabase.Refresh();
            }
        }

        bool RemoveButton(Object value, params GUILayoutOption[] ops)
        {
            Texture2D tex = GetPreview(value);

            if (GL.Button(tex, "label", ops))
            {
                selected.Remove(value);

                if(active)
                    editor.CreatePreviews();

                if (view)
                    view.Repaint();

                RepaintAll();

                return true;
            }

            return false;
        }

        void Separator()
        {
            GL.Space(standardVerticalSpacing * 4);
        }

        void SceneGUIButtons()
        {
            Vector3 x = new Vector3(1, 0, 0);
            Vector3 y = new Vector3(0, 1, 0);
            Vector3 z = new Vector3(0, 0, 1);

            Handles.BeginGUI();
            GL.FlexibleSpace();
            BeginHorizontal();

            EditorGUI.BeginChangeCheck();
            GL.Toggle(rotationAxis == x, "X", "Button");
            if (EditorGUI.EndChangeCheck())
            {
                rotationAxis = x;
            }

            EditorGUI.BeginChangeCheck();
            GL.Toggle(rotationAxis == y, "Y", "Button");
            if (EditorGUI.EndChangeCheck())
            {
                rotationAxis = y;
            }


            EditorGUI.BeginChangeCheck();
            GL.Toggle(rotationAxis == z, "Z", "Button");
            if (EditorGUI.EndChangeCheck())
            {
                rotationAxis = z;
            }

            HelpBox("Press (x) to rotate", MessageType.None);

            GL.FlexibleSpace();
            EndHorizontal();
            GL.Space(25);
            Handles.EndGUI();
        }
    }
}

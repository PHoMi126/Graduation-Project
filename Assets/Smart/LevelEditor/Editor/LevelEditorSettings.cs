using UnityEditor;
using UnityEngine;

namespace Smart
{
    using static EditorGUI;
    using static EditorGUILayout;
    using static EditorGUIUtility;
    using GL = GUILayout;

    public partial class LevelEditor : EditorWindow
    {
        void OnDrawGUI()
        {
            if (null == data) { CREATE_DATA_GUI(); return; }

            e = Event.current;

            wideMode = true;

            if (isSettings)
            {
                SETTINGS_GUI();
                return;
            }

            GROUPS_BUTTON_LIST_GUI();

            DRAW_SELECTED();

            mainScroll = BeginScrollView(mainScroll);
            GROUPS_GUI();
            EndScrollView();
        }

        static LevelEditor settingsWindow;

        void OpenSettingsButton()
        {
            if(settingsWindow)
            {
                return;
            }

            GUIContent content = IconContent("d_SettingsIcon@2x");

            GUILayoutOption[] op =
            {
                GL.Width(singleLineHeight * 1.5f),
                GL.Height(singleLineHeight * 1.5f),
                GL.ExpandHeight(true)
            };

            if(GL.Button(content, "button", op ))
            {
                settingsWindow = CreateInstance<LevelEditor>();
                settingsWindow.titleContent.text = "Level Editor Settings";
                settingsWindow.isSettings = true;
                settingsWindow.Show();
            }
        }

        void SETTINGS_GUI()
        {
            if (false == isSettings) { return; }

            mainScroll = BeginScrollView(mainScroll);
            BeginVertical("Framebox");
            SETTINGS_FIELDS_GUI();
            EndVertical();
            EndScrollView();

            if (GL.Button("Close"))
            {
                Close();
            }
        }

        void SETTINGS_FIELDS_GUI()
        {
            // Label
            GL.Label("Settings", "boldlabel");
            // Active
            ActiveField();
            // 2D
            view2D = GL.Toggle(view2D, "2D");

            Separator();
            // Mode
            ModeField();
            // Paint
            paint = (Paint)EnumPopup("Paint (B)", paint);
            // Layer
            layer = LayerField("Layer", layer);
            // Size
            SizeField();
            // Height
            height = FloatField("Height(+/ -)", height);

            // Label
            Separator();
            GL.Label("Snap", "boldlabel");
            // Move
            move = Vector3Field("Move", move);
            // Rotate
            rotate = Vector3Field("Rotate", rotate);
            // Scale
            scale = Vector3Field("Scale", scale);

            Separator();
            GL.Label("Parent", "boldlabel");

            if(Selection.activeTransform)
            {
                GL.Label($"Children Count: {Selection.activeTransform.childCount}");
            }

            maxChildCount = IntField("Max Child Count", maxChildCount);

            UtilityGUI();
        }

        void UtilityGUI()
        {
            Separator();
            GL.Label("Developer Settings", "boldlabel");

            //for(int i = 0; i < previews.Count; i++)
            //{
            //    ObjectField(previews[i], previews[i].GetType(), false);
            //}
            
            HelpBox("Use this if there are some GameObjects visible on your Scene but not on the Hierarchy", MessageType.Info);

            if(GL.Button("Show all"))
            {
                if(EditorUtility.DisplayDialog("Level Editor", "Show all hidden GameObjects in Hierarchy?", "Yes", "No"))
                {
                    GameObject[] allGameObjects = FindObjectsOfType<GameObject>();

                    for (int i = 0; i < allGameObjects.Length; i++)
                    {
                        if (allGameObjects[i].hideFlags == HideFlags.HideInHierarchy)
                        {
                            allGameObjects[i].hideFlags = HideFlags.None;
                        }
                    }
                }
            }
        }

        void ActiveField()
        {
            BeginChangeCheck();
            active = GL.Toggle(active, "Edit (ESC)");
            if (EndChangeCheck())
            {
                if(null == editor)
                {
                    editor = CreateInstance<LevelEditor>();
                    editor.titleContent.text = "Level Editor";
                    editor.isSettings = false;
                    editor.Show();
                }

                editor.PreviewGUIChanged();
            }
        }

        void SizeField()
        {
            BeginChangeCheck();
            size = Vector3IntField("Size", size);
            if (EndChangeCheck())
            {
                size.x = Mathf.Max(0, size.x);
                size.y = Mathf.Max(0, size.y);
                size.z = Mathf.Max(0, size.z);

                editor.PreviewGUIChanged();
            }
        }

        void ModeField()
        {
            BeginChangeCheck();
            mode = (Mode)EnumPopup("Mode (C)", mode);
            if (EndChangeCheck())
            {
                editor.PreviewGUIChanged();
            }
        }

        void LayerField()
        {
            BeginChangeCheck();
            layer = LayerField("Layer", layer);
            if (EndChangeCheck())
            {
                Tools.visibleLayers = 1 << layer;
                SceneView.RepaintAll();
            }
        }
    }
}
using UnityEditor;
using UnityEngine;

namespace Smart
{
    using static EditorGUILayout;
    using static EditorGUIUtility;
    using GL = GUILayout;

    public partial class LevelEditor
    {
        bool createGroup;
        string newGroupName;
        private bool renameGroup;
        private string renameGroupValue;
        private Vector2 mainScroll;

        void GROUPS_GUI()
        {
            Rect rect = BeginVertical("FrameBox");
            DRAW_GROUP_CONTENTS();
            GL.FlexibleSpace();
            EndVertical();

            GROUP_EVENTS(rect);
        }

        void DRAW_GROUP_CONTENTS()
        {
            //GROUP_SETTINGS_GUI();
            //GROUP_TITLE_GUI();
            GROUP_OBJECTS_GUI();
        }

        void SearchBarGUI()
        {
            BeginHorizontal();
            DrawSearchBarField();
            EndHorizontal();
        }

        void DrawSearchBarField()
        {
            filter = GL.TextField(filter, "SearchTextField");
            if (GL.Button("", "SearchCancelButton")) { filter = ""; }
        }

        void GROUPS_BUTTON_LIST_GUI()
        {
            GUIStyle style = new("Button")
            {
                alignment = TextAnchor.MiddleLeft
            };

            GUIStyle title = new("BoldLabel")
            {
                fontSize = 14
            };

            GUIStyle toolbar = new("Toolbar")
            {
                fixedHeight = 0,
                stretchHeight = true
            };

            BeginHorizontal(toolbar);
            CreateGroupButton();
            GL.Label("Groups:", title, GL.ExpandHeight(true));
            GL.FlexibleSpace();
            OpenSettingsButton();
            EndHorizontal();

            bool dataSelected = false;

            for (int i = 0; i < data.count; i++)
            {
                if (data.selected == data[i])
                {
                    dataSelected = true;
                }
            }

            if (dataSelected)
            {
                BeginHorizontal();
                GROUP_TITLE_GUI();
                EndHorizontal();
            }
            else
            {
                for (int i = 0; i < data.count; i++)
                {
                    if (data.selected == data[i])
                    {
                        dataSelected = false;
                    }

                    EditorGUI.BeginChangeCheck();
                    GL.Toggle(data.selected == data[i], data[i].name, style);
                    if (EditorGUI.EndChangeCheck())
                    {
                        createGroup = false;
                        data.selected = data[i];
                        renameGroupValue = data.selected.name;
                        RepaintAll();
                    }
                }
            }

            CREATE_GROUP_GUI();
        }

        void CreateGroupButton()
        {
            if(createGroup)
            {
                return;
            }

            GUIContent content = IconContent("d_CreateAddNew@2x");

            GUILayoutOption[] op =
            {
                GL.Width(singleLineHeight * 1.5f),
                GL.Height(singleLineHeight * 1.5f),
                GL.ExpandHeight(true),
                GL.ExpandWidth(false)
            };

            if (GL.Button(content, "Button", op))
            {
                data.selected = null;
                createGroup = !createGroup;
            }
        }

        void CREATE_GROUP_GUI()
        {
            if (createGroup)
            {
                BeginVertical("FrameBox");
                GL.Label("New Group Name", "CenteredLabel");
                newGroupName = GL.TextField(newGroupName, "BoldTextField");
                BeginHorizontal();
                if (GL.Button("Create", "ButtonLeft"))
                {
                    data.Add(new LevelEditorGroup(newGroupName));
                    data.selected = data[data.count - 1];
                    createGroup = false;

                    EditorUtility.SetDirty(data);
                }
                if (GL.Button("Cancel", "ButtonRight", GL.ExpandWidth(false)))
                {
                    createGroup = false;
                }
                EndHorizontal();
                EndVertical();
            }

            //if (createGroup)
            //{
            //    GL.FlexibleSpace();
            //}
        }

        void GROUP_TITLE_GUI()
        {
            if (createGroup) { return; }

            if (null == data.selected) { return; }

            if (!data.Contains(data.selected)) { return; }

            Separator();

            BeginHorizontal();

            if (renameGroup)
            {
                renameGroupValue = GL.TextField(renameGroupValue);

                if (GL.Button("Accept", "ButtonLeft", GL.ExpandWidth(false)))
                {
                    data.selected.name = renameGroupValue;
                    renameGroup = false;
                    EditorUtility.SetDirty(data);
                }
                if (GL.Button("Cancel", "ButtonMid", GL.ExpandWidth(false)))
                {
                    renameGroup = false;
                }
            }
            else
            {
                string title = string.Format("{0} ({1})", data.selected.name, data.selected.Count);
                //GL.Label(string.Format("Name: {0} - Objects: {1}", data.selected.name, data.selected.Count), TitleStyle);
                GL.Label(title, /*"SettingsHeader"*/ "BoldLabel", GL.ExpandHeight(true));
                if (GL.Button("Rename", "ButtonLeft", GL.ExpandWidth(false)))
                {
                    renameGroup = true;
                }
            }

            if (GL.Button("Delete", "ButtonMid", GL.ExpandWidth(false)))
            {
                if (EditorUtility.DisplayDialog("Level Editor", $"Delete ({data.selected.name}) Group?", "Yes", "No"))
                {
                    editor.PreviewGUIChanged();
                    data.Remove(data.selected);
                    data.selected = null;
                }
            }

            GUIContent content = IconContent("d_winbtn_win_close@2x");

            GUILayoutOption[] op =
            {
                GL.Width(singleLineHeight * 2),
                GL.Height(singleLineHeight),
                GL.ExpandHeight(true),
            };

            if (GL.Button(content, "ButtonRight", op))
            {
                data.selected = null;
            }

            EndHorizontal();
        }

        void GROUP_OBJECTS_GUI()
        {
            if (createGroup) { return; }

            if (null == data.selected) { return; }

            if (!data.Contains(data.selected)) { return; }

            // Search bar
            SearchBarGUI();

            Object value;
            int maxColumns = (int)(position.width / imgWidth2);
            int rowElements = 0;
            bool rowOpen = false;
            bool isSelected = false;

            for (int i = 0; i < data.selected.Count; i++)
            {
                deleteElement = false;
                value = data.selected[i];

                if (!value) { data.selected.Remove(i); break; }

                if (!value.name.Contains(filter)) { continue; }

                isSelected = selected.Contains(value);

                if (rowElements == 0) { BeginHorizontal(); rowOpen = true; }

                Rect rect = BeginVertical(isSelected ? Styles.progressBar : GUIStyle.none, GL.MaxWidth(80), GL.MaxHeight(80));
                DrawElement(value);
                EndVertical();

                ObjectEvent(value, rect);

                rowElements++;
                if (rowElements == maxColumns) { EndHorizontal(); rowOpen = false; rowElements = 0; }

                if (deleteElement) { break; }
            }

            if (rowOpen) { EndHorizontal(); }
        }

        bool deleteElement;

        void DrawElement(Object value)
        {
            GUIStyle label = new GUIStyle("label");
            label.wordWrap = true;


            Texture2D tex = GetPreview(value);
            GL.Label(tex, buttonOps100);

            BeginHorizontal();
            //GL.Label(value.name, label);
            ObjectField(value, value.GetType(), false);
            if (GL.Button("x", "MiniButtonRight", GL.ExpandWidth(false)))
            {
                data.selected.Remove(value);
                selected.Remove(value);

                deleteElement = true;
            }
            EndHorizontal();
        }
    }
}

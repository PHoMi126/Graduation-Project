using UnityEditor;
using UnityEngine;
namespace Smart
{
    public partial class LevelEditor : EditorWindow
    {
        //
        // Unity
        //

        private static LevelEditor editor;

        [MenuItem("Tools/Smart/Level Editor")]
        static void Init()
        {
            if (settingsWindow)
            {
                editor = CreateInstance<LevelEditor>();
                editor.titleContent.text = "Level Editor";
                editor.Show();
                editor.isSettings = false;

                settingsWindow.Show();
            }
            else
            {
                editor = GetWindow<LevelEditor>("Level Editor");
                editor.isSettings = false;
            }
        }

        private void OnEnable()
        {
            Load();

            FindData();

            if (isSettings)
            {
                settingsWindow = this;

                return;
            }

            editor = this;

            tool = Tools.current;

            SceneView.duringSceneGui -= OnSceneGUI;
            SceneView.duringSceneGui += OnSceneGUI;

            PreviewGUIChanged();
        }

        private void OnDisable()
        {
            // Only settings is allow to save
            if (isSettings)
            {
                Save();

                return;
            }

            DeletePreviews();

            SceneView.duringSceneGui -= OnSceneGUI;
        }

        private void OnFocus()
        {
            tool = Tools.current;

            isFocused = true;

            if(isSettings)
            {
                settingsWindow = this;
            }
            else
            {
                editor = this;
            }

            FindData();
        }

        private void OnLostFocus()
        {
            isFocused = false;
        }

        private void OnHierarchyChange()
        {
            if (isFocused)
            {
                RepaintAll();
            }
        }

        private void OnGUI()
        {
            OnDrawGUI();
        }

        //
        // Methods
        //  

        void OnSceneGUI(SceneView view)
        {
            e = Event.current;

            if (isSettings) { return; }

            this.view = view;

            ControlActive();

            if (!active) { return; }

            ControlMode();
            ControlTools();
            ControlPaint();
            ControlHeight();
            SceneGUIButtons();
            EditUpdate();

            if (isFocused)
            {
                Repaint();
            }
        }

        void RepaintAll()
        {
            Repaint();

            if(null == settingsWindow) { return; }

            settingsWindow.Repaint();
        }
    }
}
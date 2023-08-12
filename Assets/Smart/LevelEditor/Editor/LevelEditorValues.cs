using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Smart
{
    using static EditorPrefs;

    public partial class LevelEditor
    {
        //
        // Draw values
        //

        const float imgWidth1 = 80;
        const float imgHeight1 = 80;
        const float imgWidth2 = 100;
        const float imgHeight2 = 100;

        Vector3 rotationAxis = new Vector3(0, 1, 0);
        Vector3 angle = Vector3.zero;

        [SerializeField]
        bool isSettings = false;

        //
        // Edit values
        //

        private static Vector3 pickPosition = Vector3.zero;
        private readonly Vector3 labelMouseOffset = new (0, .5f, 0f);
        private readonly Vector3 labelObjectOffset = new (0, -1, 0);
        private static Pick pick = new ();
        private static Transform[] transforms => Selection.transforms;

        //
        // Properties
        //

        // Targets to delete
        private Object[] targets;

        [SerializeField]
        // The data to store the groups
        private LevelEditorData data;

        // The active scene view
        private SceneView view;

        // Is window focused
        private bool isFocused = false;

        // Is this level editor active
        private static bool active = false;

        // Is the level editor set to view2D
        private static bool view2D = false;

        // The height of an invisible plane where objects are placed
        private static float height = 0;

        // Mode
        private static Mode mode;

        // Paint
        private static Paint paint;

        // Current unity editor tool
        private static Tool tool;

        // The size of the brush
        private static Vector3Int size = Vector3Int.one;

        // Snap move value
        private static Vector3 move = Vector3.one;

        // Snap rotate value
        private static Vector3 rotate = new Vector3(15, 15, 15);

        // Snap scale value
        private static Vector3 scale = new Vector3(0.5f, 0.5f, 0.5f);

        // Current event
        private static Event e;

        // Layer mask
        private static LayerMask layer;

        [SerializeField]
        // Selected objects
        private List<Object> selected = new List<Object>();

        // Search filter
        private static string filter = "";

        // Max child count
        private static int maxChildCount = 100;

        //
        // Default properties
        //

        public readonly Vector3Int defaultSize = Vector3Int.one;
        public readonly Vector3 defaultMove = Vector3.one;
        public readonly Vector3 defaultRotate = new Vector3(15, 15, 15);
        public readonly Vector3 defaultScale = new Vector3(0.5f, 0.5f, 0.5f);
        public readonly int defaultLayer = 0;
        public readonly float defaultHeight = 0;
        public readonly bool default2DView = false;
        public readonly int defaultMaxChildCount = 100;

        //
        // Methods
        //

        void Load()
        {
            mode = (Mode)GetInt("sle.mode", 0);
            paint = (Paint)GetInt("sle.paint", 0);

            size = GetPrefsVector("sle.size", defaultSize);
            move = GetPrefsVector("sle.move", defaultMove);
            rotate = GetPrefsVector("sle.rotate", defaultRotate);
            scale = GetPrefsVector("sle.scale", defaultScale);
            layer = GetInt("sle.layer", defaultLayer);
            height = GetFloat("sle.height", defaultHeight);
            view2D = GetBool("sle.view2D", default2DView);
            maxChildCount = GetInt("sle.maxChildCount", defaultMaxChildCount);
        }

        void Save()
        {
            SetInt("sle.mode", (int)mode);
            SetInt("sle.paint", (int)paint);

            SetPrefsVector("sle.size", size);
            SetPrefsVector("sle.move", move);
            SetPrefsVector("sle.rotate", rotate);
            SetPrefsVector("sle.scale", scale);
            SetInt("sle.layer", layer);
            SetFloat("sle.height", height);
            SetBool("sle.view2D", view2D);
            SetInt("sle.maxChildCount", maxChildCount);
        }

        Vector3 GetPrefsVector(string key, Vector3 defaultValue)
        {
            Vector3 result = defaultValue;

            result.x = GetFloat(string.Format("{0}.x", key), defaultValue.x);
            result.y = GetFloat(string.Format("{0}.y", key), defaultValue.y);
            result.z = GetFloat(string.Format("{0}.z", key), defaultValue.z);

            return result;
        }

        Vector3Int GetPrefsVector(string key, Vector3Int defaultValue)
        {
            Vector3Int result = defaultValue;

            result.x = GetInt(string.Format("{0}.x", key), defaultValue.x);
            result.y = GetInt(string.Format("{0}.y", key), defaultValue.y);
            result.z = GetInt(string.Format("{0}.z", key), defaultValue.z);

            return result;
        }

        void SetPrefsVector(string key, Vector3 value)
        {
            SetFloat(string.Format("{0}.x", key), value.x);
            SetFloat(string.Format("{0}.y", key), value.y);
            SetFloat(string.Format("{0}.z", key), value.z);
        }

        void SetPrefsVector(string key, Vector3Int value)
        {
            SetInt(string.Format("{0}.x", key), value.x);
            SetInt(string.Format("{0}.y", key), value.y);
            SetInt(string.Format("{0}.z", key), value.z);
        }
    }
}
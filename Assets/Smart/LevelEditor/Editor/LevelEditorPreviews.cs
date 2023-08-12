using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Smart
{
    public partial class LevelEditor
    {
        private static List<GameObject> previews = new List<GameObject>();

        public void CreatePreviews()
        {
            editor.DeletePreviews();

            int count = size.x * size.y * size.z;

            GameObject result;

            bool isPrefab = false;

            for (int i = 0; i < count; i++)
            {
                result = GetObject() as GameObject;

                if (null == result) { continue; }

                isPrefab = true;

                result = PrefabUtility.InstantiatePrefab(result) as GameObject;

                if (null == result) { continue; }

                DisablePreviewColliders(result);
                result.transform.Rotate(angle);
                result.hideFlags = HideFlags.HideInHierarchy;

                previews.Add(result);
            }

            if(isPrefab)
            { return; }

            Object preview;

            for(int i = 0; i < count; i++)
            {
                preview = GetObject();

                if(null == preview) { continue; }

                result = new GameObject(preview.name);

                if (null == result) { continue; }

                result.AddComponent<SpriteRenderer>().sprite = preview as Sprite;

                DisablePreviewColliders(result);
                result.transform.Rotate(angle);
                result.hideFlags = HideFlags.HideInHierarchy;

                previews.Add(result);
            }
        }

        void DisablePreviewColliders(GameObject go)
        {
            Collider[] col = go.GetComponentsInChildren<Collider>();

            for (int i = 0; i < col.Length; i++)
            {
                col[i].enabled = false;
            }
        }

        void DeletePreviews()
        {
            for (int i = 0; i < previews.Count; i++)
            {
                DestroyImmediate(previews[i]);
                previews[i] = null;
            }

            previews.Clear();
        }

        void UpdatePreviews(Vector3[] pos)
        {
            if(previews.Count != pos.Length)
            {
                //Debug.Log($"previews: {previews.Count}, pos: {pos.Length}");

                return;
            }

            for (int i = 0; i < previews.Count; i++)
            {
                previews[i].transform.position = pos[i];
            }
        }

        void PreviewGUIChanged()
        {
            if (active && mode == Mode.Create)
            {
                CreatePreviews();

                return;
            }

            if (!active || mode != Mode.Create)
            {
                DeletePreviews();
            }
        }
    }
}

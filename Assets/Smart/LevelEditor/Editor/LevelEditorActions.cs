#define SET_PARENT
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;
using Rand = UnityEngine.Random;

namespace Smart
{
    public partial class LevelEditor
    {
        Object GetFirst()
        {
            if (selected.Count <= 0) { return null; }

            return selected[0];
        }

        Object GetObject()
        {
            if (selected.Count == 0) { return null; }

            if (selected.Count == 1) { return selected[0]; }

            int index = Rand.Range(0, selected.Count);

            return selected[index];
        }

        static bool parentOverflow = false;
        void Create()
        {
            if (paint == Paint.Brush)
            {
                // Exit
                if (pick.Overlap(pickPosition, size, previews.ToArray(), layer)) { return; }
            }

            if(previews.Count == 0 || selected.Count == 0)
            {
                return;
            }

            Transform parentTarget = GetParent();

            if (ParentOverflow(parentTarget))
            {
                if (EditorUtility.DisplayDialog("Level Editor", "Parent target Overflowed - Create New Parent Target?", "Accept", "Decline"))
                {
                    GameObject newParentOverflow = new GameObject(parentTarget.name);
                    newParentOverflow.transform.SetParent(parentTarget.parent);
                    Undo.RegisterCreatedObjectUndo(newParentOverflow, "LE.NewParentOverflow");
                    Selection.activeTransform = newParentOverflow.transform;
                    parentTarget = GetParent();
                }

                return;
            }

            // 1 - We are going to create the objects indicated by our previews
            // 2 - Why not just removed them from the preview list? they are already there
            // 3 - Because we need to be able to use Ctrl-z, to undo

            bool prefabCreated = false;
            GameObject result;
            GameObject prefab;


            for (int i = 0; i < previews.Count; i++)
            {
                if (ParentOverflow(parentTarget))
                {
                    return;
                }

                prefab = PrefabUtility.GetCorrespondingObjectFromSource(previews[i]);

                if (null == prefab) { continue; }

                result = CreateFromPrefab(prefab);

                if (null == result) { continue; }

                prefabCreated = true;
#if SET_PARENT
                result.transform.SetParent(parentTarget);
#endif
                result.transform.position = previews[i].transform.position;
                result.transform.localEulerAngles = previews[i].transform.localEulerAngles;
                Undo.RegisterCreatedObjectUndo(result, "LeveEditor.CreateObject");
            }

            if (false == prefabCreated)
            {
                SpriteRenderer sprRnd;

                for(int i = 0; i < previews.Count; i++)
                {
                    if (ParentOverflow(parentTarget))
                    {
                        return;
                    }

                    result = new GameObject(previews[i].name);
                    sprRnd = result.AddComponent<SpriteRenderer>();
                    sprRnd.sprite = previews[i].GetComponent<SpriteRenderer>().sprite;
#if SET_PARENT
                    result.transform.SetParent(parentTarget);
#endif
                    result.transform.position = previews[i].transform.position;
                    result.transform.localEulerAngles = previews[i].transform.localEulerAngles;
                    Undo.RegisterCreatedObjectUndo(result, "LeveEditor.CreateObject");
                }
            }

            // 4 - lastly we are going to create the previews again so we can randomize the objs
            CreatePreviews();
        }

        bool ParentOverflow(Transform parentTarget)
        {
            parentOverflow = parentTarget.childCount >= maxChildCount;

            return parentOverflow;
        }

        GameObject CreateFromPrefab(GameObject prefab)
        {
            return PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        }

        // This is currently making a mess
#if SET_PARENT
        Transform root;

        Transform GetParent()
        {
            root = Selection.activeTransform;
            // E x i t
            if (root) { return root; }
            // Create new section
            GameObject newSection = new GameObject("[NewSection]");
            // Get Transform
            root = newSection.transform;
            // Register undo
            Undo.RegisterCreatedObjectUndo(newSection, "Smart.Root.GO.Created");
            // Select transform
            Selection.activeTransform = root;

            return root;
        }
#endif

        void Delete()
        {
            targets = pick.GetGameObjects(pickPosition, size, layer);

            if (null == targets) { return; }

            GameObject prefab;

            for (int i = 0; i < targets.Length; i++)
            {
                //if (!IsPrefabAndSelected(targets[i])) { continue; }
                if (null == targets[i]) { continue; }

                prefab = PrefabUtility.GetOutermostPrefabInstanceRoot(targets[i]);

                if (prefab)
                {
                    Undo.DestroyObjectImmediate(prefab);
                }
                else
                {
                    Undo.DestroyObjectImmediate(targets[i]);
                    targets[i] = null;
                }
            }
        }
    }
}
using UnityEditor;
using UnityEngine;

namespace ZCCUtils.Editor
{
    public class EditorWindowBatchReplaceMaterials : EditorWindow
    {

        private GameObject targetGo;
        private Material targetMat;

        public void DrawGUI()
        {
            ZCCUtilsEditorWindowUtil.DrawTitle("批量替换目标游戏对象的所有材质");
            ZCCUtilsEditorWindowUtil.DrawOneContent("说明", "将目标游戏对象及其所有子对象的材质统一替换为某一材质");
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("目标游戏对象");
            targetGo = EditorGUILayout.ObjectField(targetGo, typeof(GameObject), true) as GameObject;
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("目标材质");
            targetMat = EditorGUILayout.ObjectField(targetMat, typeof(Material), true) as Material;
            EditorGUILayout.EndHorizontal();
            if (GUILayout.Button("替换"))
            {
                if (targetGo == null || targetMat == null)
                {
                    EditorUtility.DisplayDialog("提示","目标游戏对象或目标材质不能为空！","确定");
                    return;
                }
                SetMaterial(targetGo.transform);
                EditorUtility.DisplayDialog("提示", "替换完成", "确定");
            }
        }

        private void SetMaterial(Transform transform)
        {
            if (transform == null) return;

            // 设置自身的材质球
            var meshRender = transform.GetComponent<MeshRenderer>();
            if (meshRender != null)
            {
                meshRender.sharedMaterial = this.targetMat;
                var length = meshRender.sharedMaterials.Length;
                var mats = new Material[length];
                for (int i = 0; i < length; i++)
                {
                    mats[i] = meshRender.sharedMaterials[i];
                }
                meshRender.sharedMaterials = mats;
            }

            for (int i = 0; i < transform.childCount; i++)
            {
                // 设置所有子物体的材质球
                SetMaterial(transform.GetChild(i));
            }
        }
    }
}
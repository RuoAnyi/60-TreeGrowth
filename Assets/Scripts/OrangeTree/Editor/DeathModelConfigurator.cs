using UnityEngine;
using UnityEditor;

namespace TreePlanQAQ.OrangeTree.Editor
{
    /// <summary>
    /// 橘子树控制器的编辑器扩展
    /// </summary>
    [CustomEditor(typeof(OrangeTreeController))]
    public class OrangeTreeControllerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            
            GUILayout.Space(10);
            
            OrangeTreeController controller = (OrangeTreeController)target;
            
            if (GUILayout.Button("自动配置死亡模型", GUILayout.Height(40)))
            {
                ConfigureDeathModels(controller);
            }
            
            if (GUILayout.Button("清除死亡模型配置", GUILayout.Height(30)))
            {
                ClearDeathModels(controller);
            }
        }
        
        private void ConfigureDeathModels(OrangeTreeController controller)
        {
            if (controller == null || controller.stages == null)
            {
                EditorUtility.DisplayDialog("错误", "橘子树控制器或阶段配置为空", "确定");
                return;
            }
            
            // 查找 OrangeTreeGrowthing 容器
            GameObject container = GameObject.Find("OrangeTreeGrowthing");
            if (container == null)
            {
                EditorUtility.DisplayDialog("错误", "场景中未找到 OrangeTreeGrowthing 对象", "确定");
                return;
            }
            
            int configuredCount = 0;
            
            // 为每个阶段配置死亡模型
            foreach (var stage in controller.stages)
            {
                string diedModelName = GetDiedModelName(stage.stage);
                
                if (string.IsNullOrEmpty(diedModelName))
                {
                    // 种子阶段没有死亡模型
                    continue;
                }
                
                // 在容器中查找死亡模型
                Transform diedTransform = container.transform.Find(diedModelName);
                
                if (diedTransform != null)
                {
                    stage.diedModel = diedTransform.gameObject;
                    configuredCount++;
                    Debug.Log($"✅ 已配置 {stage.displayName} 的死亡模型: {diedModelName}");
                }
                else
                {
                    Debug.LogWarning($"⚠️ 未找到 {stage.displayName} 的死亡模型: {diedModelName}");
                }
            }
            
            // 标记为已修改
            EditorUtility.SetDirty(controller);
            
            EditorUtility.DisplayDialog(
                "配置完成", 
                $"成功配置了 {configuredCount} 个阶段的死亡模型", 
                "确定"
            );
        }
        
        private void ClearDeathModels(OrangeTreeController controller)
        {
            if (controller == null || controller.stages == null)
            {
                return;
            }
            
            foreach (var stage in controller.stages)
            {
                stage.diedModel = null;
            }
            
            EditorUtility.SetDirty(controller);
            EditorUtility.DisplayDialog("完成", "已清除所有死亡模型配置", "确定");
        }
        
        private string GetDiedModelName(OrangeTreeStage stage)
        {
            switch (stage)
            {
                case OrangeTreeStage.Seed:
                    return null; // 种子阶段不会死亡
                case OrangeTreeStage.Sprout:
                    return "Sprout_died";
                case OrangeTreeStage.Seedling:
                    return "Seeding_Died";
                case OrangeTreeStage.YoungTree:
                    return "YoungTreet_Died";
                case OrangeTreeStage.MatureTree:
                    return "MatureTree_Died";
                case OrangeTreeStage.Fruiting:
                    return "Fruting_Died";
                case OrangeTreeStage.Harvest:
                    return "Harvest_Died";
                default:
                    return null;
            }
        }
    }
}

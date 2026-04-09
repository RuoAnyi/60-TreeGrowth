using UnityEngine;
using UnityEditor;
using TreePlanQAQ.OrangeTree;

namespace TreePlanQAQ.Editor
{
    /// <summary>
    /// 检查场景状态和配置
    /// </summary>
    public class SceneStatusChecker : EditorWindow
    {
        [MenuItem("TreePlanQAQ/Check Scene Status")]
        public static void ShowWindow()
        {
            GetWindow<SceneStatusChecker>("场景状态检查");
        }
        
        private Vector2 scrollPosition;
        
        private void OnGUI()
        {
            GUILayout.Label("场景状态检查", EditorStyles.boldLabel);
            GUILayout.Space(10);
            
            if (GUILayout.Button("刷新状态", GUILayout.Height(30)))
            {
                Repaint();
            }
            
            GUILayout.Space(10);
            
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            
            // 检查OrangeTreeController
            CheckOrangeTreeController();
            
            GUILayout.Space(10);
            
            // 检查场景中的模型
            CheckSceneModels();
            
            EditorGUILayout.EndScrollView();
        }
        
        private void CheckOrangeTreeController()
        {
            EditorGUILayout.LabelField("OrangeTreeController 状态", EditorStyles.boldLabel);
            
            OrangeTreeController controller = FindObjectOfType<OrangeTreeController>();
            
            if (controller == null)
            {
                EditorGUILayout.HelpBox("❌ 未找到 OrangeTreeController", MessageType.Error);
                return;
            }
            
            EditorGUILayout.HelpBox("✅ 找到 OrangeTreeController", MessageType.Info);
            
            // 显示配置信息
            EditorGUILayout.LabelField("当前生长值:", controller.CurrentGrowth.ToString("F2"));
            EditorGUILayout.LabelField("当前阶段:", controller.CurrentStage.ToString());
            
            GUILayout.Space(5);
            EditorGUILayout.LabelField("阶段配置:", EditorStyles.boldLabel);
            
            if (controller.stages == null || controller.stages.Count == 0)
            {
                EditorGUILayout.HelpBox("⚠️ 没有配置阶段", MessageType.Warning);
            }
            else
            {
                EditorGUILayout.LabelField($"配置的阶段数: {controller.stages.Count}");
                
                foreach (var stageData in controller.stages)
                {
                    string status = stageData.stageModel != null ? "✅" : "❌";
                    EditorGUILayout.LabelField($"{status} Stage {stageData.stage}: {stageData.displayName}", 
                        stageData.stageModel != null ? EditorStyles.label : EditorStyles.boldLabel);
                    
                    if (stageData.stageModel == null)
                    {
                        EditorGUILayout.HelpBox($"  ⚠️ {stageData.displayName} 缺少模型引用", MessageType.Warning);
                    }
                }
            }
            
            // 检查Growth Config
            GUILayout.Space(5);
            if (controller.config == null)
            {
                EditorGUILayout.HelpBox("⚠️ Growth Config 未配置", MessageType.Warning);
            }
            else
            {
                EditorGUILayout.HelpBox($"✅ Growth Config: {controller.config.name}", MessageType.Info);
            }
        }
        
        private void CheckSceneModels()
        {
            EditorGUILayout.LabelField("场景模型检查", EditorStyles.boldLabel);
            
            // 检查是否还有Dormant和Dead模型
            GameObject dormant = GameObject.Find("PlantModel_Dormant");
            GameObject dead = GameObject.Find("PlantModel_Dead");
            
            if (dormant != null)
            {
                EditorGUILayout.HelpBox("⚠️ 发现 PlantModel_Dormant，建议删除", MessageType.Warning);
                if (GUILayout.Button("删除 PlantModel_Dormant"))
                {
                    DestroyImmediate(dormant);
                    Debug.Log("已删除 PlantModel_Dormant");
                }
            }
            else
            {
                EditorGUILayout.HelpBox("✅ PlantModel_Dormant 已清理", MessageType.Info);
            }
            
            if (dead != null)
            {
                EditorGUILayout.HelpBox("⚠️ 发现 PlantModel_Dead，建议删除", MessageType.Warning);
                if (GUILayout.Button("删除 PlantModel_Dead"))
                {
                    DestroyImmediate(dead);
                    Debug.Log("已删除 PlantModel_Dead");
                }
            }
            else
            {
                EditorGUILayout.HelpBox("✅ PlantModel_Dead 已清理", MessageType.Info);
            }
            
            // 检查8个阶段的模型
            GUILayout.Space(5);
            EditorGUILayout.LabelField("橘子树模型:", EditorStyles.boldLabel);
            
            string[] modelNames = new string[]
            {
                "OrangeTree_Seed",
                "OrangeTree_Sprout",
                "OrangeTree_Seeding",
                "OrangeTree_YoungTreet",
                "OrangeTree_MatureTree",
                "OrangeTree_Fruting",
                "OrangeTree_Harvest"
            };
            
            foreach (string modelName in modelNames)
            {
                GameObject model = GameObject.Find(modelName);
                if (model != null)
                {
                    EditorGUILayout.LabelField($"✅ {modelName}");
                }
                else
                {
                    EditorGUILayout.LabelField($"❌ {modelName} 未找到", EditorStyles.boldLabel);
                }
            }
        }
    }
}

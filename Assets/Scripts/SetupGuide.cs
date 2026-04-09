using UnityEngine;

namespace TreePlanQAQ
{
    /// <summary>
    /// 新橘子树系统设置指南
    /// 推荐使用菜单: TreePlanQAQ/Orange Tree Setup Wizard 进行自动设置
    /// </summary>
    public class SetupGuide : MonoBehaviour
    {
        [Header("快速开始")]
        [Tooltip("推荐使用菜单: TreePlanQAQ/Orange Tree Setup Wizard")]
        public bool useSetupWizard = true;
        
        [Header("Step 1: 清理旧系统")]
        [Tooltip("使用菜单: TreePlanQAQ/Cleanup Old System")]
        public bool step1Complete = false;
        
        [Header("Step 2: 创建配置")]
        [Tooltip("1. 使用 Setup Wizard 创建配置\n" +
                 "2. 或手动创建: 右键 -> Create -> OrangeTree -> Growth Config")]
        public bool step2Complete = false;
        
        [Header("Step 3: 配置模型")]
        [Tooltip("在 Setup Wizard 中配置7个阶段的模型:\n" +
                 "Seed, Sprout, Seedling, YoungTree, MatureTree, Fruiting, Harvest")]
        public bool step3Complete = false;
        
        [Header("Step 4: 创建系统")]
        [Tooltip("在 Setup Wizard 中点击'创建橘子树系统'按钮")]
        public bool step4Complete = false;
        
        [Header("Step 5: 测试运行")]
        [Tooltip("按Play测试，观察橘子树生长过程")]
        public bool step5Complete = false;
        
        [Header("Step 6: 调整参数")]
        [Tooltip("在 GrowthConfig 中调整生长速度和环境影响参数")]
        public bool step6Complete = false;
        
        public void CompleteStep(int step)
        {
            switch (step)
            {
                case 1: step1Complete = true; break;
                case 2: step2Complete = true; break;
                case 3: step3Complete = true; break;
                case 4: step4Complete = true; break;
                case 5: step5Complete = true; break;
                case 6: step6Complete = true; break;
            }
        }
    }
}

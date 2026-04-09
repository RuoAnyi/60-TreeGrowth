using System;
using UnityEngine;

namespace TreePlanQAQ.OrangeTree
{
    /// <summary>
    /// 橘子树生长阶段
    /// </summary>
    public enum OrangeTreeStage
    {
        Seed = 0,        // 种子
        Sprout = 1,      // 发芽
        Seedling = 2,    // 幼苗
        YoungTree = 3,   // 小树
        MatureTree = 4,  // 成树
        Fruiting = 5,    // 结果
        Harvest = 6      // 成熟
    }
    
    /// <summary>
    /// 阶段数据
    /// </summary>
    [Serializable]
    public class StageData
    {
        [Header("阶段信息")]
        public OrangeTreeStage stage;
        public string displayName;
        
        [Header("生长阈值")]
        [Range(0, 100)]
        public float growthThreshold;
        
        [Header("模型引用")]
        public GameObject stageModel;
        
        [Header("死亡模型引用")]
        [Tooltip("当环境条件不适宜时显示的死亡模型")]
        public GameObject diedModel;
        
        [Header("过渡设置")]
        [Range(0.1f, 5f)]
        public float transitionDuration = 1f;
        
        public StageData(OrangeTreeStage stage, string displayName, float threshold)
        {
            this.stage = stage;
            this.displayName = displayName;
            this.growthThreshold = threshold;
        }
    }
}

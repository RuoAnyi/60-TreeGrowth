using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TreePlanQAQ.OrangeTree
{
    /// <summary>
    /// 橘子树主控制器
    /// </summary>
    public class OrangeTreeController : MonoBehaviour
    {
        [Header("配置")]
        public GrowthConfig config;
        
        [Header("阶段配置")]
        public List<StageData> stages = new List<StageData>();
        
        [Header("当前状态")]
        [SerializeField]
        [Range(0, 100)]
        private float currentGrowth = 0f;
        
        [SerializeField]
        private OrangeTreeStage currentStage = OrangeTreeStage.Seed;
        
        [Header("产量系统")]
        [SerializeField]
        private int maxYield = 50; // 最大产量（公斤）
        
        [SerializeField]
        private int currentYield = 50; // 当前产量（公斤）
        
        [SerializeField]
        private int yieldLossPerStop = 1; // 每次停止生长损失的产量（公斤）
        
        [Header("环境输入")]
        [SerializeField]
        [Range(-20f, 50f)]
        private float temperature = 25f;
        
        [SerializeField]
        [Range(0f, 100f)]
        private float humidity = 70f;  // 在65-80范围内
        
        [SerializeField]
        [Range(0f, 1000f)]
        private float sunlight = 600f;
        
        // 事件
        public event Action<OrangeTreeStage, OrangeTreeStage> OnStageChanged;
        public event Action<float> OnGrowthUpdated;
        public event Action<int> OnYieldChanged; // 产量变化事件
        public event Action<int> OnHarvestComplete; // 收获完成事件
        
        // 私有变量
        private GameObject currentModel;
        private bool isTransitioning = false;
        private bool isPaused = false;
        private bool wasGrowingSuitableLastFrame = true; // 上一帧环境是否适宜
        private bool hasHarvested = false; // 是否已经收获
        
        // 属性
        public float CurrentGrowth => currentGrowth;
        public OrangeTreeStage CurrentStage => currentStage;
        public bool IsPaused => isPaused;
        public int CurrentYield => currentYield;
        public int MaxYield => maxYield;
        
        private void Start()
        {
            InitializeStages();
            ShowStageModel(currentStage);
        }
        
        private void Update()
        {
            if (config == null || isTransitioning || isPaused)
            {
                if (isPaused && Time.frameCount % 60 == 0) // 每秒输出一次
                {
                    Debug.Log($"🌳 {gameObject.name} 已暂停 - isPaused = {isPaused}");
                }
                return;
            }
            
            // 检查环境是否适合生长
            bool environmentSuitable = IsEnvironmentSuitable();
            
            // 检测环境从适宜变为不适宜（停止生长）
            if (wasGrowingSuitableLastFrame && !environmentSuitable)
            {
                OnGrowthStopped();
            }
            
            // 检测环境从不适宜恢复到适宜（恢复生长）
            if (!wasGrowingSuitableLastFrame && environmentSuitable)
            {
                ShowNormalModel();
                Debug.Log($"✅ 环境恢复适宜，继续生长");
            }
            
            // 更新上一帧状态
            wasGrowingSuitableLastFrame = environmentSuitable;
            
            // 只有在环境适宜时才生长
            if (environmentSuitable)
            {
                // 在适宜范围内就按基础速率生长，不计算环境因子
                float growthDelta = config.baseGrowthRate * Time.deltaTime;
                
                float oldGrowth = currentGrowth;
                currentGrowth = Mathf.Clamp(currentGrowth + growthDelta, 0f, 100f);
                OnGrowthUpdated?.Invoke(currentGrowth);
                
                // 检查是否达到100%（收获）
                if (oldGrowth < 100f && currentGrowth >= 100f && !hasHarvested)
                {
                    OnHarvestReached();
                }
                
                // 检查是否需要切换阶段
                OrangeTreeStage newStage = GetStageForGrowth(currentGrowth);
                if (newStage != currentStage)
                {
                    StartCoroutine(TransitionToStage(newStage));
                }
            }
            else
            {
                // 环境不适宜时停止生长
                if (Time.frameCount % 120 == 0) // 每2秒输出一次
                {
                    Debug.Log($"⚠️ 环境不适宜，生长已停止 - 温度:{temperature:F1}°C, 湿度:{humidity:F1}%, 当前产量:{currentYield}kg");
                }
            }
        }
        
        /// <summary>
        /// 检查环境是否适合生长
        /// </summary>
        private bool IsEnvironmentSuitable()
        {
            // 温度范围：12.8 ~ 37°C
            bool tempOk = temperature >= 12.8f && temperature <= 37f;
            
            // 湿度范围：65% ~ 80%
            bool humidOk = humidity >= 65f && humidity <= 80f;
            
            // 温度和湿度都在范围内才适合生长
            return tempOk && humidOk;
        }
        
        /// <summary>
        /// 生长停止时调用（环境从适宜变为不适宜）
        /// </summary>
        private void OnGrowthStopped()
        {
            // 减少产量
            currentYield = Mathf.Max(0, currentYield - yieldLossPerStop);
            OnYieldChanged?.Invoke(currentYield);
            
            // 切换到死亡模型
            ShowDiedModel();
            
            Debug.Log($"⚠️ 生长停止！产量减少 {yieldLossPerStop}kg，当前产量: {currentYield}kg");
        }
        
        /// <summary>
        /// 显示死亡模型
        /// </summary>
        private void ShowDiedModel()
        {
            StageData stageData = stages.Find(s => s.stage == currentStage);
            if (stageData != null && stageData.diedModel != null)
            {
                // 隐藏正常模型
                if (currentModel != null)
                {
                    currentModel.SetActive(false);
                }
                
                // 显示死亡模型
                stageData.diedModel.SetActive(true);
                Debug.Log($"💀 切换到死亡模型: {currentStage}");
            }
        }
        
        /// <summary>
        /// 显示正常模型（环境恢复时）
        /// </summary>
        private void ShowNormalModel()
        {
            StageData stageData = stages.Find(s => s.stage == currentStage);
            if (stageData != null)
            {
                // 隐藏死亡模型
                if (stageData.diedModel != null)
                {
                    stageData.diedModel.SetActive(false);
                }
                
                // 显示正常模型
                if (currentModel != null)
                {
                    currentModel.SetActive(true);
                    Debug.Log($"🌱 恢复正常模型: {currentStage}");
                }
            }
        }
        
        /// <summary>
        /// 达到100%生长时调用（收获）
        /// </summary>
        private void OnHarvestReached()
        {
            hasHarvested = true;
            OnHarvestComplete?.Invoke(currentYield);
            
            Debug.Log($"🎉 恭喜您！您已收获 {currentYield} 公斤的橘子！");
            
            // 停止环境动态变化
            TreePlanQAQ.OrangeTree.EnvironmentManager envManager = FindObjectOfType<TreePlanQAQ.OrangeTree.EnvironmentManager>();
            if (envManager != null)
            {
                envManager.StopDynamicChange();
                Debug.Log("🌡️ 环境动态变化已停止");
            }
            
            // 显示收获提示（可以通过UI显示）
            ShowHarvestMessage(currentYield);
        }
        
        /// <summary>
        /// 显示收获消息
        /// </summary>
        private void ShowHarvestMessage(int yield)
        {
            // 查找并调用HarvestMessageUI
            HarvestMessageUI harvestUI = FindObjectOfType<HarvestMessageUI>();
            if (harvestUI != null)
            {
                harvestUI.ShowMessage(yield);
            }
            else
            {
                Debug.LogWarning("⚠️ 未找到HarvestMessageUI组件，无法显示收获消息");
            }
        }
        
        /// <summary>
        /// 初始化阶段数据
        /// </summary>
        private void InitializeStages()
        {
            if (stages.Count == 0)
            {
                // 创建默认阶段配置
                stages.Add(new StageData(OrangeTreeStage.Seed, "种子", 0f));
                stages.Add(new StageData(OrangeTreeStage.Sprout, "发芽", 10f));
                stages.Add(new StageData(OrangeTreeStage.Seedling, "幼苗", 25f));
                stages.Add(new StageData(OrangeTreeStage.YoungTree, "小树", 50f));
                stages.Add(new StageData(OrangeTreeStage.MatureTree, "成树", 75f));
                stages.Add(new StageData(OrangeTreeStage.Fruiting, "结果", 90f));
                stages.Add(new StageData(OrangeTreeStage.Harvest, "成熟", 100f));
            }
        }
        
        /// <summary>
        /// 根据生长值获取对应阶段
        /// </summary>
        private OrangeTreeStage GetStageForGrowth(float growth)
        {
            for (int i = stages.Count - 1; i >= 0; i--)
            {
                if (growth >= stages[i].growthThreshold)
                {
                    return stages[i].stage;
                }
            }
            return OrangeTreeStage.Seed;
        }
        
        /// <summary>
        /// 过渡到新阶段
        /// </summary>
        private IEnumerator TransitionToStage(OrangeTreeStage newStage)
        {
            isTransitioning = true;
            
            OrangeTreeStage oldStage = currentStage;
            currentStage = newStage;
            
            Debug.Log($"🌳 阶段切换: {oldStage} → {newStage}");
            OnStageChanged?.Invoke(oldStage, newStage);
            
            // 淡出旧模型
            if (currentModel != null)
            {
                yield return StartCoroutine(FadeOutModel(currentModel, 0.5f));
                currentModel.SetActive(false);
            }
            
            // 显示新模型
            ShowStageModel(newStage);
            
            // 淡入新模型
            if (currentModel != null)
            {
                yield return StartCoroutine(FadeInModel(currentModel, 0.5f));
            }
            
            isTransitioning = false;
        }
        
        /// <summary>
        /// 显示指定阶段的模型
        /// </summary>
        private void ShowStageModel(OrangeTreeStage stage)
        {
            currentStage = stage;
            
            StageData stageData = stages.Find(s => s.stage == currentStage);
            
            if (stageData != null && stageData.stageModel != null)
            {
                currentModel = stageData.stageModel;
                currentModel.SetActive(true);
            }
            else
            {
                Debug.LogWarning($"未找到阶段 {currentStage} 的模型");
            }
        }
        
        /// <summary>
        /// 淡出模型
        /// </summary>
        private IEnumerator FadeOutModel(GameObject model, float duration)
        {
            // 简单禁用，不修改 scale
            yield return null;
        }
        
        /// <summary>
        /// 淡入模型
        /// </summary>
        private IEnumerator FadeInModel(GameObject model, float duration)
        {
            // 简单启用，不修改 scale，保持场景中设置的值
            yield return null;
        }
        
        /// <summary>
        /// 设置环境参数
        /// </summary>
        public void SetEnvironment(float temp, float humid, float sun)
        {
            temperature = temp;
            humidity = humid;
            sunlight = sun;
        }
        
        /// <summary>
        /// 重置生长
        /// </summary>
        [ContextMenu("重置生长")]
        public void ResetGrowth()
        {
            currentGrowth = 0f;
            currentStage = OrangeTreeStage.Seed;
            currentYield = maxYield; // 重置产量到最大值
            hasHarvested = false;
            wasGrowingSuitableLastFrame = true;
            
            if (currentModel != null)
            {
                currentModel.SetActive(false);
            }
            
            ShowStageModel(OrangeTreeStage.Seed);
            
            OnYieldChanged?.Invoke(currentYield);
            
            Debug.Log($"🔄 橘子树已重置 - 产量: {currentYield}kg");
        }
        
        /// <summary>
        /// 设置生长值（用于测试）
        /// </summary>
        public void SetGrowth(float growth)
        {
            currentGrowth = Mathf.Clamp(growth, 0f, 100f);
            OrangeTreeStage newStage = GetStageForGrowth(currentGrowth);
            
            if (newStage != currentStage)
            {
                StartCoroutine(TransitionToStage(newStage));
            }
        }
        
        /// <summary>
        /// 暂停/继续生长
        /// </summary>
        public void TogglePause()
        {
            isPaused = !isPaused;
            Debug.Log($"🌳 OrangeTreeController ({gameObject.name}) 生长{(isPaused ? "暂停" : "继续")} - isPaused = {isPaused}");
        }
        
        /// <summary>
        /// 设置暂停状态
        /// </summary>
        public void SetPaused(bool paused)
        {
            isPaused = paused;
            Debug.Log($"🌳 OrangeTreeController ({gameObject.name}) 生长{(isPaused ? "暂停" : "继续")} - isPaused = {isPaused}");
        }
    }
}

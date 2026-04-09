using UnityEngine;
using UnityEngine.UI;

namespace TreePlanQAQ.OrangeTree
{
    /// <summary>
    /// 生长UI控制器
    /// </summary>
    public class GrowthUIController : MonoBehaviour
    {
        [Header("UI引用")]
        public Text stageText;
        public Text growthText;
        public Image growthBar;
        
        public Text temperatureText;
        public Text humidityText;
        public Text sunlightText;
        
        [Header("控制按钮")]
        public Button pauseButton;
        public Button resetButton;
        public Text pauseButtonText;
        
        [Header("目标")]
        public OrangeTreeController treeController;
        public EnvironmentManager environmentManager;
        
        private void Start()
        {
            // 自动查找
            if (treeController == null)
            {
                treeController = FindObjectOfType<OrangeTreeController>();
            }
            
            if (environmentManager == null)
            {
                environmentManager = EnvironmentManager.Instance;
            }
            
            // 订阅事件
            if (treeController != null)
            {
                treeController.OnStageChanged += OnStageChanged;
                treeController.OnGrowthUpdated += OnGrowthUpdated;
            }
            
            if (environmentManager != null)
            {
                environmentManager.OnEnvironmentChanged += OnEnvironmentChanged;
            }
            
            // 设置按钮事件 - 确保清除旧的监听器
            if (pauseButton != null)
            {
                pauseButton.onClick.RemoveAllListeners();
                pauseButton.onClick.AddListener(OnPauseButtonClicked);
                Debug.Log("暂停按钮事件已绑定");
            }
            else
            {
                Debug.LogWarning("pauseButton 为空！请在 Inspector 中设置按钮引用");
            }
            
            if (resetButton != null)
            {
                resetButton.onClick.RemoveAllListeners();
                resetButton.onClick.AddListener(OnResetButtonClicked);
                Debug.Log("重置按钮事件已绑定");
            }
            else
            {
                Debug.LogWarning("resetButton 为空！请在 Inspector 中设置按钮引用");
            }
            
            // 初始更新
            UpdateUI();
            UpdatePauseButtonText();
        }
        
        private void OnDestroy()
        {
            if (treeController != null)
            {
                treeController.OnStageChanged -= OnStageChanged;
                treeController.OnGrowthUpdated -= OnGrowthUpdated;
            }
            
            if (environmentManager != null)
            {
                environmentManager.OnEnvironmentChanged -= OnEnvironmentChanged;
            }
        }
        
        private void OnStageChanged(OrangeTreeStage oldStage, OrangeTreeStage newStage)
        {
            UpdateStageText();
        }
        
        private void OnGrowthUpdated(float growth)
        {
            UpdateGrowthDisplay(growth);
        }
        
        private void OnEnvironmentChanged(float temp, float humid, float sun)
        {
            UpdateEnvironmentDisplay(temp, humid, sun);
        }
        
        private void UpdateUI()
        {
            if (treeController != null)
            {
                UpdateStageText();
                UpdateGrowthDisplay(treeController.CurrentGrowth);
            }
            
            if (environmentManager != null)
            {
                UpdateEnvironmentDisplay(
                    environmentManager.Temperature,
                    environmentManager.Humidity,
                    environmentManager.Sunlight
                );
            }
        }
        
        private void UpdateStageText()
        {
            if (stageText != null && treeController != null)
            {
                string displayName = GetStageDisplayName(treeController.CurrentStage);
                stageText.text = $"阶段: {displayName}";
            }
        }
        
        private void UpdateGrowthDisplay(float growth)
        {
            if (growthText != null)
            {
                growthText.text = $"生长: {growth:F1}%";
            }
            
            if (growthBar != null)
            {
                growthBar.fillAmount = growth / 100f;
            }
        }
        
        private void UpdateEnvironmentDisplay(float temp, float humid, float sun)
        {
            if (temperatureText != null)
            {
                temperatureText.text = $"温度: {temp:F1}°C";
            }
            
            if (humidityText != null)
            {
                humidityText.text = $"湿度: {humid:F1}%";
            }
            
            if (sunlightText != null)
            {
                sunlightText.text = $"光照: {sun:F0} lux";
            }
        }
        
        private string GetStageDisplayName(OrangeTreeStage stage)
        {
            switch (stage)
            {
                case OrangeTreeStage.Seed: return "种子";
                case OrangeTreeStage.Sprout: return "发芽";
                case OrangeTreeStage.Seedling: return "幼苗";
                case OrangeTreeStage.YoungTree: return "小树";
                case OrangeTreeStage.MatureTree: return "成树";
                case OrangeTreeStage.Fruiting: return "结果";
                case OrangeTreeStage.Harvest: return "成熟";
                default: return stage.ToString();
            }
        }
        
        /// <summary>
        /// 暂停按钮点击
        /// </summary>
        private void OnPauseButtonClicked()
        {
            Debug.Log("暂停按钮被点击");
            if (treeController != null)
            {
                treeController.TogglePause();
                UpdatePauseButtonText();
                Debug.Log($"生长状态: {(treeController.IsPaused ? "暂停" : "继续")}");
            }
            else
            {
                Debug.LogError("treeController 为空！");
            }
        }
        
        /// <summary>
        /// 重置按钮点击
        /// </summary>
        private void OnResetButtonClicked()
        {
            Debug.Log("重置按钮被点击");
            if (treeController != null)
            {
                treeController.ResetGrowth();
                Debug.Log("橘子树已重置");
            }
            else
            {
                Debug.LogError("treeController 为空！");
            }
        }
        
        /// <summary>
        /// 更新暂停按钮文本
        /// </summary>
        private void UpdatePauseButtonText()
        {
            if (pauseButtonText != null && treeController != null)
            {
                pauseButtonText.text = treeController.IsPaused ? "继续" : "暂停";
            }
        }
    }
}

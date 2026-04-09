using UnityEngine;
using UnityEngine.UI;

namespace TreePlanQAQ.OrangeTree
{
    /// <summary>
    /// 增强版生长UI控制器 - 圆形按钮和三角形调节
    /// </summary>
    public class EnhancedGrowthUIController : MonoBehaviour
    {
        [Header("UI引用")]
        public Text stageText;
        public Text growthText;
        public Image growthBar;
        
        [Header("环境显示")]
        public Text temperatureText;
        public Text humidityText;
        public Text sunlightText;
        
        [Header("温度控制 - 三角形按钮")]
        public Button temperatureUpButton;
        public Button temperatureDownButton;
        
        [Header("湿度控制 - 三角形按钮")]
        public Button humidityUpButton;
        public Button humidityDownButton;
        
        [Header("圆形控制按钮")]
        public Button pauseButton;
        public Button resetButton;
        public Text pauseButtonText;
        
        [Header("调节参数")]
        [Tooltip("每次点击温度变化量")]
        public float temperatureStep = 1f;
        [Tooltip("每次点击湿度变化量")]
        public float humidityStep = 5f;
        
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
            
            // 设置按钮事件
            SetupButtonEvents();
            
            // 初始更新
            UpdateUI();
            UpdatePauseButtonText();
        }
        
        private void SetupButtonEvents()
        {
            // 暂停/继续按钮
            if (pauseButton != null)
            {
                pauseButton.onClick.RemoveAllListeners();
                pauseButton.onClick.AddListener(OnPauseButtonClicked);
                Debug.Log("暂停按钮事件已绑定");
            }
            
            // 重置按钮
            if (resetButton != null)
            {
                resetButton.onClick.RemoveAllListeners();
                resetButton.onClick.AddListener(OnResetButtonClicked);
                Debug.Log("重置按钮事件已绑定");
            }
            
            // 温度调节按钮
            if (temperatureUpButton != null)
            {
                temperatureUpButton.onClick.RemoveAllListeners();
                temperatureUpButton.onClick.AddListener(() => AdjustTemperature(temperatureStep));
            }
            
            if (temperatureDownButton != null)
            {
                temperatureDownButton.onClick.RemoveAllListeners();
                temperatureDownButton.onClick.AddListener(() => AdjustTemperature(-temperatureStep));
            }
            
            // 湿度调节按钮
            if (humidityUpButton != null)
            {
                humidityUpButton.onClick.RemoveAllListeners();
                humidityUpButton.onClick.AddListener(() => AdjustHumidity(humidityStep));
            }
            
            if (humidityDownButton != null)
            {
                humidityDownButton.onClick.RemoveAllListeners();
                humidityDownButton.onClick.AddListener(() => AdjustHumidity(-humidityStep));
            }
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
                temperatureText.text = $"{temp:F1}°C";
            }
            
            if (humidityText != null)
            {
                humidityText.text = $"{humid:F1}%";
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
        /// 调节温度
        /// </summary>
        private void AdjustTemperature(float delta)
        {
            if (environmentManager != null)
            {
                float newTemp = Mathf.Clamp(environmentManager.Temperature + delta, 0f, 50f);
                environmentManager.SetTemperature(newTemp);
                Debug.Log($"温度调整: {environmentManager.Temperature:F1}°C");
            }
        }
        
        /// <summary>
        /// 调节湿度
        /// </summary>
        private void AdjustHumidity(float delta)
        {
            if (environmentManager != null)
            {
                float newHumidity = Mathf.Clamp(environmentManager.Humidity + delta, 0f, 100f);
                environmentManager.SetHumidity(newHumidity);
                Debug.Log($"湿度调整: {environmentManager.Humidity:F1}%");
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

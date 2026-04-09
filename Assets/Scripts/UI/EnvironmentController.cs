using UnityEngine;
using UnityEngine.UI;
using TreePlanQAQ.OrangeTree;

/// <summary>
/// 环境参数控制器
/// 控制温度和湿度的升降，直接修改 EnvironmentManager 的数据
/// </summary>
public class EnvironmentController : MonoBehaviour
{
    [Header("数值范围")]
    [SerializeField] private float minTemperature = 0f;
    [SerializeField] private float maxTemperature = 100f;
    [SerializeField] private float minHumidity = 0f;
    [SerializeField] private float maxHumidity = 100f;
    
    [Header("调节步长")]
    [SerializeField] private float temperatureStep = 2f;
    [SerializeField] private float humidityStep = 5f;
    
    [Header("显示文本（可选，右侧面板用）")]
    [SerializeField] private Text temperatureText;
    [SerializeField] private Text humidityText;
    
    [Header("按钮引用")]
    [SerializeField] private Button tempUpButton;
    [SerializeField] private Button tempDownButton;
    [SerializeField] private Button humidUpButton;
    [SerializeField] private Button humidDownButton;

    private EnvironmentManager environmentManager;

    private void Start()
    {
        // 获取 EnvironmentManager 实例
        environmentManager = EnvironmentManager.Instance;
        
        if (environmentManager == null)
        {
            Debug.LogError("找不到 EnvironmentManager！请确保场景中有 EnvironmentManager");
            return;
        }

        // 绑定按钮事件
        if (tempUpButton != null)
            tempUpButton.onClick.AddListener(() => AdjustTemperature(temperatureStep));
        
        if (tempDownButton != null)
            tempDownButton.onClick.AddListener(() => AdjustTemperature(-temperatureStep));
        
        if (humidUpButton != null)
            humidUpButton.onClick.AddListener(() => AdjustHumidity(humidityStep));
        
        if (humidDownButton != null)
            humidDownButton.onClick.AddListener(() => AdjustHumidity(-humidityStep));
        
        // 订阅环境变化事件，更新右侧显示（如果有）
        if (environmentManager != null)
        {
            environmentManager.OnEnvironmentChanged += UpdateDisplay;
        }
        
        // 初始化显示
        UpdateDisplay(environmentManager.Temperature, environmentManager.Humidity, environmentManager.Sunlight);
    }

    /// <summary>
    /// 调节温度
    /// </summary>
    /// <param name="delta">变化量</param>
    public void AdjustTemperature(float delta)
    {
        if (environmentManager == null) return;
        
        environmentManager.AdjustTemperature(delta);
        Debug.Log($"温度调整: {environmentManager.Temperature:F1}°C");
    }

    /// <summary>
    /// 调节湿度
    /// </summary>
    /// <param name="delta">变化量</param>
    public void AdjustHumidity(float delta)
    {
        if (environmentManager == null) return;
        
        environmentManager.AdjustHumidity(delta);
        Debug.Log($"湿度调整: {environmentManager.Humidity:F1}%");
    }

    /// <summary>
    /// 更新显示文本（右侧面板，可选）
    /// </summary>
    private void UpdateDisplay(float temp, float humid, float sun)
    {
        if (temperatureText != null)
            temperatureText.text = $"温度: {temp:F1}°C";
        
        if (humidityText != null)
            humidityText.text = $"湿度: {humid:F0}%";
    }


    private void OnDestroy()
    {
        // 取消订阅
        if (environmentManager != null)
        {
            environmentManager.OnEnvironmentChanged -= UpdateDisplay;
        }
        
        // 清理按钮事件
        if (tempUpButton != null)
            tempUpButton.onClick.RemoveAllListeners();
        if (tempDownButton != null)
            tempDownButton.onClick.RemoveAllListeners();
        if (humidUpButton != null)
            humidUpButton.onClick.RemoveAllListeners();
        if (humidDownButton != null)
            humidDownButton.onClick.RemoveAllListeners();
    }
}
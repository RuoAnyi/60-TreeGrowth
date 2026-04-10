using UnityEngine;
using UnityEngine.UI;
using TreePlanQAQ.OrangeTree;

/// <summary>
/// 环境警告UI控制器
/// 当温度或湿度过高或过低时显示警告文本
/// 使用两个独立的文本框分别显示温度和湿度警告
/// </summary>
public class EnvironmentWarningUI : MonoBehaviour
{
    [Header("警告文本框")]
    [SerializeField] private Text temperatureWarningText;
    [SerializeField] private Text humidityWarningText;
    
    [Header("警告阈值（根据0-100温度湿度系统说明）")]
    [Tooltip("温度安全范围：30-70")]
    [SerializeField] private float temperatureMin = 30f;
    [SerializeField] private float temperatureMax = 70f;
    
    [Tooltip("湿度安全范围：40-80")]
    [SerializeField] private float humidityMin = 40f;
    [SerializeField] private float humidityMax = 80f;
    
    [Header("警告颜色")]
    [SerializeField] private Color warningColor = Color.red;
    
    private EnvironmentManager environmentManager;

    private void Start()
    {
        // 获取 EnvironmentManager 实例
        environmentManager = EnvironmentManager.Instance;
        
        if (environmentManager == null)
        {
            Debug.LogError("找不到 EnvironmentManager！");
            return;
        }
        
        if (temperatureWarningText == null)
        {
            Debug.LogError("温度警告文本未设置！请在Inspector中分配Text组件");
        }
        
        if (humidityWarningText == null)
        {
            Debug.LogError("湿度警告文本未设置！请在Inspector中分配Text组件");
        }
        
        // 订阅环境变化事件
        environmentManager.OnEnvironmentChanged += CheckEnvironmentWarnings;
        
        // 初始化警告文本
        InitializeWarningText(temperatureWarningText);
        InitializeWarningText(humidityWarningText);
        
        // 初始检查
        CheckEnvironmentWarnings(environmentManager.Temperature, environmentManager.Humidity, environmentManager.Sunlight);
    }

    /// <summary>
    /// 初始化警告文本
    /// </summary>
    private void InitializeWarningText(Text text)
    {
        if (text != null)
        {
            text.color = warningColor;
            text.text = "";
            text.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 检查环境参数并显示警告
    /// </summary>
    private void CheckEnvironmentWarnings(float temperature, float humidity, float sunlight)
    {
        // 检查并更新温度警告
        CheckTemperatureWarning(temperature);
        
        // 检查并更新湿度警告
        CheckHumidityWarning(humidity);
    }

    /// <summary>
    /// 检查温度警告
    /// </summary>
    private void CheckTemperatureWarning(float temperature)
    {
        if (temperatureWarningText == null) return;
        
        string warningMessage = "";
        
        if (temperature < temperatureMin)
        {
            warningMessage = "警告：温度过低";
        }
        else if (temperature > temperatureMax)
        {
            warningMessage = "警告：温度过高";
        }
        
        // 更新温度警告文本显示
        if (!string.IsNullOrEmpty(warningMessage))
        {
            temperatureWarningText.text = warningMessage;
            temperatureWarningText.gameObject.SetActive(true);
        }
        else
        {
            temperatureWarningText.text = "";
            temperatureWarningText.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 检查湿度警告
    /// </summary>
    private void CheckHumidityWarning(float humidity)
    {
        if (humidityWarningText == null) return;
        
        string warningMessage = "";
        
        if (humidity < humidityMin)
        {
            warningMessage = "警告：湿度过低";
        }
        else if (humidity > humidityMax)
        {
            warningMessage = "警告：湿度过高";
        }
        
        // 更新湿度警告文本显示
        if (!string.IsNullOrEmpty(warningMessage))
        {
            humidityWarningText.text = warningMessage;
            humidityWarningText.gameObject.SetActive(true);
        }
        else
        {
            humidityWarningText.text = "";
            humidityWarningText.gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        // 取消订阅
        if (environmentManager != null)
        {
            environmentManager.OnEnvironmentChanged -= CheckEnvironmentWarnings;
        }
    }
}

using System;
using UnityEngine;

namespace TreePlanQAQ.OrangeTree
{
    /// <summary>
    /// 环境管理器 - 动态环境系统
    /// </summary>
    public class EnvironmentManager : MonoBehaviour
    {
        [Header("当前环境参数")]
        [SerializeField]
        [Range(-20f, 50f)]
        private float temperature = 26f;  // 默认26°C（橘子树最适温度23-29°C的中间值）
        
        [SerializeField]
        [Range(0f, 100f)]
        private float humidity = 75f;  // 默认75%（橘子树最适湿度）
        
        [SerializeField]
        [Range(0f, 1000f)]
        private float sunlight = 600f;
        
        [Header("动态变化设置")]
        [Tooltip("是否启用自动变化")]
        public bool enableDynamicChange = false;  // 默认关闭，等待开始生长
        
        [Tooltip("温度变化速度（°C/秒）")]
        [Range(0.1f, 10f)]
        public float temperatureChangeSpeed = 1f;  // 温度变化速度（摄氏度/秒）
        
        [Tooltip("湿度变化速度（%/秒）")]
        [Range(1f, 20f)]
        public float humidityChangeSpeed = 2.5f;  // 湿度变化速度（百分比/秒）
        
        [Tooltip("变化方向改变的时间间隔（秒）")]
        [Range(1f, 10f)]
        public float directionChangeInterval = 3f;
        
        [Header("变化范围")]
        [Tooltip("温度变化范围（摄氏度）")]
        public Vector2 temperatureRange = new Vector2(-20f, 50f);
        
        [Tooltip("湿度变化范围（百分比）")]
        public Vector2 humidityRange = new Vector2(0f, 100f);
        
        // 私有变量
        private float temperatureDirection = 1f; // 1为上升，-1为下降
        private float humidityDirection = 1f;
        private float timeSinceLastDirectionChange = 0f;
        
        // 事件
        public event Action<float, float, float> OnEnvironmentChanged;
        
        // 属性
        public float Temperature => temperature;
        public float Humidity => humidity;
        public float Sunlight => sunlight;
        
        // 单例
        public static EnvironmentManager Instance { get; private set; }
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        private void Start()
        {
            // 强制确保初始状态不启用动态变化
            enableDynamicChange = false;
            
            // 随机初始方向
            temperatureDirection = UnityEngine.Random.value > 0.5f ? 1f : -1f;
            humidityDirection = UnityEngine.Random.value > 0.5f ? 1f : -1f;
            
            Debug.Log($"🌡️ EnvironmentManager 初始化 - 温度:{temperature:F1}°C, 湿度:{humidity:F1}%, 动态变化:{enableDynamicChange}");
        }
        
        private void Update()
        {
            if (enableDynamicChange)
            {
                // 更新计时器
                timeSinceLastDirectionChange += Time.deltaTime;
                
                // 定期随机改变方向
                if (timeSinceLastDirectionChange >= directionChangeInterval)
                {
                    timeSinceLastDirectionChange = 0f;
                    
                    // 随机决定是否改变方向
                    if (UnityEngine.Random.value > 0.5f)
                    {
                        temperatureDirection *= -1f;
                    }
                    if (UnityEngine.Random.value > 0.5f)
                    {
                        humidityDirection *= -1f;
                    }
                }
                
                // 动态改变温度
                float tempDelta = temperatureChangeSpeed * temperatureDirection * Time.deltaTime;
                temperature += tempDelta;
                
                // 温度到达边界时反转方向
                if (temperature >= temperatureRange.y)
                {
                    temperature = temperatureRange.y;
                    temperatureDirection = -1f;
                }
                else if (temperature <= temperatureRange.x)
                {
                    temperature = temperatureRange.x;
                    temperatureDirection = 1f;
                }
                
                // 动态改变湿度
                float humidDelta = humidityChangeSpeed * humidityDirection * Time.deltaTime;
                humidity += humidDelta;
                
                // 湿度到达边界时反转方向
                if (humidity >= humidityRange.y)
                {
                    humidity = humidityRange.y;
                    humidityDirection = -1f;
                }
                else if (humidity <= humidityRange.x)
                {
                    humidity = humidityRange.x;
                    humidityDirection = 1f;
                }
                
                NotifyEnvironmentChanged();
            }
        }
        
        /// <summary>
        /// 设置温度（玩家调节）
        /// </summary>
        public void SetTemperature(float value)
        {
            temperature = Mathf.Clamp(value, temperatureRange.x, temperatureRange.y);
            NotifyEnvironmentChanged();
        }
        
        /// <summary>
        /// 调整温度（增加或减少）
        /// </summary>
        public void AdjustTemperature(float delta)
        {
            temperature = Mathf.Clamp(temperature + delta, temperatureRange.x, temperatureRange.y);
            NotifyEnvironmentChanged();
        }
        
        /// <summary>
        /// 设置湿度（玩家调节）
        /// </summary>
        public void SetHumidity(float value)
        {
            humidity = Mathf.Clamp(value, humidityRange.x, humidityRange.y);
            NotifyEnvironmentChanged();
        }
        
        /// <summary>
        /// 调整湿度（增加或减少）
        /// </summary>
        public void AdjustHumidity(float delta)
        {
            humidity = Mathf.Clamp(humidity + delta, humidityRange.x, humidityRange.y);
            NotifyEnvironmentChanged();
        }
        
        /// <summary>
        /// 设置光照
        /// </summary>
        public void SetSunlight(float value)
        {
            sunlight = Mathf.Clamp(value, 0f, 1000f);
            NotifyEnvironmentChanged();
        }
        
        /// <summary>
        /// 通知环境变化
        /// </summary>
        private void NotifyEnvironmentChanged()
        {
            OnEnvironmentChanged?.Invoke(temperature, humidity, sunlight);
            
            // 更新所有橘子树
            OrangeTreeController[] trees = FindObjectsOfType<OrangeTreeController>();
            foreach (var tree in trees)
            {
                tree.SetEnvironment(temperature, humidity, sunlight);
            }
        }
        
        private void OnValidate()
        {
            // 在Inspector中修改值时也触发更新
            if (Application.isPlaying)
            {
                NotifyEnvironmentChanged();
            }
        }
        
        /// <summary>
        /// 重置环境到最佳状态
        /// </summary>
        public void ResetToOptimal()
        {
            temperature = 26f;  // 26°C（橘子树最适温度）
            humidity = 75f;     // 75%（橘子树最适湿度）
            sunlight = 600f;
            enableDynamicChange = false;  // 重置时停止动态变化
            NotifyEnvironmentChanged();
        }
        
        /// <summary>
        /// 开始动态变化（由开始生长按钮调用）
        /// </summary>
        public void StartDynamicChange()
        {
            enableDynamicChange = true;
            Debug.Log("🌡️ 环境开始动态变化");
        }
        
        /// <summary>
        /// 停止动态变化
        /// </summary>
        public void StopDynamicChange()
        {
            enableDynamicChange = false;
            Debug.Log("🌡️ 环境停止动态变化");
        }
    }
}

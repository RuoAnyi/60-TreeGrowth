using UnityEngine;

namespace TreePlanQAQ.OrangeTree
{
    /// <summary>
    /// 橘子树生长配置
    /// </summary>
    [CreateAssetMenu(fileName = "OrangeTreeConfig", menuName = "OrangeTree/Growth Config")]
    public class GrowthConfig : ScriptableObject
    {
        [Header("基础生长参数")]
        [Tooltip("基础生长速率（每秒增长的百分比）")]
        [Range(0.1f, 10f)]
        public float baseGrowthRate = 3.33f; // 约30秒完成生长（100% ÷ 3.33 ≈ 30秒）
        
        [Header("环境影响系数")]
        [Tooltip("温度影响权重")]
        [Range(0f, 1f)]
        public float temperatureWeight = 0.4f;
        
        [Tooltip("湿度影响权重")]
        [Range(0f, 1f)]
        public float humidityWeight = 0.3f;
        
        [Tooltip("光照影响权重")]
        [Range(0f, 1f)]
        public float sunlightWeight = 0.3f;
        
        [Header("最佳环境条件")]
        [Tooltip("最佳温度（摄氏度）")]
        public float optimalTemperature = 25f;
        
        [Tooltip("温度容差范围")]
        public float temperatureTolerance = 10f;
        
        [Tooltip("最佳湿度（百分比）")]
        public float optimalHumidity = 60f;
        
        [Tooltip("湿度容差范围")]
        public float humidityTolerance = 20f;
        
        [Tooltip("最佳光照（lux）")]
        public float optimalSunlight = 600f;
        
        [Tooltip("光照容差范围")]
        public float sunlightTolerance = 300f;
        
        /// <summary>
        /// 计算环境因子（0-1）
        /// </summary>
        public float CalculateEnvironmentFactor(float temperature, float humidity, float sunlight)
        {
            float tempFactor = CalculateFactor(temperature, optimalTemperature, temperatureTolerance);
            float humidFactor = CalculateFactor(humidity, optimalHumidity, humidityTolerance);
            float sunFactor = CalculateFactor(sunlight, optimalSunlight, sunlightTolerance);
            
            return tempFactor * temperatureWeight + 
                   humidFactor * humidityWeight + 
                   sunFactor * sunlightWeight;
        }
        
        private float CalculateFactor(float value, float optimal, float tolerance)
        {
            float deviation = Mathf.Abs(value - optimal);
            if (deviation >= tolerance)
                return 0f;
            return 1f - (deviation / tolerance);
        }
    }
}

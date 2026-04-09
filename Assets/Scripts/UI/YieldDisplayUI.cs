using UnityEngine;
using UnityEngine.UI;
using TreePlanQAQ.OrangeTree;

/// <summary>
/// 产量显示UI
/// 实时显示当前预期产量
/// </summary>
public class YieldDisplayUI : MonoBehaviour
{
    [Header("UI引用")]
    [SerializeField] private Text yieldText;
    
    [Header("显示格式")]
    [SerializeField] private string displayFormat = "预期产量: {0}kg";
    
    [Header("颜色设置")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color warningColor = Color.yellow;
    [SerializeField] private Color dangerColor = Color.red;
    [SerializeField] private int warningThreshold = 30; // 警告阈值
    [SerializeField] private int dangerThreshold = 10;  // 危险阈值
    
    private OrangeTreeController treeController;
    
    private void Start()
    {
        // 查找树控制器
        treeController = FindObjectOfType<OrangeTreeController>();
        if (treeController != null)
        {
            // 订阅产量变化事件
            treeController.OnYieldChanged += OnYieldChanged;
            
            // 初始化显示
            UpdateDisplay(treeController.CurrentYield);
        }
        else
        {
            Debug.LogWarning("找不到 OrangeTreeController！");
        }
    }
    
    /// <summary>
    /// 产量变化回调
    /// </summary>
    private void OnYieldChanged(int yield)
    {
        UpdateDisplay(yield);
    }
    
    /// <summary>
    /// 更新显示
    /// </summary>
    private void UpdateDisplay(int yield)
    {
        if (yieldText == null) return;
        
        // 更新文本
        yieldText.text = string.Format(displayFormat, yield);
        
        // 根据产量设置颜色
        if (yield <= dangerThreshold)
        {
            yieldText.color = dangerColor;
        }
        else if (yield <= warningThreshold)
        {
            yieldText.color = warningColor;
        }
        else
        {
            yieldText.color = normalColor;
        }
    }
    
    private void OnDestroy()
    {
        if (treeController != null)
        {
            treeController.OnYieldChanged -= OnYieldChanged;
        }
    }
}

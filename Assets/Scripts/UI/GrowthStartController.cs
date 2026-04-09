using UnityEngine;
using UnityEngine.UI;
using TreePlanQAQ.OrangeTree;

/// <summary>
/// 生长开始控制器
/// 独立的开始按钮，控制植物生长的启动
/// </summary>
public class GrowthStartController : MonoBehaviour
{
    [Header("按钮引用")]
    [SerializeField] private Button startButton;
    [SerializeField] private Text buttonText;
    
    [Header("目标控制器")]
    [SerializeField] private OrangeTreeController treeController;
    
    private bool isGrowthStarted = false;

    private void Start()
    {
        // 自动查找树控制器
        if (treeController == null)
        {
            treeController = FindObjectOfType<OrangeTreeController>();
        }
        
        if (treeController == null)
        {
            Debug.LogError("找不到 OrangeTreeController！请确保场景中有橘子树");
            return;
        }
        
        // 绑定按钮事件
        if (startButton != null)
        {
            startButton.onClick.RemoveAllListeners();
            startButton.onClick.AddListener(OnStartButtonClicked);
        }
        else
        {
            Debug.LogWarning("startButton 为空！请在 Inspector 中设置按钮引用");
        }
        
        // 初始化按钮文本和状态
        UpdateButtonState();
        
        // 初始时暂停生长
        if (treeController != null && !treeController.IsPaused)
        {
            treeController.TogglePause();
        }
    }

    /// <summary>
    /// 开始按钮点击事件
    /// </summary>
    private void OnStartButtonClicked()
    {
        if (treeController == null) return;
        
        if (!isGrowthStarted)
        {
            // 第一次点击：开始生长
            isGrowthStarted = true;
            
            if (treeController.IsPaused)
            {
                treeController.TogglePause();
            }
            
            // 启动环境动态变化
            EnvironmentManager envManager = EnvironmentManager.Instance;
            if (envManager != null)
            {
                envManager.StartDynamicChange();
            }
            
            Debug.Log("植物生长已开始！环境开始动态变化！");
        }
        else
        {
            // 之后点击：切换暂停/继续
            treeController.TogglePause();
            
            // 同步环境变化状态
            EnvironmentManager envManager = EnvironmentManager.Instance;
            if (envManager != null)
            {
                if (treeController.IsPaused)
                {
                    envManager.StopDynamicChange();
                }
                else
                {
                    envManager.StartDynamicChange();
                }
            }
            
            Debug.Log($"植物生长状态: {(treeController.IsPaused ? "暂停" : "继续")}");
        }
        
        UpdateButtonState();
    }

    /// <summary>
    /// 更新按钮状态和文本
    /// </summary>
    private void UpdateButtonState()
    {
        if (buttonText == null || treeController == null) return;
        
        if (!isGrowthStarted)
        {
            buttonText.text = "开始生长";
        }
        else
        {
            buttonText.text = treeController.IsPaused ? "继续生长" : "暂停生长";
        }
    }

    /// <summary>
    /// 重置状态（供外部调用）
    /// </summary>
    public void ResetGrowthState()
    {
        isGrowthStarted = false;
        
        if (treeController != null && !treeController.IsPaused)
        {
            treeController.TogglePause();
        }
        
        // 停止环境动态变化
        EnvironmentManager envManager = EnvironmentManager.Instance;
        if (envManager != null)
        {
            envManager.StopDynamicChange();
            envManager.ResetToOptimal();  // 重置到初始值
        }
        
        UpdateButtonState();
        Debug.Log("生长状态已重置");
    }

    private void OnDestroy()
    {
        if (startButton != null)
        {
            startButton.onClick.RemoveAllListeners();
        }
    }
}

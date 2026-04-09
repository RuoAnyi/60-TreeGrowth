using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

/// <summary>
/// 自动设置收获消息UI的引用
/// </summary>
public class SetupHarvestMessageUI : EditorWindow
{
    [MenuItem("橘子树/设置收获消息UI")]
    public static void Setup()
    {
        // 查找HarvestMessageUI对象
        HarvestMessageUI harvestUI = FindObjectOfType<HarvestMessageUI>();
        if (harvestUI == null)
        {
            Debug.LogError("❌ 未找到HarvestMessageUI组件！");
            return;
        }

        // 获取序列化对象
        SerializedObject so = new SerializedObject(harvestUI);

        // 查找子对象
        Transform messagePanel = harvestUI.transform.Find("MessagePanel");
        if (messagePanel != null)
        {
            // 设置messagePanel引用
            SerializedProperty messagePanelProp = so.FindProperty("messagePanel");
            if (messagePanelProp != null)
            {
                messagePanelProp.objectReferenceValue = messagePanel.gameObject;
                Debug.Log("✅ 设置messagePanel引用");
            }

            // 查找MessageText
            Transform messageText = messagePanel.Find("MessageText");
            if (messageText != null)
            {
                Text textComponent = messageText.GetComponent<Text>();
                if (textComponent != null)
                {
                    SerializedProperty messageTextProp = so.FindProperty("messageText");
                    if (messageTextProp != null)
                    {
                        messageTextProp.objectReferenceValue = textComponent;
                        Debug.Log("✅ 设置messageText引用");
                    }
                }
            }

            // 查找CloseButton
            Transform closeButton = messagePanel.Find("CloseButton");
            if (closeButton != null)
            {
                Button buttonComponent = closeButton.GetComponent<Button>();
                if (buttonComponent != null)
                {
                    SerializedProperty closeButtonProp = so.FindProperty("closeButton");
                    if (closeButtonProp != null)
                    {
                        closeButtonProp.objectReferenceValue = buttonComponent;
                        Debug.Log("✅ 设置closeButton引用");
                    }
                }
            }
        }

        // 应用修改
        so.ApplyModifiedProperties();
        EditorUtility.SetDirty(harvestUI);

        Debug.Log("🎉 收获消息UI设置完成！");
    }
}

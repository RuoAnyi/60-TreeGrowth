using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

/// <summary>
/// 创建环境警告UI的编辑器工具
/// 创建两个独立的文本框分别显示温度和湿度警告
/// </summary>
public class CreateEnvironmentWarningUI : EditorWindow
{
    [MenuItem("GameObject/UI/Environment Warning UI", false, 10)]
    static void CreateWarningUI(MenuCommand menuCommand)
    {
        // 查找或创建Canvas
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObj = new GameObject("Canvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
            Undo.RegisterCreatedObjectUndo(canvasObj, "Create Canvas");
        }

        // 创建温度警告文本对象
        GameObject tempWarningTextObj = new GameObject("TemperatureWarningText");
        tempWarningTextObj.transform.SetParent(canvas.transform, false);
        
        Text tempWarningText = tempWarningTextObj.AddComponent<Text>();
        tempWarningText.text = "警告：温度过高";
        tempWarningText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        tempWarningText.fontSize = 24;
        tempWarningText.color = Color.red;
        tempWarningText.alignment = TextAnchor.MiddleCenter;
        tempWarningText.horizontalOverflow = HorizontalWrapMode.Overflow;
        tempWarningText.verticalOverflow = VerticalWrapMode.Overflow;
        
        // 设置温度警告文本位置（屏幕上方）
        RectTransform tempRectTransform = tempWarningTextObj.GetComponent<RectTransform>();
        tempRectTransform.anchorMin = new Vector2(0.5f, 0.85f);
        tempRectTransform.anchorMax = new Vector2(0.5f, 0.85f);
        tempRectTransform.pivot = new Vector2(0.5f, 0.5f);
        tempRectTransform.anchoredPosition = Vector2.zero;
        tempRectTransform.sizeDelta = new Vector2(400, 50);
        
        // 创建湿度警告文本对象
        GameObject humidWarningTextObj = new GameObject("HumidityWarningText");
        humidWarningTextObj.transform.SetParent(canvas.transform, false);
        
        Text humidWarningText = humidWarningTextObj.AddComponent<Text>();
        humidWarningText.text = "警告：湿度过低";
        humidWarningText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        humidWarningText.fontSize = 24;
        humidWarningText.color = Color.red;
        humidWarningText.alignment = TextAnchor.MiddleCenter;
        humidWarningText.horizontalOverflow = HorizontalWrapMode.Overflow;
        humidWarningText.verticalOverflow = VerticalWrapMode.Overflow;
        
        // 设置湿度警告文本位置（温度警告下方）
        RectTransform humidRectTransform = humidWarningTextObj.GetComponent<RectTransform>();
        humidRectTransform.anchorMin = new Vector2(0.5f, 0.78f);
        humidRectTransform.anchorMax = new Vector2(0.5f, 0.78f);
        humidRectTransform.pivot = new Vector2(0.5f, 0.5f);
        humidRectTransform.anchoredPosition = Vector2.zero;
        humidRectTransform.sizeDelta = new Vector2(400, 50);
        
        // 添加警告控制器组件
        GameObject controllerObj = new GameObject("EnvironmentWarningController");
        controllerObj.transform.SetParent(canvas.transform, false);
        EnvironmentWarningUI controller = controllerObj.AddComponent<EnvironmentWarningUI>();
        
        // 使用反射设置私有字段
        var tempWarningTextField = typeof(EnvironmentWarningUI).GetField("temperatureWarningText", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (tempWarningTextField != null)
        {
            tempWarningTextField.SetValue(controller, tempWarningText);
        }
        
        var humidWarningTextField = typeof(EnvironmentWarningUI).GetField("humidityWarningText", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (humidWarningTextField != null)
        {
            humidWarningTextField.SetValue(controller, humidWarningText);
        }
        
        // 注册Undo
        Undo.RegisterCreatedObjectUndo(tempWarningTextObj, "Create Temperature Warning Text");
        Undo.RegisterCreatedObjectUndo(humidWarningTextObj, "Create Humidity Warning Text");
        Undo.RegisterCreatedObjectUndo(controllerObj, "Create Warning Controller");
        
        // 选中创建的对象
        Selection.activeGameObject = controllerObj;
        
        Debug.Log("✅ 环境警告UI创建成功！已创建两个独立的警告文本框：\n" +
                  "- TemperatureWarningText（温度警告）\n" +
                  "- HumidityWarningText（湿度警告）");
    }

    [MenuItem("Tools/Environment/Setup Warning UI")]
    static void SetupWarningUI()
    {
        // 查找现有的EnvironmentWarningUI
        EnvironmentWarningUI existingController = FindObjectOfType<EnvironmentWarningUI>();
        
        if (existingController != null)
        {
            EditorUtility.DisplayDialog("警告UI已存在", 
                "场景中已经存在EnvironmentWarningUI组件。", "确定");
            Selection.activeGameObject = existingController.gameObject;
            return;
        }
        
        CreateWarningUI(null);
    }
}

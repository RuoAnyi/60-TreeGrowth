# 环境警告UI使用说明

## 功能说明

使用**两个独立的文本框**分别显示温度和湿度警告：

### 温度警告文本框
- **温度过低**：温度 < 30
- **温度过高**：温度 > 70

### 湿度警告文本框
- **湿度过低**：湿度 < 40
- **湿度过高**：湿度 > 80

每个警告文本框独立显示和隐藏，互不影响。当环境恢复正常时，对应的警告自动隐藏。

## 快速设置（推荐）

### 方法1：使用菜单命令
1. 在Unity编辑器中，点击菜单 **Tools → Environment → Setup Warning UI**
2. 系统会自动创建：
   - **TemperatureWarningText**（温度警告文本框）
   - **HumidityWarningText**（湿度警告文本框）
   - **EnvironmentWarningController**（控制器）
3. 完成！

### 方法2：使用GameObject菜单
1. 在Unity编辑器中，点击菜单 **GameObject → UI → Environment Warning UI**
2. 系统会自动创建Canvas（如果不存在）和两个警告文本框
3. 完成！

## 手动设置（高级）

如果你想自定义警告UI的位置和样式：

### 步骤1：创建温度警告文本
1. 在Hierarchy中右键点击 **UI → Text**
2. 命名为 "TemperatureWarningText"
3. 设置文本属性：
   - Font Size: 24
   - Color: 红色 (255, 0, 0)
   - Alignment: 居中
   - Text: 留空（运行时自动填充）
4. 设置位置（RectTransform）：
   - Anchor: 顶部居中 (0.5, 0.85)
   - Position: (0, 0)
   - Width: 400
   - Height: 50

### 步骤2：创建湿度警告文本
1. 在Hierarchy中右键点击 **UI → Text**
2. 命名为 "HumidityWarningText"
3. 设置文本属性：
   - Font Size: 24
   - Color: 红色 (255, 0, 0)
   - Alignment: 居中
   - Text: 留空（运行时自动填充）
4. 设置位置（RectTransform）：
   - Anchor: 顶部居中 (0.5, 0.78)
   - Position: (0, 0)
   - Width: 400
   - Height: 50

### 步骤3：添加控制器
1. 在Canvas下创建空对象，命名为 "EnvironmentWarningController"
2. 添加组件 **EnvironmentWarningUI**
3. 在Inspector中：
   - 将 TemperatureWarningText 拖到 **Temperature Warning Text** 字段
   - 将 HumidityWarningText 拖到 **Humidity Warning Text** 字段
   - 检查阈值设置（默认值已配置好）

## 配置参数

在 **EnvironmentWarningUI** 组件的Inspector中可以调整：

### 警告阈值
- **Temperature Min**: 30（温度下限）
- **Temperature Max**: 70（温度上限）
- **Humidity Min**: 40（湿度下限）
- **Humidity Max**: 80（湿度上限）

### 警告颜色
- **Warning Color**: 红色（可自定义）

## 警告文本示例

### 只有温度警告
```
[温度警告文本框]
警告：温度过高

[湿度警告文本框]
（隐藏）
```

### 只有湿度警告
```
[温度警告文本框]
（隐藏）

[湿度警告文本框]
警告：湿度过低
```

### 同时显示两个警告
```
[温度警告文本框]
警告：温度过低

[湿度警告文本框]
警告：湿度过高
```

### 无警告
```
[温度警告文本框]
（隐藏）

[湿度警告文本框]
（隐藏）
```

## 工作原理

1. **实时监控**：EnvironmentWarningUI 订阅 EnvironmentManager 的环境变化事件
2. **独立检测**：
   - `CheckTemperatureWarning()` 检查温度并更新温度警告文本框
   - `CheckHumidityWarning()` 检查湿度并更新湿度警告文本框
3. **动态显示**：
   - 温度超出范围：显示温度警告文本框
   - 湿度超出范围：显示湿度警告文本框
   - 恢复正常：隐藏对应的警告文本框
4. **互不干扰**：两个警告文本框完全独立，可以同时显示或分别显示

## 自定义样式

### 修改文本位置

#### 温度警告文本
选中 TemperatureWarningText，调整 RectTransform：
- 左上角：Anchor (0.0, 1.0), Position (200, -50)
- 右上角：Anchor (1.0, 1.0), Position (-200, -50)
- 顶部居中：Anchor (0.5, 0.85), Position (0, 0)

#### 湿度警告文本
选中 HumidityWarningText，调整 RectTransform：
- 左上角：Anchor (0.0, 1.0), Position (200, -120)
- 右上角：Anchor (1.0, 1.0), Position (-200, -120)
- 顶部居中：Anchor (0.5, 0.78), Position (0, 0)

### 修改文本样式
分别选中两个警告文本，调整 Text 组件：
- **字体大小**：Font Size（建议：20-30）
- **颜色**：Color（温度可用橙红色，湿度可用蓝色）
- **对齐方式**：Alignment
- **字体**：Font

### 添加背景
1. 选中警告文本对象
2. 添加组件 **Image**
3. 设置背景颜色和透明度
4. 调整 Image 的 Z 顺序（在Text下方）

### 不同颜色区分
```csharp
// 在 EnvironmentWarningUI.cs 中添加
[Header("警告颜色")]
[SerializeField] private Color temperatureWarningColor = new Color(1f, 0.5f, 0f); // 橙色
[SerializeField] private Color humidityWarningColor = new Color(0f, 0.5f, 1f);    // 蓝色

// 在 InitializeWarningText 中使用不同颜色
private void InitializeWarningText(Text text, Color color)
{
    if (text != null)
    {
        text.color = color;
        text.text = "";
        text.gameObject.SetActive(false);
    }
}
```

### 添加动画效果
1. 选中 WarningText
2. 添加组件 **Animator**
3. 创建闪烁或淡入淡出动画

## 测试

### 测试温度警告
1. 运行游戏
2. 在Inspector中找到 EnvironmentManager
3. 手动调整 Temperature 值：
   - 设置为 20 → **温度警告文本框**应显示"警告：温度过低"
   - 设置为 80 → **温度警告文本框**应显示"警告：温度过高"
   - 设置为 50 → **温度警告文本框**应隐藏

### 测试湿度警告
1. 运行游戏
2. 在Inspector中找到 EnvironmentManager
3. 手动调整 Humidity 值：
   - 设置为 30 → **湿度警告文本框**应显示"警告：湿度过低"
   - 设置为 90 → **湿度警告文本框**应显示"警告：湿度过高"
   - 设置为 60 → **湿度警告文本框**应隐藏

### 测试独立显示
1. 设置温度为 20（过低），湿度为 60（正常）
   - 结果：只显示温度警告，湿度警告隐藏
2. 设置温度为 50（正常），湿度为 90（过高）
   - 结果：只显示湿度警告，温度警告隐藏

### 测试同时警告
1. 同时设置温度为 20（过低）和湿度为 90（过高）
2. 结果：**两个警告文本框同时显示**
   - 温度警告文本框：警告：温度过低
   - 湿度警告文本框：警告：湿度过高

## 故障排除

### 温度警告不显示
1. 检查 EnvironmentWarningUI 组件的 **Temperature Warning Text** 字段是否已分配
2. 检查 TemperatureWarningText 对象是否存在
3. 查看Console是否有错误信息

### 湿度警告不显示
1. 检查 EnvironmentWarningUI 组件的 **Humidity Warning Text** 字段是否已分配
2. 检查 HumidityWarningText 对象是否存在
3. 查看Console是否有错误信息

### 警告文本位置重叠
1. 检查两个文本框的 RectTransform 位置
2. 调整 Anchor 和 Position 使它们分开显示
3. 建议：温度在上（Y=0.85），湿度在下（Y=0.78）

### 警告一直显示
1. 检查当前温度和湿度值
2. 确认阈值设置是否正确
3. 查看Console的调试信息

## 扩展功能建议

### 1. 为两个警告添加不同的声音
```csharp
[SerializeField] private AudioClip temperatureWarningSound;
[SerializeField] private AudioClip humidityWarningSound;
private AudioSource audioSource;

private void CheckTemperatureWarning(float temperature)
{
    // ... 检查逻辑 ...
    
    if (!string.IsNullOrEmpty(warningMessage) && !temperatureWarningText.gameObject.activeSelf)
    {
        audioSource.PlayOneShot(temperatureWarningSound);
    }
}
```

### 2. 为两个警告添加独立的闪烁效果
```csharp
private void Update()
{
    // 温度警告闪烁
    if (temperatureWarningText.gameObject.activeSelf)
    {
        float alpha = Mathf.PingPong(Time.time * 2f, 1f);
        Color color = temperatureWarningText.color;
        color.a = alpha;
        temperatureWarningText.color = color;
    }
    
    // 湿度警告闪烁
    if (humidityWarningText.gameObject.activeSelf)
    {
        float alpha = Mathf.PingPong(Time.time * 2f, 1f);
        Color color = humidityWarningText.color;
        color.a = alpha;
        humidityWarningText.color = color;
    }
}
```

### 3. 添加警告级别（黄色预警 + 红色警告）
```csharp
[Header("预警阈值")]
[SerializeField] private float temperatureWarningMin = 35f;  // 黄色预警
[SerializeField] private float temperatureWarningMax = 65f;
[SerializeField] private float humidityWarningMin = 45f;
[SerializeField] private float humidityWarningMax = 75f;

private void CheckTemperatureWarning(float temperature)
{
    if (temperature < temperatureMin || temperature > temperatureMax)
    {
        temperatureWarningText.text = "警告：温度危险";
        temperatureWarningText.color = Color.red;
        temperatureWarningText.gameObject.SetActive(true);
    }
    else if (temperature < temperatureWarningMin || temperature > temperatureWarningMax)
    {
        temperatureWarningText.text = "注意：温度偏离";
        temperatureWarningText.color = Color.yellow;
        temperatureWarningText.gameObject.SetActive(true);
    }
    else
    {
        temperatureWarningText.gameObject.SetActive(false);
    }
}
```

### 4. 显示具体数值
```csharp
private void CheckTemperatureWarning(float temperature)
{
    string warningMessage = "";
    
    if (temperature < temperatureMin)
    {
        warningMessage = $"警告：温度过低 ({temperature:F1})";
    }
    else if (temperature > temperatureMax)
    {
        warningMessage = $"警告：温度过高 ({temperature:F1})";
    }
    
    // ... 更新显示 ...
}
```

### 5. 添加警告图标
在每个警告文本旁边添加图标：
1. 创建 Image 对象作为警告文本的子对象
2. 设置图标精灵（⚠️ 或自定义图标）
3. 与文本一起显示/隐藏

## 相关文件

- `Assets/Scripts/UI/EnvironmentWarningUI.cs` - 警告UI控制器
- `Assets/Scripts/Editor/CreateEnvironmentWarningUI.cs` - 编辑器工具
- `Assets/Scripts/OrangeTree/EnvironmentManager.cs` - 环境管理器
- `0-100温度湿度系统说明.md` - 温度湿度系统说明

## 技术细节

### 事件订阅
```csharp
environmentManager.OnEnvironmentChanged += CheckEnvironmentWarnings;
```

### 独立检测逻辑
```csharp
// 温度检测
private void CheckTemperatureWarning(float temperature)
{
    if (temperature < temperatureMin)
        warningMessage = "警告：温度过低";
    else if (temperature > temperatureMax)
        warningMessage = "警告：温度过高";
}

// 湿度检测
private void CheckHumidityWarning(float humidity)
{
    if (humidity < humidityMin)
        warningMessage = "警告：湿度过低";
    else if (humidity > humidityMax)
        warningMessage = "警告：湿度过高";
}
```

### 独立显示控制
```csharp
// 温度警告文本框
if (!string.IsNullOrEmpty(warningMessage))
{
    temperatureWarningText.text = warningMessage;
    temperatureWarningText.gameObject.SetActive(true);
}
else
{
    temperatureWarningText.gameObject.SetActive(false);
}

// 湿度警告文本框（独立控制）
if (!string.IsNullOrEmpty(warningMessage))
{
    humidityWarningText.text = warningMessage;
    humidityWarningText.gameObject.SetActive(true);
}
else
{
    humidityWarningText.gameObject.SetActive(false);
}
```

## UI布局建议

### 推荐布局1：垂直堆叠（默认）
```
屏幕顶部
├─ [温度警告] Y=0.85
└─ [湿度警告] Y=0.78
```

### 推荐布局2：左右分布
```
屏幕顶部
├─ [温度警告] X=0.25, Y=0.9
└─ [湿度警告] X=0.75, Y=0.9
```

### 推荐布局3：角落显示
```
├─ [温度警告] 左上角 (0.0, 1.0)
└─ [湿度警告] 右上角 (1.0, 1.0)
```

## 更新日志

### v2.0 (当前版本)
- ✅ **重大更新**：使用两个独立的文本框分别显示温度和湿度警告
- ✅ 温度警告和湿度警告独立显示和隐藏
- ✅ 支持同时显示两个警告
- ✅ 更清晰的UI布局
- ✅ 更新编辑器工具以创建两个文本框

### v1.0
- ✅ 初始版本
- ✅ 温度和湿度警告检测
- ✅ 实时更新显示
- ✅ 自动隐藏功能
- ✅ 编辑器快速设置工具

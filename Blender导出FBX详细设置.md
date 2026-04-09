# Blender 导出 FBX 详细设置指南

## 找到 "Embed Textures" 选项

### 步骤1：打开导出对话框

在 Blender 中：
1. 选择你的橘子树模型
2. 点击菜单：**File** → **Export** → **FBX (.fbx)**

### 步骤2：展开设置面板

导出对话框打开后，你会看到右侧有一个设置面板。

**如果看不到设置面板：**
- 点击右上角的 **齿轮图标** ⚙️
- 或者点击右侧的 **小箭头** ▶️ 展开面板

### 步骤3：找到 Path Mode

在右侧设置面板中，向下滚动，找到这些选项：

```
┌─────────────────────────────┐
│ Include                      │
│ ✅ Selected Objects          │
│ ✅ Active Collection         │
│ ✅ Custom Properties         │
│                              │
│ Transform                    │
│ Scale: 1.00                  │
│ ✅ Apply Scalings            │
│ Forward: -Z Forward          │
│ Up: Y Up                     │
│ ✅ Apply Unit                │
│ ✅ Use Space Transform       │
│                              │
│ Geometry                     │
│ ✅ Apply Modifiers           │
│ ✅ Loose Edges               │
│ ✅ Tangent Space             │
│                              │
│ ⬇️ 继续向下滚动...           │
└─────────────────────────────┘
```

### 步骤4：找到 Path Mode（关键！）

继续向下滚动，找到 **Path Mode** 部分：

```
┌─────────────────────────────┐
│ Path Mode                    │  ← 这里！
│ ▼ Auto                       │
│   - Auto                     │
│   - Absolute                 │
│   - Relative                 │
│   - Match                    │
│   - Strip Path               │
│   - Copy                     │  ← 选择这个！
└─────────────────────────────┘
```

**重要：** 将 Path Mode 改为 **Copy**

### 步骤5：找到 Embed Textures

**Path Mode 改为 Copy 后**，下面会出现一个新选项：

```
┌─────────────────────────────┐
│ Path Mode                    │
│ ▼ Copy                       │  ← 已选择 Copy
│                              │
│ ✅ Embed Textures            │  ← 这就是！勾选它！
└─────────────────────────────┘
```

**✅ 勾选 "Embed Textures"**

---

## 完整的推荐导出设置

### 基本设置

```
Include:
✅ Selected Objects          （只导出选中的对象）
✅ Custom Properties         （自定义属性）

Transform:
Scale: 1.00                  （缩放比例）
✅ Apply Scalings            （应用缩放）
Forward: -Z Forward          （前方向）
Up: Y Up                     （上方向）
✅ Apply Unit                （应用单位）
✅ Use Space Transform       （使用空间变换）

Geometry:
✅ Apply Modifiers           （应用修改器）
✅ Mesh                      （网格）
Smoothing: Face              （平滑方式）
✅ Tangent Space             （切线空间）
```

### 关键设置（纹理相关）

```
Path Mode: Copy              ← 必须选择 Copy
✅ Embed Textures            ← 必须勾选
```

---

## 如果找不到 "Embed Textures"

### 原因1：Path Mode 没有设置为 Copy

**解决：**
- 确保 Path Mode 选择的是 **Copy**
- 不是 Auto、Absolute、Relative 等其他选项

### 原因2：Blender 版本太旧

**检查版本：**
- Help → About Blender
- 查看版本号

**如果版本低于 2.8：**
- 建议升级到 Blender 3.0 或更高版本
- 下载地址：https://www.blender.org/download/

### 原因3：设置面板没有完全展开

**解决：**
- 滚动到最底部
- 点击各个折叠的部分展开

---

## 替代方案（如果还是找不到）

### 方案A：手动复制纹理

1. **找到纹理文件位置**
   - 在 Blender 中选择材质
   - Shader Editor → 选择 Image Texture 节点
   - 查看文件路径

2. **复制纹理文件**
   - 找到纹理文件所在文件夹
   - 复制所有纹理文件
   - 粘贴到 Unity 项目的 `Assets/Textures/` 文件夹

3. **导出 FBX**
   - 正常导出，不用担心纹理
   - 纹理已经手动复制了

### 方案B：打包资源后导出

1. **在 Blender 中打包资源**
   - File → External Data → Pack Resources
   - 这会将所有纹理打包到 .blend 文件中

2. **导出 FBX**
   - 正常导出
   - Path Mode 可以选择 Auto

3. **提取纹理**
   - File → External Data → Unpack Resources
   - 选择 "Write files to current directory"
   - 纹理会被提取到 .blend 文件所在文件夹

4. **复制到 Unity**
   - 将提取的纹理复制到 Unity 项目

---

## 导出步骤总结

### 完整流程：

1. **选择模型**
   - 在 3D 视图中选择橘子树

2. **打开导出**
   - File → Export → FBX (.fbx)

3. **设置导出选项**
   ```
   Include:
   ✅ Selected Objects
   
   Geometry:
   ✅ Apply Modifiers
   ✅ Mesh
   
   Path Mode: Copy              ← 重要！
   ✅ Embed Textures            ← 重要！
   ```

4. **选择保存位置**
   - 导航到：`TreePlanQAQ/Assets/Models/`
   - 文件名：`OrangeTree.fbx`

5. **点击导出**
   - 点击右下角的 "Export FBX" 按钮

---

## 验证导出是否成功

### 在 Unity 中检查：

1. **导入 FBX**
   - Unity 会自动导入

2. **展开 FBX**
   - 在 Project 窗口中，点击 FBX 左侧的小箭头
   - 查看是否有纹理文件

3. **如果看到纹理文件**
   - ✅ 导出成功！
   - 纹理已嵌入

4. **如果没有纹理文件**
   - ❌ 需要手动复制纹理
   - 或者重新导出，确保勾选了 Embed Textures

---

## 常见错误

### 错误1：Path Mode 是 Auto

```
❌ Path Mode: Auto
   Embed Textures: （看不到这个选项）
```

**修复：**
```
✅ Path Mode: Copy
   ✅ Embed Textures
```

### 错误2：没有选择模型

```
❌ Include: Active Collection
```

**修复：**
```
✅ Include: Selected Objects
```

### 错误3：纹理路径丢失

**症状：** Blender 中材质显示粉红色

**修复：**
1. File → External Data → Find Missing Files
2. 找到纹理文件所在文件夹
3. 重新链接纹理

---

## 快速检查清单

导出前检查：
- [ ] 模型已选中
- [ ] Path Mode 设置为 Copy
- [ ] Embed Textures 已勾选
- [ ] 保存位置正确（Unity 项目内）

导出后检查：
- [ ] FBX 文件已生成
- [ ] Unity 中能看到 FBX
- [ ] 展开 FBX 能看到纹理（如果有）

---

## 视频教程参考

如果还是不清楚，可以搜索：
- "Blender export FBX with textures"
- "Blender embed textures FBX"
- "Blender to Unity texture export"

---

## 需要帮助？

如果按照这个指南还是找不到 "Embed Textures"：

1. **告诉我你的 Blender 版本**
   - Help → About Blender

2. **截图你的导出设置面板**
   - 我可以帮你找到具体位置

3. **或者使用替代方案**
   - 手动复制纹理文件到 Unity

---

记住：**Path Mode 必须是 Copy，才会出现 Embed Textures 选项！**

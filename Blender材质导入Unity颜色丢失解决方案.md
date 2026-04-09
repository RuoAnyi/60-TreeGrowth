# Blender 材质导入 Unity 颜色丢失解决方案

## 问题原因

Blender 使用的是 **Principled BSDF** 材质系统（基于物理的渲染），而 Unity 使用的是自己的材质系统（Standard Shader 或 URP/HDRP）。两者不兼容，所以：

- ❌ Blender 的材质节点不会自动转换
- ❌ 纹理贴图不会自动导入
- ❌ 颜色信息会丢失

## 解决方案

### 方案一：导出纹理并在Unity中重新设置（推荐）

#### 步骤1：在Blender中导出纹理

你的材质使用了这些纹理：
- `texture_pbr_20250901.png` - Base Color（基础颜色）
- `texture_pbr_20250901_roughness.png` - Roughness（粗糙度）
- `texture_pbr_20250901_metallic.png` - Metallic（金属度）
- `texture_pbr_20250901_normal.png` - Normal Map（法线贴图）

**导出这些纹理文件：**

1. 在Blender中，找到这些纹理文件的位置
2. 将它们复制到Unity项目的 `Assets/Textures/` 文件夹

#### 步骤2：导出FBX模型

在Blender中：

1. 选择你的橘子树模型
2. File → Export → FBX (.fbx)
3. 导出设置：
   - ✅ Selected Objects（只导出选中的）
   - ✅ Apply Modifiers（应用修改器）
   - ✅ Mesh（网格）
   - Path Mode: Copy（复制纹理）
   - ✅ Embed Textures（嵌入纹理）- **重要！**
4. 导出到Unity项目的 `Assets/Models/` 文件夹

#### 步骤3：在Unity中设置材质

1. **导入纹理**
   - 将纹理文件放到 `Assets/Textures/OrangeTree/` 文件夹
   - 选中法线贴图，在Inspector中：
     - Texture Type: Normal map
     - 点击 Apply

2. **创建材质**
   - 在Project窗口右键 → Create → Material
   - 命名为 `OrangeTreeMaterial`

3. **设置材质属性**
   - Shader: Standard（或URP/Lit）
   - Albedo: 拖入 `texture_pbr_20250901.png`
   - Metallic: 设置为 0（如果有金属贴图，拖入）
   - Smoothness: 调整粗糙度（或拖入粗糙度贴图）
   - Normal Map: 拖入 `texture_pbr_20250901_normal.png`

4. **应用材质到模型**
   - 在Hierarchy中选择橘子树模型
   - 将材质拖到模型上

---

### 方案二：使用顶点颜色（简单但效果有限）

如果你在Blender中使用了顶点颜色：

#### 在Blender中：

1. 进入 Vertex Paint 模式
2. 给模型上色
3. 导出FBX时确保勾选 "Vertex Colors"

#### 在Unity中：

1. 创建材质
2. Shader 选择支持顶点颜色的着色器
3. 应用到模型

---

### 方案三：烘焙纹理（最佳效果）

#### 在Blender中烘焙：

1. **准备UV**
   - 确保模型有正确的UV展开
   - UV Editing 模式检查UV

2. **烘焙设置**
   - 切换到 Cycles 渲染引擎
   - 选择模型
   - 在Shader Editor中添加一个新的Image Texture节点
   - 创建新图像：2048x2048，命名为 `OrangeTree_Baked`
   - 选中这个节点（重要！）

3. **烘焙**
   - Render Properties → Bake
   - Bake Type: Combined（或Diffuse）
   - 勾选 "Selected to Active"（如果需要）
   - 点击 Bake

4. **保存烘焙纹理**
   - Image → Save As
   - 保存为PNG格式

5. **导出模型**
   - 将烘焙的纹理应用到材质
   - 导出FBX

#### 在Unity中：

1. 导入烘焙的纹理
2. 创建材质并应用纹理
3. 应用到模型

---

## 快速修复步骤（针对你的情况）

### 第1步：检查纹理文件

在你的Blender文件所在文件夹中，找到这些文件：
```
texture_pbr_20250901.png
texture_pbr_20250901_roughness.png
texture_pbr_20250901_metallic.png
texture_pbr_20250901_normal.png
```

### 第2步：复制到Unity

将这些文件复制到：
```
TreePlanQAQ/Assets/Textures/OrangeTree/
```

### 第3步：设置法线贴图

在Unity中：
1. 选中 `texture_pbr_20250901_normal.png`
2. Inspector → Texture Type: Normal map
3. 点击 Apply

### 第4步：创建材质

1. Project窗口右键 → Create → Material
2. 命名：`OrangeTreeMaterial`
3. 设置：
   - Albedo: 拖入 `texture_pbr_20250901.png`
   - Normal Map: 拖入 `texture_pbr_20250901_normal.png`
   - Metallic: 拖入 `texture_pbr_20250901_metallic.png`（如果有）
   - Smoothness: 使用 `texture_pbr_20250901_roughness.png`

### 第5步：应用材质

1. 在Hierarchy中找到你的橘子树模型
2. 展开模型，找到子对象（Mesh）
3. 将 `OrangeTreeMaterial` 拖到模型上

---

## 常见问题

### Q1: 纹理文件在哪里？

**查找方法：**

在Blender中：
1. 选择材质节点中的纹理节点
2. 查看文件路径
3. 或者：File → External Data → Find Missing Files

### Q2: 导出FBX时纹理没有一起导出？

**解决：**

导出FBX时：
- Path Mode: Copy
- ✅ Embed Textures

或者手动复制纹理文件到Unity

### Q3: Unity中模型是粉红色的？

**原因：** 材质丢失或着色器不支持

**解决：**
1. 选中模型
2. Inspector → Materials
3. 手动指定材质

### Q4: 颜色看起来不对？

**检查：**
- 纹理的Color Space设置（sRGB 或 Linear）
- 材质的Rendering Mode
- 光照设置

### Q5: 法线贴图不起作用？

**确保：**
- Texture Type 设置为 Normal map
- 法线贴图格式正确（OpenGL 或 DirectX）
- Normal Map 强度不为0

---

## 推荐工作流程

### 最佳实践：

1. **在Blender中：**
   - 使用简单的材质
   - 烘焙复杂的材质到纹理
   - 确保UV展开正确
   - 导出时嵌入纹理

2. **在Unity中：**
   - 创建专门的材质
   - 手动设置所有纹理
   - 调整材质参数以匹配Blender效果

3. **文件组织：**
   ```
   Assets/
   ├── Models/
   │   └── OrangeTree.fbx
   ├── Textures/
   │   └── OrangeTree/
   │       ├── BaseColor.png
   │       ├── Normal.png
   │       ├── Metallic.png
   │       └── Roughness.png
   └── Materials/
       └── OrangeTreeMaterial.mat
   ```

---

## 自动化脚本（可选）

如果你有很多模型需要处理，可以使用Unity编辑器脚本自动设置材质。

需要的话我可以为你创建一个自动化脚本。

---

## 总结

**核心问题：** Blender和Unity使用不同的材质系统

**解决方案：**
1. ✅ 导出纹理文件
2. ✅ 在Unity中重新创建材质
3. ✅ 手动设置纹理贴图
4. ✅ 应用材质到模型

**不要依赖：**
- ❌ FBX自动转换材质
- ❌ Blender材质直接导入
- ❌ 自动纹理映射

---

需要更详细的步骤或遇到其他问题，随时告诉我！

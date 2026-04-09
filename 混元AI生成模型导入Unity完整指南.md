# 混元 AI 生成模型导入 Unity 完整指南

## 问题说明

混元（Hunyuan）生成的 3D 模型特点：
- ✅ 纹理已嵌入在 .blend 文件中
- ✅ 在 Blender 中可以看到完整的颜色和材质
- ❌ 直接导出 FBX 到 Unity 会丢失颜色
- ❌ 纹理不是独立的文件，而是打包在 .blend 中

## 解决方案

### 方法一：烘焙纹理（推荐，最简单）

#### 步骤1：在 Blender 中准备烘焙

1. **打开你的混元模型**
   - 在 Blender 中打开 .blend 文件

2. **切换到 Cycles 渲染引擎**
   - 右侧属性面板
   - Render Properties（相机图标）
   - Render Engine: **Cycles**

3. **选择模型**
   - 在 3D 视图中点击橘子树
   - 确保整个模型被选中

#### 步骤2：创建烘焙目标纹理

1. **切换到 Shading 工作区**
   - 顶部标签：Shading

2. **在 Shader Editor 中添加新纹理节点**
   - 确保模型被选中
   - 在 Shader Editor（下方）中
   - 按 Shift+A
   - 选择：Texture → Image Texture

3. **创建新图像**
   - 在新的 Image Texture 节点中
   - 点击 "New" 按钮
   - 设置：
     ```
     Name: OrangeTree_Baked
     Width: 2048
     Height: 2048
     Color: RGB
     Alpha: ✅ (勾选)
     ```
   - 点击 OK

4. **选中这个新节点**
   - **非常重要！** 点击这个新的 Image Texture 节点
   - 确保它被选中（边框高亮）
   - **不要连接这个节点到任何地方**

#### 步骤3：烘焙纹理

1. **打开烘焙设置**
   - 右侧属性面板
   - Render Properties（相机图标）
   - 滚动到底部找到 "Bake"

2. **设置烘焙选项**
   ```
   Bake Type: Diffuse
   
   Influence:
   ❌ Direct
   ❌ Indirect
   ✅ Color (只勾选这个)
   
   Selected to Active: ❌ (不勾选)
   Margin: 16 px
   ```

3. **开始烘焙**
   - 点击 **Bake** 按钮
   - 等待烘焙完成（可能需要几秒到几分钟）
   - 完成后会在 Image Texture 节点中看到烘焙的纹理

#### 步骤4：保存烘焙的纹理

1. **在 Shader Editor 中**
   - 选中包含烘焙纹理的 Image Texture 节点

2. **保存图像**
   - Image → Save As...
   - 或者在节点上：Image → Save As...

3. **保存位置和格式**
   ```
   位置: TreePlanQAQ/Assets/Textures/
   文件名: OrangeTree_Baked.png
   格式: PNG
   Color Depth: 8
   Compression: 15
   ```

4. **点击 Save As Image**

#### 步骤5：应用烘焙纹理到材质

1. **在 Shader Editor 中**
   - 删除原来复杂的材质节点
   - 只保留 Principled BSDF 和 Material Output

2. **连接烘焙纹理**
   - 将刚才烘焙的 Image Texture 节点
   - 连接到 Principled BSDF 的 Base Color

3. **简化材质**
   - 现在材质应该只有：
     ```
     Image Texture → Principled BSDF → Material Output
     ```

#### 步骤6：导出 FBX

1. **选择模型**
   - 在 3D 视图中选择橘子树

2. **导出 FBX**
   - File → Export → FBX (.fbx)

3. **导出设置**
   ```
   Include:
   ✅ Selected Objects
   
   Object Types:
   ✅ Mesh
   
   Path Mode: Copy
   ```

4. **保存**
   - 保存到：`TreePlanQAQ/Assets/Models/OrangeTree.fbx`

#### 步骤7：在 Unity 中设置

1. **导入检查**
   - Unity 会自动导入 FBX
   - 纹理文件应该已经在 `Assets/Textures/` 中

2. **创建材质**
   - Project 窗口右键 → Create → Material
   - 命名：`OrangeTreeMaterial`

3. **设置材质**
   ```
   Shader: Standard (或 URP/Lit)
   Albedo: 拖入 OrangeTree_Baked.png
   ```

4. **应用到模型**
   - 将材质拖到橘子树模型上

完成！🎉

---

### 方法二：提取嵌入的纹理

#### 步骤1：在 Blender 中提取纹理

1. **打开混元模型**

2. **打包资源**（如果还没打包）
   - File → External Data → Pack Resources
   - 确保所有纹理都在 .blend 文件中

3. **解包资源**
   - File → External Data → Unpack Resources
   - 选择：**Write files to current directory**
   - 纹理会被提取到 .blend 文件所在的文件夹

4. **查找提取的纹理**
   - 在 .blend 文件所在文件夹
   - 应该会有一个 `textures` 文件夹
   - 或者纹理文件直接在同一文件夹

#### 步骤2：复制到 Unity

1. **找到提取的纹理文件**
   - 通常是 PNG 或 JPG 格式

2. **复制到 Unity**
   - 复制到：`TreePlanQAQ/Assets/Textures/`

3. **在 Unity 中创建材质**
   - 按照方法一的步骤7设置

---

### 方法三：使用 GLB/GLTF 格式（最兼容）

#### 步骤1：安装 glTF 导出插件

1. **在 Blender 中**
   - Edit → Preferences
   - Add-ons
   - 搜索：glTF
   - 勾选：**Import-Export: glTF 2.0 format**

#### 步骤2：导出 GLB

1. **选择模型**

2. **导出 GLB**
   - File → Export → glTF 2.0 (.glb/.gltf)

3. **导出设置**
   ```
   Format: glTF Binary (.glb)
   
   Include:
   ✅ Selected Objects
   
   Data:
   ✅ UVs
   ✅ Normals
   ✅ Materials: Export
   ✅ Images: Automatic
   ```

4. **保存**
   - 保存到：`TreePlanQAQ/Assets/Models/OrangeTree.glb`

#### 步骤3：在 Unity 中导入

1. **安装 glTF 导入插件**（如果需要）
   - Window → Package Manager
   - 搜索：glTF
   - 安装 glTFast 或 UnityGLTF

2. **导入 GLB**
   - Unity 会自动导入
   - 材质和纹理应该会自动设置

---

## 快速对比

| 方法 | 难度 | 效果 | 推荐度 |
|------|------|------|--------|
| 方法一：烘焙纹理 | ⭐⭐ | ⭐⭐⭐⭐⭐ | ✅ 最推荐 |
| 方法二：提取纹理 | ⭐ | ⭐⭐⭐ | ⚠️ 可能找不到文件 |
| 方法三：GLB格式 | ⭐⭐⭐ | ⭐⭐⭐⭐ | ✅ 最兼容 |

---

## 推荐工作流程（针对混元模型）

### 最佳实践：

1. **在 Blender 中烘焙纹理**
   - 将所有材质信息烘焙到一张纹理
   - 简单、可靠、效果好

2. **保存烘焙纹理**
   - 直接保存到 Unity 项目的 Textures 文件夹

3. **简化材质**
   - 只使用烘焙的纹理
   - 删除复杂的节点网络

4. **导出 FBX**
   - 使用简化后的材质导出

5. **在 Unity 中设置**
   - 创建简单的材质
   - 应用烘焙的纹理

---

## 常见问题

### Q1: 烘焙后颜色不对？

**检查：**
- Bake Type 是否选择了 Diffuse
- Influence 是否只勾选了 Color
- 是否选中了正确的 Image Texture 节点

### Q2: 烘焙很慢？

**解决：**
- 降低纹理分辨率（1024x1024）
- 使用 Eevee 渲染引擎（虽然效果可能差一点）

### Q3: 烘焙的纹理有黑边？

**解决：**
- 增加 Margin 值（16 px 或更高）
- 确保 UV 展开正确

### Q4: Unity 中还是灰色？

**检查：**
- 材质是否正确应用
- 纹理是否正确导入
- Shader 是否支持纹理

---

## 混元模型特殊说明

### 混元生成的模型特点：

1. **纹理嵌入**
   - 纹理数据存储在 .blend 文件内部
   - 不是独立的文件

2. **材质复杂**
   - 使用程序化纹理
   - 节点网络复杂

3. **需要烘焙**
   - 必须将程序化材质烘焙成纹理
   - 才能导出到其他软件

### 为什么直接导出不行：

- FBX 格式不支持 Blender 的程序化材质
- Unity 无法理解 Blender 的节点系统
- 纹理数据需要转换成标准图片格式

---

## 总结

**对于混元生成的模型，最佳方案是：**

1. ✅ **烘焙纹理**（方法一）
   - 最可靠
   - 效果最好
   - 适合所有情况

2. ✅ **使用 GLB 格式**（方法三）
   - 最兼容
   - 自动处理材质
   - 适合快速测试

3. ❌ **不要直接导出 FBX**
   - 会丢失颜色
   - 材质无法转换

---

需要我帮你一步步操作烘焙纹理吗？这是最简单可靠的方法！

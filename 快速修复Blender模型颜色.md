# 🎨 快速修复 Blender 模型颜色

## 问题
Blender中有颜色 → Unity中变成灰色/白色

## 原因
Blender的材质和纹理不会自动转换到Unity

---

## ⚡ 5分钟快速修复

### 方法1：重新导出（最简单）

#### 在 Blender 中：

1. **选择模型**
   - 点击你的橘子树

2. **导出FBX**
   - File → Export → FBX (.fbx)
   
3. **重要设置：**
   ```
   ✅ Selected Objects
   ✅ Apply Modifiers
   ✅ Mesh
   Path Mode: Copy
   ✅ Embed Textures  ← 关键！
   ```

4. **保存位置**
   - 保存到：`TreePlanQAQ/Assets/Models/`

#### 在 Unity 中：

1. **找到纹理文件**
   - 在Project窗口中，找到导入的FBX
   - 展开FBX，看看有没有纹理文件
   - 如果有，跳到步骤3

2. **手动复制纹理**（如果FBX没有纹理）
   - 找到Blender项目文件夹
   - 找到这些文件：
     ```
     texture_pbr_20250901.png
     texture_pbr_20250901_normal.png
     texture_pbr_20250901_roughness.png
     texture_pbr_20250901_metallic.png
     ```
   - 复制到：`TreePlanQAQ/Assets/Textures/`

3. **创建材质**
   - Project窗口右键 → Create → Material
   - 命名：`OrangeTreeMaterial`

4. **设置材质**
   - 选中材质
   - Inspector中：
     - Albedo: 拖入颜色纹理（`texture_pbr_20250901.png`）
     - Normal Map: 拖入法线纹理（`texture_pbr_20250901_normal.png`）

5. **应用材质**
   - 在Hierarchy中选择橘子树
   - 将材质拖到模型上

完成！🎉

---

### 方法2：烘焙纹理（最佳效果）

#### 在 Blender 中：

1. **选择模型**

2. **切换到 Cycles 渲染**
   - 顶部：Render Properties
   - Render Engine: Cycles

3. **准备烘焙**
   - 切换到 Shading 工作区
   - 在Shader Editor中，添加新节点：
     - Shift+A → Texture → Image Texture
   - 点击 "New"
   - 名称：`OrangeTree_Baked`
   - 大小：2048 x 2048
   - **选中这个新节点**（很重要！）

4. **烘焙**
   - Render Properties → Bake
   - Bake Type: Diffuse
   - 取消勾选 "Direct" 和 "Indirect"（只保留Color）
   - 点击 **Bake** 按钮
   - 等待烘焙完成

5. **保存纹理**
   - Image → Save As
   - 保存为：`OrangeTree_Baked.png`
   - 保存到：`TreePlanQAQ/Assets/Textures/`

6. **应用烘焙纹理**
   - 在Shader Editor中
   - 删除原来的复杂节点
   - 只保留：Image Texture → Principled BSDF
   - Image Texture 选择刚烘焙的图

7. **导出FBX**
   - File → Export → FBX
   - 保存到：`TreePlanQAQ/Assets/Models/`

#### 在 Unity 中：

1. **创建材质**
   - Create → Material
   - 命名：`OrangeTreeMaterial`

2. **设置材质**
   - Albedo: 拖入 `OrangeTree_Baked.png`

3. **应用到模型**
   - 拖到橘子树上

完成！🎉

---

### 方法3：使用简单颜色（临时方案）

如果只是想快速看到颜色：

#### 在 Unity 中：

1. **创建材质**
   - Create → Material
   - 命名：`OrangeTreeSimple`

2. **设置颜色**
   - Albedo: 点击颜色框
   - 选择绿色（叶子）或橙色（果实）

3. **应用到模型**
   - 拖到模型上

---

## 🔍 检查清单

导入前检查：
- [ ] Blender中模型有UV展开
- [ ] 纹理文件存在且可访问
- [ ] 导出FBX时勾选了 "Embed Textures"

导入后检查：
- [ ] Unity中能看到纹理文件
- [ ] 法线贴图设置为 Normal map 类型
- [ ] 材质已创建并设置
- [ ] 材质已应用到模型

---

## ❓ 常见问题快速解答

### Q: 找不到纹理文件？

**A:** 在Blender中：
1. 选择材质节点中的纹理
2. 查看文件路径
3. 或者：File → External Data → Pack Resources（打包资源）

### Q: Unity中模型是粉红色？

**A:** 材质丢失了
1. 选中模型
2. Inspector → Materials
3. 手动拖入材质

### Q: 颜色很暗？

**A:** 检查光照
1. Window → Rendering → Lighting
2. 添加光源：GameObject → Light → Directional Light

### Q: 纹理模糊？

**A:** 提高纹理分辨率
1. 选中纹理
2. Inspector → Max Size: 2048 或 4096
3. 点击 Apply

---

## 💡 专业建议

### 推荐工作流程：

1. **在Blender中保持简单**
   - 使用基础材质
   - 避免复杂的节点网络
   - 烘焙复杂效果到纹理

2. **在Unity中精细调整**
   - 创建专门的材质
   - 调整参数以匹配效果
   - 使用Unity的光照系统

3. **文件命名规范**
   ```
   OrangeTree_BaseColor.png
   OrangeTree_Normal.png
   OrangeTree_Metallic.png
   OrangeTree_Roughness.png
   ```

---

## 🎯 推荐方案

根据你的情况，我推荐：

**如果追求效果：** 使用方法2（烘焙纹理）
**如果追求速度：** 使用方法1（重新导出）
**如果只是测试：** 使用方法3（简单颜色）

---

需要更详细的步骤截图或视频教程吗？告诉我！

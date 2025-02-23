# WinHelper

## 一、Windows 可视化命令行管理工具

在日常工作中，经常需要执行大量的命令行脚本，重复输入操作非常繁琐。为了解决这个问题，开发了一个多任务可视化管理工具，可以异步或按顺序执行命令行任务。

![readme_1.png](https://raw.githubusercontent.com/laijingming/WinHelper/refs/heads/main/ScriptManagement/Resources/readme_1.png)
### 下载地址：[https://raw.githubusercontent.com/laijingming/WinHelper/refs/heads/main/ScriptManagement/file/Release.rar](https://raw.githubusercontent.com/laijingming/WinHelper/refs/heads/main/ScriptManagement/file/Release.rar)
### 命令配置
配置文件位置：[file\command.json](file\command.json)

| 属性       | 说明                                       |
|------------|--------------------------------------------|
| name       | 命令名称                                   |
| command    | 实际执行的命令（前缀命令 + 命令）          |
| type       | 命令类型：<br/>0 异步；<br/>1 异步参数；<br/>2 阻塞；<br/>3 阻塞参数 |
| children   | 子命令                                     |

### 命令类型说明

#### 1. 异步（type = 0）

多个命令会并发执行，不会相互等待。

```json
[
  {
    "name": "类别1",
    "command": "前缀命令",
    "children": [
      { "name": "命令名1", "command": "命令1", "type": 0 },
      { "name": "命令名2", "command": "命令1,命令2", "type": 0 }
    ]
  }
]
```

**例子：**
- 执行 `命令名2`：  
  `前缀命令 + 命令1 && 前缀命令 + 命令2`
- 执行所有 `children`，会并发执行两个命令：  
  `命令名1 = 前缀命令 + 命令1`  
  `命令名2 = 前缀命令 + 命令1 && 前缀命令 + 命令2`

#### 2. 异步参数（type = 1）

所有子命令的参数会拼接起来作为前缀命令的参数，并异步执行。

```json
[
  {
    "name": "类别2",
    "command": "前缀命令",
    "children": [
      { "name": "命令名", "command": "参数1", "type": 1 },
      { "name": "命令名", "command": "参数2", "type": 1 }
    ]
  }
]
```

**例子：**
- 执行所有 `children`，拼接参数：  
  `前缀命令 + 参数1, 参数2`

#### 3. 阻塞（type = 2）

阻塞命令按顺序执行，必须等待前面命令完成后才能继续。

```json
[
  {
    "name": "类别3",
    "command": "前缀命令",
    "children": [
      { "name": "命令名", "command": "阻塞命令1", "type": 2 },
      { "name": "命令名", "command": "阻塞命令2", "type": 2 }
    ]
  }
]
```
**例子：**  
当执行顺序为 `类别1-命令名1`、`类别1-命令名2`、`类别3-阻塞命令1`、`类别2-参数1`，执行逻辑如下：
- `类别3-阻塞命令1` 会等待 `类别1` 的命令完成后再执行；
- `类别2-参数1` 会在 `阻塞命令1` 完成后执行。

#### 4. 阻塞参数（type = 3）

与异步参数类似，但按顺序阻塞执行，详细说明参考前面异步与阻塞的介绍。

### 使用方法

#### 1. 命令列表
- 点击父节点执行按钮，执行该节点下所有子命令。
- 点击子节点执行按钮，单独执行该子命令。
- 勾选多个节点，右键点击“执行选择”，一次性执行所有选中的命令。

#### 2. 历史记录
- 每次执行命令都会自动记录，包含执行的任务、时间和次数，便于下次一键重复执行。
- 历史记录会按置顶、执行次数和时间进行排序。
- 历史记录缓存文件地址：[file\log.json](file\log.json)
---

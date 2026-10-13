# 一、分层

大标题

=========

小标题

---------
# 一级标题

## 二级标题

### 三级标题

#### 四级标题

# FirstTitle
## <font face = "仿宋" color = orange>二级标题结合HTML</font>
<font face = "楷体" color = blue size=128><center>居中</center></font>
#Label

***
# 二、换行
段内换行：[space] [space] [enter]  
本段
段外换行：[enter] [enter]

次段
分割线
***
---
# 三、列表
## 程序
- Github与repo
    - README.md 介绍、功能、用例、下载、文档等
    - doc 文档
    - scripts 脚本
    - config 配置
    - src 源码
    - test 测试与数据
    - PSP 预估耗时
- 需求
    - 基本
    - 扩展
    - 高级
- 功能
## 无序列表
- 列表1
    - 列表1.1
        - 列表1.1.1
- 列表2
* 列表3
+ 列表4
## 有序列表
1. **第一点**
2. 第二点
    1. 次一点
    2. 次二点
## TodoList
- [ ] 待办
- [x] 已办

---
# 四、字体格式
- **加粗**
- *斜体*
- ***加粗斜体***
- ==高亮==
- ~~划去~~
- <u>下划线</u>
- 脚注^[1]

---
# 五、可视化、
## 表格
|左对齐|居中对齐|右对齐|
|:---|:---:|---:|
|a|b|c|
## 引用
>你说的对
--鲁迅
## 代码
1. single
`print('Python')`
2. segment
```cpp
#include<iostream>
int main(){
    std::cout<<"Hello, world!"<<endl;
    return 0;
}
```
```c
#include<stdio.h>
int main(){
    printf("Hello,world!");
    return 0;
}
```
## 超链接
[Github 地址]https://github.com/
[Click Here](https://github.com/)
## 图片
[![pmVvvM6.png](https://s41.ax1x.com/2026/06/04/pmVvvM6.png)](https://imgchr.com/i/pmVvvM6)
- 结合HTML调整
<a href="https://imgchr.com/i/pmVvvM6"><div align = center><img src="https://s41.ax1x.com/2026/06/04/pmVvvM6.md.png" alt="pmVvvM6.png" border="0" width = 100% height = 100%/></div></a>

## 流程图
```mermaid
sequenceDiagram
participant A
participant B
participant C

A->B:
B->>A:desc
C->>C:
alt xxx
    A->>B: 1
else
    A->B: 2
end
```
```mermaid
graph TD
    A{A} -->|if 1| B[B0]
    B[B1] --> C[C]
    A -->|if 2| D[B2]

```

***
# 六、数学公式
# KaTex
## Formulas

$$
x^\frac{1}{y} = \sqrt[y]{x}     \\
\sin 90^{\circ} = 1     \\
\pi \approx 3.14 \leq 4     \\
e^{ix} = i\sin{x} + \cos{x}     \\
x \times y = x \div \frac{1}{y}\ (y \not= 0)    \\
(x \pm y)^2 = x^2 \pm 2xy + y^2     \\
\int _{-\infty} ^{+\infty} \frac{1}{\sqrt{2\pi}\sigma}e^{-\frac{(x-\mu)^2}{2\sigma^2}}dx = 1    \\
\iint _{D} dxdy = \pi \ (D=\{(x,y)|x^2+y^2\leq 1\}),\ (0,0) \in D,(1,2) \notin D    \\
(\ln{x})^\prime = \frac{1}{x}   \\
\lim_{x \to a}f(x) = A  \\
x! = \prod _{i=1} ^{x} i    \\
e^x = \sum _{k=0} ^\infty \frac{x^k}{k!}    \\
\overline x = \frac{\sum _{i=0} ^{n} x_i}{n}    \\
f(x) = 1 + \cdots + n
$$

$$
\begin{Bmatrix}
a & b\\
c & d\\
\end{Bmatrix}
$$

## Symbols
 $\alpha,\beta,\gamma,\theta,\eta,\omega,\epsilon,\delta,\mu,\pi$ 
 $\emptyset,\ni,\notni,\supset,\supseteq,\bigcup,\bigcap$ 

 注意：在github中正常渲染，等号不放在行首且 `$` `$$` 相关代码段前后需要添加空白符
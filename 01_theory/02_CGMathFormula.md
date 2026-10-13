### Computer Graphic Math Principle

---

#### Vector multiplication in Matrix form

* Dot product

$$
\begin{pmatrix} x_1 & y_1 & z_1 \end{pmatrix}
\cdot
\begin{pmatrix} x_2 & y_2 & z_2 \end{pmatrix}^T= x_1 x_2 + y_1 y_2 + z_1 z_2
$$

* Cross product

$$
\begin{pmatrix} x_1 & y_1 & z_1 \end{pmatrix}
\times
\begin{pmatrix} x_2 & y_2 & z_2 \end{pmatrix}^T=
\begin{pmatrix}
0 & -z_1 & y_1 \\
z_1 & 0 & -x_1 \\
-y_1 & x_1 & 0
\end{pmatrix}
\begin{pmatrix} x_2 \\ y_2 \\ z_2 \end{pmatrix}
$$

---

* Rotation Matrix around an axis

$$
R_x(\alpha)=
\begin{bmatrix}
1 & 0 & 0 & 0 \\
0 & \cos \alpha & -\sin \alpha & 0 \\
0 & \sin \alpha & \cos \alpha & 0 \\
0 & 0 & 0 & 1
\end{bmatrix}
$$

$$
R_y(\alpha)=
\begin{bmatrix}
\cos \alpha & 0 & \sin \alpha & 0 \\
0 & 1 & 0 & 0 \\
-\sin \alpha & 0 & \cos \alpha & 0 \\
0 & 0 & 0 & 1
\end{bmatrix}
$$

$$
R_z(\alpha)=
\begin{bmatrix}
\cos \alpha & -\sin \alpha & 0 & 0 \\
\sin \alpha & \cos \alpha & 0 & 0 \\
0 & 0 & 1 & 0 \\
0 & 0 & 0 & 1
\end{bmatrix}
$$

* Sequence of Vertex Transformation (S - R(Y - X - Z) - T)

$$
P_t=T \cdot R \cdot S \cdot P_0
$$

---

* View/Camera Transformation (Position to Zero, up to y, forward to -z)

$$
M_{view}=RT=
\begin{pmatrix}
x_{right} & y_{right} & z_{right} & 0 \\
x_{up} & y_{up} & z_{up} & 0 \\
x_{back} & y_{back} & z_{back} & 0 \\
0 & 0 & 0 & 1
\end{pmatrix}
\begin{pmatrix}
1 & 0 & 0 & -x_c \\
0 & 1 & 0 & -y_c \\
0 & 0 & 1 & -z_c \\
0 & 0 & 0 & 1
\end{pmatrix}
$$

注意， $R$ 由正交性，逆向转置求解

---

* Orthographic/Perspective Projection 略

---

* **TangentToWorldSpaceNormal**

$$
M_{tbn}=\begin{pmatrix} T_{w}, B_{w}, N_{w}\end{pmatrix}
$$

$$
N=M_{tbn}N_{t}=xT+yB+zN
$$

其中， $B_{w}=N_{w} \times T_{w}$ ，三者构成坐标转换基底

* ###### 注：单位正交矩阵几何意义：旋转与基变换

---

* **Phong**

$$I_p = k_a I_a + \sum_{m \in lights} \left( k_d I_{m,d} (n_m \cdot l) + k_s I_{m,s} (r_m \cdot v) \right)$$

> $$L_{specular}=L_{light} \cdot max(v \cdot r,0)^{gloss}$$
> $$r=-l-2(n \cdot (-l))n$$

>其中，世界空间单位法向 $n$ ，入射光方向 $l$ （表面指向光源），反射光方向 $r$ 

---

* **Diffuse (Lambertian)**

$$L_d = k_d \frac{I}{r^2} \max(n \cdot l, 0)$$

> $$L_{diffuse}=albedo \cdot L_{light} \cdot max(n \cdot l,0)$$
> $$l=pos_{light}-pos_{world}$$
> $$Factor=max(n\cdot l,0) \in [-1,1]$$

> 半兰伯特优化（区间重映射）
> $$(Factor+1)/2 \in [0,1]$$

---

* **Specular (Blinn-Phong)**

$$L_s = k_s \frac{I}{r^2} \max(n \cdot h, 0)^p$$

> $$L_{specular}=L_{light} \cdot max(n \cdot h,0)^{gloss}$$
> $$h=normalize(l+v)$$
> $$v=pos_{camera}-pos_{world}$$

>其中，高光反射颜色 $k_s$ ，观察方向向量 $v$ （表面指向相机），半角向量 $h$ ，微表面理论经验近似，优化计算

---

* **Ambient**

$$L_a = k_a I_a$$

其中， $k_a$ 为环境光， $I_a$ 为表面固有色

> $$L_{ambient}=L_{ambientLight} \cdot albedo$$

---

* **Lighting Reflection Model**

> $$L = L_{ambient} + L_{diffuse} + L_{specular}$$

* Barycentric coordinates（重心坐标）

$$
(x,y) = \alpha A + \beta B + \gamma C
$$

$$
\alpha + \beta + \gamma = 1
$$

其中， $A$ ， $B$ ， $C$ 为三角形的顶点坐标，若系数皆非负，则坐标在三角形内

推广： $V = V_A + V_B + V_C$ ，对内部插值参数颜色、坐标、法线、深度、材质

---

* Bilinear interpolation 略
* Mip Map Level Computing
* Displacement mapping
* Constructive Solid Geometry
* Catmull-Clark Subdivision
* Spatial Partitioning 
    * KD-Tree
    * Oct-Tree
    * BSP-Tree

---

* The Plenoptic Function

$$
P(\theta, \phi, \lambda, t, V_x, V_y, V_z)
$$

---

* RayStep

* RayTracing
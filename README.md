# MeshCut

Given a mesh object and a plane parallel to one of the mesh's faces (e.g., the YZ plane), the goal is to move the plane so that it intersects with the mesh. The objective is to obtain a new display that shows only the parts of the mesh that are outside the plane (or in the direction of plane movement), while ensuring that the intersecting parts are not empty and are displayed as well.


## Dissection Method
剖分法

将三维网格对象和平面都进行剖分，将其表示为离散的小元素（如三角形）。然后，比较每个小元素与平面的相交情况，将相交的部分保留下来并进行显示。这种方法可以使用诸如扫描线算法、空间分割树（如包围盒层次结构）等技术来实现。

## Boundary Representation Method

## (Implicit Geometry Representation Method

~~~~
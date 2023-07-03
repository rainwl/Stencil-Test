# MeshCut
*date:2023-7-2*

*author:wanglin*

https://github.com/rainwl/MeshCut.git
## Overview
Given a mesh object and a plane parallel to one of the mesh's faces 
(e.g., the YZ plane), the goal is to move the plane so that it 
intersects with the mesh. The objective is to obtain a new display 
that shows only the parts of the mesh that are outside the plane 
(or in the direction of plane movement), while ensuring that the 
intersecting parts are not empty and are displayed as well.

I will approach this problem from two perspectives: geometry and shading.


And , I have tried several simple geometry algorithms,
but the results have not been satisfactory because these
algorithms are designed for convex polygons and do not adequately
consider holes and other complex situations.

I will prioritize the geometry approach and refine one of 
the algorithms to address this problem. However, 
if none of the geometry algorithms can effectively solve 
the problem, taking into account both accuracy and real-time 
performance (frame rate), I will then consider implementing
the solution from the shading perspective.

Starting directly from shading would actually be simpler, 
but I prefer to challenge myself by approaching it from 
the geometry perspective.



## Geometry algorithm
### I.Dissection Method
I will discretize both the three-dimensional mesh objects and
the plane by dividing them into smaller elements, such as triangles.
Then, I will compare each small element with the plane to determine
if they intersect. The intersecting parts will be retained and
displayed. This approach can be implemented using techniques like
scanline algorithms, spatial partitioning trees (such as bounding
volume hierarchies), and other related methods.

### II.Boundary Representation Method

### III.Implicit Geometry Representation Method

## Shading algorithm


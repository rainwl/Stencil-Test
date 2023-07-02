// using UnityEngine;
//
// public class MeshClippingImplicit : MonoBehaviour
// {
//     public Mesh mesh; // 网格对象
//     public Plane plane; // 平面对象
//
//     private Mesh slicedMesh; // 切割后的网格
//
//     private void Start()
//     {
//         SliceMesh();
//         DisplaySlicedMesh();
//     }
//
//     private void SliceMesh()
//     {
//         // 剖分网格和平面
//         Vector3[] vertices = mesh.vertices;
//         int[] triangles = mesh.triangles;
//
//         Vector3 planeNormal = plane.normal;
//         float planeDistance = plane.distance;
//
//         // 创建切割后的网格对象
//         slicedMesh = new Mesh();
//
//         // 遍历三角形，找到与平面相交的三角形
//         for (int i = 0; i < triangles.Length; i += 3)
//         {
//             Vector3 v1 = vertices[triangles[i]];
//             Vector3 v2 = vertices[triangles[i + 1]];
//             Vector3 v3 = vertices[triangles[i + 2]];
//
//             // 检查三角形和平面的相交关系
//             bool isIntersecting = IsTriangleIntersectingPlane(v1, v2, v3, planeNormal, planeDistance);
//
//             if (isIntersecting)
//             {
//                 // 如果与平面相交，将相交部分的顶点和三角形添加到切割后的网格中
//                 SliceTriangle(v1, v2, v3, planeNormal, planeDistance);
//             }
//         }
//
//         slicedMesh.RecalculateNormals();
//     }
//
//     private bool IsTriangleIntersectingPlane(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 planeNormal, float planeDistance)
//     {
//         // 判断三角形和平面的相交关系
//         // 使用点法式进行相交检测
//         float d1 = Vector3.Dot(planeNormal, v1) - planeDistance;
//         float d2 = Vector3.Dot(planeNormal, v2) - planeDistance;
//         float d3 = Vector3.Dot(planeNormal, v3) - planeDistance;
//
//         // 如果三个顶点在平面同一侧，则不相交；如果三个顶点在平面两侧，则相交
//         if ((d1 > 0 && d2 > 0 && d3 > 0) || (d1 < 0 && d2 < 0 && d3 < 0))
//         {
//             // 三角形完全在平面的一侧
//             return false;
//         }
//         else if ((d1 == 0 && d2 == 0 && d3 == 0))
//         {
//             // 三角形在平面上
//             return false;
//         }
//         else
//         {
//             // 三角形与平面相交
//             return true;
//         }
//     }
//
//     private void SliceTriangle(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 planeNormal, float planeDistance)
//     {
//         // 计算三角形的法线
//         Vector3 triangleNormal = Vector3.Cross(v2 - v1, v3 - v1).normalized;
//
//         // 计算三角形与平面的相交点
//         Vector3 intersection1, intersection2;
//         bool hasIntersection = IntersectionLineTriangle(v1, v2, v3, planeNormal, planeDistance, out intersection1, out intersection2);
//
//         if (hasIntersection)
//         {
//             // 计算相交点与原始三角形顶点的相对位置
//             float t1 = Vector3.Dot(triangleNormal, Vector3.Cross(v2 - intersection1, v3 - intersection1));
//             float t2 = Vector3.Dot(triangleNormal, Vector3.Cross(v3 - intersection2, v1 - intersection2));
//             float t3 = Vector3.Dot(triangleNormal, Vector3.Cross(v1 - intersection1, v2 - intersection1));
//
//             // 添加相交点和原始三角形顶点到切割后的网格中
//             slicedMesh.vertices = new Vector3[] { intersection1, intersection2, v1, v2, v3 };
//             slicedMesh.triangles = new int[] { 0, 1, 2, 0, 2, 3, 0, 3, 4 };
//         }
//     }
//
//     private bool IntersectionLineTriangle(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 planeNormal, float planeDistance, out Vector3 intersection1, out Vector3 intersection2)
//     {
//         intersection1 = Vector3.zero;
//         intersection2 = Vector3.zero;
//
//         // 计算三角形的法线
//         Vector3 triangleNormal = Vector3.Cross(v2 - v1, v3 - v1).normalized;
//
//         // 计算射线与平面的相交点
//         Vector3 rayDirection = triangleNormal;
//         Vector3 rayOrigin = v1 + rayDirection * 100f; // 将射线起点放在远离三角形的位置
//
//         float t;
//         if (plane.Raycast(new Ray(rayOrigin, -rayDirection), out t) && t > 0)
//         {
//             // 计算相交点
//             Vector3 intersectionPoint = rayOrigin - rayDirection * t;
//
//             // 检查相交点是否在三角形内部
//             if (IsPointInsideTriangle(intersectionPoint, v1, v2, v3))
//             {
//                 // 找到一个相交点
//                 intersection1 = intersectionPoint;
//                 return true;
//             }
//         }
//
//         return false;
//     }
//
//     // 其他辅助函数和显示切割后的网格的代码与前面的示例相同
//
//     private void DisplaySlicedMesh()
//     {
//         // 创建一个游戏对象来显示切割后的网格
//         GameObject slicedMeshObject = new GameObject("Sliced Mesh");
//         slicedMeshObject.AddComponent<MeshFilter>().sharedMesh = slicedMesh;
//         slicedMeshObject.AddComponent<MeshRenderer>().sharedMaterial = GetComponent<MeshRenderer>().sharedMaterial;
//     }
// }

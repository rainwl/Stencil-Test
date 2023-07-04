// using UnityEngine;
// using System.Collections.Generic;
//
// public class MeshClippingDissectionMethod : MonoBehaviour
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
//         List<Vector3> slicedVertices = new List<Vector3>();
//         List<int> slicedTriangles = new List<int>();
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
//                 // 如果与平面相交，将相交部分的顶点和三角形添加到剖分后的网格中
//                 List<Vector3> intersectingVertices;
//                 List<int> intersectingTriangles;
//
//                 // 获取相交部分的顶点和三角形
//                 SliceTriangle(v1, v2, v3, planeNormal, planeDistance, out intersectingVertices, out intersectingTriangles);
//
//                 // 添加相交部分的顶点和三角形到剖分后的网格中
//                 int vertexIndexOffset = slicedVertices.Count;
//                 slicedVertices.AddRange(intersectingVertices);
//                 for (int j = 0; j < intersectingTriangles.Count; j++)
//                 {
//                     slicedTriangles.Add(intersectingTriangles[j] + vertexIndexOffset);
//                 }
//             }
//             else
//             {
//                 // 如果不与平面相交，将整个三角形添加到剖分后的网格中
//                 slicedVertices.Add(v1);
//                 slicedVertices.Add(v2);
//                 slicedVertices.Add(v3);
//
//                 slicedTriangles.Add(i);
//                 slicedTriangles.Add(i + 1);
//                 slicedTriangles.Add(i + 2);
//             }
//         }
//
//         // 构建剖分后的网格对象
//         slicedMesh = new Mesh();
//         slicedMesh.SetVertices(slicedVertices);
//         slicedMesh.SetTriangles(slicedTriangles, 0);
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
//     private void SliceTriangle(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 planeNormal, float planeDistance, out List<Vector3> intersectingVertices, out List<int> intersectingTriangles)
//     {
//         intersectingVertices = new List<Vector3>();
//         intersectingTriangles = new List<int>();
//
//         // 计算三角形与平面的相交点
//         Vector3 intersection1, intersection2;
//         bool hasIntersection = IntersectionLineTriangle(v1, v2, v3, planeNormal, planeDistance, out intersection1, out intersection2);
//
//         if (hasIntersection)
//         {
//             // 将相交点添加到顶点列表
//             intersectingVertices.Add(intersection1);
//             intersectingVertices.Add(intersection2);
//
//             // 将相交点添加到相交三角形索引列表
//             intersectingTriangles.Add(0);
//             intersectingTriangles.Add(1);
//
//             // 根据相交点和原始三角形顶点的关系，计算相交三角形
//             if (Vector3.Dot(planeNormal, Vector3.Cross(v1 - intersection1, v2 - intersection1)) > 0)
//             {
//                 intersectingTriangles.Add(2);
//             }
//             else
//             {
//                 intersectingTriangles.Add(1);
//                 intersectingTriangles.Add(2);
//             }
//
//             if (Vector3.Dot(planeNormal, Vector3.Cross(v2 - intersection2, v3 - intersection2)) > 0)
//             {
//                 intersectingTriangles.Add(0);
//                 intersectingTriangles.Add(2);
//             }
//             else
//             {
//                 intersectingTriangles.Add(0);
//                 intersectingTriangles.Add(1);
//                 intersectingTriangles.Add(2);
//             }
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
//         if (Plane.Raycast(new Ray(rayOrigin, -rayDirection), out t) && t > 0)
//         {
//             Vector3 intersectionPoint = rayOrigin + (-rayDirection) * t;
//
//             // 检查相交点是否在三角形内部
//             if (IsPointInsideTriangle(intersectionPoint, v1, v2, v3))
//             {
//                 intersection1 = intersectionPoint;
//                 intersection2 = intersectionPoint;
//
//                 // 找到相交边上的另一个点
//                 Vector3 edgePoint1, edgePoint2;
//                 bool hasEdgePoint = GetEdgePointOnTriangle(intersectionPoint, v1, v2, v3, out edgePoint1, out edgePoint2);
//
//                 if (hasEdgePoint)
//                 {
//                     intersection2 = edgePoint1;
//                 }
//
//                 return true;
//             }
//         }
//
//         return false;
//     }
//
//     private bool IsPointInsideTriangle(Vector3 point, Vector3 v1, Vector3 v2, Vector3 v3)
//     {
//         // 使用叉积法判断点是否在三角形内部
//         Vector3 cross1 = Vector3.Cross(v2 - v1, point - v1);
//         Vector3 cross2 = Vector3.Cross(v3 - v2, point - v2);
//         Vector3 cross3 = Vector3.Cross(v1 - v3, point - v3);
//
//         return Vector3.Dot(cross1, cross2) >= 0 && Vector3.Dot(cross2, cross3) >= 0;
//     }
//
//     private bool GetEdgePointOnTriangle(Vector3 point, Vector3 v1, Vector3 v2, Vector3 v3, out Vector3 edgePoint1, out Vector3 edgePoint2)
//     {
//         edgePoint1 = Vector3.zero;
//         edgePoint2 = Vector3.zero;
//
//         // 检查边1
//         if (IsPointOnLine(point, v1, v2))
//         {
//             edgePoint1 = v1;
//             edgePoint2 = v2;
//             return true;
//         }
//
//         // 检查边2
//         if (IsPointOnLine(point, v2, v3))
//         {
//             edgePoint1 = v2;
//             edgePoint2 = v3;
//             return true;
//         }
//
//         // 检查边3
//         if (IsPointOnLine(point, v3, v1))
//         {
//             edgePoint1 = v3;
//             edgePoint2 = v1;
//             return true;
//         }
//
//         return false;
//     }
//
//     private bool IsPointOnLine(Vector3 point, Vector3 lineStart, Vector3 lineEnd)
//     {
//         // 检查点是否在直线上
//         float distance = Vector3.Distance(lineStart, lineEnd);
//         float distanceToStart = Vector3.Distance(point, lineStart);
//         float distanceToEnd = Vector3.Distance(point, lineEnd);
//
//         return Mathf.Approximately(distance, distanceToStart + distanceToEnd);
//     }
//
//     private void DisplaySlicedMesh()
//     {
//         // 创建一个游戏对象来显示剖分后的网格
//         GameObject slicedMeshObject = new GameObject("Sliced Mesh");
//         slicedMeshObject.AddComponent<MeshFilter>().sharedMesh = slicedMesh;
//         slicedMeshObject.AddComponent<MeshRenderer>().sharedMaterial = GetComponent<MeshRenderer>().sharedMaterial;
//     }
// }

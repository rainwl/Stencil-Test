// using UnityEngine;
//
// public class MeshClippingSplit : MonoBehaviour
// {
//     public Mesh mesh; // 网格对象
//     private Plane _plane; // 平面对象
//     public Vector3 planeNormal;
//     public Vector3 planePosition;
//     private Mesh slicedMesh; // 切割后的网格
//     public MeshFilter meshFilter;
//     private void Start()
//     {
//         mesh = meshFilter.mesh;
//         SliceMesh();
//         DisplaySlicedMesh();
//     }
//
//     private void SliceMesh()
//     {
//         // 剖分网格和平面
//         Vector3[] vertices = mesh.vertices;
//         int[] triangles = mesh.triangles;
//         Plane plane = new Plane(planeNormal, planePosition);
//         _plane = plane;
//         //Vector3 planeNormal = plane.normal;
//         float planeDistance = plane.distance;
//
//         // 创建切割后的网格对象
//         slicedMesh = new Mesh();
//
//         // 用于存储平面未扫过的部分的顶点和三角形
//         Vector3[] slicedVertices = new Vector3[vertices.Length];
//         int[] slicedTriangles = new int[triangles.Length];
//
//         int slicedVertexCount = 0;
//         int slicedTriangleCount = 0;
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
//                 SliceTriangle(v1, v2, v3, slicedVertices, slicedTriangles, ref slicedVertexCount, ref slicedTriangleCount);
//             }
//             else
//             {
//                 // 如果未与平面相交，将三角形顶点添加到平面未扫过的部分的顶点数组中
//                 slicedVertices[slicedVertexCount++] = v1;
//                 slicedVertices[slicedVertexCount++] = v2;
//                 slicedVertices[slicedVertexCount++] = v3;
//             }
//         }
//
//         // 设置切割后的网格的顶点和三角形数组
//         slicedMesh.vertices = slicedVertices;
//         slicedMesh.triangles = slicedTriangles;
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
//             return false;
//         }
//
//         return true;
//     }
//
//     private void SliceTriangle(Vector3 v1, Vector3 v2, Vector3 v3, Vector3[] slicedVertices, int[] slicedTriangles, ref int slicedVertexCount, ref int slicedTriangleCount)
//     {
//         // 计算三角形和平面的交点
//         Vector3 intersection1, intersection2, intersection3;
//         CalculateTrianglePlaneIntersections(v1, v2, v3, _plane, out intersection1, out intersection2, out intersection3);
//
//         // 将交点添加到平面未扫过的部分的顶点数组中
//         slicedVertices[slicedVertexCount++] = intersection1;
//         slicedVertices[slicedVertexCount++] = intersection2;
//         slicedVertices[slicedVertexCount++] = intersection3;
//
//         // 添加与平面相交的三角形的顶点索引到切割后的网格的三角形数组中
//         slicedTriangles[slicedTriangleCount++] = slicedVertexCount - 3;
//         slicedTriangles[slicedTriangleCount++] = slicedVertexCount - 2;
//         slicedTriangles[slicedTriangleCount++] = slicedVertexCount - 1;
//     }
//
//     private void CalculateTrianglePlaneIntersections(Vector3 v1, Vector3 v2, Vector3 v3, Plane plane, out Vector3 intersection1, out Vector3 intersection2, out Vector3 intersection3)
//     {
//         // 计算三角形和平面的交点
//         intersection1 = CalculateIntersectionPoint(v1, v2, plane);
//         intersection2 = CalculateIntersectionPoint(v2, v3, plane);
//         intersection3 = CalculateIntersectionPoint(v3, v1, plane);
//     }
//
//     private Vector3 CalculateIntersectionPoint(Vector3 v1, Vector3 v2, Plane plane)
//     {
//         // 计算直线和平面的交点
//         Vector3 intersectionPoint;
//         float t;
//         Ray ray = new Ray(v1, v2 - v1);
//         if (plane.Raycast(ray, out t))
//         {
//             intersectionPoint = ray.GetPoint(t);
//         }
//         else
//         {
//             // 如果直线和平面平行，则取其中一个顶点作为交点
//             intersectionPoint = v1;
//         }
//
//         return intersectionPoint;
//     }
//
//     private void DisplaySlicedMesh()
//     {
//         // 创建一个游戏对象来显示切割后的网格
//         GameObject slicedMeshObject = new GameObject("Sliced Mesh");
//         slicedMeshObject.AddComponent<MeshFilter>().sharedMesh = slicedMesh;
//         slicedMeshObject.AddComponent<MeshRenderer>().sharedMaterial = GetComponent<MeshRenderer>().sharedMaterial;
//     }
//     
//     
//     private void DrawPlane()
//     {
//         GameObject planeObject = new GameObject("Clipping Plane");
//         planeObject.transform.position = planePosition;
//         planeObject.transform.rotation = Quaternion.LookRotation(planeNormal);
//         planeObject.transform.localScale = new Vector3(10f, 0.01f, 10f); // 调整平面的大小
//         planeObject.AddComponent<MeshFilter>().sharedMesh = CreatePlaneMesh();
//         planeObject.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));
//     }
//
//     private Mesh CreatePlaneMesh()
//     {
//         Mesh planeMesh = new Mesh();
//
//         // 定义平面的顶点和三角形
//         Vector3[] vertices = new Vector3[4]
//         {
//             new Vector3(-0.5f, 0f, -0.5f),
//             new Vector3(-0.5f, 0f, 0.5f),
//             new Vector3(0.5f, 0f, -0.5f),
//             new Vector3(0.5f, 0f, 0.5f)
//         };
//
//         int[] triangles = new int[6] { 0, 1, 2, 2, 1, 3 };
//
//         // 设置平面的顶点和三角形数据
//         planeMesh.vertices = vertices;
//         planeMesh.triangles = triangles;
//
//         return planeMesh;
//     }
// }

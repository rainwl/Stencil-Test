using UnityEngine;
using UnityEngine.Serialization;

// 这个脚本有问题,不能用
public class MeshClippingShader : MonoBehaviour
{
    private Mesh mesh; // 网格对象

    public MeshFilter meshFilter;
    public Vector3 planeNormal;
    public Vector3 planePosition;
    private void Start()
    {
        ClipMesh();

        DrawPlane();
    }

    private void ClipMesh()
    {
        
        // 创建裁剪后的网格
        Mesh clippedMesh = new Mesh();

        mesh = meshFilter.mesh;
        Plane plane = new Plane(planeNormal, planePosition);
        // 获取网格的顶点和三角形数据
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;

        // 存储裁剪后的顶点和三角形数据
        Vector3[] clippedVertices = new Vector3[vertices.Length];
        int[] clippedTriangles = new int[triangles.Length];

        // 遍历网格的每个顶点
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 vertex = vertices[i];

            // 判断顶点是否在平面的同一侧
            bool isInside = IsPointInside(vertex, plane);

            // 根据顶点的位置设置颜色
            Color color = isInside ? Color.green : Color.red;

            // 存储裁剪后的顶点和三角形数据
            clippedVertices[i] = vertex;
            //clippedMesh.colors[i] = color;
        }

        // 设置裁剪后的网格的顶点和三角形数据
        clippedMesh.vertices = clippedVertices;
        clippedMesh.triangles = triangles;

        // 显示裁剪后的网格
        GameObject clippedMeshObject = new GameObject("Clipped Mesh");
        clippedMeshObject.AddComponent<MeshFilter>().sharedMesh = clippedMesh;
        //clippedMeshObject.AddComponent<MeshRenderer>().sharedMaterial = GetComponent<MeshRenderer>().sharedMaterial;
    }

    private bool IsPointInside(Vector3 point, Plane plane)
    {
        float distance = plane.GetDistanceToPoint(point);
        return distance >= 0f;
    }
    private void DrawPlane()
    {
        GameObject planeObject = new GameObject("Clipping Plane");
        planeObject.transform.position = planePosition;
        planeObject.transform.rotation = Quaternion.LookRotation(planeNormal);
        planeObject.transform.localScale = new Vector3(10f, 0.01f, 10f); // 调整平面的大小
        planeObject.AddComponent<MeshFilter>().sharedMesh = CreatePlaneMesh();
        planeObject.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));
    }

    private Mesh CreatePlaneMesh()
    {
        Mesh planeMesh = new Mesh();

        // 定义平面的顶点和三角形
        Vector3[] vertices = new Vector3[4]
        {
            new Vector3(-0.5f, 0f, -0.5f),
            new Vector3(-0.5f, 0f, 0.5f),
            new Vector3(0.5f, 0f, -0.5f),
            new Vector3(0.5f, 0f, 0.5f)
        };

        int[] triangles = new int[6] { 0, 1, 2, 2, 1, 3 };

        // 设置平面的顶点和三角形数据
        planeMesh.vertices = vertices;
        planeMesh.triangles = triangles;

        return planeMesh;
    }
}

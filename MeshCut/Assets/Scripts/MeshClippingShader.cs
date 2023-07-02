using UnityEngine;

public class MeshClippingShader : MonoBehaviour
{
    public Mesh mesh; // 网格对象
    public Plane plane; // 平面对象

    private void Start()
    {
        ClipMesh();
    }

    private void ClipMesh()
    {
        // 创建裁剪后的网格
        Mesh clippedMesh = new Mesh();

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
            clippedMesh.colors[i] = color;
        }

        // 设置裁剪后的网格的顶点和三角形数据
        clippedMesh.vertices = clippedVertices;
        clippedMesh.triangles = triangles;

        // 显示裁剪后的网格
        GameObject clippedMeshObject = new GameObject("Clipped Mesh");
        clippedMeshObject.AddComponent<MeshFilter>().sharedMesh = clippedMesh;
        clippedMeshObject.AddComponent<MeshRenderer>().sharedMaterial = GetComponent<MeshRenderer>().sharedMaterial;
    }

    private bool IsPointInside(Vector3 point, Plane plane)
    {
        float distance = plane.GetDistanceToPoint(point);
        return distance >= 0f;
    }
}
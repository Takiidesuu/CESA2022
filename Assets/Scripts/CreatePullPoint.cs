using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class CreatePullPoint : MonoBehaviour
{
    Mesh mesh;
    
    Vector3[] vertices;
    int[] triangles;
    
    string tagName;
    
    private void Awake() 
    {
        if (this.gameObject.GetComponent<MeshRenderer>() == null)
        {
            this.gameObject.AddComponent<MeshRenderer>();
        }
        
        mesh = GetComponent<MeshFilter>().mesh;
        
        tagName = this.transform.parent.gameObject.tag;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        MakeMeshData();
        CreateMesh();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void MakeMeshData()
    {
        switch (tagName)
        {
            case "UpPoint":
                vertices = new Vector3[]{new Vector3(0, 0, 0), new Vector3(1, 0, 0), new Vector3 (0, -2, 0)};
                break;
            case "DownPoint":
                vertices = new Vector3[]{new Vector3(0, 0, 0), new Vector3(1, -2, 0), new Vector3 (0, -2, 0)};
                break;
            case "LeftPoint":
                vertices = new Vector3[]{new Vector3(0, 0, 0), new Vector3(1, 0, 0), new Vector3 (1, -2, 0)};
                break;
            case "RightPoint":
                vertices = new Vector3[]{new Vector3(0, 0, 0), new Vector3(1, 2, 0), new Vector3 (1, 0, 0)};
                break;
        }
        
        triangles = new int[]{0, 1, 2};
    }
    
    void CreateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.uv = new Vector2[] {new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1)};
        mesh.triangles = triangles;
    }
}

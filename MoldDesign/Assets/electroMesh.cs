using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class electroMesh : MonoBehaviour {

    // Use this for initialization
    public float scale;
    public float vertexColumbs;
	void Start () {
        Vector3[] vertices = CreateMesh();
        ChargeVertices(vertices);
	}

    private Vector3[] CreateMesh()
    {
        Vector3 center = gameObject.transform.position;
        Vector3[] vertices =  {
            center - gameObject.transform.right * scale,
            center + gameObject.transform.forward * scale,
            center + gameObject.transform.right * scale,
            center - gameObject.transform.forward * scale,
            center - gameObject.transform.up * scale,
            center + gameObject.transform.up  * scale};


        int[] triangles = {
            0, 1, 5,
            1, 2, 5,
            2, 3, 5,
            0, 5, 3,
            1, 0, 4,
            2, 1, 4,
            3, 2, 4,
            0, 3, 4};

        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        
        return vertices;
    }
	
    private void ChargeVertices(Vector3[] vertices)
    {
        GameObject parent;
        GameObject child; //electric component needs to be the child of a parent game object
        Rigidbody rigid;
        SphereCollider col;

        StaticChargeScript scs;
        for( int i = 0; i < vertices.Length; i++)
        {
            parent = new GameObject("Vertex" + i);
            parent.transform.position = vertices[i];
            parent.transform.parent = gameObject.transform;
            rigid = parent.AddComponent<Rigidbody>();
            rigid.useGravity = false;
            col = parent.AddComponent<SphereCollider>();
            col.radius = 0;

            child = new GameObject("Charge" + i);
            child.transform.parent = parent.transform;
            scs = child.AddComponent<StaticChargeScript>();
            scs.strength = vertexColumbs;
        }
    }


	// Update is called once per frame
	void Update () {
		
	}
}

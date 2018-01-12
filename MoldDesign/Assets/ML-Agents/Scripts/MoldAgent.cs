using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoldAgent : Agent {

    public float scale;
    public Vector3 boundingSize;
    public GameObject floor;
    Mesh baseMesh;
    Vector3[] baseVertices;
    Vector3[] newVertices;
    Vector3 newVertex;
    int[] triangles;
    Bounds meshBounds;
    bool valid;
    bool hit = false;
    Rigidbody rigid;

    float total_wait;
    float initial_count;

    int a_idx;
    public override List<float> CollectState()
    {
        List<float> state = new List<float>();
        return state;
    }

    public override void AgentStep(float[] act)
    {
        total_wait = gameObject.GetComponent<drop>().wait_frames;
        initial_count = gameObject.GetComponent<drop>().counter;
        if(initial_count >= total_wait)
            rigid.constraints = RigidbodyConstraints.None;


        baseVertices = baseMesh.vertices;
        newVertices = baseVertices;
        for (int i = 0; i < baseVertices.Length; i++)
        {
            newVertex = baseVertices[i];

            a_idx = i * 3;
            newVertex.x += act[a_idx] * scale;
            newVertex.y += act[a_idx + 1] * scale;
            newVertex.z += act[a_idx + 2] * scale;

            if (Mathf.Abs(newVertex.x) > Mathf.Abs(meshBounds.extents.x) || Mathf.Abs(newVertex.y) > Mathf.Abs(meshBounds.extents.y) || Mathf.Abs(newVertex.z) > Mathf.Abs(meshBounds.extents.z)){
                valid = false;
            }
            else {
               valid = ValidateMove(baseMesh, newVertex, baseVertices[i]);
               if (valid)
                   newVertices[i] = newVertex;
            }
        }
        baseMesh.vertices = newVertices;
        baseMesh.RecalculateNormals();

        MeshCollider mc = gameObject.GetComponent<MeshCollider>();
        mc.sharedMesh = null;
        mc.sharedMesh = baseMesh;
        gameObject.GetComponent<ConcaveCollider>().ComputeHulls(new ConcaveCollider.LogDelegate(Log), new ConcaveCollider.ProgressDelegate(Progress));


        if (hit == true)
        {
            done = true;
        }
    }

    public bool ValidateMove(Mesh baseMesh, Vector3 newVertex, Vector3 baseVertex)
    {
        bool validMove = true;
        Ray shiftRay = new Ray(baseVertex, newVertex-baseVertex);
        triangles = baseMesh.triangles;
        Vector3 normal;
        Vector3 vec;
        float distanceToPlane;
        int j = 0;
        while(validMove == true && j < triangles.Length)
        {
            if (newVertices[triangles[j]] != baseVertex && newVertices[triangles[j + 1]] != baseVertex && newVertices[triangles[j + 2]] != baseVertex) {
                normal = Vector3.Cross(newVertices[triangles[j]] - newVertices[triangles[j + 1]], newVertices[triangles[j + 2]] - newVertices[triangles[j + 1]]);
                vec = baseVertex - newVertices[triangles[j]];
                distanceToPlane = Mathf.Abs(Vector3.Dot(normal, vec))/ Vector3.SqrMagnitude(normal);
                if (Vector3.Distance(newVertex, baseVertex) > distanceToPlane)
                {
                    validMove = !Intersect(newVertices[triangles[j]], newVertices[triangles[j + 1]], newVertices[triangles[j + 2]], shiftRay);
                }
            }
            j = j + 3;
        }
        return validMove;
    }

    public static bool Intersect(Vector3 p1, Vector3 p2, Vector3 p3, Ray ray)
    {
        float Epsilon = Mathf.Epsilon;
        // Vectors from p1 to p2/p3 (edges)
        Vector3 e1, e2;
        Vector3 p, q, t;
        float det, invDet, u, v;


        //Find vectors for two edges sharing vertex/point p1
        e1 = p2 - p1;
        e2 = p3 - p1;      

        // calculating determinant 
        p = Vector3.Cross(ray.direction, e2);

        //Calculate determinat
        det = Vector3.Dot(e1, p);

        //if determinant is near zero, ray lies in plane of triangle otherwise not
        if (det > -Epsilon && det < Epsilon) { return false; }
        invDet = 1.0f / det;

        //calculate distance from p1 to ray origin
        t = ray.origin - p1;

        //Calculate u parameter
        u = Vector3.Dot(t, p) * invDet;

        //Check for ray hit
        if (u < 0 || u > 1) { return false; }

        //Prepare to test v parameter
        q = Vector3.Cross(t, e1);

        //Calculate v parameter
        v = Vector3.Dot(ray.direction, q) * invDet;

        //Check for ray hit
        if (v < 0 || u + v > 1) { return false; }

        if ((Vector3.Dot(e2, q) * invDet) > Epsilon)
        {
            return true;
        }

        // No hit at all
        return false;
    }

    /*
    
    void OnCollisionEnter (Collision col)
    {
        Debug.Log("here");
       if(col.gameObject == floor)
       {
           CummulativeReward -= 0.1f;
           hit = true;
       }
    }*/

    public override void AgentReset()
	{
        rigid = gameObject.GetComponent<Rigidbody>();

        //rigid.velocity = new Vector3(0f, 0f, 0f);
        //rigid.angularVelocity = new Vector3(0f, 0f, 0f);
        //rigid.constraints = RigidbodyConstraints.FreezeAll;
        //gameObject.transform.position = new Vector3(0f, 2.2f, 0f);

        baseMesh = gameObject.GetComponent<MeshFilter>().mesh;
        meshBounds = new Bounds(gameObject.transform.position, boundingSize);
        baseMesh.bounds = meshBounds;        
    }
    //These need to be defined to use the Concave Mesh API, but don't actually do anything
    void Log(string message){Debug.Log(message);}
    void Progress(string message, float fPercent) { }
}

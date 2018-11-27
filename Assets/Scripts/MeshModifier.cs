using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshModifier : MonoBehaviour
{
    private static Mesh mesh;
    Vector3[] originalVertices;
    Vector2[] originalUV;
    int[] originalTriangles;
    static Plane plane;
    static private List<Vector3> new_vertices = new List<Vector3>();
    private static MeshSide Left_side = new MeshSide();
    private static MeshSide Right_side = new MeshSide();
    private static List<Vector3> capVertTracker = new List<Vector3>();
    private static List<Vector3> capVertpolygon = new List<Vector3>();
    private List<GameObject> newGameObjects = new List<GameObject>();
 
    // Use this for initialization
    void Start()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Band")
        {
            collision.gameObject.tag = "Untagged";
            ContactPoint contact = collision.contacts[0];
             newGameObjects.AddRange(CutMesh(collision.gameObject, contact.point, transform.right, collision.gameObject.GetComponent<MeshRenderer>().sharedMaterial));
          
           foreach(GameObject objects in newGameObjects)
            {
                objects.AddComponent<Rigidbody>();
                objects.GetComponent<Rigidbody>().useGravity = true;
                objects.AddComponent<BoxCollider>();
                objects.gameObject.tag = "Untagged";
                var joint = objects.GetComponent<ConfigurableJoint>();
                if (joint != null)
                    Destroy(joint);
            }
            
        }
    }


    static void Capping()
    {
        capVertTracker.Clear();

        for (int i = 0; i < new_vertices.Count; ++i)
        {
            if (capVertTracker.Contains(new_vertices[i]))
            {
                continue;
            }

            capVertpolygon.Clear();
            capVertpolygon.Add(new_vertices[i + 0]);
            capVertpolygon.Add(new_vertices[i + 1]);
            capVertTracker.Add(new_vertices[i + 0]);
            capVertTracker.Add(new_vertices[i + 1]);

            bool isDone = false;
            while (!isDone)
            {
                isDone = true;

                for (int k = 0; k < new_vertices.Count; k += 2)
                {
                    if (new_vertices[k] == capVertpolygon[capVertpolygon.Count - 1] && !capVertTracker.Contains(new_vertices[k + 1]))
                    {
                        isDone = false;
                        capVertpolygon.Add(new_vertices[k + 1]);
                        capVertTracker.Add(new_vertices[k + 1]);
                    }

                    else if (new_vertices[k + 1] == capVertpolygon[capVertpolygon.Count - 1] && !capVertTracker.Contains(new_vertices[k]))
                    {
                        isDone = false;
                        capVertpolygon.Add(new_vertices[k]);
                        capVertTracker.Add(new_vertices[k]);
                    }

                }

            }

            FillCap(capVertpolygon);
        }
    }

    static void FillCap(List<Vector3> vertices)
    {

        Vector3 center = Vector3.zero;

        foreach (Vector3 point in vertices)
        {
            center += point;
        }

        center = center / vertices.Count;

        Vector3 upward = Vector3.zero;

        upward.x = plane.normal.y;
        upward.y = -plane.normal.x;
        upward.z = plane.normal.z;

        Vector3 left = Vector3.Cross(plane.normal, upward);
        Vector3 displacement = Vector3.zero;
        Vector3 newUV1 = Vector3.zero;
        Vector3 newUV2 = Vector3.zero;


        for (int i = 0; i < vertices.Count; i++)
        {
            displacement = vertices[i] - center;

            newUV1 = Vector3.zero;
            newUV1.x = 0.5f + Vector3.Dot(displacement, left);
            newUV1.y = 0.5f + Vector3.Dot(displacement, upward);
            newUV1.z = 0.5f + Vector3.Dot(displacement, plane.normal);

            displacement = vertices[(i + 1) % vertices.Count] - center;

            newUV2 = Vector3.zero;
            newUV2.x = 0.5f + Vector3.Dot(displacement, left);
            newUV2.y = 0.5f + Vector3.Dot(displacement, upward);
            newUV2.z = 0.5f + Vector3.Dot(displacement, plane.normal);


            Left_side.insertTriangle(new Vector3[] { vertices[i], vertices[(i + 1) % vertices.Count], center },
                new Vector3[] { -plane.normal, -plane.normal, -plane.normal }, new Vector2[] { newUV1, newUV2, new Vector2(0.5f, 0.5f) }, -plane.normal, Left_side.subIndices.Count - 1);

            Right_side.insertTriangle(new Vector3[] { vertices[i], vertices[(i + 1) % vertices.Count], center }, new Vector3[] { plane.normal, plane.normal, plane.normal }, new Vector2[]{ newUV1, newUV2, new Vector2(0.5f, 0.5f) }, plane.normal, Right_side.subIndices.Count - 1);
        }
    }



    public static GameObject[] CutMesh(GameObject _toCut, Vector3 _hitPos, Vector3 _normalvector, Material capMaterial)
    {
        mesh = _toCut.GetComponent<MeshFilter>().mesh;
        new_vertices.Clear();

        plane = new Plane(_toCut.transform.InverseTransformDirection(-_normalvector), _toCut.transform.InverseTransformPoint(_hitPos));

        Left_side.ResetAll();
        Right_side.ResetAll();

        bool[] Sides = new bool[3];
        int[] indices;
        int p1, p2, p3;


        for (int sub = 0; sub < mesh.subMeshCount; ++sub)
        {
            indices = mesh.GetIndices(sub);
            Left_side.subIndices.Add(new List<int>());
            Right_side.subIndices.Add(new List<int>());

            for (int i = 0; i < indices.Length; i += 3)
            {
                p1 = indices[i + 0];
                p2 = indices[i + 1];
                p3 = indices[i + 2];

                Sides[0] = plane.GetSide(mesh.vertices[p1]);
                Sides[1] = plane.GetSide(mesh.vertices[p2]);
                Sides[2] = plane.GetSide(mesh.vertices[p3]);

                if (Sides[0] == Sides[1] && Sides[0] == Sides[2])
                {
                    if (Sides[0])
                    {
                        Left_side.makeTriangle(p1, p2, p3, sub);
                    }

                    else
                    {
                        Right_side.makeTriangle(p1, p2, p3, sub);
                    }
                }

                else
                {
                    CutFace(sub, Sides, p1, p2, p3);
                }
            }
        }



        Material[] mats = _toCut.GetComponent<MeshRenderer>().sharedMaterials;

        if (mats[mats.Length - 1].name != capMaterial.name)
        {
            Left_side.subIndices.Add(new List<int>());
            Right_side.subIndices.Add(new List<int>());

            Material[] newMats = new Material[mats.Length + 1];

            mats.CopyTo(newMats, 0);

            newMats[mats.Length] = capMaterial;

            mats = newMats;
        }

        Capping();

        Mesh left_newMesh = new Mesh();
        left_newMesh.name = "MeshLeft";
        left_newMesh.vertices = Left_side.vertices.ToArray();
        left_newMesh.triangles = Left_side.triangles.ToArray();
        left_newMesh.normals = Left_side.normals.ToArray();
        left_newMesh.uv = Left_side.uvs.ToArray();

        left_newMesh.subMeshCount = Left_side.subIndices.Count;

        for (int i = 0; i < Left_side.subIndices.Count; i++)
        {
            left_newMesh.SetIndices(Left_side.subIndices[i].ToArray(), MeshTopology.Triangles, i);
        }


        Mesh right_newMesh = new Mesh();
        right_newMesh.name = "MeshRight";
        right_newMesh.vertices = Right_side.vertices.ToArray();
        right_newMesh.triangles = Right_side.triangles.ToArray();
        right_newMesh.normals = Right_side.normals.ToArray();
        right_newMesh.uv = Right_side.uvs.ToArray();

        right_newMesh.subMeshCount = Right_side.subIndices.Count;
        for (int i = 0; i < Right_side.subIndices.Count; i++)
        {
            right_newMesh.SetIndices(Right_side.subIndices[i].ToArray(), MeshTopology.Triangles, i);
        }

        _toCut.name = "left side";
        _toCut.GetComponent<MeshFilter>().mesh = left_newMesh;

        GameObject leftSideObject = _toCut;

        GameObject rightSideObject = new GameObject("right side", typeof(MeshFilter), typeof(MeshRenderer));
        rightSideObject.transform.position = _toCut.transform.position;
        rightSideObject.transform.rotation = _toCut.transform.rotation;
        rightSideObject.GetComponent<MeshFilter>().mesh = right_newMesh;

        leftSideObject.GetComponent<MeshRenderer>().materials = mats;
        rightSideObject.GetComponent<MeshRenderer>().materials = mats;

        return new GameObject[] { leftSideObject, rightSideObject };

    }
    static void CutFace(int submesh, bool[] sides, int index1, int index2, int index3)
    {
        // left and right sequences gun to hold information 
        Vector3[] LeftPoints = new Vector3[2];
        Vector3[] LeftNormals = new Vector3[2];
        Vector2[] LeftUvs = new Vector2[2];
        Vector3[] RightPoints = new Vector3[2];
        Vector3[] RightNormals = new Vector3[2];
        Vector2[] RightUvs = new Vector2[2];

        bool didSetLeft = false;
        bool didSetRight = false;


        int p = index1;
        for (int side = 0; side < 3; side++)
        {
            switch (side)
            {
                case 0:
                    p = index1;
                    break;
                case 1:
                    p = index2;
                    break;
                case 2:
                    p = index3;
                    break;
            }

            if (sides[side])
            {
                if (!didSetLeft)
                {
                    didSetLeft = true;

                    LeftPoints[0] = mesh.vertices[p];
                    LeftPoints[1] = LeftPoints[0];

                    LeftUvs[0] = mesh.uv[p];
                    LeftUvs[1] = LeftUvs[0];

                    LeftNormals[0] = mesh.normals[p];
                    LeftNormals[1] = LeftNormals[0];
                }
                else
                {
                    LeftPoints[1] = mesh.vertices[p];
                    LeftUvs[1] = mesh.vertices[p];
                    LeftNormals[1] = mesh.normals[p];

                }
            }

            else
            {
                if (!didSetRight)
                {
                    didSetRight = true;

                    RightPoints[0] = mesh.vertices[p];
                    RightPoints[1] = RightPoints[0];

                    RightUvs[0] = mesh.uv[p];
                    RightUvs[1] = RightUvs[0];

                    RightNormals[0] = mesh.normals[p];
                    RightNormals[1] = RightNormals[0];
                }

                else
                {
                    RightPoints[1] = mesh.vertices[p];
                    RightUvs[1] = mesh.vertices[p];
                    RightNormals[1] = mesh.normals[p];
                }
            }
        }


        float normalizedDistance = 0.0f;
        float distance = 0.0f;

        plane.Raycast(new Ray(LeftPoints[0], (RightPoints[0] - LeftPoints[0]).normalized), out distance);
        normalizedDistance = distance / (RightPoints[0] - LeftPoints[0]).magnitude;

        Vector3 newVertex1 = Vector3.Lerp(LeftPoints[0], RightPoints[0], normalizedDistance);
        Vector3 newNormal1 = Vector3.Lerp(LeftNormals[0], RightNormals[0], normalizedDistance);
        Vector2 newUv1 = Vector2.Lerp(LeftUvs[0], RightUvs[0], normalizedDistance);

        new_vertices.Add(newVertex1);




        plane.Raycast(new Ray(LeftPoints[1], (RightPoints[1] - LeftPoints[1]).normalized), out distance);
        normalizedDistance = distance / (RightPoints[1] - LeftPoints[1]).magnitude;
        Vector3 newVertex2 = Vector3.Lerp(LeftPoints[1], RightPoints[1], normalizedDistance);
        Vector2 newUv2 = Vector2.Lerp(LeftUvs[1], RightUvs[1], normalizedDistance);
        Vector3 newNormal2 = Vector3.Lerp(LeftNormals[1], RightNormals[1], normalizedDistance);

        new_vertices.Add(newVertex2);

        Left_side.insertTriangle(new Vector3[] { LeftPoints[0], newVertex1, newVertex2 },
            new Vector3[] { LeftNormals[0], newNormal1, newNormal2 }, new Vector2[] { LeftUvs[0], newUv1, newUv2 }, newNormal1, submesh);

        Left_side.insertTriangle(new Vector3[] { LeftPoints[0], LeftPoints[1], newVertex2 }, new Vector3[] { LeftNormals[0], LeftNormals[1], newNormal2 },
            new Vector2[] { LeftUvs[0], LeftUvs[1], newUv2 }, newNormal2, submesh);

        Right_side.insertTriangle(new Vector3[] { RightPoints[0], newVertex1, newVertex2 },
            new Vector3[] { RightNormals[0], newNormal1, newNormal2 }, new Vector2[] { RightUvs[0], newUv1, newUv2 }, newNormal1, submesh);

        Right_side.insertTriangle(new Vector3[] { RightPoints[0], RightPoints[1], newVertex2 },
            new Vector3[] { RightNormals[0], RightNormals[1], newNormal2 }, new Vector2[] { RightUvs[0], RightUvs[1], newUv2 }, newNormal2, submesh);


    }

    public class MeshSide
    {
        public List<Vector3> vertices = new List<Vector3>();
        public List<int> triangles = new List<int>();
        public List<Vector3> normals = new List<Vector3>();
        public List<Vector2> uvs = new List<Vector2>();
        public List<List<int>> subIndices = new List<List<int>>();



        public void ResetAll()
        {
            vertices.Clear();
            triangles.Clear();
            normals.Clear();
            uvs.Clear();
            subIndices.Clear();
        }



        public void makeTriangle(int p1, int p2, int p3, int submesh)
        {
            int defaultIndex = vertices.Count;
            subIndices[submesh].Add(defaultIndex + 0);
            subIndices[submesh].Add(defaultIndex + 1);
            subIndices[submesh].Add(defaultIndex + 2);

            triangles.Add(defaultIndex + 0);
            triangles.Add(defaultIndex + 1);
            triangles.Add(defaultIndex + 2);

            vertices.Add(mesh.vertices[p1]);
            vertices.Add(mesh.vertices[p2]);
            vertices.Add(mesh.vertices[p3]);

            uvs.Add(mesh.uv[p1]);
            uvs.Add(mesh.uv[p2]);
            uvs.Add(mesh.uv[p3]);

            normals.Add(mesh.normals[p1]);
            normals.Add(mesh.normals[p2]);
            normals.Add(mesh.normals[p3]);

        }

        public void insertTriangle(Vector3[] points3, Vector3[] normals3, Vector2[] uv3, Vector3 faceNormal, int submesh)
        {
            Vector3 calculatedNormal = Vector3.Cross((points3[1] - points3[0]).normalized, (points3[2] - points3[0]).normalized);

            int p1 = 0;
            int p2 = 1;
            int p3 = 2;

            if (Vector3.Dot(calculatedNormal, faceNormal) < 0)
            {
                p1 = 2;
                p2 = 1;
                p3 = 0;
            }

            int defaultIndex = vertices.Count;

            subIndices[submesh].Add(defaultIndex + 0);
            subIndices[submesh].Add(defaultIndex + 1);
            subIndices[submesh].Add(defaultIndex + 2);

            triangles.Add(defaultIndex + 0);
            triangles.Add(defaultIndex + 1);
            triangles.Add(defaultIndex + 2);

            uvs.Add(uv3[p1]);
            uvs.Add(uv3[p2]);
            uvs.Add(uv3[p3]);

            vertices.Add(points3[p1]);
            vertices.Add(points3[p2]);
            vertices.Add(points3[p3]);

            normals.Add(normals3[p1]);
            normals.Add(normals3[p2]);
            normals.Add(normals3[p3]);

        }
    }




}

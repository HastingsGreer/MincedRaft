using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ChunkRenderer : MonoBehaviour
{
    private Mesh mesh;

    private Dictionary<float, int> vertLookup;
    FastNoise noise = new FastNoise();

    public int xx = 0, yy = 0, zz = 0; 
    private void Awake() {
        
        StartCoroutine(Generate());
    }
    private bool isBlock(int x, int y, int z) {
        x = x + xx;
        y = y + yy;
        z = z + zz;
        return noise.GetNoise(x , y , z ) > y / 30f;
    }
    private IEnumerator Generate()
    {

        WaitForSeconds wait = new WaitForSeconds(0.01f);
        yield return wait;
        xx = (int)transform.position.x;
        yy = (int)transform.position.y;
        zz = (int)transform.position.z;

        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "chunk mesh";
        List<Vector3> vertexList = new List<Vector3>();
        List<int> triangleList = new List<int>();

        for (int y = -32; y < 32; y++) {
            for (int x = -32; x <   32; x++) {
                for (int z = -32; z < 32; z++ ) {
                    if (isBlock(x, y, z)) {
                        var normal = new Vector3(0, 0, .5f);
                        var orientation = 1;
                        for (int itr = 0; itr < 3; itr ++) {
                            normal = permute(normal);
                            for (int i2 = 0; i2 < 2; i2++) {
                                normal = -normal;
                                orientation = - orientation;
                                var otherpos = (normal * 2) + new Vector3(x, y, z);
                                if (!isBlock((int)otherpos.x, (int)otherpos.y, (int)otherpos.z)) {
                                    var c = vertexList.Count;
                                    var blk = normal + new Vector3(x, y, z);
                                    var u = permute(normal);
                                    var v = orientation * permute(u);
                                    vertexList.Add(blk + u + v);
                                    vertexList.Add(blk + u - v);
                                    vertexList.Add(blk - u - v);
                                    vertexList.Add(blk - u + v);
                                    triangleList.Add(c);
                                    triangleList.Add(c + 1);
                                    triangleList.Add(c + 3);
                                    triangleList.Add(c + 1);
                                    triangleList.Add(c + 2);
                                    triangleList.Add(c + 3);
                                }
                            }
                        }
                    }

                }
            }
            //yield return wait;
        }
        
        mesh.vertices = vertexList.ToArray();
        mesh.triangles = triangleList.ToArray();
        mesh.RecalculateNormals();


    }

    private Vector3 permute(Vector3 v) {
        return new Vector3(v.y, v.z, v.x);
    }



    // Update is called once per frame
    void Update()
    {

    }

}

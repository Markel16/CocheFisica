using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class GroundGenerator : MonoBehaviour
{
    public int tamaño = 600;
    public int rejilla = 10;

    void Start()
    {
        Crear();
    }

    void Crear()
    {
        int vCount = (rejilla + 1) * (rejilla + 1);
        Vector3[] v = new Vector3[vCount];
        int[] t = new int[rejilla * rejilla * 6];
        Vector3[] nrm = new Vector3[vCount];

        int idx = 0;
        for (int z = 0; z <= rejilla; z++)
            for (int x = 0; x <= rejilla; x++)
            {
                float fx = (x / (float)rejilla - 0.5f) * tamaño;
                float fz = (z / (float)rejilla - 0.5f) * tamaño;
                v[idx] = new Vector3(fx, -0.1f, fz);
                nrm[idx] = Vector3.up;
                idx++;
            }

        int ti = 0;
        for (int z = 0; z < rejilla; z++)
            for (int x = 0; x < rejilla; x++)
            {
                int i0 = z * (rejilla + 1) + x;
                int i1 = i0 + 1;
                int i2 = i0 + (rejilla + 1);
                int i3 = i2 + 1;

                t[ti++] = i0; t[ti++] = i2; t[ti++] = i1;
                t[ti++] = i1; t[ti++] = i2; t[ti++] = i3;
            }

        Mesh m = new Mesh();
        m.vertices = v; m.triangles = t; m.normals = nrm;
        GetComponent<MeshFilter>().sharedMesh = m;
        GetComponent<MeshCollider>().sharedMesh = m;
    }
}

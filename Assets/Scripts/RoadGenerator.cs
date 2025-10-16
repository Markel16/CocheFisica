using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class RoadGenerator : MonoBehaviour
{
    public int puntos = 24;
    public float radio = 60f;
    public float ancho = 7f;
    public float irregularidad = 10f;

    [HideInInspector] public List<Vector3> linea;

    void Start()
    {
        Generar();
    }

    void Generar()
    {
        linea = new List<Vector3>();
        for (int i = 0; i < puntos; i++)
        {
            float ang = (i / (float)puntos) * Mathf.PI * 2f;
            float r = radio + Random.Range(-irregularidad, irregularidad);
            linea.Add(new Vector3(Mathf.Cos(ang) * r, 0, Mathf.Sin(ang) * r));
        }

        CrearMalla(linea, ancho);
    }

    void CrearMalla(List<Vector3> pts, float w)
    {
        int n = pts.Count;
        Vector3[] v = new Vector3[n * 2];
        int[] t = new int[n * 6];
        Vector3[] nrm = new Vector3[n * 2];

        for (int i = 0; i < n; i++)
        {
            Vector3 p = pts[i];
            Vector3 sig = pts[(i + 1) % n];
            Vector3 dir = (sig - p).normalized;
            Vector3 izq = new Vector3(-dir.z, 0, dir.x);

            v[i * 2] = p + izq * (w / 2);
            v[i * 2 + 1] = p - izq * (w / 2);
            nrm[i * 2] = nrm[i * 2 + 1] = Vector3.up;
        }

        int ti = 0;
        for (int i = 0; i < n; i++)
        {
            int i0 = i * 2;
            int i1 = i * 2 + 1;
            int i2 = ((i + 1) % n) * 2;
            int i3 = ((i + 1) % n) * 2 + 1;

            t[ti++] = i0; t[ti++] = i2; t[ti++] = i1;
            t[ti++] = i1; t[ti++] = i2; t[ti++] = i3;
        }

        Mesh m = new Mesh();
        m.vertices = v;
        m.triangles = t;
        m.normals = nrm;

        GetComponent<MeshFilter>().sharedMesh = m;
        GetComponent<MeshCollider>().sharedMesh = m;
    }
}

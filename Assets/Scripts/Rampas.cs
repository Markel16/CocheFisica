using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class Rampas : MonoBehaviour
{
    public float ancho = 6f;   //eje X
    public float largo = 12f;  //eje Z
    public float altura = 3f;  //eje Y en el extremo alto

    void Start()
    {
        var mf = GetComponent<MeshFilter>();
        var mc = GetComponent<MeshCollider>();

        
        float hx = ancho * 0.5f, hz = largo * 0.5f;
        Vector3 A = new(-hx, 0f, -hz);
        Vector3 B = new(hx, 0f, -hz);
        Vector3 C = new(hx, 0f, hz);
        Vector3 D = new(-hx, 0f, hz);
        Vector3 E = new(-hx, altura, hz);
        Vector3 F = new(hx, altura, hz);

        
        Vector3[] verts = { A, B, C, D, E, F };
        int[] tris = {
            // base
            0,1,2, 0,2,3,
            // parte de encima inclinada
            0,1,5, 0,5,4,
            // cara izquierda
            0,3,4,
            // cara derecha
            1,5,2,
            // cara trasera vertical
            3,2,5, 3,5,4
        };

        Mesh m = new();
        m.name = "RampWedge";
        m.vertices = verts;
        m.triangles = tris;
        m.RecalculateNormals();
        m.RecalculateBounds();

        mf.sharedMesh = m;
        mc.sharedMesh = m;
    }
}

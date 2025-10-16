using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform objetivo;
    public Vector3 offset = new Vector3(0, 5f, -10f);
    public float suavizado = 6f;

    void LateUpdate()
    {
        if (!objetivo) return;

        Vector3 posDeseada = objetivo.TransformPoint(offset);
        transform.position = Vector3.Lerp(transform.position, posDeseada, Time.deltaTime * suavizado);
        transform.LookAt(objetivo);
    }
}
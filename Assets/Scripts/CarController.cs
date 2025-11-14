using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    [Header("Acciones del asset (.inputactions)")]
    public InputActionReference Move;       // Vector2 (2D Vector: W/S/A/D)
    public InputActionReference Brake;      // Button (S)
    public InputActionReference Handbrake;  // Button (Espacio)
    public InputActionReference Reset;      // Button (R)

    [Header("Físicas")]
    public float fuerzaMotor = 2500f;
    public float fuerzaFreno = 4000f;
    public float friccionLateral = 10f;
    public float velocidadMax = 40f;
    public float fuerzaGiro = 4f;
    public float arrastre = 0.5f;

    Rigidbody rb;
    Vector3 posIni; Quaternion rotIni;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        posIni = transform.position; rotIni = transform.rotation;
    }

    void OnEnable()
    {
        if (Move) Move.action.Enable();
        if (Brake) Brake.action.Enable();
        if (Handbrake) Handbrake.action.Enable();
        if (Reset) Reset.action.Enable();
    }

    void OnDisable()
    {
        if (Move) Move.action.Disable();
        if (Brake) Brake.action.Disable();
        if (Handbrake) Handbrake.action.Disable();
        if (Reset) Reset.action.Disable();
    }

    void FixedUpdate()
    {
        // Lectura segura (si no hay referencia, devuelve por defecto)
        Vector2 move = Move ? Move.action.ReadValue<Vector2>() : Vector2.zero;
        bool frenar = (Brake && Brake.action.IsPressed()) || move.y < 0f;
        bool frenoMano = Handbrake && Handbrake.action.IsPressed();
        if (Reset && Reset.action.WasPressedThisFrame()) Reiniciar();

        float girar = Mathf.Clamp(move.x, -1f, 1f);
        float acelerar = Mathf.Max(0f, move.y);

        Vector3 fwd = transform.forward;
        Vector3 right = transform.right;
        float vFwd = Vector3.Dot(rb.linearVelocity, fwd);
        float vLat = Vector3.Dot(rb.linearVelocity, right);

        if (rb.linearVelocity.magnitude < velocidadMax)
            rb.AddForce(fwd * (acelerar * fuerzaMotor), ForceMode.Force);

        if (frenar)
            rb.AddForce(-fwd * Mathf.Sign(vFwd) * fuerzaFreno, ForceMode.Force);

        float mult = frenoMano ? 3f : 1f;
        rb.AddForce(-right * vLat * friccionLateral * mult, ForceMode.Force);

        rb.AddForce(-rb.linearVelocity * arrastre, ForceMode.Force);

        rb.AddTorque(Vector3.up * girar * fuerzaGiro * Mathf.Abs(vFwd), ForceMode.Force);
    }

    void Reiniciar()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.SetPositionAndRotation(posIni, rotIni);
    }
}


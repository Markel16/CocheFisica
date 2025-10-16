using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    public ActionHub input;
    public float fuerzaMotor = 2500f;
    public float fuerzaFreno = 4000f;
    public float friccionLateral = 10f;
    public float velocidadMax = 40f;
    public float fuerzaGiro = 4f;
    public float arrastre = 0.5f;

    private Rigidbody rb;
    private Vector3 posInicial;
    private Quaternion rotInicial;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        posInicial = transform.position;
        rotInicial = transform.rotation;
    }

    void FixedUpdate()
    {
        if (input == null) return;

        float girar = input.Girar.ReadValue<float>();
        float acelerar = input.Acelerar.ReadValue<float>();
        bool frenar = input.Frenar.IsPressed();
        bool frenoMano = input.FrenoMano.IsPressed();
        if (input.Reiniciar.WasPressedThisFrame()) Reiniciar();

        Vector3 dirAdelante = transform.forward;
        Vector3 dirDerecha = transform.right;
        float velAdelante = Vector3.Dot(rb.linearVelocity, dirAdelante);
        float velLateral = Vector3.Dot(rb.linearVelocity, dirDerecha);

        // Tracci�n
        if (rb.linearVelocity.magnitude < velocidadMax)
            rb.AddForce(dirAdelante * (acelerar * fuerzaMotor), ForceMode.Force);

        // Freno principal
        if (frenar)
            rb.AddForce(-dirAdelante * Mathf.Sign(velAdelante) * fuerzaFreno, ForceMode.Force);

        // Fricci�n lateral y freno de mano
        float mult = frenoMano ? 3f : 1f;
        rb.AddForce(-dirDerecha * velLateral * friccionLateral * mult, ForceMode.Force);

        // Arrastre del aire
        rb.AddForce(-rb.linearVelocity * arrastre, ForceMode.Force);

        // Giro
        rb.AddTorque(Vector3.up * girar * fuerzaGiro * velAdelante, ForceMode.Force);
    }

    void Reiniciar()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.SetPositionAndRotation(posInicial, rotInicial);
    }
}

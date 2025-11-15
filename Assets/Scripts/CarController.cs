using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{
    [Header("Acciones del asset (.inputactions)")]
    public InputActionReference Move;    
    public InputActionReference Reset;   

    [Header("Movimiento directo (no físico)")]
    public float velocidad = 10f;        
    public float velocidadGiro = 120f;   

    private Vector3 posIni;
    private Quaternion rotIni;

    void Awake()
    {
        posIni = transform.position;
        rotIni = transform.rotation;
    }

    void OnEnable()
    {
        if (Move) Move.action.Enable();
        if (Reset) Reset.action.Enable();
    }

    void OnDisable()
    {
        if (Move) Move.action.Disable();
        if (Reset) Reset.action.Disable();
    }

    void Update()
    {
        if (Move == null) return;

        // Leer input WASD
        Vector2 m = Move.action.ReadValue<Vector2>(); //x=A/D y W/S
        // Debug para comprobar
        if (m != Vector2.zero)
            Debug.Log("Move: " + m);

        // Reiniciar
        if (Reset != null && Reset.action.WasPressedThisFrame())
        {
            Reiniciar();
        }

        
        float adelante = m.y;   
        
        float giro = m.x;       

       
        Vector3 desplazamiento = transform.forward * adelante * velocidad * Time.deltaTime;
        transform.position += desplazamiento;

        // Girar sobre el eje Y
        transform.Rotate(0f, giro * velocidadGiro * Time.deltaTime, 0f);
    }

    void Reiniciar()
    {
        transform.SetPositionAndRotation(posIni, rotIni);
        Debug.Log("RESET coche");
    }
}

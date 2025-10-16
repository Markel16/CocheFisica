using UnityEngine;
using UnityEngine.InputSystem;

public class ActionHub : MonoBehaviour
{
    public InputAction Girar = new InputAction(type: InputActionType.Value);
    public InputAction Acelerar = new InputAction(type: InputActionType.Value);
    public InputAction Frenar = new InputAction(type: InputActionType.Button);
    public InputAction FrenoMano = new InputAction(type: InputActionType.Button);
    public InputAction Reiniciar = new InputAction(type: InputActionType.Button);

    void Awake()
    {
        Girar.AddCompositeBinding("1DAxis")
            .With("negative", "<Keyboard>/a").With("negative", "<Keyboard>/leftArrow")
            .With("positive", "<Keyboard>/d").With("positive", "<Keyboard>/rightArrow");

        Acelerar.AddCompositeBinding("1DAxis")
            .With("positive", "<Keyboard>/w").With("positive", "<Keyboard>/upArrow");

        Frenar.AddBinding("<Keyboard>/s").WithGroup("Keyboard");
        FrenoMano.AddBinding("<Keyboard>/space");
        Reiniciar.AddBinding("<Keyboard>/r");
    }

    void OnEnable()
    {
        Girar.Enable(); Acelerar.Enable(); Frenar.Enable(); FrenoMano.Enable(); Reiniciar.Enable();
    }

    void OnDisable()
    {
        Girar.Disable(); Acelerar.Disable(); Frenar.Disable(); FrenoMano.Disable(); Reiniciar.Disable();
    }
}


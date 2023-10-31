using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class TouchManager : MonoBehaviour
{
    [SerializeField] private Transform player;

    private PlayerInput playerInput;

    private InputAction touchPositionAction;
    private InputAction touchPressAction;

    private Camera mainCamera;

    private void OnValidate()
    {
        playerInput ??= GetComponent<PlayerInput>();
        touchPressAction ??= playerInput.actions["TouchPress"];
        touchPositionAction ??= playerInput.actions["TouchPosition"];
    }

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        touchPressAction.performed += TouchPressed;
    }

    private void OnDisable()
    {
        touchPressAction.performed -= TouchPressed;
    }

    private void TouchPressed(InputAction.CallbackContext context)
    {
        Vector3 position = mainCamera.ScreenToWorldPoint(touchPositionAction.ReadValue<Vector2>());
        position.z = player.transform.position.z;
        player.transform.position = position;
    }

    private void Update()
    {
        if (touchPressAction.WasPerformedThisFrame())
        {
            Vector3 position = mainCamera.ScreenToWorldPoint(touchPositionAction.ReadValue<Vector2>());
            position.z = player.transform.position.z;
            player.transform.position = position;
        }
    }
}

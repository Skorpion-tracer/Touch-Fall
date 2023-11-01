using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class TouchManager : MonoBehaviour
{
    [SerializeField] private Ball player;

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
        touchPressAction.canceled += TouchCanceled;
    }

    private void TouchCanceled(InputAction.CallbackContext obj)
    {
        player.IsMove = false;
        Debug.Log("Завершено");
    }

    private void OnDisable()
    {
        touchPressAction.performed -= TouchPressed;
        touchPressAction.canceled -= TouchCanceled;
    }

    private void TouchPressed(InputAction.CallbackContext context)
    {
        Vector3 position = mainCamera.ScreenToWorldPoint(touchPositionAction.ReadValue<Vector2>());
        position.z = player.transform.position.z;
        Debug.Log(position);
        player.IsMove = true;
        player.Poistion = position;
        Debug.Log("Запущено");
    }

    //private void Update()
    //{
    //    if (touchPressAction.WasPerformedThisFrame())
    //    {
    //        Vector3 position = mainCamera.ScreenToWorldPoint(touchPositionAction.ReadValue<Vector2>());
    //        position.z = player.transform.position.z;

    //        player.transform.position = position;
    //    }
    //}
}

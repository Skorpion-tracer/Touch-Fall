using System;
using TouchFall.Helper;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TouchFall.Input
{
    [DefaultExecutionOrder(-1)]
    public class InputManager : Singleton<InputManager>
    {
        private PlayerControl playerControl;
        private Camera mainCamera;

        public event Action<Vector2, float> StartTouch;
        public event Action<Vector2, float> EndTouch;

        private void Awake()
        {
            playerControl = new PlayerControl();
            mainCamera = Camera.main;
            EndTouch?.Invoke(Utils.ScreenToWorld(mainCamera, playerControl.Touch.PrimaryPosition.ReadValue<Vector2>()), 0f);
        }

        private void OnEnable()
        {
            playerControl.Enable();
            playerControl.Touch.PrimaryPosition.ReadValue<Vector2>();
        }

        private void OnDisable()
        {
            playerControl.Disable();
        }

        private void Start()
        {
            playerControl.Touch.PrimaryContact.started += ctx => StartTocuhPrimary(ctx);
            playerControl.Touch.PrimaryContact.canceled += ctx => EndTocuhPrimary(ctx);
        }

        private void StartTocuhPrimary(InputAction.CallbackContext ctx)
        {
            StartTouch?.Invoke(Utils.ScreenToWorld(mainCamera, playerControl.Touch.PrimaryPosition.ReadValue<Vector2>()), (float)ctx.startTime);
        }

        private void EndTocuhPrimary(InputAction.CallbackContext ctx)
        {
            EndTouch?.Invoke(Utils.ScreenToWorld(mainCamera, playerControl.Touch.PrimaryPosition.ReadValue<Vector2>()), (float)ctx.time);
        }

        public Vector2 PrimaryPosition()
        {
            return Utils.ScreenToWorld(mainCamera, playerControl.Touch.PrimaryPosition.ReadValue<Vector2>());
        }
    }
}


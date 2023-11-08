using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public sealed class Ball : MonoBehaviour
{
    [SerializeField] private float speed = 3f;

    private Rigidbody2D _body;

    private bool _isTouch;
    private bool _isEndTouch;
    private Vector2 endPosition;

    private InputManager inputManager;

    private void OnValidate()
    {
        inputManager ??= InputManager.Instance;
        _body ??= GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        inputManager.StartTouch += OnStartSwipe;
        inputManager.EndTouch += OnEndSwipe;
    }

    private void OnDisable()
    {
        inputManager.StartTouch -= OnStartSwipe;
        inputManager.EndTouch -= OnEndSwipe;
    }

    private void OnStartSwipe(Vector2 position, float time)
    {
        _isTouch = true;
        _isEndTouch = false;
    }

    private void OnEndSwipe(Vector2 position, float time)
    {
        _isTouch = false;
        _isEndTouch = true;
        endPosition = position;
    }

    private void Update()
    {
        if (_isTouch)
        {
            //transform.position = Vector3.Lerp(transform.position, inputManager.PrimaryPosition(), speed * Time.deltaTime);
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, -2f, 2f), Mathf.Clamp(transform.position.y, -4f, 4f), transform.position.z);
        }
        else if (_isEndTouch)
        {
            //transform.position = Vector3.Lerp(transform.position, endPosition, speed * Time.deltaTime);
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, -2f, 2f), Mathf.Clamp(transform.position.y, -4f, 4f), transform.position.z);
            if (transform.position == (Vector3)endPosition)
            {
                _isEndTouch = false;
            }
        }
    }

    private void FixedUpdate()
    {
        if (_isTouch)
        {
            //transform.position = Vector3.Lerp(transform.position, inputManager.PrimaryPosition(), speed * Time.deltaTime);
            _body.MovePosition(Vector3.Lerp(transform.position, inputManager.PrimaryPosition(), speed * Time.fixedDeltaTime));
        }
        else if (_isEndTouch)
        {
            //transform.position = Vector3.Lerp(transform.position, endPosition, speed * Time.deltaTime);
            _body.MovePosition(Vector3.Lerp(transform.position, endPosition, speed * Time.fixedDeltaTime));
        }
    }
}

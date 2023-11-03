using UnityEngine;
using UnityEngine.Windows;

[RequireComponent(typeof(Rigidbody2D))]
public class Ball : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    private Rigidbody2D body;

    public bool IsMove { get; set; }
    public Vector2 Poistion { get; set; }

    private void OnValidate()
    {
        body ??= GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (IsMove)
        {
            body.AddForce(Poistion * speed);
            body.velocity = Vector2.Lerp(body.velocity, Vector2.zero, 15f);
        }
        else
        {
            body.velocity = Vector2.Lerp(body.velocity, Vector2.zero, 15f);
        }
    }
}

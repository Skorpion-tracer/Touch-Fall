using UnityEngine;

public class StayInside : MonoBehaviour
{
    [SerializeField] private Transform boundleft;
    [SerializeField] private Transform boundRight;

    private Vector2 screenBounds;

    private void Start()
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

        boundleft.position = new Vector2(screenBounds.x + (boundleft.localScale.x * 0.5f), 0);
        boundleft.localScale = new Vector2(boundleft.localScale.x, screenBounds.y * 2);

        boundRight.position = new Vector2((screenBounds.x + (boundleft.localScale.x * 0.5f)) * -1, 0);
        boundRight.localScale = new Vector2(boundRight.localScale.x, screenBounds.y * 2);
    }
}

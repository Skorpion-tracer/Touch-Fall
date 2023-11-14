using UnityEngine;

public class DestroyBoundsObjects : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.SetActive(false);
    }
}

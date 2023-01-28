using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] AudioClip coinPickupSFX;
    bool isPickedUp = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isPickedUp)
        {
            isPickedUp = true;
            FindObjectOfType<GameSession>().ProcessPlayerScore();
            AudioSource.PlayClipAtPoint(coinPickupSFX, transform.position);
            Destroy(gameObject);
        }
    }
}
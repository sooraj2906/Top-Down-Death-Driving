using UnityEngine;

// <summary>
/// This class manages the enemy bullets
/// </summary>
public class EnemyBulletController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float speed = 10f;

    //Adds a force to the bullet in the specified direction
    public void Shoot(Vector3 direction)
    {
        rb.AddForce(
            direction * speed, ForceMode2D.Impulse);
    }

    //Handles collision with player and obstacles
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstacle"))
        {
            gameObject.SetActive(false);
        }

        if (other.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            GameManager.gameManagerPInstance.carController.ReduceHealth(2f);
        }
    }

//Disable the bullet when they leave the camera fov
    private void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }
}
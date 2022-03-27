using System;
using UnityEngine;
using Random = UnityEngine.Random;

// <summary>
/// This class manages the player bullets
/// </summary>
public class PlayerBulletController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float critMultiplier = 2f;

    //Adds a force to the bullet in the specified direction
    public void Shoot(Vector3 direction)
    {
        rb.AddForce(
            direction * speed, ForceMode2D.Impulse);
    }

    //Handles collision with turret and calculates the amount of damage to be dealt to the turret taking in to consideration of the crit rate of the weapon
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Turret"))
        {
            if (other.GetComponentInParent<EnemyTurretController>().health <= 0)
            {
                Destroy(other.GetComponentInParent<EnemyTurretController>().gameObject);
                GameManager.gameManagerPInstance.enemyTurrets.Remove(
                    other.GetComponentInParent<EnemyTurretController>().gameObject);
                gameObject.SetActive(false);
                GameManager.onTurretDestroyed?.Invoke();
            }
            else
            {
                var playerWeapon = GameManager.gameManagerPInstance.GetPlayerWeapon();
                if (Random.value > playerWeapon.critRate / 100f)
                {
                    //Deal normal damage
                    other.GetComponentInParent<EnemyTurretController>().health -= playerWeapon.damage / 10f;
                }
                else
                {
                    //Deal crit damage(2x)
                    other.GetComponentInParent<EnemyTurretController>().health -= critMultiplier * (playerWeapon.damage / 10f);
                }
            }
        }

    }

    //Handles collision with obstacles
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Obstacle"))
        {
            gameObject.SetActive(false);
        }
    }

    //Disable the bullet when they leave the camera fov
    private void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }
}
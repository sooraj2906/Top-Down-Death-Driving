using System.Collections;
using UnityEngine;

/// <summary>
/// This is the main weapon class. It holds all the stats of the weapon and handles the firing of the weapon
/// </summary>
public class WeaponController : MonoBehaviour
{
    [SerializeField] private Transform bulletSpawnTransform;
    public float rateOfFire;
    public float damage;
    public float critRate;
    public float maxAmmo;
    public bool isPlayer;
    public bool isButtonPressed;

    public bool canFire = true;

    private void Awake()
    {
        GameManager.onGameStart += StartGame;
    }

    private void StartGame()
    {
        canFire = true;
    }

    //Check if player can shoot and call the shoot method
    private void Update()
    {
        if (!GameManager.gameManagerPInstance.isPlaying) return;
        if (isPlayer)
        {
            if (isButtonPressed && canFire)
            {
                StartCoroutine(Fire());
            }
        }
    }

    //Fires bullets taking into consideration the rate of fire of the current weapon
    private IEnumerator Fire()
    {
        canFire = false;
        var bulletGo = ObjectPooling.ObjectPoolInstance.GetPooledBullets(true);
        if (bulletGo != null)
        {
            if (maxAmmo > 0)
            {
                bulletGo.transform.position = bulletSpawnTransform.position;
                bulletGo.transform.rotation = Quaternion.identity;
                bulletGo.SetActive(true);
                bulletGo.GetComponent<PlayerBulletController>().Shoot(GetBulletDirection());
                maxAmmo--;
            }
        }

        yield return new WaitForSeconds(1f / rateOfFire);
        canFire = true;
    }

    //Returns the direction of the bullet
    private Vector3 GetBulletDirection()
    {
        return bulletSpawnTransform.position - transform.position;
    }

    public float GetDamage()
    {
        return damage;
    }
}
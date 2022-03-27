using System.Collections;
using UnityEngine;

/// <summary>
/// This class handles the enemy turrets
/// </summary>
public class EnemyTurretController : MonoBehaviour
{
    [SerializeField] private GameObject weaponSpawn;
    [SerializeField] private float range = 5f;
    public float health = 10f;

    private bool _canFire = true;

    public float rateOfFire;
    private bool isShooting;

    private void Update()
    {
        if (!GameManager.gameManagerPInstance.isPlaying) return;
        var carController = GameManager.gameManagerPInstance.carController;
        //Checks if player in specified range of the turret
        if (CheckPlayerInRange(range))
        {
            //Make the turret look at the player and fire
            transform.up = carController.GetPlayerPos() - transform.position;
            if (_canFire)
                StartCoroutine(Fire());
        }
    }

    ////Fires bullets taking into consideration the rate of fire of the current weapon
    private IEnumerator Fire()
    {
        _canFire = false;
        var missileGo = ObjectPooling.ObjectPoolInstance.GetPooledBullets(false);
        if (missileGo != null)
        {
            if (weaponSpawn != null) missileGo.transform.position = weaponSpawn.transform.position;
            missileGo.transform.rotation = Quaternion.identity;
            missileGo.SetActive(true);
            missileGo.GetComponent<EnemyBulletController>().Shoot(GetBulletDirection());
        }

        yield return new WaitForSeconds(1f / (rateOfFire / 10f));
        _canFire = true;
    }
    
    //Return whether the player is in specified range
    private bool CheckPlayerInRange(float range)
    {
        var carController = GameManager.gameManagerPInstance.carController;
        return Vector3.Distance(transform.position, carController.GetPlayerPos()) < range;
    }

    //Returns the direction in which the bullet should travel
    public Vector3 GetBulletDirection()
    {
        return weaponSpawn.transform.position - transform.position;
    }
}
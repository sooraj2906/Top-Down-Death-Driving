using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// <summary>
/// This class is used for Object Pooling
/// </summary>
public class ObjectPooling : MonoBehaviour
{
    public static ObjectPooling ObjectPoolInstance;

    public List<GameObject> pooledPlayerBullets;
    public List<GameObject> pooledTurretBullets;

    public GameObject playerBullet;
    public GameObject turretBullet;

    public int poolAmount;

    private void Awake()
    {
        ObjectPoolInstance = this;
    }

    private void Start()
    {
        CreatePlayerBulletPool();
        CreateTurretBulletPool();
    }

    public void CreatePlayerBulletPool()
    {
        var parent = new GameObject("PlayerBulletPool");
        pooledPlayerBullets = new List<GameObject>();
        for (var i = 0; i < poolAmount; i++)
        {
            var temp1 = Instantiate(playerBullet, parent.transform, true);
            temp1.SetActive(false);
            pooledPlayerBullets.Add(temp1);
        }
    }
    
    public void CreateTurretBulletPool()
    {
        var parent = new GameObject("TurretBulletPool");
        pooledTurretBullets = new List<GameObject>();
        for (var i = 0; i < poolAmount; i++)
        {
            var temp1 = Instantiate(turretBullet, parent.transform, true);
            temp1.SetActive(false);
            pooledTurretBullets.Add(temp1);
        }
    }

    public GameObject GetPooledBullets(bool isPlayerBullet)
    {
        if (isPlayerBullet)
        {
            for (var i = 0; i < poolAmount; i++)
            {
                if (!pooledPlayerBullets[i].activeInHierarchy) return pooledPlayerBullets[i];
            }
        }
        else
        {
            for (var i = 0; i < poolAmount; i++)
            {
                if (!pooledTurretBullets[i].activeInHierarchy) return pooledTurretBullets[i];
            }
        }

        return null;
    }

    // <summary>
    /// Disables all the missiles
    /// </summary>
    public void DisableAllMissiles()
    {
        foreach (var bullet in pooledPlayerBullets.Where(bullet => bullet.activeInHierarchy))
            bullet.SetActive(false);
        foreach (var bullet in pooledTurretBullets.Where(bullet => bullet.activeInHierarchy))
            bullet.SetActive(false);
    }
}
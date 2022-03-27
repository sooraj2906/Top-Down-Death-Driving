using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Stores the weapon data of individual weapons
/// </summary>
[System.Serializable]
public class GunMap
{
    public GameObject gun;
    public float maxAmmo;
    public float fireRate;
    public float damage;
    public float critRate;
}

//Creates a new scriptable object that helps us to store the data of different types of weapons
//NOTE: This gun data holds the sprites of different weapons as well. As its a 2d game its ok to do so.
//      But in a 3d game refrain from storing meshes in scriptable objects as they take up a lot of memory.
[CreateAssetMenu(menuName = "DesertDeathRace/GunData")]
public class GunData : ScriptableObject
{
    public List<GunMap> gunMaps = new List<GunMap>();

    public float GetGunMaxAmmo(int gunId)
    {
        var value = 0f;
        var gunMap = gunMaps.Find(x => x.gun.name.Contains(gunId.ToString()));
        if (gunMap != null)
            value = gunMap.maxAmmo;
        return value;
    }

    public float GetGunFireRate(int gunId)
    {
        var value = 0f;
        var gunMap = gunMaps.Find(x => x.gun.name.Contains(gunId.ToString()));
        if (gunMap != null)
            value = gunMap.fireRate;
        return value;
    }

    public float GetGunDamage(int gunId)
    {
        var value = 0f;
        var gunMap = gunMaps.Find(x => x.gun.name.Contains(gunId.ToString()));
        if (gunMap != null)
            value = gunMap.damage;
        return value;
    }

    public float GetGunCritRate(int gunId)
    {
        var value = 0f;
        var gunMap = gunMaps.Find(x => x.gun.name.Contains(gunId.ToString()));
        if (gunMap != null)
            value = gunMap.critRate;
        return value;
    }
    
    public GameObject GetGun(int gunId)
    {
        var value = new GameObject();
        var gunMap = gunMaps.Find(x => x.gun.name.Contains(gunId.ToString()));
        if (gunMap != null)
            value = gunMap.gun;
        return value;
    }
}
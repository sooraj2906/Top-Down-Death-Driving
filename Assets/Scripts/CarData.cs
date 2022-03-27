using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CarMap
{
    public GameObject car;
    public float healthPoints;
    public float speed;
    public float handling;
}

[CreateAssetMenu(menuName = "DesertDeathRace/CarData")]
public class CarData : ScriptableObject
{
    public List<CarMap> carMaps = new List<CarMap>();

    public float GetCarHp(int carId)
    {
        var value = 0f;
        var carMap = carMaps.Find(x => x.car.name.Contains(carId.ToString()));
        if (carMap != null)
            value = carMap.healthPoints;
        return value;
    }

    public float GetCarSpeed(int carId)
    {
        var value = 0f;
        var carMap = carMaps.Find(x => x.car.name.Contains(carId.ToString()));
        if (carMap != null)
            value = carMap.speed;
        return value;
    }

    public float GetCarHandling(int carId)
    {
        var value = 0f;
        var carMap = carMaps.Find(x => x.car.name.Contains(carId.ToString()));
        if (carMap != null)
            value = carMap.handling;
        return value;
    }

    public GameObject GetCardPrefab(int carId)
    {
        var value = new GameObject();
        var carMap = carMaps.Find(x => x.car.name.Contains(carId.ToString()));
        if (carMap != null)
            value = carMap.car;
        return value;
    }
}

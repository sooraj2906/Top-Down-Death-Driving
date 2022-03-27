using UnityEngine;

/// <summary>
/// This class manages the powerup drops
/// </summary>
public class CrateManager : MonoBehaviour
{
    [SerializeField] private GameObject[] pickups;

    private void OnDestroy()
    {
        if (!gameObject.scene.isLoaded) return;
        var go = Instantiate(pickups[Random.Range(0, pickups.Length - 1)], transform.position, Quaternion.identity);
        GameManager.gameManagerPInstance.IncreaseSpawnedPowerUps(go);
    }
}
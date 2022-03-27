using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// This class handles the spawning of enemies
/// </summary>
public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GameObject[] turret;
    [SerializeField] private int numberOfTurrets;

    private void Start()
    {
        GameManager.onGameStart += StartGame;
    }

    private void StartGame()
    {
        CreateTurrets();
    }

    //Spawns specified number of enemy turrets in random location in the mao
    private void CreateTurrets()
    {
        var x = GameManager.gameManagerPInstance.tileManager.GetBounds().x;
        var y = GameManager.gameManagerPInstance.tileManager.GetBounds().y;
        for (var i = 0; i < numberOfTurrets; i++)
        {
            GameManager.gameManagerPInstance.enemyTurrets.Add(Instantiate(turret[0],
                new Vector3(Random.Range(x, -x), Random.Range(y, -y), -0.1f), quaternion.identity));
            GameManager.gameManagerPInstance.ActivateEnemyTurrets();
        }
    }
}
using UnityEngine;

/// <summary>
/// This class manages the level.
/// </summary>
public class TileManager : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private GameObject crate;
    [SerializeField] private int numberOfCrates;

    private void Start()
    {
        GameManager.onGameStart += StartGame;
    }

    private void StartGame()
    {
        CreateCrates();
    }

    //Creates the powerup crates at random locations
    private void CreateCrates()
    {
        var x = GetBounds().x - 1f;
        var y = GetBounds().y - 1f;
        for (var i = 0; i < numberOfCrates; i++)
        {
            Instantiate(crate, new Vector3(Random.Range(x, -x), Random.Range(y, -y), 0f), Quaternion.identity);
        }
    }

    //Returns the ends of the background
    public Vector3 GetBounds()
    {
        return sr.bounds.extents;
    }
}
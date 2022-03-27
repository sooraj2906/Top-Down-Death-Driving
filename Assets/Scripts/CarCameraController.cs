using UnityEngine;

/// <summary>
/// This handles the following of the camera with the player
/// </summary>
///
/// TODO: Add a zoom effect with reference to the speed the player so that the camera looks more engaging
public class CarCameraController : MonoBehaviour
{
    public Transform player = null;
    private float cameraHeight = 10.0f;
    public Transform cam = null;


    public void Start()
    {
        GameManager.onGameStart += OnGameStarted;
    }

    //Gets the player and camera transform at start
    private void OnGameStarted()
    {
        player = GameManager.gameManagerPInstance.carController.transform;
        if (Camera.main != null) cam = Camera.main.transform;
    }

    //Updates the camera position to the player position with the height of the camera set to -10f
    public void Update()
    {
        if (!GameManager.gameManagerPInstance.isPlaying) return;
        Vector3 pos = player.position;
        pos.z = -cameraHeight;
        cam.position = pos;
    }

    private void OnDestroy()
    {
        GameManager.onGameStart -= OnGameStarted;
    }
}
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles all the UI functions of the game
/// </summary>
public class UiManager : MonoBehaviour
{
    [SerializeField] private Slider playerHp;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject gameWinScreen;
    [SerializeField] private TMP_Text score;
    [SerializeField] private TMP_Text timer;

    private void Start()
    {
        GameManager.onGameStart += StartGame;
        GameManager.onPlayerHpChanged += UpdatePlayerHp;
        GameManager.onGameOver += OnGameOver;
        GameManager.onGameWin += OnGameWin;
    }

    //Updates the player hp slider on the UI
    private void UpdatePlayerHp(float health)
    {
        playerHp.value = health;
    }

    private void StartGame()
    {
        ConstructUi();
    }

    //Sets the max value and the value of the health slider at the beginning of the game
    private void ConstructUi()
    {
        playerHp.maxValue = GameManager.gameManagerPInstance.carController.maxHealthPoints;
        playerHp.value = GameManager.gameManagerPInstance.carController.maxHealthPoints;
    }

    //Shows the game over screen
    private void OnGameOver()
    {
        gameOverScreen.SetActive(true);
    }

    //Shows the game win screen with the score(number of turrets destroyed)
    private void OnGameWin()
    {
        score.text = GameManager.gameManagerPInstance.GetTurretDestroyed().ToString();
        gameWinScreen.SetActive(true);
    }

    //updates the timer in the ui with the given value
    public void UpdateTimer(string timerValue)
    {
        timer.text = timerValue;
    }

    private void OnDestroy()
    {
        GameManager.onGameStart -= StartGame;
        GameManager.onPlayerHpChanged -= UpdatePlayerHp;
        GameManager.onGameOver -= OnGameOver;
    }
}
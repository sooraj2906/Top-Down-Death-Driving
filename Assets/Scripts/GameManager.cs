using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// GameManager class manages the main flow of the game and contains all the public reference.
/// Its a singelton so that we can access it to access the other classes
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager gameManagerPInstance;

    public static Action onGameStart;
    public static Action<float> onPlayerHpChanged;
    public static Action onGameOver;
    public static Action onGameWin;
    public static Action onTurretDestroyed;

    [Header("Public References")] 
    public CarData carData;
    public GunData gunData;
    public TileManager tileManager;
    public CarController carController;
    public UiManager uiManager;
    public JoyStickHandler joystick;
    public List<WeaponController> playerWeapons = new List<WeaponController>();
    [SerializeField] private List<GameObject> spawnedPowerUps = new List<GameObject>();

    public List<GameObject> enemyTurrets;
    public bool isPlaying;
    private int turretDestroyed;

    private CarMap _selectedCarMap;
    private GunMap _selectedGunMap;

    public float timer = 30f;

    //Initializes the singleton on Awake()
    private void Awake()
    {
        if (gameManagerPInstance != null) return;
        gameManagerPInstance = this;
    }

    //Subscribe to events
    private void Start()
    {
        onTurretDestroyed += OnTurretDestroyed;
    }

    //This method helps to select a car from the car selection screen with the help of the CarData scriptable object
    public void SelectCar(int carId)
    {
        _selectedCarMap = carData.carMaps.Find(x => x.car.name.Contains(carId.ToString()));
        var car = Instantiate(_selectedCarMap.car, Vector3.zero, Quaternion.identity);
        carController = car.GetComponent<CarController>();
        carController.maxHealthPoints = carData.GetCarHp(carId);
        carController.maxSpeed = carData.GetCarSpeed(carId) / 10f;
        carController.accelerationControl = carData.GetCarHandling(carId) / 10f;
        carController.steeringControl = carData.GetCarHandling(carId) / 10f;
    }

    //This method helps to select a gun from the weapon selection screen with the help of the GunData scriptable object
    public void SelectGun(int gunId)
    {
        _selectedGunMap = gunData.gunMaps.Find(x => x.gun.name.Contains(gunId.ToString()));
        var weaponSpawns = carController.GetWeaponSpawns();
        foreach (var gun in weaponSpawns.Select(weaponSpawn =>
            Instantiate(_selectedGunMap.gun, weaponSpawn.position, weaponSpawn.rotation, weaponSpawn.transform)))
        {
            var weaponController = gun.GetComponent<WeaponController>();
            gun.GetComponent<EnemyTurretController>().enabled = false;
            playerWeapons.Add(weaponController);
            carController.weapons.Add(weaponController);
        }

        SetGunStats(gunId);
    }

    //Sets the stats of the weapon selected from the selected from the weapons screen with the data from the GunMap of the selected weapon
    private void SetGunStats(int gunId)
    {
        foreach (var playerWeapon in playerWeapons)
        {
            playerWeapon.maxAmmo = gunData.GetGunMaxAmmo(gunId);
            playerWeapon.damage = gunData.GetGunDamage(gunId);
            playerWeapon.critRate = gunData.GetGunCritRate(gunId) / 10;
            playerWeapon.rateOfFire = gunData.GetGunFireRate(gunId) / 10;
            playerWeapon.isPlayer = true;
            playerWeapon.canFire = true;
        }
        //Invokes the onGameStart so that other classes can listen to it and initialize
        onGameStart?.Invoke();
        isPlaying = true;
        StartTimer();
    }

    //Activates the enemy turret after instantiated
    public void ActivateEnemyTurrets()
    {
        foreach (var turret in enemyTurrets)
        {
            turret.GetComponentInChildren<PolygonCollider2D>().gameObject.tag = "Turret";
            turret.GetComponent<WeaponController>().enabled = false;
            turret.GetComponent<EnemyTurretController>().enabled = true;
        }
    }

    //Used to set the timer value from when the player selects in the menu screen
    public void OnTimerValueChanged(TMP_Dropdown value)
    {
        timer = value.value switch
        {
            0 => 30f,
            1 => 60f,
            2 => 0f,
            _ => 30f
        };
    }

    //Starts the timer when the game starts
    private void StartTimer()
    {
        if (timer != 0)
            StartCoroutine(Countdown(timer));
    }

    //Countdown timer coroutine
    private IEnumerator Countdown(float seconds)
    {
        var counter = seconds;
        while (counter > 0)
        {
            yield return new WaitForSeconds(1);
            timer = counter--;
            uiManager.UpdateTimer(timer.ToString());
        }

        onGameWin?.Invoke();
    }

    //Called when a turret is destroyed. Check if there are any turrets left to invoke the onGameWin
    private void OnTurretDestroyed()
    {
        turretDestroyed++;
        if (enemyTurrets.Count <= 0)
        {
            onGameWin?.Invoke();
        }
    }

    //Gets the number of turrets destroyed
    public int GetTurretDestroyed()
    {
        return turretDestroyed;
    }

    //Restarts the game
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //Adds the spawned powerups to a list for reference
    public void IncreaseSpawnedPowerUps(GameObject obj)
    {
        spawnedPowerUps.Add(obj);
    }

    //used to set the ammo of the weapon
    public void SetAmmo(float ammo)
    {
        foreach (var weapon in playerWeapons)
        {
            weapon.maxAmmo += ammo;
        }
    }

    //Returns the 1st player weapon in the list
    public WeaponController GetPlayerWeapon()
    {
        return playerWeapons[0];
    }

    //Unsibscribe to events and destroy spawned gameobjects
    private void OnDestroy()
    {
        foreach (var powerUp in spawnedPowerUps)
        {
            Destroy(powerUp);
        }

        spawnedPowerUps.Clear();
        onTurretDestroyed -= OnTurretDestroyed;
    }
}
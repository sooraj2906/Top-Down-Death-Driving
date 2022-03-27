using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Main Car Class. Handles the movement of the car and handles all the collision with obstacles and powerups
/// </summary>
public class CarController : MonoBehaviour
{
    public float maxHealthPoints;
    public float maxSpeed;
    public float accelerationControl;
    public float steeringControl;
    [SerializeField] private Camera carCamera;
    [SerializeField] private Transform[] weaponSpawns;
    public List<WeaponController> weapons;

    private float _driftFactor = 0.7f;
    private Rigidbody2D _carRigidBody;
    private float _accelerationInput;
    private float _steeringInput;
    private float _rotationAngle;

    private void Awake()
    {
        _carRigidBody = GetComponent<Rigidbody2D>();
        carCamera = Camera.main;
    }

    private void Update()
    {
        if (GameManager.gameManagerPInstance.isPlaying)
            GetInput();
    }

    //We use fixed update for our controls as its called at a fixedrate and not on every frame
    private void FixedUpdate()
    {
        if (!GameManager.gameManagerPInstance.isPlaying) return;
        CarAcceleration();
        CarSteering();
        Drift();
    }

    //Gets the steering input from the on screen joystick 
    private void GetInput()
    {
        _steeringInput = GameManager.gameManagerPInstance.joystick.GetDirectionVector().x;
    }

    //Used to set the acceleration value
    public void SetAccelerationInput(float value)
    {
        _accelerationInput = value;
    }

    //Returns velocity of the car with respect to the transform.up
    private float GetVelocityWrtUp()
    {
        return Vector2.Dot(_carRigidBody.velocity, transform.up);
    }

    //handles the acceleration of the car(forward and backward movement)
    private void CarAcceleration()
    {
        if (GetVelocityWrtUp() > maxSpeed && _accelerationInput > 0) return;

        if (_accelerationInput < 0 && GetVelocityWrtUp() < -maxSpeed * 0.5f)
        {
            return;
        }

        var drag = _carRigidBody.drag;
        if (_accelerationInput == 0)
        {
            drag = Mathf.Lerp(drag, 3f, Time.deltaTime * 3);
        }
        else
        {
            drag = 0;
        }

        _carRigidBody.drag = drag;

        var forceVector = transform.up * (_accelerationInput * accelerationControl);

        _carRigidBody.AddForce(forceVector, ForceMode2D.Force);
    }

    //Handles the steering of the car
    private void CarSteering()
    {
        var minSpeedForSteering = _carRigidBody.velocity.magnitude / 10f;
        minSpeedForSteering = Mathf.Clamp01(minSpeedForSteering);
        if (GetVelocityWrtUp() >= 0)
        {
            _rotationAngle -= _steeringInput * steeringControl * minSpeedForSteering;
        }
        else
        {
            _rotationAngle += _steeringInput * steeringControl * minSpeedForSteering;
        }

        _carRigidBody.MoveRotation(_rotationAngle);
    }

    //Handles the drift of the car. Without handling drift the car appeared to turn like a spaceship
    private void Drift()
    {
        var movementVelocity = transform.up * Vector2.Dot(transform.up, _carRigidBody.velocity);
        var turnVelocity = transform.right * Vector2.Dot(transform.right, _carRigidBody.velocity);

        _carRigidBody.velocity = movementVelocity + turnVelocity * _driftFactor;
    }

    //Handles collision with obstacles, turrets and powerups 
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("Crate"))
        {
            Destroy(other.gameObject);
        }

        if (other.transform.CompareTag("Turret"))
        {
            ReduceHealth(5f);
        }

        if (other.transform.CompareTag("SmallAmmo"))
        {
            GameManager.gameManagerPInstance.SetAmmo(20f);
            Destroy(other.gameObject);
        }

        if (other.transform.CompareTag("LargeAmmo"))
        {
            GameManager.gameManagerPInstance.SetAmmo(40f);
            Destroy(other.gameObject);
        }

        if (other.transform.CompareTag("SmallHealth"))
        {
            SetHealth(25f);
            Destroy(other.gameObject);
        }

        if (other.transform.CompareTag("LargeHealth"))
        {
            SetHealth(50f);
            Destroy(other.gameObject);
        }
    }
    
    //Reduces the health of the car based to the passed damage value
    public void ReduceHealth(float damage)
    {
        if (maxHealthPoints > damage)
        {
            maxHealthPoints -= damage;
            GameManager.onPlayerHpChanged?.Invoke(maxHealthPoints);
        }
        else
        {
            GameManager.onGameOver?.Invoke();
        }
    }

    //Returns the player's current position
    public Vector3 GetPlayerPos()
    {
        return transform.position;
    }

    //Returns the list of weapons attached to the car
    public List<Transform> GetWeaponSpawns()
    {
        return weaponSpawns.ToList();
    }

    //helper method to set the health of the car
    public void SetHealth(float health)
    {
        maxHealthPoints += health;
    }
}
using System;
using UnityEngine;

public enum GearState
{
    DRIVE, 
    REVERSE
}

[RequireComponent(typeof(Rigidbody2D))]
public class CarController : MonoBehaviour
{
    [SerializeField] private bool disableInputs = false;       // to disble input
    
    [SerializeField] private float acceleration =  2f;               // acceleration  
    [SerializeField] private float steeringLimiter = 700f;           // it will limit steering rotation    

    [SerializeField] private bool b_Acce = false;                    // true if accelerator pressed
    [SerializeField] private bool b_Brake = false;                   // true if brake pressed   
    [SerializeField] private bool b_Left = false;                    // true if steering moving left
    [SerializeField] private bool b_Right = false;                   // true if steering moving right


    private Rigidbody2D _rb;                          // contains rigidbody 2d component

    private GearState _currentGear;                   // contains Current Gear state 

    private float _turnDirection = 0;                 //right= 1 left = -1 
    private float _input = 0;                         //forward = 1 backward = -1
    private float _steeringAngle = 0;                 // contains angle of steering UI 

    private bool _stopAcceleration = false;           // true if any button  is not pressed

    public float SteeringAngle 
    {
        get { return _steeringAngle; }
        set { _steeringAngle = value; }
    }                                // getter setter of _steering Angle
    public GearState CurrentGear
    {
        get { return _currentGear; }
    }               // getter of _currentGear


    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _currentGear = GearState.DRIVE;
    }
    private void FixedUpdate()
    {
        if (disableInputs)
        {
            _rb.Sleep();
            return;
        }
		CarMovementHandler();
    }

    private void CarMovementHandler()
    {
        //Applying Movememts to the car
        if(b_Acce)
        {
            if(_currentGear == GearState.DRIVE)
                _input = Mathf.MoveTowards(_input, 1, Time.deltaTime * acceleration);
            else if(_currentGear == GearState.REVERSE)
                _input = Mathf.MoveTowards(_input, -1, Time.deltaTime * acceleration);

            _stopAcceleration = false;
        }
        else if(b_Brake)
        {
            _input = Mathf.MoveTowards(_input, 0, Time.deltaTime * acceleration * 2);  // apply break 2 times faster than acceleration
            _stopAcceleration = true;
        }
        else
        {
            _input = Mathf.MoveTowards(_input, 0, Time.deltaTime * acceleration / 2);  // slow down speed 2 time slower than acceleration
            _stopAcceleration = true;
        }

        //applying rotation to the car
        if (b_Right)
        {
            _turnDirection = Mathf.MoveTowards(_turnDirection, 1, Time.deltaTime * acceleration);
        }
        else if (b_Left)
        {
            _turnDirection = Mathf.MoveTowards(_turnDirection, -1, Time.deltaTime * acceleration);
        }
        else
        {
            _turnDirection = Mathf.MoveTowards(_turnDirection, 0, Time.deltaTime * acceleration);
        }
            
        ApplyRotationToTheCar();
        ApplyForceToTheCar();
    }

    public void ApplyForceToTheCar()
    {
        if (!_stopAcceleration)
            _rb.AddForce(_input * acceleration * transform.right);
        else
        {
            _rb.velocity = Vector2.MoveTowards(_rb.velocity, Vector2.zero, Time.deltaTime * acceleration);
        }
    }

    private void ApplyRotationToTheCar()
    {
        if (_turnDirection > 0f)
            TurnRight();
        else if (_turnDirection < 0f)
            TurnLeft();
    }

    private void TurnLeft()
    {
        if(CurrentGear == GearState.DRIVE)
            _rb.rotation += _turnDirection * _steeringAngle * _rb.velocity.magnitude / steeringLimiter;
        if(CurrentGear == GearState.REVERSE)
            _rb.rotation -= _turnDirection * _steeringAngle * _rb.velocity.magnitude / steeringLimiter;
    }

    private void TurnRight()
    {
        if(CurrentGear ==  GearState.DRIVE)
            _rb.rotation -= _turnDirection * _steeringAngle * _rb.velocity.magnitude / steeringLimiter;
        if (CurrentGear == GearState.REVERSE)
            _rb.rotation += _turnDirection * _steeringAngle * _rb.velocity.magnitude / steeringLimiter;
    }

	#region ChangeGearMethods
	public void ChangeGearToDrive()
    {
        _currentGear = GearState.DRIVE;
    }
    public void ChangeGearToReverse()
    {
        _currentGear = GearState.REVERSE;
    }
	#endregion

	#region BooleanMethods
	public void AccelerationActivate()
    {
        b_Acce = true;
        b_Brake = false;
    }
    public void AcceleartionDeactivate()
    {
        b_Acce = false;
        b_Brake = false;
    }

    public void BreakActivate()
    {
        b_Acce = false;
        b_Brake = true;
    }
    public void BreakDeactivate()
    {
        b_Acce = false;
        b_Brake = false;
    }

    public void LeftActivate()
    {
        b_Left = true;
        b_Right = false;
    }
    public void LeftDeactivate()
    {
        b_Left = false;
    }

    public void RightActivate()
    {
        b_Right = true;
        b_Left = false;
    }
    public void RightDeactivate()
    {
        b_Right = false;
    }
	#endregion

	private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Walls"))
        {
            disableInputs = true;
            UIManager.Instance.EnableCarCrashedPanel();
        }
        if (collision.gameObject.CompareTag("OtherCars"))
        {
            disableInputs = true;
            UIManager.Instance.EnableCarCrashedPanel();
        }   
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Walls"))
            b_Acce = false;
        if (collision.gameObject.CompareTag("OtherCars"))
            b_Acce = false;
    }
}

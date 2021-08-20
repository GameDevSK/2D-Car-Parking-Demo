using UnityEngine;
using UnityEngine.EventSystems;

public class SteeringController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public CarController carController;

    [SerializeField] private float minAngle;             // minimum angle for steering to rotate
    [SerializeField] private float maxAngle;             // maximum angle for steering to rotate

    [SerializeField] private float releaseSpeed;         //  if steering is not pressed, release speed will apply to steering 

    private RectTransform _steeringRectTransform;        // Rect Tranform of steering
    private Vector2 _center;                             // cneter of steering

    private float _currentWheelAngle;                   // current angle of steering
    private float _lastSteeringAngle;                   // last angle of steering 

    private bool _isBeingHeld = false;                  // true if steering is being rotated

    private void Awake()
    {
        _steeringRectTransform = GetComponent<RectTransform>();
    }
    private void Update()
    {
        _steeringRectTransform.localEulerAngles = new Vector3(0f, 0f, -_currentWheelAngle);       // apply rotation to the steering
        DecideTheTurnDirection();

        if (_isBeingHeld) return;
        SetSteeringToNormalPosition();
    }

    private void DecideTheTurnDirection()
    {
        carController.SteeringAngle = _currentWheelAngle;

        if (!_isBeingHeld)
        {
            carController.RightDeactivate();
            carController.LeftDeactivate();   
            return;
        }

        if (_currentWheelAngle == 0)
        {
            carController.LeftDeactivate();
            carController.RightDeactivate();
        }
        else if(_currentWheelAngle > 0)
        {
            carController.RightActivate();
        }
        else
        {
            carController.LeftActivate();
        }
    }

    private void SetSteeringToNormalPosition()
    {
        if (_currentWheelAngle != 0f)
        {
            float _deltaAngle = releaseSpeed * Time.deltaTime;
            if (Mathf.Abs(_deltaAngle) > Mathf.Abs(_currentWheelAngle))
                _currentWheelAngle = 0f;
            else if (_currentWheelAngle > 0f)
                _currentWheelAngle -= _deltaAngle;
            else
                _currentWheelAngle += _deltaAngle;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isBeingHeld = true;

        _center = RectTransformUtility.WorldToScreenPoint(eventData.pressEventCamera, _steeringRectTransform.position);
        _lastSteeringAngle = Vector2.Angle(Vector2.up, eventData.position - _center);
    }

    public void OnDrag(PointerEventData eventData)
    {
        float _newAngle = Vector2.Angle(Vector2.up, eventData.position - _center);

        if (eventData.position.x > _center.x)
            _currentWheelAngle += _newAngle - _lastSteeringAngle;
        else
            _currentWheelAngle -= _newAngle - _lastSteeringAngle;
        
        _currentWheelAngle = Mathf.Clamp(_currentWheelAngle, minAngle, maxAngle);
        _lastSteeringAngle = _newAngle;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnDrag(eventData);

        _isBeingHeld = false;
    }
}

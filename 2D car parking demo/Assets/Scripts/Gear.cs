using UnityEngine;
using UnityEngine.UI;

public class Gear : MonoBehaviour
{
    [SerializeField] private CarController carController;

    [SerializeField] private Sprite driveSprite;
    [SerializeField] private Sprite reverseSprite;
    
    private GearState _currentGear;

    private Image _gearImage;
    private void Start()
    {
        _gearImage = GetComponent<Image>();
        if (_gearImage)
            _gearImage.sprite = driveSprite;

        _currentGear = carController.CurrentGear;
    }

    public void UpdateGearUI()
    {
        if (_currentGear == GearState.DRIVE)
        {
            _gearImage.sprite = reverseSprite;
            carController.ChangeGearToReverse();
        }
        if(_currentGear == GearState.REVERSE)
        {
            _gearImage.sprite = driveSprite;
            carController.ChangeGearToDrive();
        }
        _currentGear = carController.CurrentGear;
    }
}

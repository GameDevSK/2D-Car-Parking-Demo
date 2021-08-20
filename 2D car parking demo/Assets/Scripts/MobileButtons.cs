using UnityEngine;
using UnityEngine.EventSystems;

public class MobileButtons : MonoBehaviour
{
	public CarController carController;

    [SerializeField] private bool btn_Accelerate;            // true for accelerator
    [SerializeField] private bool btn_Brake;                 // true for brake

    private void Start()
    {
		EventTrigger trigger = GetComponent<EventTrigger>();
		EventTrigger.Entry entry = new EventTrigger.Entry();                                                    // Create event Pointer Down
		entry.eventID = EventTriggerType.PointerDown;
		entry.callback.AddListener((data) => { OnPointerDownDelegate((PointerEventData)data); });
		trigger.triggers.Add(entry);
		EventTrigger.Entry entry2 = new EventTrigger.Entry();                                                   // Create event Pointer Up
		entry2.eventID = EventTriggerType.PointerUp;
		entry2.callback.AddListener((data) => { OnPointerUpDelegate((PointerEventData)data); });
		trigger.triggers.Add(entry2);
	}
	private void OnPointerDownDelegate(PointerEventData data)
	{
		if (btn_Accelerate)
		{
			carController.BreakDeactivate();
			carController.AccelerationActivate();

		}
		if (btn_Brake)
		{
			carController.AcceleartionDeactivate();
			carController.BreakActivate();
		}
	}

	private void OnPointerUpDelegate(PointerEventData data)
	{
		if (btn_Accelerate)
			carController.AcceleartionDeactivate();
		if (btn_Brake)
			carController.BreakDeactivate();
	}
}

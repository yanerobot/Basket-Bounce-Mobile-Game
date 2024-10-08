using UnityEngine;

public class ButtonActivator : Activator
{
	[SerializeField] SpriteRenderer sr;
	[SerializeField] Sprite pressed, idle;
	[SerializeField] LayerMask layerMask;

	int touches;
	bool isPressed;

	private void Awake()
	{
		touches = 0;
		isPressed = false;
	}

	private void UpdateState()
	{
		if (touches > 0 && !isPressed)
		{
			Toggle();
			if (sr != null)
				sr.sprite = pressed;
			isPressed = true;
		}
		else if (touches == 0 && isPressed)
		{
			if (sr != null)
				sr.sprite = idle;
			isPressed = false;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (layerMask.Contains(collision.gameObject.layer))
		{
			touches++;
			UpdateState();
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (layerMask.Contains(collision.gameObject.layer))
		{
			touches--;
			UpdateState();
		}
	}
}

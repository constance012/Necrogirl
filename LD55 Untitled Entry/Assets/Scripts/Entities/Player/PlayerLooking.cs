using UnityEngine;

public class PlayerLooking : MonoBehaviour
{
	[Header("Player Graphics"), Space]
	[SerializeField] private Transform graphic;

	// Private fields.
	private bool _facingRight = true;
	private float _lookAngle;

	private void Update()
	{
		LookAtMouse();
	}

	private void LookAtMouse()
	{
		Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector2 lookDirection = (mousePos - PlayerMovement.Position).normalized;

		_lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;

		CheckFlip();
	}

	private void CheckFlip()
	{		
		bool mustFlip = (_facingRight && Mathf.Abs(_lookAngle) > 90f) || (!_facingRight && Mathf.Abs(_lookAngle) <= 90f);

		if (mustFlip)
		{
			graphic.FlipByScale('x');

			_facingRight = !_facingRight;
		}
	}
}
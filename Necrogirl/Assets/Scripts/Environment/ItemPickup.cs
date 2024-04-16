using UnityEngine;

public class ItemPickup : Interactable
{
	[Header("Current Item Info"), Space]
	[Tooltip("The scriptable object represents this item.")]
	public Item itemSO;

	[Header("Fly Settings"), Space]
	[SerializeField] private Rigidbody2D rb2D;
	[SerializeField] protected float flyDistance;
	[SerializeField] private float flySpeed;
	[SerializeField] private float pickUpMinDistance;
	[SerializeField] private float pickUpFailDelay;

	public int ItemQuantity
	{
		get { return _currentItem.quantity; }
		set { _overrideQuantity = value; }
	}

	// Private fields.
	private Item _currentItem;
	private int _overrideQuantity = -1;
	private float _delay;

	private void Start()
	{
		_currentItem = Instantiate(itemSO);
		_currentItem.name = itemSO.name;

		if (_overrideQuantity != -1)
			_currentItem.quantity = _overrideQuantity;

		spriteRenderer.sprite = _currentItem.icon;		
	}

	protected override void CheckForInteraction(float mouseDistance, float playerDistance)
	{
		if (playerDistance <= flyDistance)
			FlyTowardsPlayer();

		base.CheckForInteraction(mouseDistance, playerDistance);
	}

    protected override void TriggerInteraction(float playerDistance)
    {
        base.TriggerInteraction(playerDistance);

		if (InputManager.Instance.GetKeyDown(KeybindingActions.Interact) && _delay > 0f)
			TryPickup(true);
    }

    private void FlyTowardsPlayer()
	{
		_delay -= Time.deltaTime;

		if (_delay <= 0f)
		{
			Vector2 flyDirection = (player.position - transform.position).normalized;
			rb2D.velocity = flyDirection * flySpeed;

			if (Vector3.Distance(transform.position, player.position) <= pickUpMinDistance)
				TryPickup();
		}
	}

	protected override void CreatePopupLabel()
	{
		Transform foundLabel = worldCanvas.transform.Find("Popup Label");

		string itemName = _currentItem.itemName;
		int quantity = _currentItem.quantity;
		Color textColor = _currentItem.rarity.color;

		// Create a clone if not already exists.
		if (foundLabel == null)
		{
			base.CreatePopupLabel();
			clone.SetObjectName(itemName, quantity, textColor);
		}

		// Otherwise, append to the existing one.
		else
		{
			clone = foundLabel.GetComponent<InteractionPopupLabel>();

			clone.SetObjectName(itemName, quantity, textColor, true);
			clone.RestartAnimation();
		}
	}

	private void TryPickup(bool forced = false)
	{
		Debug.Log("You're picking up a(n) " + _currentItem.itemName);

		if (ItemsManager.Instance.AddItem(_currentItem, forced))
		{
			Destroy(clone.gameObject);
			Destroy(gameObject);
		}
		else
		{
			_delay = pickUpFailDelay;
		}
	}

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere(transform.position, flyDistance);
    }
}

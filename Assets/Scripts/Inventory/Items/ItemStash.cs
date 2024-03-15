using UnityEngine;

public class ItemStash : ItemContainer
{
	[SerializeField] GameObject storage;
	//[SerializeField] SpriteRenderer spriteRenderer;
	[SerializeField] KeyCode openKeyCode = KeyCode.U;

	private bool isOpen;
	private bool isInRange;

	private Character character;

	protected override void OnValidate()
	{

		//if (itemsParent != null)
		//	itemsParent.GetComponentsInChildren(includeInactive: true, result: ItemSlots);

		//if (spriteRenderer == null)
		//	spriteRenderer = GetComponentInChildren<SpriteRenderer>(includeInactive: true);

		//spriteRenderer.enabled = false;
	}

	protected override void Awake()
	{
		base.Awake();
		//itemsParent.gameObject.SetActive(false);
	}

	private void Update()
	{
		if (isInRange && Input.GetKeyDown(openKeyCode))
		{
			isOpen = !isOpen;
			storage.SetActive(isOpen);

			if (isOpen)
				character.OpenItemContainer(this);
			else
				character.CloseItemContainer(this);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		CheckCollision(other.gameObject, true);
		GameObject player = GameObject.FindWithTag("Hero");
		storage = GameManager.instance.Storage;
	}

	private void OnTriggerExit(Collider other)
	{
		CheckCollision(other.gameObject, false);
	}

	private void CheckCollision(GameObject gameObject, bool state)
	{
		if (gameObject.CompareTag("Player"))
		{
			isInRange = state;
			//spriteRenderer.enabled = state;

			if (!isInRange && isOpen)
			{
				isOpen = false;
				storage.SetActive(false);
				character.CloseItemContainer(this);
			}

			if (isInRange)
				character = gameObject.GetComponent<Character>();
			else
				character = null;
		}
	}
}

using System.Collections.Generic;
using UnityEngine;

public class Destructibles : MonoBehaviour
{
	[Header("References"), Space]
	public List<GameObject> listPotPieces;
	public GameObject hpBottle, manaBottle, Coin;

	[Header("Drop Settings"), Space]
	[SerializeField] private Vector2Int coinCount;
	[SerializeField, Range(0f, 1f)] private float manaDropChance;
	[SerializeField, Range(0f, 1f)] private float healthDropChance;

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.layer == LayerMask.NameToLayer("Our Projectile"))
		{
			if(Random.value < healthDropChance)
			{
				GameObject healthPotion = Instantiate(hpBottle, transform.position + (Vector3)Random.insideUnitCircle, Quaternion.identity);
				healthPotion.name = hpBottle.name;
			}

			if (Random.value < manaDropChance)
			{
				GameObject manaPotion = Instantiate(manaBottle, transform.position + (Vector3)Random.insideUnitCircle, Quaternion.identity);
				manaPotion.name = manaBottle.name;
			}
			
			int coinQuantity = Random.Range(coinCount.x, coinCount.y);
			GameObject coins = Instantiate(Coin, transform.position + (Vector3)Random.insideUnitCircle, Quaternion.identity);
			coins.name = Coin.name;
			coins.GetComponent<ItemPickup>().ItemQuantity = coinQuantity;

			for(int i = 0; i < 10; i++)
			{
				GameObject piece = listPotPieces[Random.Range(0, listPotPieces.Count)];
				Instantiate(piece, transform.position, Quaternion.identity);
			}
			
			Destroy(gameObject);
		}
	}
}

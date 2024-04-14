using System.Collections.Generic;
using UnityEngine;

public class Destructibles : MonoBehaviour
{
	[Header("References"), Space]
	public List<GameObject> listPotPieces;
	public GameObject hpBottle, manaBottle, Coin;

	[Header("Drop Settings"), Space]
	public Vector2Int coinCount;

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.layer == LayerMask.NameToLayer("Our Projectile"))
		{
			int randomHP = Random.Range(0, 101);
			if(randomHP >= 0 && randomHP <= 50)
			{
				GameObject healthPotion = Instantiate(hpBottle, transform.position + (Vector3)Random.insideUnitCircle, Quaternion.identity);
				healthPotion.name = hpBottle.name;
			}
			int randomMana = Random.Range(0, 101);
			if (randomMana >= 0 && randomMana <= 50)
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

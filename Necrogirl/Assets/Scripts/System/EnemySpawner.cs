using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform container;
    public List<GameObject> enemies = new List<GameObject>();
    public Vector2Int spawnCount;
    public float range;

    private void Start()
    {
        int count = Random.Range(spawnCount.x, spawnCount.y);
        for (int i = 0; i < count; i++)
        {
            float x = Random.Range(transform.position.x - range, transform.position.x + range);
            float y = Random.Range(transform.position.y - range, transform.position.y + range);

            GameObject prefab = enemies[Random.Range(0, enemies.Count)];
            GameObject enemy = Instantiate(prefab, new Vector2(x, y), Quaternion.identity);
            enemy.name = prefab.name;
            enemy.transform.SetParent(container);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
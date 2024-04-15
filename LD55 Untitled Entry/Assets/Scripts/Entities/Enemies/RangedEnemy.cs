using UnityEngine;

public class RangedEnemy : EnemyStats
{
    [Header("Projectile Prefab"), Space]
	[SerializeField] private GameObject projectilePrefab;

    private void Update()
    {
        _attackInterval -= Time.deltaTime;

        if (_attackInterval <= 0f && !PlayerStats.IsDeath)
        {
            Transform currentTarget = (brain as EntityAI).target;

            if (currentTarget != null && Vector3.Distance(currentTarget.position, transform.position) <= attackRange.x)
            {
                Vector2 direction = (currentTarget.position - transform.position).normalized;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                projectile.name = projectilePrefab.name;
                projectile.transform.eulerAngles = Vector3.forward * angle;
                projectile.GetComponent<SimpleProjectile>().Initialize(this.stats, currentTarget);

                _attackInterval = AttackInterval;
            }
        }
    }
}
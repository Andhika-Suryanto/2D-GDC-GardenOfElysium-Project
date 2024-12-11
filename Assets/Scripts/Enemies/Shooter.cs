using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour, IEnemy
{
    public enum EnemyType {
        Ghost,   // Tidak bisa summon
        Urn      // Bisa summon
    }

    [Header("Basic Shooting Settings")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletMoveSpeed;
    [SerializeField] private int burstCount;
    [SerializeField] private int projectilesPerBurst;
    [SerializeField][Range(0, 359)] private float angleSpread;
    [SerializeField] private float startingDistance = 0.1f;
    [SerializeField] private float timeBetweenBursts;
    [SerializeField] private float restTime = 1f;
    [SerializeField] private bool stagger;
    [SerializeField] private bool oscillate;

    [Header("Enemy Type Settings")]
    [SerializeField] private EnemyType currentEnemyType;

    [Header("Summon Settings")]
    [SerializeField] private GameObject slimePrefab; // Prefab untuk summon (hanya untuk Urn)
    [SerializeField] private int maxSlimeCount = 3;
    [SerializeField] private float summonRadius = 5f;

    private bool isShooting = false;
    private List<GameObject> spawnedSlimes = new List<GameObject>();

    private void OnValidate() {
        // Existing validation checks
        if (projectilesPerBurst < 1) { projectilesPerBurst = 1; }
        if (burstCount < 1) { burstCount = 1; }
        if (timeBetweenBursts < 0.1f) { timeBetweenBursts = 0.1f; }
        if (restTime < 0.1f) { restTime = 0.1f; }
        if (startingDistance < 0.1f) { startingDistance = 0.1f; }
        if (angleSpread == 0) { projectilesPerBurst = 1; }
        if (bulletMoveSpeed <= 0) { bulletMoveSpeed = 0.1f; }
    }

    public void Attack() {
        if (!isShooting) {
            StartCoroutine(AttackRoutine());
        }
    }

    private IEnumerator AttackRoutine()
    {
        // Shooting routine
        yield return StartCoroutine(ShootRoutine());

        // Summon untuk Urn type
        if (currentEnemyType == EnemyType.Urn)
        {
            yield return new WaitForSeconds(1f);
            SummonSlimes();
        }
    }

    private void SummonSlimes()
    {
        // Hapus slime yang sudah mati dari list
        spawnedSlimes.RemoveAll(slime => slime == null);

        // Cek apakah jumlah slime sudah mencapai maksimum
        if (spawnedSlimes.Count >= maxSlimeCount)
        {
            return;
        }

        // Spawn slime di sekitar boss
        for (int i = 0; i < maxSlimeCount - spawnedSlimes.Count; i++)
        {
            Vector2 randomPosition = (Vector2)transform.position + Random.insideUnitCircle * summonRadius;
            
            // Raycast untuk memastikan spawn di area yang valid
            RaycastHit2D hit = Physics2D.Raycast(randomPosition, Vector2.down, 1f);
            if (hit.collider != null) // Pastikan ada ground
            {
                GameObject newSlime = Instantiate(slimePrefab, randomPosition, Quaternion.identity);
                spawnedSlimes.Add(newSlime);
            }
        }
    }

    private IEnumerator ShootRoutine()
    {
        isShooting = true;

        float startAngle, currentAngle, angleStep, endAngle;
        float timeBetweenProjectiles = 0f;

        TargetConeOfInfluence(out startAngle, out currentAngle, out angleStep, out endAngle);

        if (stagger) { timeBetweenProjectiles = timeBetweenBursts / projectilesPerBurst; }

        for (int i = 0; i < burstCount; i++)
        {
            if (!oscillate) {
                TargetConeOfInfluence(out startAngle, out currentAngle, out angleStep, out endAngle);
            } 
            
            if (oscillate && i % 2 != 1) {
                TargetConeOfInfluence(out startAngle, out currentAngle, out angleStep, out endAngle);
            } else if (oscillate) {
                currentAngle = endAngle;
                endAngle = startAngle;
                startAngle = currentAngle;
                angleStep *= -1;
            }
           
            for (int j = 0; j < projectilesPerBurst; j++)
            {
                Vector2 pos = FindBulletSpawnPos(currentAngle);

                GameObject newBullet = Instantiate(bulletPrefab, pos, Quaternion.identity);
                newBullet.transform.right = newBullet.transform.position - transform.position;

                if (newBullet.TryGetComponent(out Projectile projectile))
                {
                    projectile.UpdateMoveSpeed(bulletMoveSpeed);
                }

                currentAngle += angleStep;

                if (stagger) { yield return new WaitForSeconds(timeBetweenProjectiles); }
            }

            currentAngle = startAngle;

            if (!stagger) { yield return new WaitForSeconds(timeBetweenBursts); }
        }

        yield return new WaitForSeconds(restTime);
        isShooting = false;
    }

    private void TargetConeOfInfluence(out float startAngle, out float currentAngle, out float angleStep, out float endAngle)
    {
        Vector2 targetDirection = PlayerController.Instance.transform.position - transform.position;
        float targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        startAngle = targetAngle;
        endAngle = targetAngle;
        currentAngle = targetAngle;
        float halfAngleSpread = 0f;
        angleStep = 0;
        if (angleSpread != 0)
        {
            angleStep = angleSpread / (projectilesPerBurst - 1);
            halfAngleSpread = angleSpread / 2f;
            startAngle = targetAngle - halfAngleSpread;
            endAngle = targetAngle + halfAngleSpread;
            currentAngle = startAngle;
        }
    }

    private Vector2 FindBulletSpawnPos(float currentAngle) {
        float x = transform.position.x + startingDistance * Mathf.Cos(currentAngle * Mathf.Deg2Rad);
        float y = transform.position.y + startingDistance * Mathf.Sin(currentAngle * Mathf.Deg2Rad);

        Vector2 pos = new Vector2(x, y);

        return pos;
        
    }
    private void OnDestroy()
    {
        // Hapus semua slime yang di-spawn (hanya untuk Urn)
        if (currentEnemyType == EnemyType.Urn)
        {
            foreach (GameObject slime in spawnedSlimes)
            {
                if (slime != null)
                {
                    Destroy(slime);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Hanya gambar radius summon untuk Urn
        if (currentEnemyType == EnemyType.Urn)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, summonRadius);
        }
    }
}


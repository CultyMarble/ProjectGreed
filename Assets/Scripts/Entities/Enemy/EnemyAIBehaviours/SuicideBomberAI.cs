using UnityEngine;

public class SuicideBomberAI : MonoBehaviour
{
    [SerializeField] private float distanceThreshold = default;
    [SerializeField] private int damage = default;

    [Header("Extra")]
    [SerializeField] private bool spawnProjectile = default;
    [SerializeField] private int numberOfProjectile = default;
    [SerializeField] private float projectileSpeed = default;

    private Transform enemyProjectilePool = default;
    private Vector2 bulletDirection = default;

    //======================================================================
    private void Start()
    {
        enemyProjectilePool = GameObject.Find("EnemyProjectilePool").transform;
    }

    private void OnEnable()
    {
        GetComponent<EnemyHealth>().OnDespawnEvent += SuicideBomberAI_OnDespawnEvent;
    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, Player.Instance.transform.position) <= distanceThreshold)
        {
            Debug.Log("Explode!!!");
            Player.Instance.GetComponent<PlayerHeart>().UpdateCurrentHeart(-damage);

            GetComponent<EnemyHealth>().UpdateCurrentHealth(-999);
        }
    }

    //===========================================================================
    private void SuicideBomberAI_OnDespawnEvent(object sender, System.EventArgs e)
    {
        if (spawnProjectile == false)
            return;

        // Get player direction
        bulletDirection = (Player.Instance.transform.position - transform.position).normalized;

        for (int i = 0; i < numberOfProjectile; i++)
        {
            foreach (Transform projectile in enemyProjectilePool)
            {
                if (projectile.gameObject.activeInHierarchy == false)
                {
                    projectile.position = transform.position;

                    Vector2 _bulletDirection = new Vector2(bulletDirection.x, bulletDirection.y).normalized;

                    projectile.GetComponent<EnemyProjectile>().SetMoveDirectionAndSpeed(_bulletDirection, projectileSpeed);
                    projectile.gameObject.SetActive(true);
                    break;
                }
            }
        }
    }
}

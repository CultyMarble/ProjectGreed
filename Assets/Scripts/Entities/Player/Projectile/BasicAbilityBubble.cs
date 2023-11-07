using UnityEngine;

public class BasicAbilityBubble : MonoBehaviour
{
    private float moveSpeed;
    private float lifeTime = default;
    private float particleDamage = default;

    private float timeUntilChangeDirectionMax = default;
    private float timeUntilChangeDirectionMin = default;
    private float timeUntilChangeDirection = default;

    private float particleGrowthRate = 0;
    private float swingMagtitude = default;
    private Vector3 moveDirection = default;
    private Animator animator;

    //===========================================================================
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && collision.GetType().ToString() != Tags.CIRCLECOLLIDER2D)
        {
            collision.gameObject.GetComponent<EnemyHealth>().UpdateCurrentHealth(-particleDamage);
        }
        else if (collision.gameObject.CompareTag("Breakable"))
        {
            collision.gameObject.GetComponent<BreakableItem>().UpdateCurrentHealth(-particleDamage);
        }

        if (collision.gameObject.CompareTag("FinalBoss"))
        {
            collision.transform.parent.GetComponent<EnemyHealth>().UpdateCurrentHealth(-particleDamage);
        }

        //StopMovement();
        animator.SetTrigger("Pop");
    }

    //===========================================================================
    private void OnEnable()
    {
        timeUntilChangeDirection = Random.Range(timeUntilChangeDirectionMin, timeUntilChangeDirectionMax);
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (SceneControlManager.Instance.CurrentGameplayState == GameplayState.Pause)
            return;
        // Update Moving Direction
        timeUntilChangeDirection -= Time.deltaTime;
        if (timeUntilChangeDirection <= 0)
        {
            timeUntilChangeDirection = Random.Range(timeUntilChangeDirectionMin, timeUntilChangeDirectionMax);

            moveDirection += new Vector3(Random.Range(moveDirection.x - swingMagtitude, moveDirection.x + swingMagtitude),
                Random.Range(moveDirection.y - swingMagtitude, moveDirection.y + swingMagtitude), moveDirection.z).normalized;
        }

        // Movement Pattern
        transform.position += moveSpeed * Time.deltaTime * moveDirection;

        //Particle Growth
        if (gameObject.activeSelf)
        {
            Vector3 growthVector = new Vector3();
            growthVector.x = particleGrowthRate;
            growthVector.y = particleGrowthRate;
            //int rnd = Random.Range(1, 20);

            //if (lifeTime < 0.1 && rnd == 3)
            //{
            //    Vector3 emberVector = new Vector3(0.1f, 0.1f, 0.1f);
            //    transform.localScale = emberVector;
            //}
            //else
            //{
                transform.localScale += growthVector * Time.deltaTime;
            //}
        }

        // Partical LifeTime
        lifeTime -= Time.deltaTime;
        if(lifeTime < 0.1f)
        {

        }
        if (lifeTime <= 0)
        {
            animator.SetTrigger("Pop");
        }
    }

    private void Despawn()
    {
        gameObject.SetActive(false);
        gameObject.transform.localPosition = Vector2.zero;
    }
    private void StopMovement()
    {
        moveSpeed = 0;
        particleGrowthRate = 0;
    }

    //===========================================================================
    public void SetMovementSpeed(Vector3 dir, float newMoveSpeed, float newLifeTime)
    {
        moveSpeed = newMoveSpeed;
        lifeTime = newLifeTime;
        moveDirection = dir;
    }

    public void SetMovementPattern(float newTimeMax, float newTimeMin, float newSwingMagnitude)
    {
        timeUntilChangeDirectionMax = newTimeMax;
        timeUntilChangeDirectionMin = newTimeMin;
        swingMagtitude = newSwingMagnitude;
    }

    public void SetSizeAndGrowth(float  size, float growthRate)
    {
        Vector3 sizeVector = new Vector3();
        sizeVector.x = size;
        sizeVector.y = size;
        transform.localScale = sizeVector;
        particleGrowthRate = growthRate;
    }

    public void SetDamage(float damage)
    {
        particleDamage = damage;
    }
}
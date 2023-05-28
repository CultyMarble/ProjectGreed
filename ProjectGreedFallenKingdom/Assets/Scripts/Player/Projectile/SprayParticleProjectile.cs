using UnityEngine;

public class SprayParticleProjectile : MonoBehaviour
{
    private float moveSpeed;
    private float lifeTime = default;
    private int particleDamage = default;
    private float particlePushPower = default;

    private float timeUntilChangeDirectionMax = default;
    private float timeUntilChangeDirectionMin = default;
    private float timeUntilChangeDirection = default;

    private float particleGrowthRate = 0;
    private float swingMagtitude = default;
    private Vector3 moveDirection = default;

    //===========================================================================
    private void OnEnable()
    {
        timeUntilChangeDirection = Random.Range(timeUntilChangeDirectionMin, timeUntilChangeDirectionMax);
        moveDirection = (CultyMarbleHelper.GetMouseToWorldPosition() - this.transform.position).normalized;
    }

    private void Update()
    {
        // Update Moving Direction
        timeUntilChangeDirection -= Time.deltaTime;
        if (timeUntilChangeDirection <= 0)
        {
            timeUntilChangeDirection = Random.Range(timeUntilChangeDirectionMin, timeUntilChangeDirectionMax);

            moveDirection += new Vector3(moveDirection.x,
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
            int rnd = Random.Range(1, 20);

            if (lifeTime < 0.1 && rnd == 3)
            {
                Vector3 emberVector = new Vector3(0.1f, 0.1f, 0.1f);
                transform.localScale = emberVector;
            }
            //else if (lifeTime < 0.1 && Random.Range(0,3) == 3)
            //{
            //    transform.localScale += growthVector;
            //}
            else
            {
                transform.localScale += growthVector;
            }
        }

        // Partical LifeTime
        lifeTime -= Time.deltaTime;
        if(lifeTime < 0.1f)
        {

        }
        if (lifeTime <= 0)
        {
            gameObject.SetActive(false);
            gameObject.transform.localPosition = Vector2.zero;
        }
    }

    //===========================================================================
    public void ConfigParticleMovementSpeed(float newMoveSpeed, float newLifeTime)
    {
        moveSpeed = newMoveSpeed;
        lifeTime = newLifeTime;
    }


    public void ConfigParticleMovementPattern(float newTimeMax, float newTimeMin, float newSwingMagnitude)
    {
        timeUntilChangeDirectionMax = newTimeMax;
        timeUntilChangeDirectionMin = newTimeMin;
        swingMagtitude = newSwingMagnitude;
    }

    public void ConfigParticleSizeAndGrowth(float  size, float growthRate)
    {
        Vector3 sizeVector = new Vector3();
        sizeVector.x = size;
        sizeVector.y = size;
        transform.localScale = sizeVector;
        particleGrowthRate = growthRate;
    }
    public void ConfigParticleDamage(int damage, float pushPower)
    {
        particleDamage = damage;
        particlePushPower = pushPower;

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Vector2 _pushDirection = (collision.gameObject.GetComponent<Transform>().position - GetComponent<Transform>().position).normalized;
            collision.gameObject.GetComponent<EnemyHealth>().UpdateCurrentHealth(-particleDamage);
            //collision.gameObject.GetComponent<Transform>().Translate(-particlePushPower * GetComponent<Rigidbody2D>().velocity.normalized);
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(particlePushPower * _pushDirection);
        }
    }
}

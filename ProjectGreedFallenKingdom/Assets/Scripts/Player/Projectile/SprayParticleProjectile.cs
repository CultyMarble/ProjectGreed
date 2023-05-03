using UnityEngine;

public class SprayParticleProjectile : MonoBehaviour
{
    private float moveSpeed;
    private float lifeTime = default;

    private float timeUntilChangeDirectionMax = default;
    private float timeUntilChangeDirectionMin = default;
    private float timeUntilChangeDirection = default;

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

        // Movement Parttern
        transform.position += moveSpeed * Time.deltaTime * moveDirection;

        // Partical LifeTime
        lifeTime -= Time.deltaTime;
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
}

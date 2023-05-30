using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombObject : MonoBehaviour
{
    [Header("Effect Animation Settings:")]
    [SerializeField] private SpriteRenderer[] abilityEffect;
    [SerializeField] private Sprite[] effectSprites;

    private int damage;
    private float radius;
    private float pushBack;
    private float maxFuseTime;
    private float currentFuseTime;


    private readonly float effectAnimationSpeed = 0.1f;
    private float effectAnimationTimer;
    private int currentAnimationIndex;
    private float flashTime = 0.25f;
    private float flashTimer;

    private bool isExploding = false;
    private bool isFlashing = false;

    //===========================================================================
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerHealth>().UpdateCurrentHealth(-damage);
        }
        if (collision.gameObject.CompareTag("Collisions"))
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    //===========================================================================
    private void Update()
    {
        // Partical LifeTime
        if(currentFuseTime <= 0)
        {
            if (!isExploding)
            {
                PushEnemyInRadius();
            }
            isExploding = true;
        }

        if (isExploding)
        {
            AbilityEffectAnimation();
        }
        else
        {
            currentFuseTime -= Time.deltaTime;
            flashTimer += Time.deltaTime;

            if (flashTimer >= flashTime)
            {
                isFlashing = !isFlashing;
                flashTimer -= flashTime;
                flashTime *= 0.66f;
            }
            if (isFlashing)
            {
                transform.GetComponentInParent<SpriteRenderer>().color = Color.red;
            }
            else
            {
                transform.GetComponentInParent<SpriteRenderer>().color = Color.white;
            }
        }
    }

    //===========================================================================
    public void SetBombConfig(int _damage, float _pushback, float _radius, float _fuse)
    {
        damage = _damage;
        pushBack = _pushback;
        radius = _radius;
        maxFuseTime = _fuse;
        currentFuseTime = maxFuseTime;
    }

    private void AbilityEffectAnimation()
    {
        effectAnimationTimer += Time.deltaTime;
        if (effectAnimationTimer >= effectAnimationSpeed)
        {
            effectAnimationTimer -= effectAnimationSpeed;

            if (currentAnimationIndex == effectSprites.Length)
            {
                // Hide effect sprite
                foreach (SpriteRenderer spriteRenderer in abilityEffect)
                {
                    spriteRenderer.sprite = null;
                }
                isExploding = false;
                currentFuseTime = maxFuseTime;
                gameObject.SetActive(false);
                Destroy(gameObject);
                return;
            }
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            foreach (SpriteRenderer spriteRenderer in abilityEffect)
            {
                spriteRenderer.sprite = effectSprites[currentAnimationIndex];
            }
            currentAnimationIndex++;
        }
    }

    private void PushEnemyInRadius()
    {
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(transform.position, radius);
        if (collider2DArray.Length == 0)
            return;

        foreach (Collider2D collider2D in collider2DArray)
        {
            if (collider2D.CompareTag("Enemy"))
            {
                // Reduce health
                collider2D.GetComponent<EnemyHealth>().UpdateCurrentHealth(-damage/2);

                // Push back
                Transform enemyTranform = collider2D.GetComponent<Transform>();

                Vector2 _pushDirection = (enemyTranform.position - transform.position).normalized;
                float _eulerAngle = CultyMarbleHelper.GetAngleFromVector(_pushDirection);

                // Stop current movement
                //if (collider2D.GetComponent<ChasingAI>() != null)
                //{
                //    collider2D.GetComponent<ChasingAI>().holdMovementDirection = true;
                //    collider2D.GetComponent<ChasingAI>().holdtimer = 0.5f;
                //}
                //else if (collider2D.GetComponent<ChasingAIBasic>() != null)
                //{
                //    collider2D.GetComponent<ChasingAIBasic>().holdMovementDirection = true;
                //    collider2D.GetComponent<ChasingAIBasic>().holdtimer = 0.5f;
                //}
                collider2D.GetComponent<TargetingAI>().HoldMovement();

                // Add force
                collider2D.GetComponent<Enemy>().isPushBack = true;
                collider2D.GetComponent<Rigidbody2D>().AddForce(_pushDirection * pushBack, ForceMode2D.Force);
            }
        }
    }
}

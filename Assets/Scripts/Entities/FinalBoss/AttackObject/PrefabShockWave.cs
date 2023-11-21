using UnityEngine;

public class PrefabShockWave : MonoBehaviour
{
    private readonly int damage = 1;
    private int hitCount = 1;
    private readonly float moveSpeed = 7;
    private readonly float sizeScaleFactorX = 0.005f;
    private readonly float sizeScaleFactorY = 0.01f;
    private Vector3 moveDirection;

    //===========================================================================
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (SceneControlManager.Instance.CurrentGameplayState == GameplayState.Pause)
            return;

        if (collision.gameObject.tag == "Player" && Player.Instance.actionState != PlayerActionState.IsDashing)
        {
            if (hitCount > 0)
            {
                hitCount--;
                collision.gameObject.GetComponent<PlayerHeart>().UpdateCurrentHeart(-damage);
            }

            Despawn();
        }

        if (collision.gameObject.CompareTag("Collisions"))
        {
            Destroy(gameObject, 2);
        }
    }

    //===========================================================================
    private void Update()
    {
        if (SceneControlManager.Instance.CurrentGameplayState == GameplayState.Pause)
            return;

        transform.position += moveSpeed * Time.deltaTime * moveDirection;
    }

    private void FixedUpdate()
    {
        if (SceneControlManager.Instance.CurrentGameplayState == GameplayState.Pause)
            return;

        transform.localScale = new Vector2((float)(transform.localScale.x + sizeScaleFactorX),
            (float)(transform.localScale.y + sizeScaleFactorY));
    }

    //===========================================================================
    private void Despawn()
    {
        gameObject.SetActive(false);
        Destroy(gameObject);
    }

    //===========================================================================
    public void SetMoveDirection()
    {
        Vector3 _toPlayerDirection = (Player.Instance.transform.position - transform.position).normalized;

        // Set move direction
        moveDirection = _toPlayerDirection;

        // Rotate to MoveDirection
        float _zEulerAngle = CultyMarbleHelper.GetAngleFromVector(_toPlayerDirection);
        transform.eulerAngles = new Vector3(0.0f, 0.0f, _zEulerAngle);
    }
}

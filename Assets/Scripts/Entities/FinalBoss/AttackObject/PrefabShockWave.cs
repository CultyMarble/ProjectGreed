using UnityEngine;

public class PrefabShockWave : MonoBehaviour
{
    private readonly int damage = 1;
    private readonly float moveSpeed = 7;
    private readonly float sizeScaleFactorX = 0.008f;
    private readonly float sizeScaleFactorY = 0.015f;
    private Vector3 moveDirection;

    //===========================================================================
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (SceneControlManager.Instance.CurrentGameplayState == GameplayState.Pause)
            return;

        if (collision.gameObject.tag == "Player")
        {
            if (Player.Instance.actionState != PlayerActionState.IsDashing)
                collision.gameObject.GetComponent<PlayerHeart>().UpdateCurrentHeart(-damage);
        }

        if (collision.gameObject.CompareTag("Collisions"))
        {
            Despawn();
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

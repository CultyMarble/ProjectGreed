using UnityEngine;

public class MoveToPlayer : MonoBehaviour
{
    public bool moveToPlayer;
    [SerializeField] private float speed;

    private Transform player;

    private void Update()
    {
        if (moveToPlayer)
        {
            FindPlayer();
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

            if (Mathf.Approximately(transform.position.x, player.position.x) &&
                Mathf.Approximately(transform.position.y, player.position.y))
            {
                PlayerCurrencies.Instance.UpdateTempCurrencyAmount(5);
                Destroy(this.gameObject);
            }
        }
    }

    private void FindPlayer()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
    }
}

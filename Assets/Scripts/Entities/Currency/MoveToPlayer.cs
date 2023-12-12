using UnityEngine;

public class MoveToPlayer : MonoBehaviour
{
    private enum CurrencyType { Temp, Perm, }

    [SerializeField] private CurrencyType type;
    [SerializeField] private float speed;
    public bool moveToPlayer;

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
                AudioManager.Instance.playSFXClip(AudioManager.SFXSound.coinPickup, gameObject.GetComponent<AudioSource>());

                switch (type)
                {
                    case CurrencyType.Temp:
                        PlayerCurrencies.Instance.UpdateTempCurrencyAmount(5);
                        break;
                    case CurrencyType.Perm:
                        int _amount = Random.Range(1, 3);
                        PlayerCurrencies.Instance.UpdatePermCurrencyAmount(_amount);
                        break;
                    default:
                        break;
                }

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

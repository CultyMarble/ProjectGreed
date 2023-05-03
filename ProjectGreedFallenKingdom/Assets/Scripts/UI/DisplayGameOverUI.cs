using UnityEngine;

public class DisplayGameOverUI : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private GameObject gameoverUICanvas;

    private void OnEnable()
    {
        player.GetComponent<EnemyHealth>().OnDespawnEvent += DisplayGameOverUI_OnDestroyHandler;
    }

    private void DisplayGameOverUI_OnDestroyHandler(object sender, System.EventArgs e)
    {
        if (gameoverUICanvas.activeSelf == false)
        {
            gameoverUICanvas.SetActive(true);
        }
    }
}
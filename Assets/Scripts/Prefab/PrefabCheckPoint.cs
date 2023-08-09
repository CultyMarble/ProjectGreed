using UnityEngine;

public class PrefabCheckPoint : MonoBehaviour
{
    [SerializeField] private SceneName checkPointScene;
    [SerializeField] private Transform spawnPositition;

    private bool canActivateCheckPoint = default;

    //======================================================================
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() == null)
            return;

        Player.Instance.SetInteractPromtTextActive(true);
        canActivateCheckPoint = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() == null)
            return;

        Player.Instance.SetInteractPromtTextActive(false);
        canActivateCheckPoint = true;
    }

    //======================================================================
    private void Update()
    {
        if (canActivateCheckPoint == false)
            return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            ActivateCheckPoint();
            Debug.Log("CHECKPOINT SAVED!");
        }
    }

    //======================================================================
    private void ActivateCheckPoint()
    {
        SaveDataManager.Instance.SAVE01.SaveCheckPointData(checkPointScene, spawnPositition.position);
    }
}

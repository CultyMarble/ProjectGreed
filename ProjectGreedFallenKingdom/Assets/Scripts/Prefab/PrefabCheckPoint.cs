using UnityEngine;

public class PrefabCheckPoint : MonoBehaviour
{
    [SerializeField] private SceneName checkPointScene;
    [SerializeField] private Transform spawnPositition;

    private bool canActivateCheckPoint = default;

    //======================================================================
    private void OnTriggerEnter2D(Collider2D collision)
    {
        canActivateCheckPoint = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        canActivateCheckPoint = false;
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

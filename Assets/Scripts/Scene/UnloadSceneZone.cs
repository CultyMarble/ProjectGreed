using UnityEngine;

public class UnloadSceneZone : MonoBehaviour
{
    [HideInInspector] public bool canUnload;

    private void Start()
    {
        canUnload = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player.Instance.SetInteractPromtTextActive(true);
            canUnload = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player.Instance.SetInteractPromtTextActive(false);
            canUnload = false;
        }
    }
}

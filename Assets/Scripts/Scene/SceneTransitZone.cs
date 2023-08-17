using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class SceneTransitZone : MonoBehaviour
{
    [SerializeField] private SceneName sceneNameGoTo;
    [SerializeField] private Transform spawnLocation;

    private bool playFootStep = true;

    //===========================================================================
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            Player.Instance.SetInteractPromtTextActive(true);

            if (Input.GetKey(KeyCode.F))
            {
                if (playFootStep)
                {
                    SoundManager.Instance.PlaySound(SoundManager.Sound.footstep);
                    playFootStep = false;
                }

                Debug.Log(spawnLocation.position);
                SceneControlManager.Instance.LoadScene(sceneNameGoTo.ToString(), spawnLocation.position);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            Player.Instance.SetInteractPromtTextActive(false);
        }
    }
}
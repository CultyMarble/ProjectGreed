using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]
public class SceneTransitZone : MonoBehaviour
{
    [SerializeField] private SceneName sceneNameGoTo;
    [SerializeField] private Transform spawnLocation;

    private bool playFootStep = true;
    private bool canEnter = false;
    //===========================================================================
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.GetType().ToString() == Tags.BOXCOLLIDER2D)
        {
            Player.Instance.SetInteractPromtTextActive(true);
            canEnter = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.GetType().ToString() == Tags.BOXCOLLIDER2D)
        {
            Player.Instance.SetInteractPromtTextActive(false);
            canEnter = false;
        }
    }
    //===========================================================================
    private void Awake()
    {
        Player.Instance.GetComponent<PlayerInteractTrigger>().OnPlayerInteractTrigger += SceneTransitZone_OnPlayerInteractTrigger;
    }

    private void SceneTransitZone_OnPlayerInteractTrigger(object sender, System.EventArgs e)
    {
        if (canEnter)
        {
            if (playFootStep)
            {
                AudioManager.Instance.playSFXClip(AudioManager.SFXSound.footstep);
                playFootStep = false;
            }
            canEnter = false;
            SceneControlManager.Instance.LoadScene(sceneNameGoTo.ToString(), Vector3.zero);

            if (sceneNameGoTo == SceneName.DemoSceneBossRoom)
            {
                SceneControlManager.Instance.CurrentActiveScene = SceneName.DemoSceneBossRoom;
            }
        }
    }
}
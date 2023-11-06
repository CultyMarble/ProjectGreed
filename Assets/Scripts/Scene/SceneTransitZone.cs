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
        if (collision.CompareTag("Player") && collision.GetType().ToString() == Tags.BOXCOLLIDER2D)
        {
            Player.Instance.SetInteractPromtTextActive(true);

            if (Input.GetKey(KeyCode.F))
            {
                if (playFootStep)
                {
                    AudioManager.Instance.playSFXClip(AudioManager.SFXSound.footstep);
                    playFootStep = false;
                }

                SceneControlManager.Instance.LoadScene(sceneNameGoTo.ToString(), Vector3.zero);

                if (sceneNameGoTo == SceneName.DemoSceneBossRoom)
                {
                    SceneControlManager.Instance.CurrentActiveScene = SceneName.DemoSceneBossRoom;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.GetType().ToString() == Tags.BOXCOLLIDER2D)
        {
            Player.Instance.SetInteractPromtTextActive(false);
        }
    }
}
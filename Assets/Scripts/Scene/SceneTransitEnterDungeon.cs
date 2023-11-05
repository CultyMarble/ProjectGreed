using UnityEngine;

public class SceneTransitEnterDungeon : MonoBehaviour
{
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
                    AudioManager.Instance.playSFXClip(AudioManager.SFXSound.footstep);
                    playFootStep = false;
                }

                SceneControlManager.Instance.LoadDemoDungeonWrapper();
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
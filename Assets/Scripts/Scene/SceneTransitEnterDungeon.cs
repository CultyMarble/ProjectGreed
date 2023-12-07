using UnityEngine;
public class SceneTransitEnterDungeon : MonoBehaviour
{
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
        Player.Instance.GetComponent<PlayerInteractTrigger>().OnPlayerInteractTrigger += SceneTransitDungeon_OnPlayerInteractTrigger;
    }

    private void SceneTransitDungeon_OnPlayerInteractTrigger(object sender, System.EventArgs e)
    {
        if (canEnter)
        {
            if (playFootStep)
            {
                AudioManager.Instance.playSFXClip(AudioManager.SFXSound.footstep);
                playFootStep = false;
            }
            canEnter = false;
            SceneControlManager.Instance.LoadDemoDungeonWrapper();
        }
    }
}
using UnityEngine;

public class MapGUI : SingletonMonobehaviour<MapGUI>
{
    [SerializeField] private GameObject content = default;

    //[Header("UI Content")]

    //===========================================================================

    public void ToggleMapUI()
    {
        content.SetActive(!content.activeSelf);

        if (content.activeInHierarchy == true)
        {
            SceneControlManager.Instance.CurrentGameplayState = GameplayState.Pause;
        }
        else
        {
            SceneControlManager.Instance.CurrentGameplayState = GameplayState.Ongoing;
        }
    }

    public bool CheckMapOpen()
    {
        if (content.activeSelf == true)
        {
            return true;
        }
        return false;
    }
}
using UnityEngine;

public class PlayerDirectionIndicator : MonoBehaviour
{
    private void FixedUpdate()
    {
        if (SceneControlManager.Instance.CurrentGameplayState == GameplayState.Pause)
            return;

        CultyMarbleHelper.RotateGameObjectToMouseDirection(this.transform);
    }
}
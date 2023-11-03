using UnityEngine;

public class PlayerDirectionIndicator : MonoBehaviour
{
    private void FixedUpdate()
    {
        if (SceneControlManager.Instance.GameState == GameState.PauseMenu ||
            SceneControlManager.Instance.GameState == GameState.OptionMenu ||
            SceneControlManager.Instance.GameState == GameState.Dialogue)
            return;

        CultyMarbleHelper.RotateGameObjectToMouseDirection(this.transform);
    }
}
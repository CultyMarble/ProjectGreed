using UnityEngine;

public class PlayerDirectionIndicator : MonoBehaviour
{
    private void FixedUpdate()
    {
        CultyMarbleHelper.RotateGameObjectToMouseDirection(this.transform);
    }
}
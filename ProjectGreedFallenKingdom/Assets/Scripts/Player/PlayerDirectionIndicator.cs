using UnityEngine;

public class PlayerDirectionIndicator : MonoBehaviour
{
    private void Update()
    {
        CultyMarbleHelper.RotateGameObjectToMouseDirection(this.transform);
    }
}
using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField] private Vector3 offSet = default;

    //===========================================================================
    private void Update()
    {
        transform.position = Player.Instance.transform.position + offSet;
    }
}
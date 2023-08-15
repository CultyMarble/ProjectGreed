using UnityEngine;
using Cinemachine;

public class CameraTrigger : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    private PlayerMovement controller = default;

    private void Awake()
    {
        controller = FindObjectOfType<PlayerMovement>();
    }

    private void Start()
    {
        PlayerMovement.OnPlayerReady += SetPlayerFollow;
        if (controller == null)
        {
            controller = FindObjectOfType<PlayerMovement>();
        }
        if (virtualCamera.Follow == null)
        {
            virtualCamera.Follow = controller.transform;
        }
    }

    private void OnDisable()
    {
        PlayerMovement.OnPlayerReady -= SetPlayerFollow;
    }

    private void SetPlayerFollow(Transform playerTransform)
    {
        virtualCamera.Follow = playerTransform;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.GetType().ToString() != Tags.CAPSULECOLLIDER2D)
        {
            virtualCamera.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.GetType().ToString() != Tags.CAPSULECOLLIDER2D)
        {
            virtualCamera.gameObject.SetActive(false);
        }
    }

}

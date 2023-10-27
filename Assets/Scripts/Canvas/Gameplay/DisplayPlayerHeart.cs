using UnityEngine;

public class DisplayPlayerHeart : MonoBehaviour
{
    [Header("UI Settings")]
    [SerializeField] private PlayerHeart playerHeartManager;

    [Header("Pooling Settings:")]
    [Header("Full Heart:")]
    [SerializeField] private Transform pfFullHeart = default;
    [SerializeField] private Transform fullHeartPool = default;
    [Header("Empty Heart:")]
    [SerializeField] private Transform pfEmptyHeart = default;
    [SerializeField] private Transform emptyHeartPool = default;
    [Header("False Heart:")]
    [SerializeField] private Transform pfFalseHeart = default;
    [SerializeField] private Transform falseHeartPool = default;

    private readonly int poolSize = 20;
    private readonly float offsetX = 17.0f;

    //===========================================================================
    private void Awake()
    {
        PopulatePool(pfFullHeart, fullHeartPool);
        PopulatePool(pfEmptyHeart, emptyHeartPool);
        PopulatePool(pfFalseHeart, falseHeartPool);
    }

    private void OnEnable()
    {
        playerHeartManager.OnMaxHeartChangedEvent += UpdateMaxHeartUI;
        playerHeartManager.OnHeartChangedEvent += UpdateCurrentHeartUI;
    }

    private void OnDisable()
    {
        playerHeartManager.OnMaxHeartChangedEvent -= UpdateMaxHeartUI;
        playerHeartManager.OnHeartChangedEvent -= UpdateCurrentHeartUI;
    }

    //===========================================================================
    private void UpdateMaxHeartUI(object sender, PlayerHeart.OnMaxHealthChangedEventArgs e)
    {
        foreach (Transform fullHeart in emptyHeartPool)
        {
            fullHeart.gameObject.SetActive(false);
        }

        for (int i = 0; i < e.maxHeart; i++)
        {
            if (emptyHeartPool.GetChild(i).gameObject.activeInHierarchy == false)
            {
                emptyHeartPool.GetChild(i).GetComponent<RectTransform>().anchoredPosition = new Vector3(i * offsetX, 0, 0);
                emptyHeartPool.GetChild(i).gameObject.SetActive(true);
            }
        }
    }

    private void UpdateCurrentHeartUI(object sender, PlayerHeart.OnHealthChangedEventArgs e)
    {
        foreach (Transform fullHeart in fullHeartPool)
        {
            fullHeart.gameObject.SetActive(false);
        }

        for (int i = 0; i < e.currentHeart; i++)
        {
            if (fullHeartPool.GetChild(i).gameObject.activeInHierarchy == false)
            {
                fullHeartPool.GetChild(i).GetComponent<RectTransform>().anchoredPosition = new Vector3(i * offsetX, 0, 0);
                fullHeartPool.GetChild(i).gameObject.SetActive(true);
            }
        }
    }

    //===========================================================================
    private void PopulatePool(Transform pfObject, Transform pool)
    {
        for (int i = 0; i < poolSize; i++)
        {
            Instantiate(pfObject, pool).gameObject.SetActive(false);
        }
    }
}

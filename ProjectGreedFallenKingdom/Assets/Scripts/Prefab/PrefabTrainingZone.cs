using UnityEngine;
using TMPro;

public class PrefabTrainingZone : MonoBehaviour
{
    [Header("Text Refereces:")]
    [SerializeField] private TextMeshProUGUI objectiveText = default;
    [SerializeField] private TextMeshProUGUI keyHintText = default;
    [SerializeField] private TextMeshProUGUI hintText = default;

    [Header("Text")]
    [SerializeField] private string objective = default;
    [SerializeField] private string keyHint = default;
    [SerializeField] private string hint = default;

    private string objectiveDefault = default;
    private string keyHintDefault = default;
    private string hintDefault = default;

    //===========================================================================
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") == false)
            return;

        objectiveText.SetText(objective);
        keyHintText.SetText(keyHint);
        hintText.SetText(hint);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") == false)
            return;

        objectiveText.SetText(objectiveDefault);
        keyHintText.SetText(keyHintDefault);
        hintText.SetText(hintDefault);
    }

    //===========================================================================
    private void Awake()
    {
        objectiveDefault = "Enter any training zone to start tutorial";
        keyHintDefault = "[W][A][S][D] to Move | [Space] to Dash";
        hintDefault = "Training zone is red circle with a dummy inside. Use movement key to move around";

        objectiveText.SetText(objectiveDefault);
        keyHintText.SetText(keyHintDefault);
        hintText.SetText(hintDefault);
    }

    //===========================================================================
}

using System.Collections;
using UnityEngine;

public class BossStageController : MonoBehaviour
{
    [SerializeField] private EnemyHealth firstFormHealth = default;
    [SerializeField] private GameObject secondForm = default;

    //======================================================================
    private void OnEnable()
    {
        firstFormHealth.OnDespawnEvent += Stage1Health_OnDespawnEvent;
    }

    private void OnDisable()
    {
        firstFormHealth.OnDespawnEvent -= Stage1Health_OnDespawnEvent;
    }

    //======================================================================
    private void Stage1Health_OnDespawnEvent(object sender, System.EventArgs e)
    {
        StartCoroutine(SummonSecondStageBoss());
    }

    //======================================================================
    private IEnumerator SummonSecondStageBoss()
    {
        SceneControlManager.Instance.CurrentGameplayState = GameplayState.Pause;
        secondForm.SetActive(true);

        while (secondForm.transform.position != Vector3.zero)
        {
            secondForm.transform.position =
                Vector3.MoveTowards(secondForm.transform.position, Vector3.zero, Time.deltaTime * 10);

            yield return null;
        }

        SceneControlManager.Instance.CurrentGameplayState = GameplayState.Ongoing;
    }
}
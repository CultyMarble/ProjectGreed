using System.Collections;
using UnityEngine;

public class BossStageController : MonoBehaviour
{
    [SerializeField] private EnemyHealth firstFormHealth = default;
    [SerializeField] private GameObject secondForm = default;

    private bool transitToSecondBoss = false;
    private float transitDuration = 3.0f;
    private float durationTimer = default;

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
    private void Update()
    {
        if (transitToSecondBoss)
        {
            StartCoroutine(SummonSecondStageBoss());
            transitToSecondBoss = false;
        }
    }

    //======================================================================
    private void Stage1Health_OnDespawnEvent(object sender, System.EventArgs e)
    {
        transitToSecondBoss = true;
        durationTimer = transitDuration;

        CameraShake.Instance.SetCameraShakeOn();
    }

    //======================================================================
    private IEnumerator SummonSecondStageBoss()
    {
        secondForm.SetActive(true);

        while (durationTimer > 0.0f)
        {
            durationTimer -= Time.deltaTime;
            yield return null;
        }
    }
}
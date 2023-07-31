using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateManager : MonoBehaviour
{
    [Header("Gate Data")]
    [SerializeField] private bool playerInsideRoom = false;
    [SerializeField] private bool clearedRoom = false;
    [SerializeField] private bool disableGate = false;

    [Header("Gate Referance")]
    [SerializeField] private GameObject[] gates;

    private void ActiveGates(bool active)
    {
        foreach (var gate in gates)
        {
            gate.SetActive(active);
        }
    }

    public void RoomCleared()
    {
        clearedRoom = true;
        ActiveGates(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.GetType().ToString() != Tags.CAPSULECOLLIDER2D)
        {
            playerInsideRoom = true;

            if (clearedRoom || disableGate) return;

            ActiveGates(true);

            Test();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.GetType().ToString() != Tags.CAPSULECOLLIDER2D)
        {
            playerInsideRoom = false;
        }
    }

    private void Test()
    {

    }




}

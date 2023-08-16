using UnityEngine;
using Cinemachine;

public class SwitchConfineBoundingShape : MonoBehaviour
{
    private void OnEnable()
    {
        EventManager.AfterSceneLoadEvent += SwitchBoundingShape;
    }

    private void OnDisable()
    {
        EventManager.AfterSceneLoadEvent -= SwitchBoundingShape;
    }

    //===========================================================================
    private void SwitchBoundingShape()
    {
        PolygonCollider2D polygonCollider2D = GameObject.FindGameObjectWithTag(Tags.VCAMBoundsConfiner).GetComponent<PolygonCollider2D>();

        CinemachineConfiner2D _cinemachineConfiner2D = GetComponent<CinemachineConfiner2D>();

        _cinemachineConfiner2D.m_BoundingShape2D = polygonCollider2D;

        _cinemachineConfiner2D.InvalidateCache();
    }
}

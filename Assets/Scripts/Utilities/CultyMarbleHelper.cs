using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CultyMarbleHelper : MonoBehaviour
{
    private static Camera mainCamera;

    public static Vector3 GetMouseToWorldPosition()
    {
        if (mainCamera == null) mainCamera = Camera.main;

        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0.0f;

        return mousePosition;
    }

    public static void RotateGameObjectToMouseDirection(Transform transform, bool flipSprite = false)
    {
        Vector3 toMouseDirectionVector = (CultyMarbleHelper.GetMouseToWorldPosition() - transform.position).normalized;
        float _zEulerAngle = CultyMarbleHelper.GetAngleFromVector(toMouseDirectionVector);

        transform.eulerAngles = new Vector3(0.0f, 0.0f, _zEulerAngle);

        if (flipSprite)
        {
            if (CultyMarbleHelper.GetAngleFromVector(toMouseDirectionVector) > 90 ||
                CultyMarbleHelper.GetAngleFromVector(toMouseDirectionVector) < -90)
            {
                transform.GetComponent<SpriteRenderer>().flipY = true;
            }
            else
            {
                transform.GetComponent<SpriteRenderer>().flipY = false;
            }
        }
    }

    public static float GetAngleFromVector(Vector3 vector)
    {
        float radians = Mathf.Atan2(vector.y, vector.x);
        float degrees = radians * Mathf.Rad2Deg;

        return degrees;
    }

    public static Vector3 GetRandomDirection()
    {
        return new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }
}

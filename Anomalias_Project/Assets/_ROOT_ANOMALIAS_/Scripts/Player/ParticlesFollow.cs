using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform cam;

    void LateUpdate()
    {
        transform.position =
            cam.position;
    }
}
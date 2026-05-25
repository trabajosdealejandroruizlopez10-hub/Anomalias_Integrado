using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform cam;

    void Update()
    {
        transform.position = cam.position;
    }
}
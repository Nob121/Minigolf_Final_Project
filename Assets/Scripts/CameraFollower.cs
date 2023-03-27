using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] private Transform objectToFollow;

    void Update()
    {
        transform.position = objectToFollow.position + new Vector3(0, 5, -10);
    }
}

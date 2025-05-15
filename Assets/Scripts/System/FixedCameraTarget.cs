using UnityEngine;

public class FixedCameraTarget : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 sceneCenter = Vector3.zero;
    [Range(0f, 1f)]
    [SerializeField] private float weight = 0.75f;
    [SerializeField] private float maxOffset = 3f;

    void LateUpdate()
    {
        Vector3 desiredPosition = Vector3.Lerp(sceneCenter, player.position, weight);
        Vector3 offset = desiredPosition - sceneCenter;
        if (offset.magnitude > maxOffset)
        {
            offset = offset.normalized * maxOffset;
        }
        transform.position = sceneCenter + offset;
    }
}

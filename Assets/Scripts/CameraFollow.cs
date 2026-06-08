using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private bool followPlayerX = false;
    [SerializeField] private float fixedX = 0f;
    [SerializeField] private float height = 7f;
    [SerializeField] private float distanceBehind = 8f;
    [SerializeField] private float lookAtHeight = 1.2f;

    private void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        float cameraX = followPlayerX ? target.position.x : fixedX;

        transform.position = new Vector3(
            cameraX,
            target.position.y + height,
            target.position.z - distanceBehind
        );

        Vector3 lookTarget = target.position + Vector3.up * lookAtHeight;
        transform.LookAt(lookTarget);
    }
}

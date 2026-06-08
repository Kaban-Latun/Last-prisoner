using UnityEngine;

public class Telekinesis : MonoBehaviour
{
    [Header("Настройки физики")]
    public float frequency = 5f;
    public float dampingRatio = 0.7f;

    [Header("Фильтры объектов")]
    public LayerMask movableLayer;

    [Header("Защита от полетов")]
    public Transform playerTransform;
    public float safetyRadius = 2f;

    private Camera mainCamera;
    private TargetJoint2D targetJoint;
    private Rigidbody2D grabbedRigidbody;

    private void Start()
    {
        // Изначальный поиск камеры
        RefreshCamera();

        if (playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) playerTransform = player.transform;
        }
    }

    public void RefreshCamera()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (mainCamera == null) return;

        Vector2 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            TryGrabObject(mouseWorldPos);
        }

        if (Input.GetMouseButton(0) && targetJoint != null && playerTransform != null)
        {
            Vector2 directionToMouse = mouseWorldPos - (Vector2)playerTransform.position;
            float distanceToMouse = directionToMouse.magnitude;

            Vector2 finalTargetPos;

            if (distanceToMouse < safetyRadius)
            {
                finalTargetPos = (Vector2)playerTransform.position + directionToMouse.normalized * safetyRadius;
            }
            else
            {
                finalTargetPos = mouseWorldPos;
            }

            targetJoint.target = finalTargetPos;
        }

        if (Input.GetMouseButtonUp(0))
        {
            ReleaseObject();
        }
    }

    private void TryGrabObject(Vector2 pos)
    {
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f, movableLayer);

        if (hit.collider != null && hit.rigidbody != null)
        {
            grabbedRigidbody = hit.rigidbody;

            targetJoint = grabbedRigidbody.gameObject.AddComponent<TargetJoint2D>();
            targetJoint.anchor = grabbedRigidbody.transform.InverseTransformPoint(hit.point);

            targetJoint.frequency = frequency;
            targetJoint.dampingRatio = dampingRatio;

            targetJoint.target = hit.point;
        }
    }

    public void ReleaseObject()
    {
        if (targetJoint != null)
        {
            Destroy(targetJoint);
            targetJoint = null;
        }
        grabbedRigidbody = null;
    }

    private void OnDrawGizmosSelected()
    {
        if (playerTransform != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(playerTransform.position, safetyRadius);
        }
    }
}

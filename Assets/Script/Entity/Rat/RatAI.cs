using UnityEngine;

public class RatAI : MonoBehaviour
{
    [Header("Параметры движения")]
    public float moveSpeed = 3f;

    [Header("Настройки обнаружения стен")]
    [Tooltip("Слой, который считается стеной (например, Ground или Walls)")]
    public LayerMask wallLayer;
    [Tooltip("Максимальная дистанция поиска стен при старте")]
    public float maxSearchDistance = 50f;
    [Tooltip("Отступ от стены, чтобы крыса не втыкалась в неё носом")]
    public float wallOffset = 0.5f;

    private Vector2 leftPoint;
    private Vector2 rightPoint;
    private Vector2 currentTarget;

    private Rigidbody2D rb;
    private bool facingRight = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        CalculatePatrolPoints();
    }

    private void CalculatePatrolPoints()
    {
        Vector2 startPos = transform.position;

        RaycastHit2D hitLeft = Physics2D.Raycast(startPos, Vector2.left, maxSearchDistance, wallLayer);
        if (hitLeft.collider != null)
        {
            leftPoint = new Vector2(hitLeft.point.x + wallOffset, startPos.y);
        }
        else
        {
            leftPoint = new Vector2(startPos.x - 5f, startPos.y);
        }

        RaycastHit2D hitRight = Physics2D.Raycast(startPos, Vector2.right, maxSearchDistance, wallLayer);
        if (hitRight.collider != null)
        {
            rightPoint = new Vector2(hitRight.point.x - wallOffset, startPos.y);
        }
        else
        {
            rightPoint = new Vector2(startPos.x + 5f, startPos.y);
        }

        currentTarget = rightPoint;
        if (!facingRight) Flip();
    }

    private void FixedUpdate()
    {
        float directionX = currentTarget.x - transform.position.x;

        rb.linearVelocity = new Vector2(Mathf.Sign(directionX) * moveSpeed, rb.linearVelocity.y);

        if (Mathf.Abs(transform.position.x - currentTarget.x) < 0.2f)
        {
            SwitchTarget();
        }
    }

    private void SwitchTarget()
    {
        if (currentTarget == rightPoint)
        {
            currentTarget = leftPoint;
            if (facingRight) Flip();
        }
        else
        {
            currentTarget = rightPoint;
            if (!facingRight) Flip();
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(leftPoint, 0.3f);
            Gizmos.DrawWireSphere(rightPoint, 0.3f);
            Gizmos.DrawLine(leftPoint, rightPoint);
        }
    }
}

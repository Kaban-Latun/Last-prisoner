using UnityEngine;

public class GhostAI : MonoBehaviour
{
    [Header("Параметры движения")]
    public float moveSpeed = 3f;

    [Header("Настройки обнаружения стен и объектов")]
    [Tooltip("Слои стен (Ground) и динамических объектов (Movable)")]
    public LayerMask wallLayer;
    [Tooltip("Максимальная дистанция поиска стен при старте")]
    public float maxSearchDistance = 50f;
    [Tooltip("Отступ от стен при стартовом расчете")]
    public float wallOffset = 0.5f;

    [Header("Динамическое зрение")]
    [Tooltip("Размер зоны видимости перед носом крысы (X - ширина, Y - высота)")]
    public Vector2 visionBoxSize = new Vector2(0.4f, 0.6f);
    [Tooltip("На сколько сместить зону видимости вперед от центра крысы")]
    public float visionBoxOffset = 0.5f;

    private Vector2 leftPoint;
    private Vector2 rightPoint;
    private Vector2 currentTarget;
    
    private Rigidbody2D rb;
    private bool facingRight = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        CalculatePatrolPoints();
    }

    private void CalculatePatrolPoints()
    {
        Vector2 startPos = transform.position;

        // Ищем стену СЛЕВА при старте
        RaycastHit2D hitLeft = Physics2D.Raycast(startPos, Vector2.left, maxSearchDistance, wallLayer);
        leftPoint = hitLeft.collider != null ? new Vector2(hitLeft.point.x + wallOffset, startPos.y) : new Vector2(startPos.x - 5f, startPos.y);

        // Ищем стену СПРАВА при старте
        RaycastHit2D hitRight = Physics2D.Raycast(startPos, Vector2.right, maxSearchDistance, wallLayer);
        rightPoint = hitRight.collider != null ? new Vector2(hitRight.point.x - wallOffset, startPos.y) : new Vector2(startPos.x + 5f, startPos.y);

        currentTarget = rightPoint;
        if (!facingRight) Flip();
    }

    private void FixedUpdate()
    {
        // 1. ДИНАМИЧЕСКАЯ ПРОВЕРКА: создаем объемную коробку видимости перед носом
        float directionSign = facingRight ? 1f : -1f;
        Vector2 visionCenter = (Vector2)transform.position + new Vector2(visionBoxOffset * directionSign, 0f);

        // Проверяем, попало ли что-то со слоя wallLayer в эту коробку
        Collider2D hitObstacle = Physics2D.OverlapBox(visionCenter, visionBoxSize, 0f, wallLayer);
        
        if (hitObstacle != null)
        {
            // Чтобы крыса не путала землю под ногами с препятствием, проверяем, что это не тот коллайдер, на котором она стоит
            // (Актуально, если зона видимости случайно задевает пол)
            if (hitObstacle.transform != transform)
            {
                SwitchTarget();
                return;
            }
        }

        // 2. ДВИЖЕНИЕ К ТОЧКЕ
        float directionX = currentTarget.x - transform.position.x;
        rb.linearVelocity = new Vector2(Mathf.Sign(directionX) * moveSpeed, rb.linearVelocity.y);

        // Если дошли до стартовой границы — разворот
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

    // Отрисовка зон в редакторе Unity для удобной настройки
    private void OnDrawGizmos()
    {
        // Рисуем стартовый маршрут зеленым
        if (Application.isPlaying)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(leftPoint, 0.15f);
            Gizmos.DrawWireSphere(rightPoint, 0.15f);
            Gizmos.DrawLine(leftPoint, rightPoint);
        }

        // Рисуем красную коробку зрения перед носом крысы прямо во время игры
        Gizmos.color = Color.red;
        float directionSign = facingRight ? 1f : -1f;
        Vector2 visionCenter = (Vector2)transform.position + new Vector2(visionBoxOffset * directionSign, 0f);
        Gizmos.DrawWireCube(visionCenter, visionBoxSize);
    }
}

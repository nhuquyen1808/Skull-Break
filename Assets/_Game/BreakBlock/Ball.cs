using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Ball : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private LayerMask bounceLayerMask;
    [SerializeField] private LayerMask bottomLayerMask;

    [SerializeField] private Rigidbody2D rb;
    private Vector3 moveDirection;
    private bool isMoving = false;


    public void StartMove(Vector3 direction)
    {
        isMoving = true;
        moveDirection = direction.normalized;
        rb.bodyType = RigidbodyType2D.Dynamic; // Đặt body type là Dynamic để có thể di chuyển
        rb.linearVelocity = moveDirection * speed;
    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            rb.linearVelocity = moveDirection * speed;
        }
        else
        {
            rb.linearVelocity = Vector3.zero;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"Ball collided with {collision.gameObject.name} at {collision.contacts[0].point}");
        // Kiểm tra layer mask đúng cách
        if (((1 << collision.gameObject.layer) & bounceLayerMask) != 0)
        {
            SoundController.Instance.PlaySound(SoundName.Down);
            Vector2 normal = collision.contacts[0].normal;
            moveDirection = Vector3.Reflect(moveDirection, normal).normalized;
            rb.linearVelocity = moveDirection * speed;
        }

        // Kiểm tra layer mask đúng cách
        if (((1 << collision.gameObject.layer) & bottomLayerMask) != 0)
        {
            ShooterController.Instance.AddListBall(this);
        }

        var block = collision.gameObject.GetComponentInParent<Block>();
        if (block != null)
        {
            block.OnHit();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & bounceLayerMask) != 0)
        {
            // Raycast từ vị trí bóng theo hướng ngược lại để lấy normal
            RaycastHit2D hit = Physics2D.Raycast(transform.position, -moveDirection, 0.2f, bounceLayerMask);
            if (hit.collider != null)
            {
                Vector2 normal = hit.normal;
                moveDirection = Vector2.Reflect(moveDirection, normal).normalized;
                rb.linearVelocity = moveDirection * speed;
            }
        }

        if (((1 << collision.gameObject.layer) & bottomLayerMask) != 0)
        {
            ShooterController.Instance.AddListBall(this);
        }

    }
    public void StopMove()
    {
        isMoving = false;
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic; // Đặt body type là Dynamic để có thể di chuyển
    }
    public async UniTask MoveToFirePoint(Transform tfmFirePoint)
    {
       await transform.DOMove(tfmFirePoint.position, 0.2f);

    }
}

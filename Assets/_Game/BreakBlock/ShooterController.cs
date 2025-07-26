using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShooterController : Singleton<ShooterController>
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private int maxBounces = 3;
    [SerializeField] private float maxDistance = 20f;
    [SerializeField] private LayerMask layerWallAndEnemy;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private SpriteRenderer sprFireBall;
    [SerializeField] private Transform tfmCannon;
    [SerializeField] private TextMeshPro txtBallCount;
    [SerializeField] private Button btnBoosterArm;

    [Header("Ball Settings")]
    [SerializeField] private Ball ballPrefab;
    [SerializeField] private int ballCount = 5;
    [SerializeField] private float ballSpacing = 0.2f; // thời gian trễ giữa các bóng (nếu muốn bắn dàn hàng)

    [SerializeField] private List<Ball> activeBalls = new List<Ball>();
    [SerializeField] private List<Ball> lstWaitBall = new List<Ball>();
    private Vector3 shootDirection;
    private bool isTurnOnArm;


    private void Start()
    {
        ballCount = DatabaseController.Instance.Ball;
        txtBallCount.text = $"{ballCount}";
    }
    public async UniTask AddBallCount()
    {
        ballCount += 5;
        DOVirtual.Int(ballCount - 5, ballCount, 0.5f, (value) =>
        {
            txtBallCount.text = $"{value}";
        });
    }
    public void TurnOnArm()
    {
        isTurnOnArm = true;
        btnBoosterArm.interactable = false;
    }
    private void Update()
    {
        if (!GameplayController.Instance.IsPlaying || GameplayController.Instance.GamePlayState != GamePlayState.Ready)
        {
            lineRenderer.enabled = false;
            return; // Không xử lý nếu không đang chơi
        }
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            lineRenderer.enabled = false;
            return; // Không xử lý nếu chuột đang ở trên UI
        }
        // retrun if mouse in UI

        if (Input.GetMouseButtonDown(0))
        {

            if (isTurnOnArm)
                lineRenderer.enabled = true;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.WorldToScreenPoint(firePoint.position).z;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

            Vector2 direction = (worldPos - firePoint.position).normalized;
            shootDirection = direction;
            tfmCannon.transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
            DrawReflectingLine(firePoint.position, direction, maxBounces);
        }

        if (Input.GetMouseButtonUp(0))
        {

            lineRenderer.enabled = false;
            ShootBalls();
        }
    }

    private async UniTask ShootBalls()
    {
        GameplayController.Instance.OnShoot();
        sprFireBall.enabled = false;
        lstWaitBall.Clear();
        // Xóa các bóng cũ nếu cần
        foreach (var ball in activeBalls)
        {
            if (ball != null) Destroy(ball.gameObject);
        }
        activeBalls.Clear();

        for (int i = 0; i < ballCount; i++)
        {
            SoundController.Instance.PlaySound(SoundName.Up);

            Ball newBall = Instantiate(ballPrefab, firePoint.position, Quaternion.identity);
            newBall.StartMove(shootDirection);
            activeBalls.Add(newBall);
            await UniTask.Delay((int)(ballSpacing * 1000)); // Thêm thời gian trễ giữa các bóng
        }

        tfmCannon.transform.DOLocalRotateQuaternion(Quaternion.LookRotation(Vector3.forward, Vector3.zero), 0.2f);
    }
    public async UniTask AddListBall(Ball ball)
    {
        ball.StopMove();
        firePoint.gameObject.SetActive(true);
        lstWaitBall.Add(ball);
        if (lstWaitBall.Count == 1)
        {
            var newPos = new Vector3(ball.transform.position.x, firePoint.transform.position.y, ball.transform.position.z);
            firePoint.transform.position = newPos;
            sprFireBall.enabled = true;

        }
        if (lstWaitBall.Count == activeBalls.Count)
        {
            GameplayController.Instance.OnDoneShoot();
        }
        await ball.MoveToFirePoint(firePoint);

        ball.gameObject.SetActive(false);

    }
    private void DrawReflectingLine(Vector2 startPos, Vector2 direction, int bounces)
    {
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, startPos);

        Vector2 currentStart = startPos;
        Vector2 currentDir = direction;
        int index = 1;

        for (int i = 0; i < bounces; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(currentStart, currentDir, maxDistance, layerWallAndEnemy);

            if (hit.collider != null)
            {
                lineRenderer.positionCount++;
                lineRenderer.SetPosition(index++, hit.point);

                currentDir = Vector2.Reflect(currentDir, hit.normal);
                currentStart = hit.point + currentDir * 0.01f;
            }
            else
            {
                lineRenderer.positionCount++;
                lineRenderer.SetPosition(index++, currentStart + currentDir * maxDistance);
                break;
            }
        }
    }
}

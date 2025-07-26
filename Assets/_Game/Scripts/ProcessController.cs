using UnityEngine;
using UnityEngine.UI;

public class ProcessController : Singleton<ProcessController>
{
    [Header("UI Components")]
    [Tooltip("Thanh fill để hiển thị tiến trình")]
    [SerializeField] private Image fillBar; // Thay Slider bằng Image

    [Tooltip("Danh sách các ngôi sao (3 ngôi sao)")]
    [SerializeField] private ButtonSettings[] stars;




    [Header("Goal Settings")]
    [Tooltip("Mục tiêu điểm cần đạt")]
    private int goal;

    [Tooltip("Điểm hiện tại")]
    private int currentScore;

    /// <summary>
    /// Thiết lập mục tiêu và cập nhật vị trí các ngôi sao.
    /// </summary>
    /// <param name="goal">Mục tiêu điểm cần đạt.</param>
    public void SetGoal(int goal)
    {
        this.goal = goal;
        currentScore = 0;

        // Đảm bảo thanh fill bắt đầu từ 0
        fillBar.fillAmount = 0;

        // Cập nhật vị trí các ngôi sao dựa trên phần trăm của mục tiêu
        if (stars.Length >= 3)
        {
            RectTransform fillBarRect = fillBar.GetComponent<RectTransform>();
            stars[0].transform.localPosition = new Vector3(fillBarRect.sizeDelta.x * 0.3f - fillBarRect.sizeDelta.x / 2, 0, 0);
            stars[1].transform.localPosition = new Vector3(fillBarRect.sizeDelta.x * 0.5f - fillBarRect.sizeDelta.x / 2, 0, 0);
            stars[2].transform.localPosition = new Vector3(fillBarRect.sizeDelta.x * 0.8f - fillBarRect.sizeDelta.x / 2, 0, 0);
        }
    }

    /// <summary>
    /// Thêm điểm và cập nhật thanh fill.
    /// </summary>
    /// <param name="score">Điểm cần thêm.</param>
    public void AddScore(int score)
    {
        currentScore += score;

        // Đảm bảo điểm không vượt quá mục tiêu
        currentScore = Mathf.Clamp(currentScore, 0, goal);

        // Cập nhật giá trị thanh fill
        fillBar.fillAmount = (float)currentScore / goal;

        // Cập nhật trạng thái các ngôi sao
        UpdateStars();
    }

    /// <summary>
    /// Cập nhật trạng thái các ngôi sao dựa trên điểm hiện tại.
    /// </summary>
    private void UpdateStars()
    {
        if (stars.Length >= 3)
        {
            stars[0].SetState(currentScore >= goal * 0.3f);
            stars[1].SetState(currentScore >= goal * 0.5f);
            stars[2].SetState(currentScore >= goal * 0.8f);

            
        }
    }
    public int GetStar()
    {
        int star = 0;
        if (stars[0].GetState())
        {
            star++;
        }
        if (stars[1].GetState())
        {
            star++;
        }
        if (stars[2].GetState())
        {
            star++;
        }
        return star;
    }
}

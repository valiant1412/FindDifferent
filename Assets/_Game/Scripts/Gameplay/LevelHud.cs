using TMPro;
using UnityEngine;

namespace FindDiff.Gameplay
{
    /// <summary>
    /// Thanh thông tin trên cùng (HUD): thời gian, lượt sai, số điểm đã tìm, và dòng kết quả.
    /// Script này CHỈ HIỂN THỊ những gì LevelController đưa cho nó, không tự tính toán luật chơi.
    /// </summary>
    public class LevelHud : MonoBehaviour
    {
        [SerializeField] private TMP_Text timeText;
        [SerializeField] private TMP_Text mistakesText;
        [SerializeField] private TMP_Text foundText;
        [SerializeField] private TMP_Text resultText;

        private void Awake()
        {
            //resultText.gameObject.SetActive(false);   // ẩn dòng kết quả khi mới vào màn
        }

        public void SetTime(float secondsLeft)
        {
            // Làm tròn LÊN: còn 0.3 giây vẫn hiện "0:01", chỉ hiện "0:00" khi thật sự hết giờ.
            int totalSeconds = Mathf.CeilToInt(secondsLeft);
            int minutes = totalSeconds / 60;   // chia số nguyên: 85 / 60 = 1
            int seconds = totalSeconds % 60;   // phần dư:         85 % 60 = 25
            timeText.text = $"{minutes}:{seconds:00}";   // ":00" = luôn đủ 2 chữ số -> "1:05"
        }

        public void SetMistakes(int mistakesLeft)
        {
            mistakesText.text = $"Miss left: {mistakesLeft}";
        }

        public void SetFound(int found, int total)
        {
            foundText.text = $"{found}/{total}";
        }

        public void ShowResult(string message)
        {
            // resultText.text = message;
            // resultText.gameObject.SetActive(true);
        }
    }
}

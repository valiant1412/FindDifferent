using System.Linq;
using FindDiff.Data;
using UnityEngine;

namespace FindDiff.Gameplay
{
    /// <summary>
    /// "Bộ não" của một màn chơi:
    ///   - Lúc bắt đầu: đưa 2 ảnh từ LevelData lên 2 PictureView.
    ///   - Mỗi lần có tap: đổi sang PIXEL ảnh gốc, kiểm tra có trúng điểm khác biệt chưa tìm không.
    ///   - Trúng: đánh dấu đã tìm + khoanh tròn trên CẢ HAI ảnh.
    ///   - Trượt: hiện dấu X, trừ 1 lượt sai.
    ///   - Đồng hồ đếm ngược mỗi frame.
    ///   - Tìm đủ -> THẮNG. Hết lượt sai hoặc hết giờ -> THUA.
    /// </summary>
    public class LevelController : MonoBehaviour
    {
        [Header("Dữ liệu")]
        [SerializeField] private LevelData level;

        [Header("Tham chiếu trong scene")]
        [SerializeField] private TapDetector tapDetector;
        [SerializeField] private PictureView pictureA;
        [SerializeField] private PictureView pictureB;
        [SerializeField] private LevelHud hud;

        [Header("Hiển thị")]
        [SerializeField] private RectTransform foundMarkerPrefab;
        [Tooltip("Vẽ vòng tròn to hơn điểm khác biệt một chút cho dễ nhìn.")]
        [SerializeField] private float markerScale = 1.3f;
        [SerializeField] private RectTransform wrongMarkerPrefab;
        [Tooltip("Kích thước dấu X, pixel ảnh gốc.")]
        [SerializeField] private float wrongMarkerSize = 80f;
        [Tooltip("Dấu X tự biến mất sau số giây này.")]
        [SerializeField] private float wrongMarkerLifetime = 0.6f;

        [Header("Cảm giác chạm")]
        [Tooltip("Nới rộng vùng trúng cho ngón tay, đơn vị pixel ảnh gốc.")]
        [SerializeField] private float touchTolerance = 30f;

        [Header("Pop UP")]
        [SerializeField] private GameObject WinUI;

        [SerializeField] private GameObject LoseUI;

        // ----- Trạng thái lúc chơi (KHÔNG lưu vào LevelData) -----
        private bool[] isFound;   // isFound[i] == true nghĩa là điểm thứ i đã được tìm thấy
        private int foundCount;
        private LevelState state;
        private float timeLeft;   // giây còn lại
        private int mistakesLeft; // lượt sai còn lại

        private void OnEnable()
        {
            tapDetector.Tapped += HandleTap;
        }

        private void OnDisable()
        {
            tapDetector.Tapped -= HandleTap;
        }

        private void Start()
        {
            pictureA.Show(level.imageA);
            pictureB.Show(level.imageB);

            isFound = new bool[level.spots.Count];   // mảng mới: tất cả đều false
            foundCount = 0;
            state = LevelState.Playing;
            timeLeft = level.timeLimit;
            mistakesLeft = level.maxMistakes;

            hud.SetTime(timeLeft);
            hud.SetMistakes(mistakesLeft);
            hud.SetFound(foundCount, level.spots.Count);

            Debug.LogWarning("Chưa làm TODO BÀI TẬP 4.1 (LevelController.Update) – xoá dòng này khi xong", this);
        }

        private void Update()
        {
            // TODO BÀI TẬP 4.1 – Đồng hồ đếm ngược.
            //   - Chỉ chạy khi đang chơi (state là Playing). Không phải Playing thì return ngay.
            if (state != LevelState.Playing) return;
            //   - Trừ timeLeft đi thời gian của frame này. Gợi ý: Time.deltaTime = số giây từ frame trước tới giờ
            //     (60 FPS thì khoảng 0.016).
            timeLeft -= Time.deltaTime;
            timeLeft = Mathf.Max(timeLeft, 0f);
            //   - Không để timeLeft âm: dùng Mathf.Max(timeLeft, 0f).
            //   - Báo cho HUD: hud.SetTime(timeLeft);
            hud.SetTime(timeLeft);
            Debug.Log($"Thời gian thật đã trôi: {Time.timeSinceLevelLoad:F1}s | Đồng hồ game đã trừ: {level.timeLimit - timeLeft:F1}s");
            if (timeLeft == 0)
            {
                EndLevel(LevelState.Lost, "Time's up");
            }
            //   - Nếu timeLeft đã về 0 -> EndLevel(LevelState.Lost, "TIME'S UP");
        }

        private void HandleTap(Vector2 screenPosition)
        {
            if (pictureA.TryGetImagePixel(screenPosition, out Vector2 pixelOnA))
            {
                CheckTap(pictureA, pixelOnA);
            }
            else if (pictureB.TryGetImagePixel(screenPosition, out Vector2 pixelOnB))
            {
                CheckTap(pictureB, pixelOnB);
            }
            // Chạm ra ngoài cả 2 ảnh -> bỏ qua, không tính là sai.
        }

        private void CheckTap(PictureView tappedPicture, Vector2 pixelPoint)
        {
            // Màn đã kết thúc (thắng hoặc thua) -> không nhận chạm nữa.
            if (state != LevelState.Playing)
            {
                return;
            }

            // In toạ độ pixel ảnh gốc ra Console: vừa để debug, vừa để lấy toạ độ khi tạo level mới.
            Debug.Log($"Tap tại pixel {pixelPoint.ToString("F0")}");

            int spotIndex = FindSpotAt(pixelPoint);
            if (spotIndex < 0)
            {
                RegisterMistake(tappedPicture, pixelPoint);
                return;
            }

            MarkFound(spotIndex);
        }

        private void RegisterMistake(PictureView tappedPicture, Vector2 pixelPoint)
        {
            // Hiện dấu X đúng chỗ chạm, trên ảnh vừa chạm, rồi tự huỷ sau wrongMarkerLifetime giây.
            RectTransform wrongMarker = tappedPicture.AddMarker(wrongMarkerPrefab, pixelPoint, wrongMarkerSize);
            Destroy(wrongMarker.gameObject, wrongMarkerLifetime);

            mistakesLeft -= 1;
            hud.SetMistakes(mistakesLeft);
            if (mistakesLeft == 0) EndLevel(LevelState.Lost, "Lose");

        }

        /// <summary>
        /// Tìm điểm khác biệt CHƯA được tìm thấy mà chứa điểm chạm.
        /// Trả về số thứ tự (index) của điểm đó, hoặc -1 nếu không trúng điểm nào.
        /// </summary>
        private int FindSpotAt(Vector2 pixelPoint)
        {
            for (int i = 0; i < level.spots.Count; i++)
            {
                if (isFound[i]) continue;
                var isSpotContain = level.spots[i].Contains(pixelPoint, touchTolerance);
                if (isSpotContain) return i;
                Debug.Log(i);
                continue;
            }

            return -1;
        }

        private void MarkFound(int spotIndex)
        {
            isFound[spotIndex] = true;
            foundCount++;

            DifferenceSpot spot = level.spots[spotIndex];
            float diameter = spot.radius * 2f * markerScale;   // pixel ảnh gốc
            pictureA.AddMarker(foundMarkerPrefab, spot.center, diameter);
            pictureB.AddMarker(foundMarkerPrefab, spot.center, diameter);

            hud.SetFound(foundCount, level.spots.Count);
            Debug.Log($"ĐÚNG! Tìm thấy điểm #{spotIndex}  ({foundCount}/{level.spots.Count})");

            if (foundCount == level.spots.Count)
            {
                EndLevel(LevelState.Won, "YOU WIN!");
            }
        }

        /// <summary>Kết thúc màn: đổi trạng thái (dừng đồng hồ, khoá chạm) và báo kết quả lên HUD.</summary>
        private void EndLevel(LevelState result, string message)
        {
            state = result;
            hud.ShowResult(message);
            Debug.Log($"Kết thúc màn: {result}");

            if (result == LevelState.Won)
            {
                float timeUsed = level.timeLimit - timeLeft;          // 90 - 42.3 = 47.7 giây
                int mistakesMade = level.maxMistakes - mistakesLeft;  // 3 - 2 = sai 1 lần
                int stars = StarRating.Calculate(level, timeUsed, mistakesMade);
                Debug.Log($"Thời gian: {timeUsed:F1}s | Sai: {mistakesMade} lần | Sao: {stars}/{StarRating.MaxStars}");
                // Buổi 5 (bạn tự làm): truyền timeUsed, mistakesMade, stars sang popup để hiển thị.
                WinUI.SetActive(true);
            }
            else
            {
                LoseUI.SetActive(true);
            }
        }
    }
}

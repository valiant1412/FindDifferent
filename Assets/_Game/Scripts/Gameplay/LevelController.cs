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
    /// Buổi 4 sẽ thêm: lượt chọn sai, đồng hồ, thắng/thua.
    /// </summary>
    public class LevelController : MonoBehaviour
    {
        [Header("Dữ liệu")]
        [SerializeField] private LevelData level;

        [Header("Tham chiếu trong scene")]
        [SerializeField] private TapDetector tapDetector;
        [SerializeField] private PictureView pictureA;
        [SerializeField] private PictureView pictureB;

        [Header("Hiển thị")]
        [SerializeField] private RectTransform foundMarkerPrefab;
        [Tooltip("Vẽ vòng tròn to hơn điểm khác biệt một chút cho dễ nhìn.")]
        [SerializeField] private float markerScale = 1.3f;

        [Header("Cảm giác chạm")]
        [Tooltip("Nới rộng vùng trúng cho ngón tay, đơn vị pixel ảnh gốc.")]
        [SerializeField] private float touchTolerance = 30f;

        // ----- Trạng thái lúc chơi (KHÔNG lưu vào LevelData) -----
        private bool[] isFound;   // isFound[i] == true nghĩa là điểm thứ i đã được tìm thấy
        private int foundCount;

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
        }

        private void HandleTap(Vector2 screenPosition)
        {
            if (pictureA.TryGetImagePixel(screenPosition, out Vector2 pixelOnA))
            {
                CheckTap(pixelOnA);
            }
            else if (pictureB.TryGetImagePixel(screenPosition, out Vector2 pixelOnB))
            {
                CheckTap(pixelOnB);
            }
            // Chạm ra ngoài cả 2 ảnh -> bỏ qua, không tính là sai.
        }

        private void CheckTap(Vector2 pixelPoint)
        {
            // In toạ độ pixel ảnh gốc ra Console: vừa để debug, vừa để lấy toạ độ khi tạo level mới.
            Debug.Log($"Tap tại pixel {pixelPoint.ToString("F0")}");

            int spotIndex = FindSpotAt(pixelPoint);
            if (spotIndex < 0)
            {
                Debug.Log("SAI – không trúng điểm khác biệt nào. (Buổi 4 sẽ trừ lượt sai)");
                return;
            }

            MarkFound(spotIndex);
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

            Debug.Log($"ĐÚNG! Tìm thấy điểm #{spotIndex}  ({foundCount}/{level.spots.Count})");

            if (foundCount == level.spots.Count)
            {
                Debug.Log("THẮNG! Đã tìm hết các điểm khác biệt. (Buổi 4 sẽ hiện bảng thắng)");
            }
        }
    }
}

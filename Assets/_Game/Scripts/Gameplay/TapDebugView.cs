using UnityEngine;

namespace FindDiff.Gameplay
{
    /// <summary>
    /// SCRIPT TẠM của Buổi 2: nghe TapDetector, đổi chỗ chạm sang toạ độ chuẩn hoá,
    /// rồi đặt một chấm đỏ lên CÙNG vị trí ở CẢ HAI ảnh.
    /// Nếu 2 chấm nằm đúng trên cùng một đồ vật ở 2 ảnh -> phép đổi toạ độ đã đúng.
    /// Buổi 3 sẽ thay script này bằng phần kiểm tra điểm khác biệt thật.
    /// </summary>
    public class TapDebugView : MonoBehaviour
    {
        [SerializeField] private TapDetector tapDetector;
        [SerializeField] private PictureView pictureA;
        [SerializeField] private PictureView pictureB;
        [SerializeField] private RectTransform dotA;   // chấm đỏ, con của PictureA/.../Content
        [SerializeField] private RectTransform dotB;   // chấm đỏ, con của PictureB/.../Content

        // Đăng ký nghe event khi object bật, huỷ đăng ký khi object tắt.
        // Quên huỷ đăng ký là nguồn bug kinh điển (gọi vào object đã bị huỷ).
        private void OnEnable()
        {
            tapDetector.Tapped += HandleTap;
        }

        private void OnDisable()
        {
            tapDetector.Tapped -= HandleTap;
        }

        private void HandleTap(Vector2 screenPosition)
        {
            if (pictureA.TryGetNormalizedPoint(screenPosition, out Vector2 pointOnA))
            {
                ShowDots("A", pointOnA);
            }
            else if (pictureB.TryGetNormalizedPoint(screenPosition, out Vector2 pointOnB))
            {
                ShowDots("B", pointOnB);
            }
            else
            {
                Debug.Log($"Chạm ngoài ảnh, màn hình = {screenPosition}");
            }
        }

        private void ShowDots(string pictureName, Vector2 normalizedPoint)
        {
            Debug.Log($"Chạm ảnh {pictureName} tại {normalizedPoint.ToString("F2")}");
            dotA.anchoredPosition = pictureA.NormalizedToLocalPoint(normalizedPoint);
            dotB.anchoredPosition = pictureB.NormalizedToLocalPoint(normalizedPoint);
        }
    }
}

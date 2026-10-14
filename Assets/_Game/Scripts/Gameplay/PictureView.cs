using System;
using UnityEngine;
using UnityEngine.UI;

namespace FindDiff.Gameplay
{
    /// <summary>
    /// Hiển thị MỘT bức tranh bên trong một khung (Viewport).
    /// Cấu trúc trong Hierarchy:
    ///   PictureA            (có script này)
    ///   └── Viewport        (khung cửa sổ, có RectMask2D để cắt phần thừa)
    ///       └── Content     (tờ ảnh thật, có component Image)
    /// Script chỉ lo "hiển thị ảnh cho vừa khung, giữ đúng tỉ lệ".
    /// </summary>
    public class PictureView : MonoBehaviour
    {
        [SerializeField] private RectTransform viewport;
        [SerializeField] private RectTransform content;
        [SerializeField] private Image contentImage;

        public void Show(Sprite sprite)
        {
            if (sprite == null)
            {
                Debug.LogError($"[PictureView] {name}: sprite đang null, chưa kéo ảnh vào?", this);
                return;
            }

            contentImage.sprite = sprite;
            FitContentToViewport(sprite);
        }

        /// <summary>
        /// CHIỀU XUÔI: toạ độ chuẩn hoá (0..1) -> toạ độ local trong Content (gốc (0,0) ở GIỮA ảnh).
        /// Dùng để đặt một vật (chấm, vòng tròn) lên đúng chỗ trên ảnh.
        /// </summary>
        public Vector2 NormalizedToLocalPoint(Vector2 normalizedPoint)
        {
            Rect rect = content.rect;                 // rect.min = góc dưới-trái, rect.size = (rộng, cao)
            return rect.min + normalizedPoint * rect.size;
        }

        /// <summary>
        /// CHIỀU NGƯỢC: vị trí chạm trên màn hình -> toạ độ chuẩn hoá (0..1) trên ảnh.
        /// Trả về false nếu chạm ra ngoài khung hoặc ra ngoài phần ảnh.
        /// </summary>
        public bool TryGetNormalizedPoint(Vector2 screenPosition, out Vector2 normalizedPoint)
        {
            normalizedPoint = Vector2.zero;

            // 1) Chạm có nằm trong khung (Viewport) không? Canvas là Overlay nên camera = null.
            if (!RectTransformUtility.RectangleContainsScreenPoint(viewport, screenPosition, null))
            {
                return false;
            }

            // 2) Đổi toạ độ màn hình (pixel) -> toạ độ local của Content (gốc ở giữa ảnh).
            RectTransformUtility.ScreenPointToLocalPointInRectangle(content, screenPosition, null, out Vector2 localPoint);

            // 3) Đổi local -> chuẩn hoá (0..1).
            Rect rect = content.rect;


            normalizedPoint = (localPoint - rect.min) / rect.size;

            // 4) Chạm trong khung nhưng ngoài phần ảnh (vùng dư) -> không tính.
            bool insideImage = normalizedPoint.x >= 0f && normalizedPoint.x <= 1f
                            && normalizedPoint.y >= 0f && normalizedPoint.y <= 1f;
            return insideImage;
        }

        /// <summary>
        /// Đổi kích thước Content sao cho ảnh nằm GỌN trong Viewport mà KHÔNG bị méo.
        /// </summary>
        private void FitContentToViewport(Sprite sprite)
        {
            Vector2 viewportSize = viewport.rect.size;
            Vector2 spriteSize = sprite.rect.size;

            var ratioX = viewportSize.x / spriteSize.x;
            var ratioY = viewportSize.y / spriteSize.y;
            var ratio = Mathf.Min(ratioX, ratioY);
            Vector2 fittedSize = spriteSize * ratio;
            content.sizeDelta = fittedSize;
        }
    }
}

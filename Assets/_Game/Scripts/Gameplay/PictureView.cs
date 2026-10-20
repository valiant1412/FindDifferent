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

        // Kích thước FILE ẢNH GỐC tính bằng pixel, ví dụ (1448, 1086). Ghi lại khi Show.
        private Vector2 imagePixelSize;

        public void Show(Sprite sprite)
        {
            if (sprite == null)
            {
                Debug.LogError($"[PictureView] {name}: sprite đang null, chưa kéo ảnh vào?", this);
                return;
            }

            contentImage.sprite = sprite;
            imagePixelSize = sprite.rect.size;
            FitContentToViewport(sprite);
        }

        /// <summary>
        /// Chỗ chạm trên màn hình -> PIXEL trên file ảnh gốc (gốc (0,0) ở góc TRÊN-trái, giống Paint).
        /// Trả về false nếu chạm ra ngoài ảnh.
        /// </summary>
        public bool TryGetImagePixel(Vector2 screenPosition, out Vector2 imagePixel)
        {
            imagePixel = Vector2.zero;
            if (!TryGetNormalizedPoint(screenPosition, out Vector2 normalized))
            {
                return false;
            }

            // 0..1 -> pixel: nhân với kích thước ảnh gốc.
            // Trục y đảo lại (1 - y) vì Unity tính y từ DƯỚI lên, còn Paint tính từ TRÊN xuống.
            imagePixel = new Vector2(normalized.x * imagePixelSize.x,
                                     (1f - normalized.y) * imagePixelSize.y);
            return true;
        }

        /// <summary>
        /// Pixel ảnh gốc -> vị trí local trong Content (để đặt vòng tròn). Ngược với TryGetImagePixel.
        /// </summary>
        public Vector2 ImagePixelToLocalPoint(Vector2 imagePixel)
        {
            Vector2 normalized = new Vector2(imagePixel.x / imagePixelSize.x,
                                             1f - imagePixel.y / imagePixelSize.y);
            return NormalizedToLocalPoint(normalized);
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
        /// Tạo một bản sao của markerPrefab (vòng tròn) và đặt lên ảnh.
        /// pixelCenter, pixelDiameter: đơn vị pixel ảnh gốc.
        /// </summary>
        public void AddMarker(RectTransform markerPrefab, Vector2 pixelCenter, float pixelDiameter)
        {
            // Tạo bản sao làm CON của Content -> khi Content di chuyển/zoom, vòng tròn đi theo.
            RectTransform marker = Instantiate(markerPrefab, content);
            marker.anchoredPosition = ImagePixelToLocalPoint(pixelCenter);

            // Ảnh gốc 1448 px đang hiển thị rộng (ví dụ) 1040 đơn vị UI
            // -> 1 pixel ảnh gốc = 1040 / 1448 đơn vị UI.
            float unitsPerImagePixel = content.rect.width / imagePixelSize.x;
            float size = pixelDiameter * unitsPerImagePixel;
            marker.sizeDelta = new Vector2(size, size);
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

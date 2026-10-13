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
        /// Đổi kích thước Content sao cho ảnh nằm GỌN trong Viewport mà KHÔNG bị méo.
        /// </summary>
        private void FitContentToViewport(Sprite sprite)
        {
            Vector2 viewportSize = viewport.rect.size;   // kích thước khung, ví dụ (1040, 800)
            Vector2 spriteSize = sprite.rect.size;       // kích thước ảnh gốc, ví dụ (1024, 768)

            // TODO BÀI TẬP 1.1 – Tính fittedSize để ảnh vừa khung mà không bị méo.
            // Gợi ý:
            //   - Có 2 cách phóng ảnh: theo chiều rộng (viewportSize.x / spriteSize.x)
            //     hoặc theo chiều cao (viewportSize.y / spriteSize.y).
            var ratioX = viewportSize.x / spriteSize.x;
            var ratioY = viewportSize.y / spriteSize.y;
            var ratio = Math.Min(ratioX, ratioY);
            //   - Muốn ảnh KHÔNG tràn ra ngoài khung thì chọn tỉ lệ nào? (Mathf.Min hay Mathf.Max?)
            //   - Nhân CẢ HAI chiều của spriteSize với cùng một tỉ lệ thì ảnh sẽ không méo.
            // Làm xong thì xoá dòng Debug.LogWarning bên dưới.
            Vector2 fittedSize = spriteSize * ratio; // TẠM THỜI: kéo giãn ảnh cho đầy khung -> ảnh bị méo
            content.sizeDelta = fittedSize;
        }
    }
}

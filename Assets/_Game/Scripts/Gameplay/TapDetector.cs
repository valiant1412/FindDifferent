using System;
using UnityEngine;

namespace FindDiff.Gameplay
{
    /// <summary>
    /// Phát hiện một cú "chạm" (tap): nhấn xuống rồi nhả ra NHANH và KHÔNG kéo đi xa.
    /// Nếu người chơi kéo ngón tay (sau này dùng để di chuyển ảnh khi zoom) thì KHÔNG tính là tap.
    /// Script này CHỈ phát hiện tap rồi báo ra ngoài qua event Tapped, không biết gì về ảnh hay level.
    /// Trên điện thoại, Input.mousePosition / GetMouseButton... vẫn chạy với 1 ngón tay
    /// (Unity tự giả lập chuột từ cảm ứng).
    /// </summary>
    public class TapDetector : MonoBehaviour
    {
        [Tooltip("Ngón tay di chuyển quá số pixel này thì coi là kéo, không phải tap.")]
        [SerializeField] private float maxMoveDistance = 25f;
        [Tooltip("Giữ lâu hơn số giây này thì không tính là tap.")]
        [SerializeField] private float maxDuration = 0.4f;

        /// <summary>Báo ra ngoài: vừa có một tap tại vị trí màn hình (pixel).</summary>
        public event Action<Vector2> Tapped;

        // Trạng thái của lần nhấn hiện tại
        private bool isPressing;
        private Vector2 pressPosition;
        private float pressTime;

        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                BeginPress(Input.mousePosition);
            }

            if (isPressing && Input.GetMouseButtonUp(0))
            {
                EndPress(Input.mousePosition);
            }
        }

        private void Start()
        {

        }

        private void BeginPress(Vector2 screenPosition)
        {
            isPressing = true;
            pressPosition = screenPosition;
            pressTime = Time.time;
        }

        private void EndPress(Vector2 screenPosition)
        {
            isPressing = false;

            float movedDistance = Vector2.Distance(pressPosition, screenPosition);
            float heldDuration = Time.time - pressTime;

            if (movedDistance > maxMoveDistance || heldDuration > maxDuration)
            {
                return; // kéo hoặc giữ lâu -> không phải tap
            }

            // "?." nghĩa là: chỉ gọi nếu đang có ai đăng ký nghe event (khác null).
            Tapped?.Invoke(screenPosition);
        }
    }
}

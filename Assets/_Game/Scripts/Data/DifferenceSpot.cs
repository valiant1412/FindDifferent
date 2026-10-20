using System;
using UnityEngine;

namespace FindDiff.Data
{
    /// <summary>
    /// MỘT điểm khác biệt = một hình tròn trên ảnh.
    /// ĐƠN VỊ: pixel của FILE ẢNH GỐC, gốc (0,0) ở GÓC TRÊN-TRÁI (giống Paint).
    /// Ví dụ ảnh 1448 x 1086: quả táo ở (355, 185), bán kính 40.
    /// [Serializable] để Unity hiện được nó trong Inspector (bên trong LevelData).
    /// Là struct vì nó chỉ là "gói dữ liệu nhỏ" (giống Vector2).
    /// </summary>
    [Serializable]
    public struct DifferenceSpot
    {
        [Tooltip("Tâm hình tròn, pixel trên ảnh gốc. (0,0) = góc trên-trái.")]
        public Vector2 center;

        [Tooltip("Bán kính, pixel trên ảnh gốc.")]
        public float radius;

        /// <summary>
        /// Điểm chạm (pixel ảnh gốc) có nằm trong hình tròn này không?
        /// extraRadius = nới rộng thêm cho ngón tay (pixel ảnh gốc).
        /// </summary>
        public bool Contains(Vector2 pixelPoint, float extraRadius)
        {
            // Mọi thứ cùng là pixel ảnh gốc -> trừ thẳng, Pytago, so thẳng.
            float dx = pixelPoint.x - center.x;
            float dy = pixelPoint.y - center.y;
            return Mathf.Sqrt(dx * dx + dy * dy) <= radius + extraRadius;
        }
    }
}

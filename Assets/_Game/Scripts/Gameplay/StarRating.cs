using FindDiff.Data;
using UnityEngine;

namespace FindDiff.Gameplay
{
    /// <summary>
    /// Tính số sao khi THẮNG một level.
    /// Luật: bắt đầu 3 sao.
    ///   - Chọn sai nhiều hơn level.starMaxMistakes  -> trừ 1 sao.
    ///   - Thời gian dùng lớn hơn level.starTimeLimit -> trừ 1 sao.
    ///   - Không bao giờ dưới 1 sao (đã thắng thì ít nhất 1 sao).
    ///
    /// static class: chỉ là một "công thức", không cần gắn vào GameObject nào.
    /// Cùng đầu vào thì luôn ra cùng kết quả -> dễ kiểm tra bằng tay.
    /// </summary>
    public static class StarRating
    {
        public const int MaxStars = 3;
        public const int MinStars = 1;

        /// <param name="level">Level vừa thắng (để đọc ngưỡng starTimeLimit, starMaxMistakes).</param>
        /// <param name="timeUsed">Số giây đã dùng để hoàn thành.</param>
        /// <param name="mistakesMade">Số lần đã chọn sai.</param>
        public static int Calculate(LevelData level, float timeUsed, int mistakesMade)
        {
            // TODO BÀI TẬP 5.3 – Tính số sao theo luật ở đầu file.
            // Gợi ý:
            //   - Khai báo biến stars bắt đầu bằng MaxStars.
            //   - Mỗi điều kiện "bị trừ" đúng thì giảm stars đi 1.
            //   - Cuối cùng không để stars nhỏ hơn MinStars: Mathf.Max(...)
            // Tự kiểm tra với level 90s, starTimeLimit = 45, starMaxMistakes = 0:
            //   (timeUsed 30, sai 0) -> 3   |  (30, 1) -> 2   |  (60, 0) -> 2   |  (70, 2) -> 1
            // Làm xong thì xoá dòng Debug.LogWarning bên dưới.
            Debug.LogWarning("Chưa làm TODO BÀI TẬP 5.3 (StarRating.Calculate)");
            return MaxStars;
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

namespace FindDiff.Data
{
    /// <summary>
    /// Dữ liệu của MỘT level, lưu thành file .asset trong Project (ScriptableObject).
    /// Chỉ chứa thông tin CỐ ĐỊNH (ảnh, vị trí điểm khác). KHÔNG lưu trạng thái lúc chơi
    /// (điểm nào đã tìm thấy...) vào đây – trạng thái đó nằm trong LevelController.
    /// </summary>
    [CreateAssetMenu(menuName = "FindDiff/Level Data", fileName = "Level_00")]
    public class LevelData : ScriptableObject
    {
        public Sprite imageA;
        public Sprite imageB;

        [Tooltip("Các điểm khác biệt, đơn vị PIXEL của ảnh gốc, gốc (0,0) ở góc trên-trái.")]
        public List<DifferenceSpot> spots = new List<DifferenceSpot>();

        [Header("Luật chơi")]
        [Tooltip("Thời gian chơi, tính bằng giây.")]
        public float timeLimit = 90f;

        [Tooltip("Số lần được chọn sai. Sai đến lần này là thua.")]
        public int maxMistakes = 3;
    }
}

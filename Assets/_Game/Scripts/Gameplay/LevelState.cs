namespace FindDiff.Gameplay
{
    /// <summary>
    /// Màn chơi đang ở trạng thái nào. Tại một thời điểm chỉ có ĐÚNG MỘT trạng thái.
    /// </summary>
    public enum LevelState
    {
        Playing,   // đang chơi: nhận chạm, đồng hồ chạy
        Won,       // đã thắng: không nhận chạm, đồng hồ dừng
        Lost       // đã thua:  không nhận chạm, đồng hồ dừng
    }
}

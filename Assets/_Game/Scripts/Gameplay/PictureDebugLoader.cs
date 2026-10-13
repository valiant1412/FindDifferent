using UnityEngine;

namespace FindDiff.Gameplay
{
    /// <summary>
    /// SCRIPT TẠM của Buổi 1: kéo 2 ảnh vào Inspector, bấm Play là thấy 2 ảnh.
    /// Buổi 3 sẽ thay bằng LevelData (dữ liệu level thật) và xoá script này.
    /// </summary>
    public class PictureDebugLoader : MonoBehaviour
    {
        [SerializeField] private PictureView pictureA;
        [SerializeField] private PictureView pictureB;
        [SerializeField] private Sprite spriteA;
        [SerializeField] private Sprite spriteB;

        private void Start()
        {
            // Dùng Start (không dùng Awake) vì lúc Start, Canvas đã tính xong kích thước các khung.
            pictureA.Show(spriteA);
            pictureB.Show(spriteB);
        }
    }
}

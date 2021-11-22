using UnityEngine;

namespace WithWhat.ClassExtision
{
    public static class RectTransformExtision
    {
        /// <summary>
        /// 获取鼠标在UI上的位置 by lyb
        /// </summary>
        /// <param name="rectTransform">参照对象</param>
        /// <param name="camera">相机,Screen Space - Camera 模式下需要传入画布指定的相机</param>
        /// <returns></returns>
        public static Vector3 GetMousePosition(this RectTransform rectTransform, Camera camera=null)
        {
            Vector2 pos = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, Input.mousePosition, camera, out pos);
            return new Vector3(pos.x, pos.y, 0);
        }

        /// <summary>
        /// 世界坐标转ui坐标
        /// </summary>
        /// <param name="canvas">画布</param>
        /// <param name="worldPos">世界坐标</param>
        /// <returns></returns>
        public static Vector2 WorldToCanvasPoint(Canvas canvas, Vector3 worldPos)
        {
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform,
                Camera.main.WorldToScreenPoint(worldPos), canvas.worldCamera, out pos);
            return pos;
        }
    }
}
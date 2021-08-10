using UnityEngine;
using ZCCUtils.ClassExtision;

public class Test : MonoBehaviour
{
    /// <summary>
    /// 默认提示是红黄蓝
    /// </summary>
    public enum ColorBoardColorType
    {
        Red,
        Yellow,
        Blue
    }

    // Start is called before the first frame update
    void Start()
    {
        var list = typeof(ColorBoardColorType).GetEnumNamesList();
        foreach (var item in list)
        {
            print(item);
        }

        ColorBoardColorType colorBoardColorType = default;
        var list1 = colorBoardColorType.GetEnumNamesList();
        foreach (var item in list1)
        {
            print(item);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
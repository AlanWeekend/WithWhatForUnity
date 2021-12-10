using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WithWhat.Utils;

public class GetRandomTypeTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        print(MathUtils.GetRandomType(new List<int>() {10,20,50 }));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// �ֲ������
    /// </summary>
    /// <param name="isProb">�Ǹ��ʻ�������</param>
    /// <param name="sectionCount">����λ</param>
    /// <param name="probParams">�����������</param>
    /// <returns></returns>
    public int GetResult(bool isProb, int min, int max, List<int> probParams)
    {
        var number = Random.Range(0, 100);
        Debug.LogError("�������������Ϊ��" + number);
        var list = new List<int>();
        if (isProb)
        {
            var whileCount = 0;

            while (whileCount < probParams.Count)
            {
                var sectionNumber = 0;
                for (int i = 0; i < whileCount; i++)
                {
                    sectionNumber += probParams[i];
                }

                list.Add(sectionNumber);

                whileCount++;
            }
        }
        else
        {
            list.Add(min);

            list.AddRange(probParams);
        }

        if (max == probParams[probParams.Count - 1])
        {
            list.Add(max);
        }

        list.ForEach((t) => { Debug.LogError(t); });

        var section = 0;

        while (true)
        {
            if (list[section] < number && number < list[section + 1])
            {
                return section;
            }

            section++;
        }
    }
}

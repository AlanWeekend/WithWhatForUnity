using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WithWhat.Utils.ImportScene;

public class EditorSceneTest : MonoBehaviour
{
    private void Awake()
    {
        ImportScene.Instance.Init(LoadPrefab, AsyncLoadPrefab,Unload);
        //ImportScene.Instance.LoadScene("Cubes");
        ImportScene.Instance.AsyncLoadScene("Sphere", go =>
        {

        });
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ImportScene.Instance.AsyncSwitchScene("Sphere", go => { });
        }
    }

    private void Unload(string prefabPath, GameObject Prefab)
    {
        Resources.UnloadAsset(Prefab);
    }

    /// <summary>
    /// 自定义资源加载方法
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private UnityEngine.Object LoadPrefab(string path)
    {
        return Resources.Load(path);
    }

    /// <summary>
    /// 自定义资源异步加载方法
    /// </summary>
    /// <param name="paths"></param>
    /// <param name="action"></param>
    private void AsyncLoadPrefab(List<string> paths,Action<List<UnityEngine.Object>> action)
    {
        StartCoroutine(AsyncLoad(paths, action));
    }
    IEnumerator AsyncLoad(List<string> paths, Action<List<UnityEngine.Object>> action)
    {
        var count = 0;
        var Objects = new List<UnityEngine.Object>();
        foreach (var path in paths)
        {
            ResourceRequest request = Resources.LoadAsync(path);
            yield return request;
            if (request.isDone)
            {
                Objects.Add(request.asset);
                count++;
                if (count == paths.Count)
                {
                    action?.Invoke(Objects);
                }
            }
        }
    }
}
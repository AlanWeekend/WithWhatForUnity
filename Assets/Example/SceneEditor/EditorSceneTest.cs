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
        ImportScene.Instance.LoadScene("Cubes");
        //ImportScene.Instance.AsyncLoadScene("Cubes", go =>
        //{

        //});
    }

    private void Unload(string prefabPath, GameObject Prefab)
    {
        Resources.UnloadAsset(Prefab);
    }

    void Start()
    {
               
    }

    private UnityEngine.Object LoadPrefab(string path)
    {
        return Resources.Load(path);
    }

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
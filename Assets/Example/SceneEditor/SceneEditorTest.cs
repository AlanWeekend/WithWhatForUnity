using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneEditorTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var index = Application.streamingAssetsPath.IndexOf("Assets");
        print(Application.streamingAssetsPath.Remove(0, index));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

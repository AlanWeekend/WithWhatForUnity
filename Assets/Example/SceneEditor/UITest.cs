using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using WithWhat.Utils.ImportScene;

public class UITest : MonoBehaviour
{
    public Text text;
    public Button button;

    private void Awake()
    {
        button.onClick.AddListener(() =>
        {
            ImportScene.Instance.UnloadScene("Cubes");
        });
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        Application.logMessageReceived += Application_logMessageReceived;
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= Application_logMessageReceived;
    }

    private void Application_logMessageReceived(string condition, string stackTrace, LogType type)
    {
        text.text+=condition+"\n";
        text.text+=stackTrace+"\n";
    }
}

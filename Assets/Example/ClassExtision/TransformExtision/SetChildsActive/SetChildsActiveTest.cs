using UnityEngine;
using WithWhat.ClassExtision;

public class SetChildsActiveTest : MonoBehaviour
{
    private void Start()
    {
        this.transform.SetChildsActive(false);
    }
}
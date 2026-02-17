using UnityEngine;

public class SimpleLogOverlay : MonoBehaviour
{
    string buf = "";
    void OnEnable() => Application.logMessageReceived += Handle;
    void OnDisable() => Application.logMessageReceived -= Handle;

    void Handle(string condition, string stackTrace, LogType type)
    {
        buf = condition + "\n" + buf;
        if (buf.Length > 2000) buf = buf.Substring(0, 2000);
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, Screen.width - 20, Screen.height - 20), buf);
    }
}

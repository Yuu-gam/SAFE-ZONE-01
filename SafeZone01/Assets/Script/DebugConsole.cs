using UnityEngine;
using System.Collections.Generic;

public class DebugConsole : MonoBehaviour
{
    private List<string> logs = new List<string>();
    private string logStr = "";
    [SerializeField] private int maxLogs = 5; // 화면에 보여줄 최대 로그 줄 수
    [SerializeField] private int fontSize = 40;  // 폰트 크기 (모바일에서 잘 보이도록 조절)
    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        string colorTag = "<color=white>";

        logs.Add($"{colorTag}{logString}</color>");

        // 최대 줄 수를 넘어가면 가장 오래된 로그 삭제
        if (logs.Count > maxLogs)
        {
            logs.RemoveAt(0);
        }

        logStr = string.Join("\n", logs);
    }

    void OnGUI()
    {
        // 화면 해상도에 맞춰 글자 크기 조절
        GUI.skin.label.fontSize = fontSize;

        // 상단에 로그 출력
        GUILayout.BeginArea(new Rect(30, 30, Screen.width - 60, 300));
        GUI.color = new Color(0, 0, 0, 0.6f);
        GUI.Box(new Rect(0, 0, Screen.width - 60, 300), "");

        GUI.color = Color.white;
        GUILayout.Label(logStr);

        GUILayout.EndArea();
    }
}

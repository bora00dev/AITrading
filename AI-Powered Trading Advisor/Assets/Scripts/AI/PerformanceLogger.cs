using UnityEngine;
using System.IO;

public static class PerformanceLogger
{
    private static string logPath = Application.dataPath + "/_Project/Logs/ai_performance_log.txt";

    public static void Log(string modelName, float duration)
    {
        try
        {
            string info = $"[{System.DateTime.Now}] Model: {modelName} | Inference Time: {duration:F2}s | " +
                          $"Platform: {SystemInfo.operatingSystem} | CPU: {SystemInfo.processorType}\n";

            string directory = Path.GetDirectoryName(logPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            File.AppendAllText(logPath, info);
            Debug.Log("AI Log Saved: " + info);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to log performance: " + e.Message);
        }
    }
}

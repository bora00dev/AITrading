using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;

public class AIAdvisor : MonoBehaviour
{
    [System.Serializable]
    private class GenerateRequest
    {
        public string model;
        public string prompt;
        public bool stream;
    }

    [System.Serializable]
    private class GenerateResponse
    {
        public string response;
    }

    public string modelName = "phi3";
    private MarketManager market;
    private UIManager ui;
    private bool requestInFlight;

    void Start()
    {
        market = FindObjectOfType<MarketManager>();
        ui = FindObjectOfType<UIManager>();
        InvokeRepeating(nameof(RequestAdvice), 5f, 10f);
    }

    void RequestAdvice()
    {
        if (requestInFlight || market == null || ui == null)
            return;

        string marketData = market.GetMarketSummary();
        ui.UpdateAIAdvice("Analyzing market data...");
        requestInFlight = true;
        StartCoroutine(GetAdvice(marketData));
    }

    IEnumerator GetAdvice(string marketData)
    {
        string prompt = $"You are an AI trading assistant. Given the market data below, provide one short trading tip:\n\n{marketData}\n\nTip:";
        var payload = new GenerateRequest
        {
            model = modelName,
            prompt = prompt,
            stream = false
        };
        string json = JsonUtility.ToJson(payload);
        byte[] body = Encoding.UTF8.GetBytes(json);

        using (UnityWebRequest req = new UnityWebRequest("http://localhost:11434/api/generate", "POST"))
        {
            req.uploadHandler = new UploadHandlerRaw(body);
            req.downloadHandler = new DownloadHandlerBuffer();
            req.SetRequestHeader("Content-Type", "application/json");

            float start = Time.time;
            yield return req.SendWebRequest();
            float duration = Time.time - start;

            if (req.result == UnityWebRequest.Result.Success)
            {
                string result = req.downloadHandler.text;
                string advice = ExtractAdvice(result);
                ui.UpdateAIAdvice(advice);
                PerformanceLogger.Log(modelName, duration);
            }
            else
            {
                string message = string.IsNullOrEmpty(req.downloadHandler?.text)
                    ? req.error
                    : $"{req.responseCode}: {req.downloadHandler.text}";
                ui.UpdateAIAdvice("AI Error: " + message);
            }

            requestInFlight = false;
        }
    }

    private string ExtractAdvice(string payload)
    {
        if (string.IsNullOrEmpty(payload))
            return "No advice received.";

        try
        {
            var response = JsonUtility.FromJson<GenerateResponse>(payload);
            if (!string.IsNullOrEmpty(response.response))
                return response.response.Trim();
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("Failed to parse AI response: " + e.Message);
        }

        return payload.Trim();
    }
}

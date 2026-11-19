using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WeatherManager
{
    private const string orlandoApi = "http://api.openweathermap.org/data/2.5/weather?lat=28.54&lon=-81.38&mode=xml&appid=e50998af823151e941eb919d65f53b0d";
    private const string newYorkApi = "http://api.openweathermap.org/data/2.5/weather?lat=40.73&lon=-73.94&mode=xml&appid=e50998af823151e941eb919d65f53b0d";
    private const string londonApi = "http://api.openweathermap.org/data/2.5/weather?lat=51.51&lon=-0.12&mode=xml&appid=e50998af823151e941eb919d65f53b0d";
    private const string beijingApi = "http://api.openweathermap.org/data/2.5/weather?lat=39.92&lon=116.38&mode=xml&appid=e50998af823151e941eb919d65f53b0d";
    private const string seattleApi = "http://api.openweathermap.org/data/2.5/weather?lat=47.61&lon=-122.34&mode=xml&appid=e50998af823151e941eb919d65f53b0d";

    private IEnumerator CallAPI(string url, Action<string> callback)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError($"network problem: {request.error}");
            }
            else if (request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"response error: {request.responseCode}");
            }
            else
            {
                callback(request.downloadHandler.text);
            }
        }
    }

    public IEnumerator GetWeatherXML(Action<string> callback, string cityName)
    {
        switch (cityName.ToLower())
        {
            case "newyork":
            case "new york":
                return CallAPI(newYorkApi, callback);
            case "london":
                return CallAPI(londonApi, callback);
            case "beijing":
                return CallAPI(beijingApi, callback);
            case "seattle":
                return CallAPI(seattleApi, callback);
            default:
                return CallAPI(orlandoApi, callback);
        }
    }
}
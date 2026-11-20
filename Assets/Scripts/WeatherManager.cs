using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WeatherManager
{
    private const string orlandoApi = "http://api.openweathermap.org/data/2.5/weather?lat=28.54&lon=-81.38&mode=xml&appid=e50998af823151e941eb919d65f53b0d"; //URL for Orlando's weather data
    private const string newYorkApi = "http://api.openweathermap.org/data/2.5/weather?lat=40.73&lon=-73.94&mode=xml&appid=e50998af823151e941eb919d65f53b0d"; //URL for New York's weather data
    private const string londonApi = "http://api.openweathermap.org/data/2.5/weather?lat=51.51&lon=-0.12&mode=xml&appid=e50998af823151e941eb919d65f53b0d"; //URL for London's weather data
    private const string beijingApi = "http://api.openweathermap.org/data/2.5/weather?lat=39.92&lon=116.38&mode=xml&appid=e50998af823151e941eb919d65f53b0d"; //URL for Beijing's weather data
    private const string seattleApi = "http://api.openweathermap.org/data/2.5/weather?lat=47.61&lon=-122.34&mode=xml&appid=e50998af823151e941eb919d65f53b0d"; //URL for seattle's weather data

    //Function to grab the information from the API of the weather data website
    private IEnumerator CallAPI(string url, Action<string> callback)
    {
        //Using Unity's web request strea, request information from the URL
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest(); //Wait until the web request has the information
            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError($"network problem: {request.error}"); //Error handling for a connection error
            }
            else if (request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"response error: {request.responseCode}"); //Error handling for a protocol error
            }
            else
            {
                callback(request.downloadHandler.text); //Return the text obtained from the download handler
            }
        }
    }

    public IEnumerator GetWeatherXML(Action<string> callback, string cityName)
    {
        //Switch case statement to determine which city to return data for
        switch (cityName.ToLower())
        {
            case "newyork": //Intended for New York, New York. Seems to return Long Island City instead
            case "new york":
                return CallAPI(newYorkApi, callback);
            case "london": //For london england
                return CallAPI(londonApi, callback);
            case "beijing": //Intended for Bejing China. Returns Chang-Qia which as far as I can tell is in Bejing
                return CallAPI(beijingApi, callback);
            case "seattle": //For Seattle
                return CallAPI(seattleApi, callback);
            default: //Otherwise return data for Orlando
                return CallAPI(orlandoApi, callback);
        }
    }
}
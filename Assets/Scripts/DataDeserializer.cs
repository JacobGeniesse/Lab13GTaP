using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.IO;
using System.Linq;
using System;

public class DataDeserializer : MonoBehaviour
{
    public static DataDeserializer instance; //Singleton instance

    //Singleton setup
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }


    //Public function for deserializing the data recieved from the weather service
    public WeatherData DeserializeData(string xmlString)
    {
        var rootElement = XElement.Parse(xmlString); //Parse the string data given to the func

        //Debug.Log(xmlString); //Debug line

        string cityName = "null"; //Returned name of the city in question

        foreach(var cityAspect in rootElement.Elements("city"))
        {
            cityName = cityAspect.Attribute("name").Value.ToString(); //Find the city name as a part of the stored city class in the xml
        }

        string weatherName = "null"; //Returned name of the active weather pattern

        foreach (var weatherType in rootElement.Elements("weather"))
        {
            weatherName = weatherType.Attribute("value").Value.ToString(); //Find the documented weather pattern as a part of the weather class in the xml
        }

        XElement cityElement = rootElement.Element("city"); //grab the root element of the city

        XElement timeZoneValue = cityElement.Element("timezone"); //from that data grab the timezone

        //Debug.Log(weatherName); //Debug line

        float tVVal = float.Parse(timeZoneValue.Value.ToString()); //Transform the timezone's string into a float for returning

        WeatherData returnedData = new WeatherData(cityName, weatherName, tVVal); //Create the Weather Data class to return

        return returnedData;
    }
}


//Class to return to other functions
[System.Serializable]
public class WeatherData
{
    public string cityName; //Name of the city
    public string weather; //weather name of the city
    public float timeZone; //Time zone the city is in

    //Empty Constructor
    public WeatherData(){}

    //Constructor with the name, weather pattern, and timezone
    public WeatherData(string name, string incomingWeather, float newTimeZone)
    {
        cityName = name;
        weather = incomingWeather;
        timeZone = newTimeZone;
    }
}
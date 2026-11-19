using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.IO;
using System.Linq;
using NUnit.Framework.Interfaces;
using Unity.VisualScripting;
using System;

public class DataDeserializer : MonoBehaviour
{
    public static DataDeserializer instance;

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



    public WeatherData DeserializeData(string xmlString)
    {
        var rootElement = XElement.Parse(xmlString);
        //XmlSerializer serializer = new XmlSerializer(typeof(WeatherData));
        Debug.Log(xmlString);
        XElement xmlElement = rootElement.Element("city");

        string weatherName = "null";

        foreach (var weatherType in rootElement.Elements("weather"))
        {
            weatherName = weatherType.Attribute("value").Value.ToString();
        }

        XElement timeZoneValue = xmlElement.Element("timezone");

        Debug.Log(weatherName);

        float tVVal = float.Parse(timeZoneValue.Value.ToString());

        WeatherData returnedData = new WeatherData(weatherName, tVVal);

        return returnedData;
    }
}

[System.Serializable]
public class WeatherData
{
    public string weather;
    public float timeZone;


    public WeatherData()
    {

    }

    public WeatherData(string incomingWeather, float newTimeZone)
    {
        weather = incomingWeather;
        timeZone = newTimeZone;
    }
}
using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class WeatherLoader : MonoBehaviour
{
    private WeatherLoader instance;

    private WeatherManager weatherManager;

    private Transform directionalLight;
    private Light sun;
    private Skybox skybox;

    [SerializeField] private Material[] skyboxMatDay;
    [SerializeField] private Material[] skyboxMatNight;

    private WeatherData[] togglableCities = new WeatherData[5];

    [SerializeField, Range(0, 4)] private int currentCity;

    private int currentData = 0;

    //private string testString = "<current>\r\n<city id=\"4167147\" name=\"Orlando\">\r\n<coord lon=\"-81.38\" lat=\"28.54\"/>\r\n<country>US</country>\r\n<timezone>-18000</timezone>\r\n<sun rise=\"2026-11-19T11:51:23\" set=\"2026-11-19T22:30:46\"/>\r\n</city>\r\n<temperature value=\"295.13\" min=\"293.63\" max=\"295.99\" unit=\"kelvin\"/>\r\n<feels_like value=\"295.73\" unit=\"kelvin\"/>\r\n<humidity value=\"90\" unit=\"%\"/>\r\n<pressure value=\"1019\" unit=\"hPa\"/>\r\n<wind>\r\n<speed value=\"5.66\" unit=\"m/s\" name=\"Moderate breeze\"/>\r\n<gusts/>\r\n<direction value=\"60\" code=\"ENE\" name=\"East-northeast\"/>\r\n</wind>\r\n<clouds value=\"100\" name=\"overcast clouds\"/>\r\n<visibility value=\"10000\"/>\r\n<precipitation mode=\"no\"/>\r\n<weather number=\"804\" value=\"overcast clouds\" icon=\"04d\"/>\r\n<lastupdate value=\"2026-11-19T17:06:10\"/>\r\n</current>";
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


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        directionalLight = GameObject.Find("Directional Light").GetComponent<Transform>();
        sun = directionalLight.GetComponent<Light>();


        skybox = this.GetComponent<Skybox>();
        weatherManager = new WeatherManager();
        StartCoroutine(weatherManager.GetWeatherXML(OnXMLDataLoaded, "orlando"));
        StartCoroutine(weatherManager.GetWeatherXML(OnXMLDataLoaded, "newyork"));
        StartCoroutine(weatherManager.GetWeatherXML(OnXMLDataLoaded, "london"));
        StartCoroutine(weatherManager.GetWeatherXML(OnXMLDataLoaded, "beijing"));
        StartCoroutine(weatherManager.GetWeatherXML(OnXMLDataLoaded, "seattle"));

        //SetSkyBox(togglableCities[0]);
    }

    public void OnXMLDataLoaded(string data)
    {
        WeatherData incomingdata = DataDeserializer.instance.DeserializeData(data);
        togglableCities[currentData] = incomingdata;

        if(currentData < togglableCities.Length - 1)
        {
            currentData++;
        }
        else
        {
            EditorButton();
        }
    }

    private void SetSkyBox(WeatherData skyToSet)
    {
        Debug.Log($"Current City: {skyToSet.cityName} \n Current Weather: {skyToSet.weather} \n");

        int chosenInt = 0;
        switch (skyToSet.weather.ToLower())
        {
            case "clear":
                chosenInt = 0;
                break;
            case "clouds":
                chosenInt = 1;
                break;
            case "drizzle":
                chosenInt = 2;
                break;
            case "rain":
                chosenInt = 3;
                break;
            case "thunderstorm":
                chosenInt = 4;
                break;
            case "snow":
                chosenInt = 5;
                break;
            case "mist":
                chosenInt = 6;
                break;
            case "haze":
                chosenInt = 7;
                break;
            default:
                chosenInt = 0;
                break;
        }

        
        //Debug.Log(DateTime.Now.Hour);

        float finalNum = (DateTime.UtcNow.Hour + (skyToSet.timeZone / 3600));
        //Debug.Log(DateTime.UtcNow.Hour + " " + skyToSet.timeZone);
        directionalLight.rotation = Quaternion.Euler(finalNum, 0, 0);

        float sliderNum = finalNum - 12;

        if (sliderNum > 0)
        {
            sun.intensity = 2 + (sliderNum / 3);
        }
        else
        {
            sun.intensity = 2 + ((sliderNum + 12) / 3);
        }

        if((finalNum >= 4 && finalNum <= 5) || (finalNum >= 19 && finalNum <= 20))
        {
            sun.color = Color.orangeRed;
        }
        if(finalNum >= 6 && finalNum <= 18)
        {
            sun.color = Color.white;
            if(skyboxMatDay.Length > chosenInt)
            {
                skybox.material = skyboxMatDay[chosenInt];
            }
        }
        else
        {
            sun.color = Color.blue;
            if(skyboxMatNight.Length > chosenInt)
            {
                skybox.material = skyboxMatNight[chosenInt];
            }
        }
    }


    public void EditorButton()
    {
        SetSkyBox(togglableCities[currentCity]);
    }
}

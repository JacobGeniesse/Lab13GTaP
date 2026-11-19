using UnityEngine;
using System;
using System.Linq;

public class WeatherLoader : MonoBehaviour
{
    private WeatherLoader instance;

    private WeatherManager weatherManager;

    private Transform directionalLight;
    private Skybox skybox;

    [SerializeField] private Material[] skyboxMatDay;
    [SerializeField] private Material[] skyboxMatNight;

    [SerializeField] private string weather = "Rain";

    private WeatherData[] togglableCities = new WeatherData[5];

    private string testString = "<current>\r\n<city id=\"4167147\" name=\"Orlando\">\r\n<coord lon=\"-81.38\" lat=\"28.54\"/>\r\n<country>US</country>\r\n<timezone>-18000</timezone>\r\n<sun rise=\"2026-11-19T11:51:23\" set=\"2026-11-19T22:30:46\"/>\r\n</city>\r\n<temperature value=\"295.13\" min=\"293.63\" max=\"295.99\" unit=\"kelvin\"/>\r\n<feels_like value=\"295.73\" unit=\"kelvin\"/>\r\n<humidity value=\"90\" unit=\"%\"/>\r\n<pressure value=\"1019\" unit=\"hPa\"/>\r\n<wind>\r\n<speed value=\"5.66\" unit=\"m/s\" name=\"Moderate breeze\"/>\r\n<gusts/>\r\n<direction value=\"60\" code=\"ENE\" name=\"East-northeast\"/>\r\n</wind>\r\n<clouds value=\"100\" name=\"overcast clouds\"/>\r\n<visibility value=\"10000\"/>\r\n<precipitation mode=\"no\"/>\r\n<weather number=\"804\" value=\"overcast clouds\" icon=\"04d\"/>\r\n<lastupdate value=\"2026-11-19T17:06:10\"/>\r\n</current>";
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
        skybox = this.GetComponent<Skybox>();
        //weatherManager = new WeatherManager();
        //StartCoroutine(weatherManager.GetWeatherXML(OnXMLDataLoaded, "orlando"));

        togglableCities[0] = OnXMLDataLoaded(testString);
        SetSkyBox(togglableCities[0]);
    }

    public WeatherData OnXMLDataLoaded(string data)
    {
        WeatherData incomingdata = DataDeserializer.instance.DeserializeData(data);
        //Debug.Log(incomingdata.timeZone);
        return incomingdata;
    }

    private void SetSkyBox(WeatherData skyToSet)
    {
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
            default:
                chosenInt = 0;
                break;
        }

        
        Debug.Log(DateTime.Now.Hour);

        float finalNum = (DateTime.UtcNow.Hour + (skyToSet.timeZone / 3600)) * 15;
        directionalLight.rotation = Quaternion.Euler(finalNum, 0, 0);

        if(DateTime.Now.Hour >= 6 && DateTime.Now.Hour <= 18)
        {
            if(skyboxMatDay.Length > chosenInt)
            {
                skybox.material = skyboxMatDay[chosenInt];
            }
        }
        else
        {
            if(skyboxMatNight.Length > chosenInt)
            {
                skybox.material = skyboxMatNight[chosenInt];
            }
        }
    }

}

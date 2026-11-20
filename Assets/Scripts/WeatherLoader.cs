using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class WeatherLoader : MonoBehaviour
{
    private WeatherLoader instance; //singleton instance

    //Singleton setup
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private WeatherManager weatherManager; //Weather manager to pull data from

    private Transform directionalLight; //Transform of the directional light
    private Light sun; //Reference to the directional light of the sun
    private Skybox skybox; //Reference to the skybox component

    [Tooltip("Skyboxes for the daytime environment")]
    [SerializeField] private Material[] skyboxMatDay;

    [Tooltip("Skyboxes for the nighttime environment")]
    [SerializeField] private Material[] skyboxMatNight;

    private WeatherData[] togglableCities = new WeatherData[5]; //Array for storing the city data to toggle between

    [Tooltip("Currently selected city")]
    [SerializeField, Range(0, 4)] private int currentCity;
    private int currentData = 0; //Current data entry to grab

    //String used for testing to avoid having to call the website repeatedly (Limited number of uses)
    //private string testString = "<current>\r\n<city id=\"4167147\" name=\"Orlando\">\r\n<coord lon=\"-81.38\" lat=\"28.54\"/>\r\n<country>US</country>\r\n<timezone>-18000</timezone>\r\n<sun rise=\"2026-11-19T11:51:23\" set=\"2026-11-19T22:30:46\"/>\r\n</city>\r\n<temperature value=\"295.13\" min=\"293.63\" max=\"295.99\" unit=\"kelvin\"/>\r\n<feels_like value=\"295.73\" unit=\"kelvin\"/>\r\n<humidity value=\"90\" unit=\"%\"/>\r\n<pressure value=\"1019\" unit=\"hPa\"/>\r\n<wind>\r\n<speed value=\"5.66\" unit=\"m/s\" name=\"Moderate breeze\"/>\r\n<gusts/>\r\n<direction value=\"60\" code=\"ENE\" name=\"East-northeast\"/>\r\n</wind>\r\n<clouds value=\"100\" name=\"overcast clouds\"/>\r\n<visibility value=\"10000\"/>\r\n<precipitation mode=\"no\"/>\r\n<weather number=\"804\" value=\"overcast clouds\" icon=\"04d\"/>\r\n<lastupdate value=\"2026-11-19T17:06:10\"/>\r\n</current>";


    void Start()
    {
        //Assign a few components
        directionalLight = GameObject.Find("Directional Light").GetComponent<Transform>();
        sun = directionalLight.GetComponent<Light>();
        skybox = this.GetComponent<Skybox>();


        weatherManager = new WeatherManager(); //Create a new weather manager

        //Gather data from all of the cities that are desired
        StartCoroutine(weatherManager.GetWeatherXML(OnXMLDataLoaded, "orlando"));
        StartCoroutine(weatherManager.GetWeatherXML(OnXMLDataLoaded, "newyork"));
        StartCoroutine(weatherManager.GetWeatherXML(OnXMLDataLoaded, "london"));
        StartCoroutine(weatherManager.GetWeatherXML(OnXMLDataLoaded, "beijing"));
        StartCoroutine(weatherManager.GetWeatherXML(OnXMLDataLoaded, "seattle"));
    }

    //Function called to set the city data
    public void OnXMLDataLoaded(string data)
    {
        WeatherData incomingdata = DataDeserializer.instance.DeserializeData(data); //Deserialize the incoming information
        togglableCities[currentData] = incomingdata; //for the current slot overwrite it with the incoming data

        if(currentData < togglableCities.Length - 1)
        {
            currentData++; //If there are still more cities to go update the currentData number
        }
        else
        {
            EditorButton(); //If not update the skybox
        }
    }

    //Function for setting the skybox
    private void SetSkyBox(WeatherData skyToSet)
    {
        //Print the city name and its weather condition
        Debug.Log($"Current City: {skyToSet.cityName} \n Current Weather: {skyToSet.weather} \n");

        int chosenInt = 0; //Integer for setting weather value
        //Use a switch statement with the various weather conditions to set that value
        switch (skyToSet.weather.ToLower())
        {
            case "clear sky":
            case "clearsky":
            case "clear":
                chosenInt = 0;
                break;
            case "few clouds":
            case "fewclouds":
            case "brokenclouds":
            case "broken clouds":
            case "scattered clouds":
            case "scatteredclouds":
            case "clouds":
                chosenInt = 1;
                break;
            case "overcastclouds":
            case "overcast clouds":
            case "showerrain":
            case "shower rain":
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

        float finalNum = (DateTime.UtcNow.Hour + (skyToSet.timeZone / 3600)); //Calculate the current time for the location with the timezone offset

        directionalLight.rotation = Quaternion.Euler(finalNum * 15, 0, 0); //rotate the directional light to match

        float sliderNum = finalNum - 12; //Calculate the number for controlling the sun's intensity

        //Statement for handling the intensity of the sun
        if (sliderNum > 0)
        {
            sun.intensity = 2 + (sliderNum / 3);
        }
        else
        {
            sun.intensity = 2 + ((sliderNum + 12) / 3);
        }

        //Statement for handling the skybox and the color of the sun
        if((finalNum >= 4 && finalNum <= 5) || (finalNum >= 19 && finalNum <= 20))
        {
            sun.color = Color.orangeRed; //Sunset/Sunrise colors
            if (skyboxMatDay.Length > chosenInt)
            {
                /*If the pack had more variants of sunsets I'd have a separate list for that
                 * However for simplicity's sake I am just using the standard daytime sky
                 */
                skybox.material = skyboxMatDay[chosenInt];
            }
        }
        else if(finalNum >= 6 && finalNum <= 18)
        {
            sun.color = Color.white; //daytime colors
            if(skyboxMatDay.Length > chosenInt)
            {
                skybox.material = skyboxMatDay[chosenInt]; //daytime skybox
            }
        }
        else
        {
            sun.color = Color.blue; //nighttime colors
            if(skyboxMatNight.Length > chosenInt)
            {
                skybox.material = skyboxMatNight[chosenInt]; //nighttime skybox
            }
        }
    }

    //Function called by the inspector to toggle the current skybox
    public void EditorButton()
    {
        SetSkyBox(togglableCities[currentCity]);
    }
}

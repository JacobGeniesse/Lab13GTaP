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
        SetSkyBox();
        //weatherManager = new WeatherManager();
        //StartCoroutine(weatherManager.GetWeatherXML(OnXMLDataLoaded));
    }

    public void OnXMLDataLoaded(string data)
    {
      //Debug.Log(data);
    }

    private void SetSkyBox()
    {
        int chosenInt = 0;
        switch (weather.ToLower())
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
        directionalLight.rotation = Quaternion.Euler(DateTime.Now.Hour * 15, 0, 0);

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

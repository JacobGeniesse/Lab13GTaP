using UnityEngine;

public class WeatherLoader : MonoBehaviour
{
    private WeatherLoader instance;

    private WeatherManager weatherManager;

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
        weatherManager = new WeatherManager();
        StartCoroutine(weatherManager.GetWeatherXML(OnXMLDataLoaded));
    }

    public void OnXMLDataLoaded(string data)
    {
      Debug.Log(data);
    }
}

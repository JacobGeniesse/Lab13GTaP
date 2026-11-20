using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WeatherLoader))]
public class WeatherLoaderEditor : Editor
{
    private WeatherLoader weatherLoader; //targetted weather loader

    //on enabling this grab the targetted weather loader
    private void OnEnable()
    {
        weatherLoader = (WeatherLoader)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        //When pressing the inspector button update the stored weather pattern
        if(GUILayout.Button("Update Current Weather"))
        {
            weatherLoader.EditorButton();
        }
    }
}

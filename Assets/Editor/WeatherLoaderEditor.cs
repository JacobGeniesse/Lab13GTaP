using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WeatherLoader))]
public class WeatherLoaderEditor : Editor
{
    private WeatherLoader weatherLoader;

    private void OnEnable()
    {
        weatherLoader = (WeatherLoader)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(GUILayout.Button("Update Current Weather"))
        {
            weatherLoader.EditorButton();
        }
    }
}

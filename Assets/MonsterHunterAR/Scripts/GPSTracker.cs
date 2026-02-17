using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class GPSTracker : MonoBehaviour
{
    public static GPSTracker instance;
    //cambiar por las coordenadas donde quieras q salga el objeto
    public double targetLat = 37.19651608988916; //latitud 37.19651608988916, -3.6216332462451146
    public double targetLon = -3.6216332462451146; //longitud
    //
    public float detectionRadius = 2f; //Radio de detección en metros
    private bool isSpawned = false; //Para evitar múltiples spawns
    public GameObject pokemonPrefab;
    private GameObject spawnedObject;
    public ARRaycastManager raycastManager;

    public double currentLat;
    public double currentLon;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (!Input.location.isEnabledByUser) return;
        UIManager.Instance.MostrarMensaje("Ejecuta Start");
        Input.location.Start();
        Input.compass.enabled = true;
    }

    private void Update()
    {
        UIManager.Instance.MostrarMensaje(Input.location.status.ToString());
        if(Input.location.status == LocationServiceStatus.Running)
        {
            currentLat = Input.location.lastData.latitude;
            currentLon = Input.location.lastData.longitude;

            double distance = CalculateDistance(currentLat, currentLon, targetLat, targetLon);
            if(distance <= detectionRadius && !isSpawned)
            {
                UIManager.Instance.MostrarMensaje("¡Has encontrado un objeto Iniciando RA...");
                //Aquí activarías tu modelo 3d o cambiarias de escena
                SpawnObjectInAR_Plane();
            } else
            {
                UIManager.Instance.MostrarMensaje($"GPS Móvil: { currentLat}, {currentLon} Target (Google): {targetLat}, {targetLon} Distancia: {distance}");
            }
        }
    }

    //Formula de Haversine para calcular distancia en metros
    double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
    {
        double R = 6371000; // Radio de la Tierra en metros
        double dLat = ToRadians(lat2 - lat1);
        double dLon = ToRadians(lon2 - lon1);
        double a = Mathf.Sin((float)dLat / 2) * Mathf.Sin((float)dLat / 2) +
                   Mathf.Cos((float)ToRadians(lat1)) * Mathf.Cos((float)ToRadians(lat2)) *
                   Mathf.Sin((float)dLon / 2) * Mathf.Sin((float)dLon / 2);

        double c = 2 * Mathf.Atan2(Mathf.Sqrt((float)a), Mathf.Sqrt((float)(1 - a)));
        return R * c; // Distancia en metros
    }

    double ToRadians(double degrees) => degrees * Math.PI / 180;

    void SpawnObjectInAR()
    {
        //Posicionamos el objeto 2 metros frente a la camara de RA
        Vector3 spawnPosition = Camera.main.transform.position + Camera.main.transform.forward * 2f;

        //Ajustamos la altura al suelo 
        spawnPosition.y = -1.5f;

        spawnedObject = Instantiate(pokemonPrefab, spawnPosition, Quaternion.identity);
        isSpawned = true;
    }

    void SpawnObjectInAR_Plane()
    {
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        if (raycastManager.Raycast(
            new Vector2(Screen.width / 2, Screen.height / 2),
            hits,
            UnityEngine.XR.ARSubsystems.TrackableType.Planes))
        {
            Instantiate(pokemonPrefab, hits[0].pose.position, Quaternion.identity);
            isSpawned = true;
        }
    }
}

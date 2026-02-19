using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class GPSTracker : MonoBehaviour
{
    public static GPSTracker instance;

    public int TargetActual = 0; //indice del monstruo actual, sirve para latitud como longitud como pokemon

    [Header("Monstruos")]
    //Monstruo 1
    public double targetLat1;
    public double targetLon1;
    //Monstruo 2
    public double targetLat2;
    public double targetLon2;
    //Monstruo 3
    public double targetLat3;
    public double targetLon3;
    //Monstruo 4
    public double targetLat4;
    public double targetLon4;


    public float detectionRadius = 20f; //Radio de detección en metros
    private bool isSpawned = false; //Para evitar múltiples spawns

    public double[] monsterLat;
    
    public double[] monsterLon; 
    public GameObject[] pokemonPrefabs;
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

        //inicializamos ls arrays
        monsterLat = new double[] { instance.targetLat1, instance.targetLat2, instance.targetLat3, instance.targetLat3, instance.targetLat4 };
        monsterLon = new double[] { instance.targetLon1, instance.targetLon2, instance.targetLon3, instance.targetLon3, instance.targetLon4 };

        TargetActual = 0; //Iniciamos con el primer monstruo
        
    }

    private void Update()
    {
        UIManager.Instance.MostrarMensaje(Input.location.status.ToString());
        if(Input.location.status == LocationServiceStatus.Running)
        {
            currentLat = Input.location.lastData.latitude;
            currentLon = Input.location.lastData.longitude;

            double distance = CalculateDistance(currentLat, currentLon, monsterLat[TargetActual], monsterLon[TargetActual]);
            if(distance <= detectionRadius && !isSpawned)
            {
                UIManager.Instance.MostrarMensaje("¡Has encontrado un objeto Iniciando RA...");
                //Aquí activarías tu modelo 3d o cambiarias de escena
                SpawnObjectInAR_Plane();
            } else
            {
                UIManager.Instance.MostrarMensaje($"GPS Móvil: { currentLat}, {currentLon} Target (Google): {monsterLat[TargetActual]}, {monsterLon[TargetActual]} Distancia: {distance}");
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

        spawnedObject = Instantiate(pokemonPrefabs[TargetActual], spawnPosition, Quaternion.identity);
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

            Instantiate(pokemonPrefabs[TargetActual], hits[0].pose.position, Quaternion.identity);
            isSpawned = true;

            //CUANDO SE DERROTE SE CAMBIA AL SIGUIENTE TARGET
        }
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    [Header("Paneles del Menu")]
    [SerializeField] private GameObject panelOpciones; // Panel de opciones
    [SerializeField] private GameObject panelMenuPrincipal; // Panel del menu principal

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex; // Obtener el índice de la escena actual
        if(sceneIndex == 0)
        {
            panelMenuPrincipal.SetActive(true); // Mostrar el panel del menú principal
            panelOpciones.SetActive(false); // Ocultar el panel de opciones
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AbrirOpciones()
    {
        if(panelMenuPrincipal != null)
        {
            panelMenuPrincipal.SetActive(false); // Ocultar el panel del menú principal
        }
        panelOpciones.SetActive(true); // Mostrar el panel de opciones
    }

    public void VolverAlMenu()
    {
        panelMenuPrincipal.SetActive(true); // Activar el panel del menu principal
        panelOpciones.SetActive(false); // Desactivar el panel de opciones
    }

    public void Salir()
    {
        Debug.Log("Saliendo del juego...");

        // Directiva de preprocesador
        #if UNITY_EDITOR
                // Si estamos en el editor de Unity, usamos el comando para detener el juego.
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                                        // Si estamos en un ejecutable (Build), cerramos la aplicación.
                                        Application.Quit();
        #endif
    }

    public void Cargar() // Cargar la escena del nivel
    {
        SceneManager.LoadScene("EscenaJuego");
    }

    public void Tutorial()
    {
        SceneManager.LoadScene("EscenaTutorial");
    }

    public void VolverAMenuPrincipal() // Volver al menu principal
    {
        SceneManager.LoadScene("EscenaMenu");
    }

}

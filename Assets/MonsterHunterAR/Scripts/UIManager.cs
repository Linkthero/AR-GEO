using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public TMP_Text textoGameOver;

    private void Awake()
    {
        Instance = this;
        textoGameOver.text = "";
    }

    public void MostrarGameOver()
    {
        textoGameOver.text = "CHOQUE";
    }

    public void MostrarYAW(float yaw)
    {
        textoGameOver.text = yaw.ToString();
    }

    public void MostrarMensaje(string msj)
    {
        textoGameOver.text = msj;
    }
}

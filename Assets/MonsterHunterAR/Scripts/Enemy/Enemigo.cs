using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class Enemigo : MonoBehaviour
{

    [SerializeField] private int vida;
    public GameObject particle;
    private Animator animator;
    [SerializeField] public TextMeshProUGUI txtVida;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        vida = 15;
        animator = GetComponent<Animator>();
        animator.SetInteger("vidas", vida);
        txtVida = GameObject.Find("txtVida").GetComponent<TextMeshProUGUI>();
    }

    public void GetHurt()
    {
        
        vida--;
        animator.SetInteger("vidas", vida);
        
        if (vida <= 0)
        {
            Death();
            Destroy(gameObject, 4f); // Destruye el enemigo después de 2 segundo para permitir que la animación de muerte se reproduzca
        } else
        {
            animator.SetTrigger("Hit");
        }          
    }

    public void Death()
    {
        animator.SetTrigger("Death");
        GameObject.Find("GPSTracker").GetComponent<GPSTracker>().isSpawned = false; // Permite que el monstruo vuelva a aparecer después de morir
        txtVida.text = "";
    }

    private void Update()
    {
        txtVida.text = "Vida:" + vida.ToString();

        if(Camera.main != null)
        {
            Vector3 direction = Camera.main.transform.position - transform.position;
            direction.y = 0f; //evita q mire hacia arriba o abajo
            if(direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }
        }
    }

}


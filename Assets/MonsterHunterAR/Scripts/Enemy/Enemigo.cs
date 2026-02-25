using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class Enemigo : MonoBehaviour
{

    [SerializeField] private int vida;
    public GameObject particle;
    private Animator animator;
    [SerializeField] private TextMeshProUGUI txtVida;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        vida = 15;
        animator = GetComponent<Animator>();
        animator.SetInteger("vidas", vida);
    }

    public void GetHurt()
    {
        
        vida--;
        animator.SetInteger("vidas", vida);
        animator.SetTrigger("Hit");
        if (vida <= 0)
        {
            Death();
            Destroy(gameObject, 2f); // Destruye el enemigo después de 2 segundo para permitir que la animación de muerte se reproduzca
        }
        

    }

    public void Death()
    {
        animator.SetTrigger("Death");
    }

    private void Update()
    {
        txtVida.text = vida.ToString();
    }

}


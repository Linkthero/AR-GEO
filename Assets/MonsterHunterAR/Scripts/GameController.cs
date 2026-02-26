using TMPro;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class GameController : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //detecta toques de pantalla       
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                // Construct a ray from the current touch coordinates
                var ray = Camera.main.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    Enemigo e = hit.transform.GetComponent<Enemigo>();
                    e.GetHurt();
                }
            }
        }
    }
}

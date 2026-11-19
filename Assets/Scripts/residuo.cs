using UnityEngine;

public class Residuo : MonoBehaviour
{
    public int tipoResiduo = 1;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        tipoResiduo = Random.Range(1, 4);

        if (tipoResiduo == 1) spriteRenderer.color = Color.green;
        else if (tipoResiduo == 2) spriteRenderer.color = Color.cyan;
        else if (tipoResiduo == 3) spriteRenderer.color = Color.red;
    }

    void Update()
    {
        if (transform.position.y < -6f)
        {
            if (GameManager.instancia != null) GameManager.instancia.RestarVida();
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D otro)
    {
        if (otro.CompareTag("Jugador"))
        {
            ControlJugador jugador = otro.GetComponent<ControlJugador>();
            if (jugador != null)
            {
                if (jugador.tipoTachoActual == tipoResiduo)
                {
                    if (GameManager.instancia != null) GameManager.instancia.SumarPuntos(10);
                }
                else
                {
                    if (GameManager.instancia != null) GameManager.instancia.RestarVida();
                }
            }
            Destroy(gameObject);
        }
    }
}
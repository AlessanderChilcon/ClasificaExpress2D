using UnityEngine;

public class Residuo : MonoBehaviour
{
    public enum TipoResiduo
    {
        Organico,
        Reciclable,
        NoAprovechable,
        VidaExtra,
        CamaraLenta,
        Bomba
    }

    public TipoResiduo tipo;
    public float velocidadBase = 3f;
    public float limiteInferior = -5.5f;
    public float velocidadGiro = 60f;

    void Start()
    {
        // Cada objeto gira en un sentido aleatorio al aparecer
        if (Random.value > 0.5f) velocidadGiro = -velocidadGiro;
    }

    void Update()
    {
        float nivelActual = (GameManager.instancia != null) ? GameManager.instancia.nivel : 1;
        float factorLento = (GameManager.instancia != null) ? GameManager.instancia.factorVelocidad : 1f;

        float velocidadFinal = (velocidadBase + (nivelActual - 1) * 0.4f) * factorLento;
        transform.Translate(Vector3.down * velocidadFinal * Time.deltaTime, Space.World);
        transform.Rotate(0f, 0f, velocidadGiro * factorLento * Time.deltaTime);

        if (transform.position.y < limiteInferior)
        {
            if (tipo == TipoResiduo.Organico || tipo == TipoResiduo.Reciclable || tipo == TipoResiduo.NoAprovechable)
            {
                if (GameManager.instancia != null) GameManager.instancia.RestarVida();
            }
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D otro)
    {
        ControlJugador jugador = otro.GetComponent<ControlJugador>();
        if (jugador == null) return;

        if (tipo == TipoResiduo.VidaExtra)
        {
            GameManager.instancia.GanarVida();
            jugador.EfectoImpacto(true);
            Destroy(gameObject);
            return;
        }

        if (tipo == TipoResiduo.CamaraLenta)
        {
            GameManager.instancia.ActivarCamaraLenta(5f);
            jugador.EfectoImpacto(true);
            Destroy(gameObject);
            return;
        }

        if (tipo == TipoResiduo.Bomba)
        {
            GameManager.instancia.RestarVida();
            jugador.EfectoImpacto(false);
            Destroy(gameObject);
            return;
        }

        bool esCorrecto =
            (tipo == TipoResiduo.Organico && jugador.tipoTachoActual == 1) ||
            (tipo == TipoResiduo.Reciclable && jugador.tipoTachoActual == 2) ||
            (tipo == TipoResiduo.NoAprovechable && jugador.tipoTachoActual == 3);

        if (esCorrecto)
        {
            GameManager.instancia.SumarPuntos(10);
            jugador.EfectoImpacto(true);
        }
        else
        {
            GameManager.instancia.RestarVida();
            jugador.EfectoImpacto(false);
        }

        Destroy(gameObject);
    }
}
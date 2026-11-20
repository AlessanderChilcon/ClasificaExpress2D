using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ControlJugador : MonoBehaviour
{
    public float velocidad = 8f;
    public float limiteX = 8f;
    public int tipoTachoActual = 1; // 1: Orgánico, 2: Reciclable, 3: No aprovechable

    [Header("Sprites de Tachos")]
    public Sprite spriteOrganico;
    public Sprite spriteReciclable;
    public Sprite spriteNoAprovechable;

    [Header("Iconos UI de Tachos (Opcional)")]
    public Image iconoTacho1;
    public Image iconoTacho2;
    public Image iconoTacho3;

    private SpriteRenderer spriteRenderer;
    private Vector3 escalaOriginal;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        escalaOriginal = transform.localScale;
        CambiarTacho(1);
    }

    void Update()
    {
        float movimiento = 0f;

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            movimiento = -1f;
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            movimiento = 1f;

        transform.Translate(Vector2.right * movimiento * velocidad * Time.deltaTime);

        float xLimitada = Mathf.Clamp(transform.position.x, -limiteX, limiteX);
        transform.position = new Vector3(xLimitada, transform.position.y, transform.position.z);

        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)) CambiarTacho(1);
        if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2)) CambiarTacho(2);
        if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3)) CambiarTacho(3);
    }

    void CambiarTacho(int nuevoTipo)
    {
        tipoTachoActual = nuevoTipo;
        spriteRenderer.color = Color.white;

        if (tipoTachoActual == 1 && spriteOrganico != null)
            spriteRenderer.sprite = spriteOrganico;
        else if (tipoTachoActual == 2 && spriteReciclable != null)
            spriteRenderer.sprite = spriteReciclable;
        else if (tipoTachoActual == 3 && spriteNoAprovechable != null)
            spriteRenderer.sprite = spriteNoAprovechable;

        ActualizarIconosUI();

        StopAllCoroutines();
        StartCoroutine(AnimarRebote(Color.white, 1.12f));
    }

    void ActualizarIconosUI()
    {
        Color activo = Color.white;
        Color inactivo = new Color(1f, 1f, 1f, 0.35f);

        if (iconoTacho1 != null)
        {
            iconoTacho1.color = (tipoTachoActual == 1) ? activo : inactivo;
            iconoTacho1.transform.localScale = (tipoTachoActual == 1) ? Vector3.one * 1.15f : Vector3.one * 0.9f;
        }
        if (iconoTacho2 != null)
        {
            iconoTacho2.color = (tipoTachoActual == 2) ? activo : inactivo;
            iconoTacho2.transform.localScale = (tipoTachoActual == 2) ? Vector3.one * 1.15f : Vector3.one * 0.9f;
        }
        if (iconoTacho3 != null)
        {
            iconoTacho3.color = (tipoTachoActual == 3) ? activo : inactivo;
            iconoTacho3.transform.localScale = (tipoTachoActual == 3) ? Vector3.one * 1.15f : Vector3.one * 0.9f;
        }
    }

    public void EfectoImpacto(bool acierto)
    {
        StopAllCoroutines();
        Color colorDestello = acierto ? new Color(0.6f, 1f, 0.6f) : new Color(1f, 0.4f, 0.4f);
        StartCoroutine(AnimarRebote(colorDestello, 1.2f));
    }

    IEnumerator AnimarRebote(Color colorTemporal, float multiplicadorEscala)
    {
        spriteRenderer.color = colorTemporal;
        transform.localScale = escalaOriginal * multiplicadorEscala;

        yield return new WaitForSeconds(0.12f);

        spriteRenderer.color = Color.white;
        transform.localScale = escalaOriginal;
    }
}
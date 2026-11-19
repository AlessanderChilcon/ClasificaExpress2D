using UnityEngine;
using UnityEngine.InputSystem;

public class ControlJugador : MonoBehaviour
{
    public float velocidad = 10f;
    public float limiteX = 7.5f;
    public int tipoTachoActual = 1; // 1 = Organico (Verde), 2 = Reciclable (Azul), 3 = No Aprovechable (Rojo)

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        ActualizarColor();
    }

    void Update()
    {
        float movimiento = 0f;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
                movimiento = -1f;
            else if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
                movimiento = 1f;

            if (Keyboard.current.digit1Key.wasPressedThisFrame)
            {
                tipoTachoActual = 1;
                ActualizarColor();
            }
            else if (Keyboard.current.digit2Key.wasPressedThisFrame)
            {
                tipoTachoActual = 2;
                ActualizarColor();
            }
            else if (Keyboard.current.digit3Key.wasPressedThisFrame)
            {
                tipoTachoActual = 3;
                ActualizarColor();
            }
        }

        Vector3 nuevaPosicion = transform.position + new Vector3(movimiento * velocidad * Time.deltaTime, 0f, 0f);
        nuevaPosicion.x = Mathf.Clamp(nuevaPosicion.x, -limiteX, limiteX);
        transform.position = nuevaPosicion;
    }

    void ActualizarColor()
    {
        if (spriteRenderer == null) return;

        if (tipoTachoActual == 1) spriteRenderer.color = Color.green;
        else if (tipoTachoActual == 2) spriteRenderer.color = Color.cyan;
        else if (tipoTachoActual == 3) spriteRenderer.color = Color.red;
    }
}
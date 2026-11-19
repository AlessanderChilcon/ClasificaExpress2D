using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instancia;

    public int puntos = 0;
    public int vidas = 3;

    public TextMeshProUGUI textoPuntos;
    public TextMeshProUGUI textoVidas;
    public GameObject panelGameOver;

    void Awake()
    {
        instancia = this;
    }

    void Start()
    {
        Time.timeScale = 1f;
        ActualizarUI();
    }

    public void SumarPuntos(int cantidad)
    {
        puntos += cantidad;
        ActualizarUI();
    }

    public void RestarVida()
    {
        vidas--;
        ActualizarUI();

        if (vidas <= 0)
        {
            vidas = 0;
            ActualizarUI();
            if (panelGameOver != null) panelGameOver.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    void ActualizarUI()
    {
        if (textoPuntos != null) textoPuntos.text = "Puntos: " + puntos;
        if (textoVidas != null) textoVidas.text = "Vidas: " + vidas;
    }

    public void ReiniciarJuego()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
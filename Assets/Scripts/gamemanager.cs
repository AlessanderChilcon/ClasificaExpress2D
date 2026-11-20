using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instancia;

    [Header("Estadísticas del Juego")]
    public int puntos = 0;
    public int vidas = 3;
    public int vidasMaximas = 3;
    public int nivel = 1;
    public int racha = 0;
    public float factorVelocidad = 1f;

    [Header("Interfaz de Usuario (UI)")]
    public TextMeshProUGUI textoPuntos;
    public TextMeshProUGUI textoVida;
    public TextMeshProUGUI textoNivel;
    public TextMeshProUGUI textoRacha;
    public Image[] iconosCorazones;

    [Header("Paneles")]
    public GameObject panelGameOver;
    public TextMeshProUGUI textoRecord;
    public GameObject panelPausa;

    [Header("Sonidos (SFX)")]
    public AudioSource fuenteAudio;
    public AudioClip sonidoPuntos;
    public AudioClip sonidoError;
    public AudioClip sonidoVidaExtra;
    public AudioClip sonidoGameOver;

    private bool estaPausado = false;

    void Awake()
    {
        if (instancia == null)
            instancia = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        Time.timeScale = 1f;
        if (fuenteAudio == null)
            fuenteAudio = GetComponent<AudioSource>();

        if (panelGameOver != null)
            panelGameOver.SetActive(false);

        if (panelPausa != null)
            panelPausa.SetActive(false);

        ActualizarUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && vidas > 0)
        {
            AlternarPausa();
        }
    }

    public void AlternarPausa()
    {
        estaPausado = !estaPausado;
        Time.timeScale = estaPausado ? 0f : 1f;
        if (panelPausa != null)
            panelPausa.SetActive(estaPausado);
    }

    public void SumarPuntos(int cantidad)
    {
        racha++;
        int multiplicador = (racha >= 5) ? 2 : 1;
        puntos += cantidad * multiplicador;

        nivel = 1 + (puntos / 100);
        ReproducirSonido(sonidoPuntos);
        ActualizarUI();
    }

    public void RestarVida()
    {
        racha = 0;
        vidas--;
        ActualizarUI();

        if (vidas <= 0)
        {
            ReproducirSonido(sonidoGameOver != null ? sonidoGameOver : sonidoError);
            GameOver();
        }
        else
        {
            ReproducirSonido(sonidoError);
        }
    }

    public void GanarVida()
    {
        if (vidas < vidasMaximas)
        {
            vidas++;
            ActualizarUI();
        }
        ReproducirSonido(sonidoVidaExtra != null ? sonidoVidaExtra : sonidoPuntos);
    }

    public void ActivarCamaraLenta(float duracion)
    {
        ReproducirSonido(sonidoPuntos);
        StopCoroutine("RutinaCamaraLenta");
        StartCoroutine(RutinaCamaraLenta(duracion));
    }

    IEnumerator RutinaCamaraLenta(float duracion)
    {
        factorVelocidad = 0.5f;
        yield return new WaitForSeconds(duracion);
        factorVelocidad = 1f;
    }

    void ReproducirSonido(AudioClip clip)
    {
        if (fuenteAudio != null && clip != null)
        {
            fuenteAudio.PlayOneShot(clip);
        }
    }

    void ActualizarUI()
    {
        if (textoPuntos != null) textoPuntos.text = "Puntos: " + puntos;
        if (textoVida != null) textoVida.text = "Vidas: " + vidas;
        if (textoNivel != null) textoNivel.text = "Nivel: " + nivel;
        if (textoRacha != null) textoRacha.text = (racha >= 5) ? "Racha: " + racha + " (x2!)" : "Racha: " + racha;

        if (iconosCorazones != null)
        {
            for (int i = 0; i < iconosCorazones.Length; i++)
            {
                if (iconosCorazones[i] != null)
                    iconosCorazones[i].enabled = (i < vidas);
            }
        }
    }

    void GameOver()
    {
        Time.timeScale = 0f;
        int recordGuardado = PlayerPrefs.GetInt("Record", 0);
        if (puntos > recordGuardado)
        {
            recordGuardado = puntos;
            PlayerPrefs.SetInt("Record", recordGuardado);
            PlayerPrefs.Save();
        }

        if (textoRecord != null)
            textoRecord.text = "Récord: " + recordGuardado;

        if (panelGameOver != null)
            panelGameOver.SetActive(true);
    }

    public void ReiniciarJuego()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void VolverAlMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuPrincipal");
    }
}
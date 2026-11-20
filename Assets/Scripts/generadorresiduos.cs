using UnityEngine;

public class GeneradorResiduos : MonoBehaviour
{
    public GameObject[] residuosNormales;
    public GameObject[] objetosEspeciales;
    [Range(0f, 1f)] public float probabilidadEspecial = 0.2f;

    public float intervaloInicial = 1.8f;
    public float rangoX = 7.5f;

    private float temporizador;

    void Update()
    {
        float nivelActual = (GameManager.instancia != null) ? GameManager.instancia.nivel : 1;
        float intervaloActual = Mathf.Max(0.65f, intervaloInicial - (nivelActual - 1) * 0.12f);

        temporizador += Time.deltaTime;
        if (temporizador >= intervaloActual)
        {
            GenerarObjeto();
            temporizador = 0f;
        }
    }

    void GenerarObjeto()
    {
        float posX = Random.Range(-rangoX, rangoX);
        Vector3 posicionSpawn = new Vector3(posX, transform.position.y, 0f);

        bool lanzarEspecial = (objetosEspeciales != null && objetosEspeciales.Length > 0 && Random.value < probabilidadEspecial);

        if (lanzarEspecial)
        {
            int indice = Random.Range(0, objetosEspeciales.Length);
            Instantiate(objetosEspeciales[indice], posicionSpawn, Quaternion.identity);
        }
        else if (residuosNormales != null && residuosNormales.Length > 0)
        {
            int indice = Random.Range(0, residuosNormales.Length);
            Instantiate(residuosNormales[indice], posicionSpawn, Quaternion.identity);
        }
    }
}
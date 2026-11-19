using UnityEngine;

public class GeneradorResiduos : MonoBehaviour
{
    public GameObject residuoPrefab;
    public float intervaloGeneracion = 1.5f;
    public float rangoX = 7.5f;

    private float temporizador;

    void Update()
    {
        temporizador += Time.deltaTime;

        if (temporizador >= intervaloGeneracion)
        {
            GenerarResiduo();
            temporizador = 0f;
        }
    }

    void GenerarResiduo()
    {
        if (residuoPrefab == null) return;

        float posX = Random.Range(-rangoX, rangoX);
        Vector3 posicionSpawn = new Vector3(posX, transform.position.y, 0f);
        Instantiate(residuoPrefab, posicionSpawn, Quaternion.identity);
    }
}
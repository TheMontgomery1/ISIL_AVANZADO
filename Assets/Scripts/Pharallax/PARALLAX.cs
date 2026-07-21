using UnityEngine;

public class PARALLAX : MonoBehaviour
{
    [System.Serializable]
    public class CapaParallax
    {
        public string nombreCapa;
        public GameObject[] prefabsFondo; // Array de prefabs para esta capa
        public float velocidadParallax = 1f; // Velocidad de movimiento
        public float factorProfundidad = 0.5f; // Factor de profundidad (menor = más lejos)
        public float offsetY = 0f; // Desplazamiento vertical de la capa
        public float offsetZ = 0f; // Profundidad en Z
        public int cantidadSegmentos = 3; // Segmentos iniciales
        [HideInInspector] public float anchoSegmento; // Ancho de cada segmento

        [HideInInspector] public GameObject contenedor; // Contenedor de la capa
        [HideInInspector] public GameObject[] segmentosInstanciados; // Segmentos instanciados
        [HideInInspector] public float posicionInicialX;
    }

    [Header("Capas de Parallax")]
    [SerializeField] private CapaParallax[] capas;
    
    [Header("Configuración de Cámara")]
    [SerializeField] private Transform camara;
    [SerializeField] private bool seguirCamaraX = true;
    [SerializeField] private bool seguirCamaraY = true;
    [SerializeField] private float suavizadoSeguimiento = 5f; // Suavizado del seguimiento

    private Vector3 posicionAnteriorCamara;

    void Start()
    {
        // Si no se asigna la cámara, usar la principal
        if (camara == null)
        {
            Camera cam = Camera.main;

            if (cam != null)
                camara = cam.transform;
            else
            {
                Debug.LogError("No se encontró Main Camera.");
                enabled = false;
                return;
            }
        }

        posicionAnteriorCamara = camara.position;
        InicializarCapas();

    }

    void LateUpdate()
    {
        ActualizarCapas();
    }

    // Inicializa todas las capas
    private void InicializarCapas()
    {
        for (int i = 0; i < capas.Length; i++)
        {
            CrearContenedorCapa(i);
            InstanciarSegmentosCapa(i);
        }
    }

    // Crea el contenedor para una capa
    private void CrearContenedorCapa(int indiceCapa)
    {
        CapaParallax capa = capas[indiceCapa];
        
        // Crear GameObject contenedor
        GameObject contenedor = new GameObject("Capa_" + capa.nombreCapa);
        contenedor.transform.SetParent(transform);

        // Posicionar el contenedor
        Vector3 posicion = new Vector3(
            camara.position.x,
            camara.position.y + capa.offsetY,
            capa.offsetZ
        );
        contenedor.transform.position = posicion;
        
        capa.contenedor = contenedor;
        capa.posicionInicialX = posicion.x;
    }

    // Instancia los segmentos iniciales de una capa
    private void InstanciarSegmentosCapa(int indiceCapa)
    {
        CapaParallax capa = capas[indiceCapa];

        if (capa.prefabsFondo.Length == 0)
        return;
        SpriteRenderer sr = capa.prefabsFondo[0].GetComponent<SpriteRenderer>();

        if (sr != null)
        {
            capa.anchoSegmento =
                sr.bounds.size.x;
        }
        capa.segmentosInstanciados = new GameObject[capa.cantidadSegmentos];

        for (int i = 0; i < capa.cantidadSegmentos; i++)
        {
            // Seleccionar un prefab fijo
            GameObject prefabSeleccionado = capa.prefabsFondo[i % capa.prefabsFondo.Length];

            // Calcular posición del segmento
            Vector3 posicion = new Vector3(
                capa.posicionInicialX + (i * capa.anchoSegmento) - capa.anchoSegmento,
                0f,
                0f
            );

            // Instanciar el prefab
            capa.segmentosInstanciados[i] = 
                Instantiate(prefabSeleccionado,posicion,Quaternion.identity,capa.contenedor.transform);
            capa.segmentosInstanciados[i].name = capa.nombreCapa + "_Segmento_" + i;
        }
    }

    // Actualiza todas las capas
    private void ActualizarCapas()
    {
        Vector3 deltaCamara = camara.position - posicionAnteriorCamara;

        for (int i = 0; i < capas.Length; i++)
        {
            ActualizarCapa(i, deltaCamara);
            VerificarYReinstanciarSegmentos(i);
        }

        posicionAnteriorCamara = camara.position;
    }

    // Actualiza una capa específica
    private void ActualizarCapa(int indiceCapa, Vector3 deltaCamara)
    {
        CapaParallax capa = capas[indiceCapa];
        
        if (capa.contenedor == null) return;

        Vector3 posicionObjetivo = capa.contenedor.transform.position;

        // Seguir a la cámara en X con efecto parallax
        if (seguirCamaraX)
        {
            // Movimiento basado en el factor de profundidad
            float desplazamientoX = deltaCamara.x * capa.factorProfundidad;
            posicionObjetivo.x += desplazamientoX;
            
            // Movimiento automático adicional
            posicionObjetivo.x -= capa.velocidadParallax * Time.deltaTime;
        }

        // Seguir a la cámara en Y con efecto parallax
        if (seguirCamaraY)
        {
            float posicionCamaraY = camara.position.y + capa.offsetY;
            posicionObjetivo.y = Mathf.Lerp(
                capa.contenedor.transform.position.y,
                posicionCamaraY,
                Time.deltaTime * suavizadoSeguimiento
            );
        }

        // Aplicar la nueva posición
        capa.contenedor.transform.position = posicionObjetivo;
    }

    // Verifica y reinstancia segmentos de una capa
    private void VerificarYReinstanciarSegmentos(int indiceCapa)
    {
        CapaParallax capa = capas[indiceCapa];
        
        if (capa.segmentosInstanciados == null || capa.segmentosInstanciados.Length == 0) return;

        // Verificar si el primer segmento está fuera de vista (izquierda)
        if (capa.segmentosInstanciados[0] != null)
        {
            float limiteIzquierdo = camara.position.x - capa.anchoSegmento * 2;
            if (capa.segmentosInstanciados[0].transform.position.x < limiteIzquierdo)
            {
                ReinstanciarSegmento(indiceCapa);
            }
        }
        
    }

    // Reutiliza el primer segmento moviéndolo al final
    private void ReinstanciarSegmento(int indiceCapa)
    {
        CapaParallax capa = capas[indiceCapa];

        GameObject primerSegmento =
            capa.segmentosInstanciados[0];

        GameObject ultimoSegmento =
            capa.segmentosInstanciados[
                capa.segmentosInstanciados.Length - 1
            ];

        float nuevaPosicionX =
            ultimoSegmento.transform.position.x +
            capa.anchoSegmento;

        primerSegmento.transform.position =
            new Vector3(
                nuevaPosicionX,
                primerSegmento.transform.position.y,
                primerSegmento.transform.position.z
            );

        for (int i = 0;
             i < capa.segmentosInstanciados.Length - 1;
             i++)
        {
            capa.segmentosInstanciados[i] =
                capa.segmentosInstanciados[i + 1];
        }

        capa.segmentosInstanciados[
            capa.segmentosInstanciados.Length - 1
        ] = primerSegmento;
    }

    // Limpieza al destruir
    private void OnDestroy()
    {
        if (capas != null)
        {
            foreach (CapaParallax capa in capas)
            {
                if (capa.segmentosInstanciados != null)
                {
                    foreach (GameObject segmento in capa.segmentosInstanciados)
                    {
                        if (segmento != null)
                        {
                            Destroy(segmento);
                        }
                    }
                }

                if (capa.contenedor != null)
                {
                    Destroy(capa.contenedor);
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Controller : MonoBehaviour
{
    [Header("Objetivo")][SerializeField] private Transform objetivo;

    [Header("Configuración de Seguimiento")]
    [SerializeField]
    private Vector3 offset = new Vector3(0, 0, -10);

    [SerializeField] private float suavizado = 5f;

    [Header("Seguimiento Selectivo")]
    [SerializeField]
    private bool seguirX = true;

    [SerializeField] private bool seguirY = false;
    [SerializeField] private bool seguirZ = false;

    [Header("Límites (Opcional)")]
    [SerializeField]
    private bool usarLimites = false;

    [SerializeField] private float limiteMinX = -10f;
    [SerializeField] private float limiteMaxX = 10f;
    [SerializeField] private float limiteMinY = -5f;
    [SerializeField] private float limiteMaxY = 5f;

    void LateUpdate()
    {
        if (objetivo == null)
        {
            Debug.LogWarning("No se ha asignado un objetivo a la cámara");
            return;
        }

        Vector3 posicionDeseada = transform.position;

        // Seguir en X (avance)
        if (seguirX)
        {
            posicionDeseada.x = objetivo.position.x + offset.x;
        }

        // Seguir en Y (lateral)
        if (seguirY)
        {
            posicionDeseada.y = objetivo.position.y + offset.y;
        }

        // Seguir en Z
        if (seguirZ)
        {
            posicionDeseada.z = objetivo.position.z + offset.z;
        }
        else
        {
            posicionDeseada.z = offset.z;
        }

        // Aplicar límites si están activados
        if (usarLimites)
        {
            posicionDeseada.x = Mathf.Clamp(posicionDeseada.x, limiteMinX, limiteMaxX);
            posicionDeseada.y = Mathf.Clamp(posicionDeseada.y, limiteMinY, limiteMaxY);
        }

        // Suavizar el movimiento
        transform.position = Vector3.Lerp(transform.position, posicionDeseada, suavizado * Time.deltaTime);
    }
}

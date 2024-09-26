using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Interfaz
{
    public class OrientarCanvas : MonoBehaviour
    {
        private Transform camPrincipal;                                 // Referencia a la c�mara principal (se asignar� autom�ticamente en tiempo de ejecuci�n)

        void Update()
        {
            if (camPrincipal == null)                                                           // Verifica si la referencia al jugador est� asignada
            {  
                BuscarCamaraPrincipal();                                                        // Intenta encontrar la c�mara principal si a�n no est� asignada
            }

            if (camPrincipal != null)                                                           // Verifica si la referencia al jugador est� asignada
            {
                Vector3 direccion = camPrincipal.position - transform.position;                 // Calcula la direcci�n hacia el jugador sin girar en el eje Y
                direccion.y = 0f;
                Quaternion rotacionDeseada = Quaternion.LookRotation(direccion);                // Calcula la rotaci�n hacia la direcci�n calculada
                rotacionDeseada *= Quaternion.Euler(0f, 180f, 0f);                              // Invierte la rotaci�n en el eje Y para corregir la orientaci�n
                transform.rotation = Quaternion.Euler(0f, rotacionDeseada.eulerAngles.y, 0f);   // Aplica la rotaci�n solo al eje Y
            }
        }
        private void BuscarCamaraPrincipal()                                                    // M�todo para buscar la c�mara principal y asignar la referencia
        {
            Camera camaraPrincipal = Camera.main;
            if (camaraPrincipal != null)                                                        // Verifica si se encontr� la c�mara principal
            {
                camPrincipal = camaraPrincipal.transform;
            }
        }
    }
}
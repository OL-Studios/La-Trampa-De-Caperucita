using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jugador{
    public class RegenerarSalud : MonoBehaviour
    {
        public int saludParaRegenerar;
        public float tiempoEntreRegeneracion = 1f;
        private int saludRegenerada = 0;
        private bool jugadorDentro = false; 
        private JugadorVida jugadorVida;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                jugadorVida = other.GetComponent<JugadorVida>();
                if (jugadorVida != null)
                {
                    jugadorDentro = true;
                    StartCoroutine(RegenerarSaludConTiempo());
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                jugadorDentro = false;
            }
        }

        private IEnumerator RegenerarSaludConTiempo()
        {
            while (jugadorDentro && saludRegenerada < saludParaRegenerar) // Regenerar vida solo si al jugador le falta salud por regenerar
            {
                int saludRestanteParaRegenerar = saludParaRegenerar - saludRegenerada;
                int saludARegenerar = Mathf.Min(saludRestanteParaRegenerar, 1); // Regenerar de 1 en 1 por segundo

                jugadorVida.RecuperarVida(saludARegenerar);
                saludRegenerada += saludARegenerar;

                yield return new WaitForSeconds(tiempoEntreRegeneracion); // Espera 1 segundo antes de la siguiente regeneración
            }
            
            if (saludRegenerada >= saludParaRegenerar)      // Si ya se ha regenerado toda la salud programada, destruir la cápsula
            {
                Destroy(gameObject);
            }
        }
    }
}
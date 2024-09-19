using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jugador
{
    public class JugadorVida : MonoBehaviour
    {
        public static JugadorVida Instance { get; private set; }

        [Header ("Salud")]
        public int saludMax = 100;
        public int saludActual;
        public JugadorMovimiento jugadorMov;
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }

        void Start()
        {
            saludActual = saludMax;
        }

        public void TomarDaño(int cantidad)
        {
            saludActual -= cantidad;
            if (saludActual <= 0)
            {
                PerderJuego();
            }
        }

        public void RecuperarVida(int cantidad)
        {
            saludActual += cantidad;
            if (saludActual > saludMax)
            {
                saludActual = saludMax; // No permitir que se pase de la salud máxima
            }
        }

        void PerderJuego()
        {
            Debug.Log("Muere el jugador");
            jugadorMov.animatorJugador.SetTrigger("Muerte");
        }

    }
}
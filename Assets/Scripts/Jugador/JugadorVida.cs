using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Jugador
{
    public class JugadorVida : MonoBehaviour
    {
        public static JugadorVida Instance { get; private set; }

        [Header ("Salud")]
        public int saludMax = 100;
        public int saludActual;
        public JugadorMovimiento jugadorMov;
        public Image barSaludJugador;
        
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

        void Update(){
            ActualizaSalud();
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
                saludActual = saludMax;
            }
        }

        void PerderJuego()
        {
            Debug.Log("Muere el jugador");
            jugadorMov.animatorJugador.SetTrigger("Muere");
            Destroy(gameObject, 3f);
        }

        void ActualizaSalud()
        {
            float fillAmount = (float)saludActual / (float)saludMax;
            barSaludJugador.fillAmount = fillAmount;
        }

        public Transform PosicionJugador()
        {
            return this.transform;
        }
    }
}
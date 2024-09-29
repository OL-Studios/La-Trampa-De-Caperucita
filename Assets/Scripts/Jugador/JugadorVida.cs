using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Jugador
{
    public class JugadorVida : MonoBehaviour
    {
        #region Variables
            public static JugadorVida Instance { get; private set; }

            [Header ("SALUD")]
            public float saludMax;
            public float saludActual;

            [Header ("REFERENCIAS")]
            public JugadorMovimiento jugadorMov;

            [Header ("UI")]
            public Image barSaludJugador;

            [Header ("BANDERAS")]
            public bool estaVivo;
        #endregion
        
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
            estaVivo = true;
        }

        void Update(){
            ActualizaSalud();
        }

        public void TomarDaño(float cantidad)
        {
            saludActual -= cantidad;
            if (saludActual <= 0f)
            {
                PerderJuego();
            }
        }

        public void RecuperarVida(float cantidad)
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
            estaVivo = false;
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
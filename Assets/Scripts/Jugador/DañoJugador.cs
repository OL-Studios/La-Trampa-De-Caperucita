using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jugador
{
    public class DañoJugador : MonoBehaviour
    {
        public float dañoPuas;

        private void OnTriggerEnter(Collider other) {
            if(other.CompareTag("Puas")){
                JugadorVida.Instance.TomarDaño(dañoPuas);
            }
            if(other.CompareTag("Caida")){
                JugadorVida.Instance.saludActual = 0f;
            }
        }
    }
}

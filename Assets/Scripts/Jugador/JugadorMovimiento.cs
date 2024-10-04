using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

namespace Jugador
{
    public class JugadorMovimiento : MonoBehaviour
    {
        #region Variables
            [Header ("DESPLAZAMIENTO")]
            public float velCorrer;
            public float velRotacion;
            public Animator animatorJugador;
            private float ejeX, ejeY;

            [Header ("SALTO")]
            public Rigidbody rbJugador;
            public float alturaSalto;
            public Transform comprobacionSuelo;
            public float distanciaSuelo;
            public LayerMask mascaraSuelo;
            private bool enSuelo;
            private bool estaRodando = false;
        #endregion

        void Update()
        {
            if(GameManager.Instance.enJuego){
                if (!estaRodando)               // Solo permitir el movimiento si no está rodando
                {
                    MovimientoJugador();
                    SaltoJugador();
                }
            }

            Rollito();
        }

        void MovimientoJugador(){
            ejeX = Input.GetAxis("Horizontal");
            ejeY = Input.GetAxis("Vertical");
            transform.Rotate(0, ejeX * Time.deltaTime * velRotacion, 0);
            transform.Translate(0, 0, ejeY * Time.deltaTime * velCorrer);
            animatorJugador.SetFloat("velGirar", ejeX);
            animatorJugador.SetFloat("velCorrer", ejeY);

    }

        void SaltoJugador(){
            enSuelo = Physics.CheckSphere(comprobacionSuelo.position, distanciaSuelo, mascaraSuelo);
            if(Input.GetKey("space") && enSuelo){
                animatorJugador.Play("Saltar");
                rbJugador.AddForce(Vector3.up * alturaSalto, ForceMode.Impulse);
            }
        }

        void Rollito()
        {
            if (Input.GetKeyDown(KeyCode.LeftAlt) && !estaRodando)
            {
                animatorJugador.SetTrigger("rollo");
                StartCoroutine(EsperarFinDeAnimacion("Rollo"));
            }
        }

        private IEnumerator EsperarFinDeAnimacion(string nombreAnimacion)   // Coroutine para bloquear el movimiento durante la animación del rollo
        {
            estaRodando = true;
            yield return new WaitUntil(() => animatorJugador.GetCurrentAnimatorStateInfo(0).IsName(nombreAnimacion));   // Esperar a que comience la animación
            yield return new WaitWhile(() => animatorJugador.GetCurrentAnimatorStateInfo(0).IsName(nombreAnimacion));   // Esperar a que la animación termine
            estaRodando = false;  // Volver a habilitar el movimiento
        }

    }
}
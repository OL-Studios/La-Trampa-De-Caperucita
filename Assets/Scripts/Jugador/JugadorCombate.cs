using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jugador {
    public class JugadorCombate : MonoBehaviour
    {
        public GameObject manzanaMano;
        public GameObject nuevaManzana;
        public Transform puntoSalidaManzana;
        private bool manzanaActiva = false;

        public Animator aniJugador;

        public int cantManzanas = 0;
        public int cantBebida = 0;

        public float fuerzaLanzamiento;


        void Update()
        {
            Ataque();
            ActivarManzanaBomba();
            LanzarManzana();
        }

        


        void Ataque()
        {
            if (Input.GetMouseButtonDown(0) && !aniJugador.GetCurrentAnimatorStateInfo(0).IsName("Ataque-1") && !aniJugador.GetCurrentAnimatorStateInfo(0).IsName("Ataque-2"))
            {
                int animacion = Random.Range(1, 3);
                aniJugador.Play("Ataque-" + animacion);
            }
        }

        void ActivarManzanaBomba()
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0f && cantManzanas > 0)        // Deslizar hacia arriba
            {
                ActivarManzana();
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f)   // Deslizar hacia abajo
            {
                DesactivarManzana();
            }
        }

        void ActivarManzana()
        {
            manzanaMano.SetActive(true);
            manzanaActiva = true;
        }

        void DesactivarManzana()
        {
            manzanaMano.SetActive(false);
            manzanaActiva = false;
        }

        void LanzarManzana()
        {
            if (Input.GetMouseButtonDown(1) && manzanaActiva) // Click derecho
            { 
                manzanaActiva = false;
                cantManzanas--;
                aniJugador.Play("Lanzar");
                StartCoroutine("SalidaManzana");
            }
        }

        IEnumerator SalidaManzana(){

            yield return new WaitForSeconds(.56f);
            manzanaMano.SetActive(false);
            
            yield return new WaitForSeconds(.1f);
            GameObject manzanaLanzada = Instantiate(nuevaManzana, puntoSalidaManzana.transform.position + transform.forward, Quaternion.identity);
            manzanaLanzada.GetComponent<Rigidbody>().AddForce(transform.forward * fuerzaLanzamiento);
            //Destroy(manzanaLanzada, 3f);
        }

        public void TomaBebida(){
            if(cantBebida > 0){
                cantBebida--;
                JugadorVida.Instance.RecuperarVida(10);
                Debug.Log("Recupera 10 de salud");
            }else{
                Debug.Log("Ya no tienes bebidas");
            }
        }

    }
}
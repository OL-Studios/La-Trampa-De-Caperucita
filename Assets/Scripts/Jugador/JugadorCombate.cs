using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jugador {
    public class JugadorCombate : MonoBehaviour
    {
        public GameObject manzana;
        private bool manzanaActiva = false;

        public Animator aniJugador;

        public int cantManzanas = 0;
        public int cantBebida = 0;


        void Update()
        {
            Ataque();
            CambiarArma();
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

        void CambiarArma()
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
            manzana.SetActive(true);
            manzanaActiva = true;
        }

        void DesactivarManzana()
        {
            manzana.SetActive(false);
            manzanaActiva = false;
        }

        void LanzarManzana()
        {
            if (Input.GetMouseButtonDown(1) && manzanaActiva) // Click derecho
            {
                cantManzanas--;
                aniJugador.Play("Lanzar");
                //Aqui se desactiva la manzana mas no se elimina ni se instancia
            }
        }

        public void TomaBebida(){
            if(cantBebida > 0){
                cantBebida--;
                JugadorVida.Instance.RecuperarVida(10);
                Debug.Log("Recupera 10 de salud");
            }
        }

    }
}
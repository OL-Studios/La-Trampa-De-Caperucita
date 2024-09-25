using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Enemigos;

namespace Jugador {
    public class JugadorCombate : MonoBehaviour
    {
        [Header ("Jugador")]
        public Animator aniJugador;

        [Header ("Manzana")]
        public int cantManzanas = 0;
        public GameObject manzanaMano;
        public GameObject nuevaManzana;
        public Transform puntoSalidaManzana;
        public float fuerzaLanzamiento;

        private Araña componenteAraña;

        [Header ("Banderas")]
        private bool enContactoAraña = false;
        private bool manzanaActiva = false;
        private bool efecto1Reproducido = false;
        private bool efecto2Reproducido = false;
        private bool efecto3Reproducido = false;

        [Header ("Resta de salud a enemigos")]
        public int cantAtaqueUno;
        public int cantAtaqueDos;

        [Header ("UI")]
        public TMP_Text txtCantManzana;

        [Header ("VFX")]
        public ParticleSystem vfxAtaqueUno;
        public ParticleSystem vfxAtaqueDos;
        public ParticleSystem vfxAtaqueTres;

        void Update()
        {
            txtCantManzana.text = "" + cantManzanas;
            Ataque();
            ActivarManzanaBomba();
            LanzarManzana();
            ComprobarFrameYReproducirEfectos();
            AtacandoAraña();
        }

        private void OnTriggerEnter(Collider other) {
            if(other.CompareTag("Araña")){
                enContactoAraña = true;
                componenteAraña = other.GetComponent<Araña>();
                Debug.Log("Daga en contacto con araña");
            }
        }

        #region Ataque con daga
            void Ataque()
            {
                // Verificar si se hace click izquierdo y que no esté ya en una animación de ataque
                if (Input.GetMouseButtonDown(0) && !aniJugador.GetCurrentAnimatorStateInfo(0).IsName("Ataque-1") && !aniJugador.GetCurrentAnimatorStateInfo(0).IsName("Ataque-2"))
                {
                    int animacion = Random.Range(1, 3);  
                    aniJugador.Play("Ataque-" + animacion);  // Reproducir la animación de ataque correspondiente

                    // Reiniciar los flags para permitir que los efectos se reproduzcan nuevamente en el próximo ataque
                    efecto1Reproducido = false;
                    efecto2Reproducido = false;
                    efecto3Reproducido = false;
                }
            }

            void ComprobarFrameYReproducirEfectos()
            {
                AnimatorStateInfo estadoAnimacion = aniJugador.GetCurrentAnimatorStateInfo(0);
                if (estadoAnimacion.IsName("Ataque-1"))
                {
                    // Calcular el frame actual basado en el tiempo de la animación
                    int frameActual = (int)(estadoAnimacion.normalizedTime * estadoAnimacion.length * aniJugador.GetCurrentAnimatorClipInfo(0)[0].clip.frameRate);
                    if (frameActual >= 15 && !efecto1Reproducido)
                    {
                        ReproducirEfecto1();
                    }
                }

                if (estadoAnimacion.IsName("Ataque-2"))
                {
                    // Calcular el frame actual basado en el tiempo de la animación
                    int frameActual = (int)(estadoAnimacion.normalizedTime * estadoAnimacion.length * aniJugador.GetCurrentAnimatorClipInfo(0)[0].clip.frameRate);

                    // Reproducir el efecto 2 en el frame 19 y el efecto 3 en el frame 70
                    if (frameActual >= 19 && !efecto2Reproducido)
                    {
                        ReproducirEfecto2();
                    }
                    if (frameActual >= 70 && !efecto3Reproducido)
                    {
                        ReproducirEfecto3();
                    }
                }
            }

            // Estas funciones reproducen los efectos y marcan que ya se han reproducido
            public void ReproducirEfecto1()
            {
                vfxAtaqueUno.Play();  // Reproducir el efecto 1 en el frame 15 de "Ataque-1"
                efecto1Reproducido = true;
            }

            public void ReproducirEfecto2()
            {
                vfxAtaqueDos.Play();  // Reproducir el efecto 2 en el frame 19 de "Ataque-2"
                efecto2Reproducido = true;
            }

            public void ReproducirEfecto3()
            {
                vfxAtaqueTres.Play();  // Reproducir el efecto 3 en el frame 70 de "Ataque-2"
                efecto3Reproducido = true;
            }
        #endregion

        #region Ataque con Manzana
            void ActivarManzanaBomba()
            {
                float scrollInput = Input.GetAxis("Mouse ScrollWheel");
                if (scrollInput > 0f && cantManzanas > 0) {
                    ActivarManzana();
                } 
                else if (scrollInput < 0f) {
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
            }
        #endregion
       
        #region Ataque a Araña
            void AtacandoAraña(){
                AnimatorStateInfo estadoAnimacion = aniJugador.GetCurrentAnimatorStateInfo(0);

                // Verificar si se está reproduciendo la animación "Ataque-1" o "Ataque-2"
                if (estadoAnimacion.IsName("Ataque-1")){
                    ProcesarAtaque(19, "Golpe a araña 1", cantAtaqueUno);
                }
                else if (estadoAnimacion.IsName("Ataque-2")){
                    ProcesarAtaque(new int[] { 22, 34, 38 }, "Golpe a araña 2", cantAtaqueDos);
                }
            }

            void ProcesarAtaque(int frameObjetivo, string mensajeGolpe, int daño){
                int frameActual = CalcularFrameActual();
                if (frameActual == frameObjetivo && enContactoAraña){
                    EjecutarGolpe(mensajeGolpe, daño);
                }
            }

            void ProcesarAtaque(int[] framesObjetivo, string mensajeGolpe, int daño){
                int frameActual = CalcularFrameActual();
                foreach (int frame in framesObjetivo){
                    if (frameActual == frame && enContactoAraña){
                        EjecutarGolpe(mensajeGolpe, daño);
                        break;
                    }
                }
            }

            int CalcularFrameActual(){
                return (int)(aniJugador.GetCurrentAnimatorStateInfo(0).normalizedTime * aniJugador.GetCurrentAnimatorClipInfo(0)[0].clip.frameRate);
            }

            void EjecutarGolpe(string mensaje, int daño){
                Debug.Log(mensaje);
                if (componenteAraña != null){
                    componenteAraña.saludActualAraña -= daño;
                } else {
                    Debug.LogError("Araña no encontrada en el objeto con el que se ha colisionado.");
                }
                enContactoAraña = false;
            }
        #endregion
    }
}
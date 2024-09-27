using System.Collections;
using System.Collections.Generic;
using Jugador;
using UnityEngine;

namespace Enemigos{
    public class AtaqueJefe : MonoBehaviour
    {
        public float quitaSaludJugador;
        public Animator aniJefe;
        public bool enContactoJugador = false;
        public int frameAtaque;

        void Update()
        {
            AtacandoJugador();
        }
        
        private void OnTriggerEnter(Collider other) {
            if(other.CompareTag("Player")){
                enContactoJugador = true;
                Debug.Log("Garra en contacto con jugador");
            }
        }

        void AtacandoJugador(){
            AnimatorStateInfo estadoAnimacion = aniJefe.GetCurrentAnimatorStateInfo(0);
            if (estadoAnimacion.IsName("AtaqueDoble")){
                ProcesarAtaque(frameAtaque, "Golpe a jugador", quitaSaludJugador);
            }
        }

        void ProcesarAtaque(int frameObjetivo, string mensajeGolpe, float daño){
            int frameActual = CalcularFrameActual();
            if (frameActual == frameObjetivo && enContactoJugador){
                enContactoJugador = false;
                JugadorVida.Instance.TomarDaño(daño);
                Debug.Log(mensajeGolpe);
            }
        }

        int CalcularFrameActual(){
            return (int)(aniJefe.GetCurrentAnimatorStateInfo(0).normalizedTime * aniJefe.GetCurrentAnimatorClipInfo(0)[0].clip.frameRate);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Jugador;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace Enemigos{
    public class Araña : MonoBehaviour
    {
        [Header("PERSONAJE")]
        public Rigidbody rb;
        public int saludActualAraña;
        public int saludMaxAraña;
        public int quitaSaludJugador;
        public float rangoDeteccion;
        public float rangoAtaque;

        [Header("AI")]
        public NavMeshAgent navMeshAgent;

        [Header("ANIMATIONS")]
        public Animator animator;
        public ParticleSystem vfxVeneno;
        public ParticleSystem vfxMuerte;
        public SphereCollider colliderAraña;
        public Image barSaludAraña;

        void Start(){
            saludActualAraña = saludMaxAraña;
        }
        void Update()
        {
            MuerteAraña(0);
            AtaqueAJugador();
            ActualizaSaludAraña();
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                vfxVeneno.Play();
                JugadorVida.Instance.TomarDaño(quitaSaludJugador);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                vfxVeneno.Stop();
            }
        }

        public void AtaqueAJugador()
        {
            float distanciaAlObjetivo = Vector3.Distance(transform.position, JugadorVida.Instance.PosicionJugador().position);      // Obtener la distancia al jugador

            if (saludActualAraña > 0)
            { 
                if (distanciaAlObjetivo <= rangoAtaque)
                {
                    // Atacar al jugador
                    animator.SetBool("Corriendo", false);
                    animator.SetBool("Atacar", true);
                }

                else if (distanciaAlObjetivo <= rangoDeteccion)
                {
                    // Moverse hacia el objetivo si está dentro del segundo rango
                    animator.SetBool("Corriendo", true);
                    animator.SetBool("Atacar", false);

                    navMeshAgent.isStopped = false;
                    navMeshAgent.SetDestination(JugadorVida.Instance.PosicionJugador().position);
                }

                else
                {
                    // Fuera de los rangos de ataque
                    animator.SetBool("Corriendo", false);
                    animator.SetBool("Atacar", false);
                    navMeshAgent.isStopped = true;
                    //animator.Play("Reposo_Araña");
                }
            }

            else
            {
                MuerteAraña(0);
            }
        }

        void ActualizaSaludAraña()
        {
            float fillAmount = (float)saludActualAraña / (int)saludMaxAraña;
            barSaludAraña.fillAmount = fillAmount;
        }

        public void MuerteAraña(int caso)
        {
            switch(caso){
                case 0:
                    if (saludActualAraña <= 0)
                    {
                        animator.Play("MuerteAraña");
                        colliderAraña.enabled = false;
                        navMeshAgent.isStopped = true;
                        vfxMuerte.Play();
                        vfxVeneno.Stop();
                        Destroy(gameObject, 4f);
                    }
                break;

                case 1:
                    animator.Play("MuerteAraña");
                    colliderAraña.enabled = false;
                    navMeshAgent.isStopped = true;
                    Destroy(gameObject,2f);
                break;
            }
        }
    }
}

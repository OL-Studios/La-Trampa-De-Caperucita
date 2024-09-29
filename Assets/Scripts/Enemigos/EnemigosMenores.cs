using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Jugador;

namespace Enemigos{
    public class EnemigosMenores : MonoBehaviour
    {
        #region VARIABLES
            [Header("PERSONAJE")]
            public Rigidbody rb;
            public int saludActualEnemigo;
            public int saludMaxEnemigo;
            public SphereCollider colliderEnemigo;

            [Header ("DAÑO JUGADOR")]
            public float quitaSaludJugador;
            public float rangoDeteccion;
            public float rangoAtaque;

            [Header("AI")]
            public NavMeshAgent navMeshAgent;

            [Header("ANIMACIONES")]
            public Animator animator;
            public string nombreAniMuerte;
            public string nombreAniReaccion;
            public bool enReaccion = false;

            [Header ("UI")]
            public Image barSaludEnemigo;

            [Header ("VFX")]
            public ParticleSystem vfxVeneno;
            public ParticleSystem vfxMuerte;
        #endregion

        void Start(){
            saludActualEnemigo = saludMaxEnemigo;
        }
        void Update()
        {
            MuerteEnemigo(0);
            AtaqueAJugador();
            ActualizaSaludEnemigo();
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

        #region Persecución al jugador
            public void AtaqueAJugador()
            {
                if (JugadorVida.Instance.estaVivo == false)     // Detener el ataque y persecución
                {
                    animator.SetBool("Corriendo", false);
                    animator.SetBool("Atacar", false);
                    animator.SetTrigger("JugadorMuere");
                    navMeshAgent.isStopped = true;
                    return;
                }
                
                if (enReaccion)                                 // Si está en reacción, no hacer nada más
                {
                    navMeshAgent.isStopped = true;
                    return;
                }

                float distanciaAlObjetivo = Vector3.Distance(transform.position, JugadorVida.Instance.PosicionJugador().position);      // Obtener la distancia al jugador

                if (saludActualEnemigo > 0)
                { 
                    if (distanciaAlObjetivo <= rangoAtaque)     // Atacar al jugador
                    {
                        animator.SetBool("Corriendo", false);
                        animator.SetBool("Atacar", true);

                        // Girar hacia el jugador antes de atacar
                        Vector3 direccionHaciaJugador = JugadorVida.Instance.PosicionJugador().position - transform.position;
                        direccionHaciaJugador.y = 0;                                    // Asegura que el enemigo no gire en el eje Y si no es necesario
                        Quaternion rotacionHaciaJugador = Quaternion.LookRotation(direccionHaciaJugador);
                        transform.rotation = Quaternion.Slerp(transform.rotation, rotacionHaciaJugador, Time.deltaTime * 5f);  // Rotación suave
                    }

                    else if (distanciaAlObjetivo <= rangoDeteccion)     // Moverse hacia el objetivo si está dentro del segundo rango
                    {
                        animator.SetBool("Corriendo", true);
                        animator.SetBool("Atacar", false);
                        navMeshAgent.isStopped = false;
                        navMeshAgent.SetDestination(JugadorVida.Instance.PosicionJugador().position);
                    }

                    else        // Fuera de los rangos de ataque
                    {
                        animator.SetBool("Corriendo", false);
                        animator.SetBool("Atacar", false);
                        navMeshAgent.isStopped = true;
                    }
                }

                else
                {
                    MuerteEnemigo(0);
                }
            }
        #endregion

        #region Reacción de daño
            public void InicioReaccion()
            {
                enReaccion = true;
                navMeshAgent.isStopped = true;
            }

            public void FinReaccion()
            {
                enReaccion = false;
                navMeshAgent.isStopped = false;
            }
        #endregion

        #region Estado de vida del enemigo
            void ActualizaSaludEnemigo()
            {
                float fillAmount = (float)saludActualEnemigo / (int)saludMaxEnemigo;
                barSaludEnemigo.fillAmount = fillAmount;
            }

            public void MuerteEnemigo(int caso)
            {
                switch(caso){
                    case 0:
                        if (saludActualEnemigo <= 0)
                        {
                            animator.Play(nombreAniMuerte);
                            colliderEnemigo.enabled = false;
                            navMeshAgent.isStopped = true;
                            vfxMuerte.Play();
                            vfxVeneno.Stop();
                            Destroy(gameObject, 4f);
                        }
                    break;

                    case 1:
                        animator.Play(nombreAniMuerte);
                        colliderEnemigo.enabled = false;
                        navMeshAgent.isStopped = true;
                        Destroy(gameObject,2f);
                    break;
                }
            }
        #endregion
    }
}

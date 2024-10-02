using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Jugador;


namespace Enemigos{
    public class LoboJefe : MonoBehaviour
    {
        #region Variables
            [Header("PERSONAJE")]
            public int saludActualJefe;
            public int saludMaxJefe;
            public Rigidbody rbJefe;
            public BoxCollider colliderJefe;

            [Header ("DAÑO JUGADOR")]
            public float rangoDeteccionAJugador;
            public float rangoAtaqueAJugador;

            [Header("AI")]
            public NavMeshAgent navMeshJefe;

            [Header("ANIMACIONES")]
            public Animator aniJefe;
            public bool enReaccionGolpe = false;

            [Header("UI")]
            public Image barSaludJefe;

            [Header("VFX")]
            public ParticleSystem vfxAtaque;
            public ParticleSystem vfxMuerteJefe;

        #endregion

        void Start(){
            gameObject.SetActive(true);
            saludActualJefe = saludMaxJefe;
        }
        void Update()
        {
            ActualizaSaludEnemigo();
            Combate();
            MuerteJefe();
        }

        #region Persecución al jugador
            void Combate(){
                if (JugadorVida.Instance.estaVivo == false) // Detener el ataque y persecución
                {
                    aniJefe.SetBool("Corriendo", false);
                    aniJefe.SetBool("Atacando", false);
                    aniJefe.SetTrigger("JugadorMuere");
                    navMeshJefe.isStopped = true;
                    return;
                }

                if (enReaccionGolpe)  // Si está en reacción, no hacer nada más
                {
                    navMeshJefe.isStopped = true;  // Detener el movimiento mientras reacciona
                    return;
                }

                float distanciaAlObjetivo = Vector3.Distance(transform.position, JugadorVida.Instance.PosicionJugador().position);      // Obtener la distancia al jugador

                if (saludActualJefe > 0)
                { 
                    if (distanciaAlObjetivo <= rangoAtaqueAJugador)
                    {
                        // Atacar al jugador
                        aniJefe.SetBool("Corriendo", false);
                        aniJefe.SetBool("Atacando", true);

                        // Girar hacia el jugador antes de atacar
                        Vector3 direccionHaciaJugador = JugadorVida.Instance.PosicionJugador().position - transform.position;
                        direccionHaciaJugador.y = 0;  // Opcional: Asegura que el enemigo no gire en el eje Y si no es necesario
                        Quaternion rotacionHaciaJugador = Quaternion.LookRotation(direccionHaciaJugador);
                        transform.rotation = Quaternion.Slerp(transform.rotation, rotacionHaciaJugador, Time.deltaTime * 5f);  // Rotación suave
                    }

                    else if (distanciaAlObjetivo <= rangoDeteccionAJugador)
                    {
                        // Moverse hacia el objetivo si está dentro del segundo rango
                        aniJefe.SetBool("Corriendo", true);
                        aniJefe.SetBool("Atacando", false);

                        navMeshJefe.isStopped = false;
                        navMeshJefe.SetDestination(JugadorVida.Instance.PosicionJugador().position);
                    }

                    else
                    {
                        // Fuera de los rangos de ataque
                        aniJefe.SetBool("Corriendo", false);
                        aniJefe.SetBool("Atacando", false);
                        navMeshJefe.isStopped = true;
                    }
                }

                else
                {
                    MuerteJefe();
                }
            }
        #endregion

        #region Reacción al ataque del jugador
        public void InicioReaccionJefe()
        {
            enReaccionGolpe = true;
            navMeshJefe.isStopped = true;
        }

        public void FinReaccionJefe()
        {
            enReaccionGolpe = false;
            navMeshJefe.isStopped = false;
        }
        #endregion

        #region Salud del Jefe
            void ActualizaSaludEnemigo()
            {
                float fillAmount = (float)saludActualJefe / (float)saludMaxJefe;
                if (barSaludJefe != null) { 
                    barSaludJefe.fillAmount = fillAmount;
                }
            }

            public void MuerteJefe()
            {
                if (saludActualJefe <= 0)
                {
                    aniJefe.Play("Muerte_Jefe");
                    colliderJefe.enabled = false;
                    navMeshJefe.isStopped = true;
                    vfxMuerteJefe.Play();
                    vfxAtaque.Stop();
                    JugadorVida.Instance.jugadorMov.animatorJugador.SetTrigger("MuereJefe");
                    Invoke("Inactivar", 4f);
            }
            }


            private void Inactivar()
            {
                gameObject.SetActive(false); // Desactiva el objeto
            }
        #endregion
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Jugador;

namespace Enemigos{
    public class LoboJefe : MonoBehaviour
    {
        [Header("PERSONAJE")]
        public Rigidbody rbJefe;
        public int saludActualJefe;
        public int saludMaxJefe;
        public float rangoDeteccionAJugador;
        public float rangoAtaqueAJugador;

        [Header("AI")]
        public NavMeshAgent navMeshJefe;

        [Header("ANIMATIONS")]
        public Animator aniJefe;
        public ParticleSystem vfxAtaque;
        public ParticleSystem vfxMuerteJefe;
        public BoxCollider colliderJefe;
        public Image barSaludJefe;
        public bool enReaccionGolpe = false;
        public Cinemachine.CinemachineImpulseSource impulseSource;

        void Start(){
            saludActualJefe = saludMaxJefe;
        }
        void Update()
        {
            ActualizaSaludEnemigo();
            Combate();
            MuerteJefe();
        }

        #region 
        void Combate(){
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
                    impulseSource.GenerateImpulse();
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
                    impulseSource.GenerateImpulse();
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
            barSaludJefe.fillAmount = fillAmount;
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
                Destroy(gameObject, 4f);
            }
        }
        #endregion 
    }
}
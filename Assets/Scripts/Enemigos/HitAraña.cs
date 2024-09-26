using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemigos{
    public class HitAraña : StateMachineBehaviour
    {
        private EnemigosMenores enemigo;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // Obtener el componente EnemyReactions del mismo GameObject
            enemigo = animator.GetComponent<EnemigosMenores>();

            if (enemigo != null)
            {
                enemigo.InicioReaccion();
            }
            else
            {
                Debug.LogError("EnemyReactions script no encontrado en el mismo GameObject");
            }
        }

        //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            enemigo.FinReaccion();
        }
    }
}
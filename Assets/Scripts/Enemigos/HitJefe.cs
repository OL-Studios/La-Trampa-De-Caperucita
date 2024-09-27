using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemigos{
    public class HitJefe : StateMachineBehaviour
    {
        private LoboJefe loboJefe;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            loboJefe = animator.GetComponent<LoboJefe>();

            if (loboJefe != null)
            {
                loboJefe.InicioReaccionJefe();
            }
            else
            {
                Debug.LogError("LoboJefe script no encontrado en el mismo GameObject");
            }
        }

        //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            loboJefe.FinReaccionJefe();
        }
    }
}
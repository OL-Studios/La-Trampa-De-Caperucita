using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Enemigos{
    public class ActHUDJefe : MonoBehaviour
    {
        public GameObject hudLobo;
        private void OnTriggerEnter(Collider other) {
            if(other.CompareTag("Player")){
                hudLobo.SetActive(true);
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemigos{
    public class AILobos : MonoBehaviour
    {

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public void ImpactoExplosion()
        {
            Destroy(gameObject, 4f);
        }
    }
}
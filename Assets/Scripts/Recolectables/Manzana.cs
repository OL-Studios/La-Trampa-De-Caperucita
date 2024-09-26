using System.Collections;
using System.Collections.Generic;
using Enemigos;
using UnityEngine;

namespace Recolectables{
    public class Manzana : MonoBehaviour
    {
        [Header ("Objeto")]
        public float tiempoDemora;
        public float rango;
        public float fuerzaExplocion;
        public bool detono = false;
        private float cuentaRegresiva;

        [Header("VFX")]
        public GameObject vfxExplosion;

        
        void Start()
        {
            cuentaRegresiva = tiempoDemora;
        }
        void Update()
        {
            cuentaRegresiva -= Time.deltaTime; // Conteo regresivo para la explosión de la manzana
            if(cuentaRegresiva <= 0 && detono == false)
            {
                Explosion();
                detono = true;
            }
        }

        public void Explosion()
        {
            GameObject explosionParticles = Instantiate(vfxExplosion, transform.position, vfxExplosion.transform.rotation);
            //Esta declaración toma todos los collider con los que colisionó y los inserta en el array
            Collider[] colliders = Physics.OverlapSphere(transform.position, rango);
            foreach(var rangeObjects in colliders) //Buscará en cada objeto si posee rigidbody
            {
                EnemigosMenores enemigos = rangeObjects.GetComponent<EnemigosMenores>();
                if(enemigos != null)
                {
                    enemigos.MuerteEnemigo(1);
                }

                Rigidbody rb = rangeObjects.GetComponent<Rigidbody>();
                if(rb != null) //Si tiene rb, agregue la fuerza de explosión
                {
                    rb.AddExplosionForce(fuerzaExplocion * 10, transform.position, rango);
                }
            }
            gameObject.GetComponent<CapsuleCollider>().enabled = false;
            Destroy(gameObject);
            //Destroy(explosionParticles, 7f);
        }
    }
}
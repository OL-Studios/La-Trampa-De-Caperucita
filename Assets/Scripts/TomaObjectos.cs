using System;
using Jugador;
using UnityEngine;

public enum TipoRecolectable
{
    Manzana,
    Bebida
}

public class TomaObjetos : MonoBehaviour
{
    public GameObject panelInfo;
    public bool zonaActiva;

    public JugadorCombate jugador;

    public TipoRecolectable recolectable;

    private void Update() {
        if(Input.GetKeyDown(KeyCode.E) && zonaActiva){
            if(recolectable == TipoRecolectable.Manzana){
                Debug.Log("Tomó Manzana");
                jugador.cantManzanas++;
            }else{
                Debug.Log("Tomó Bebida");
                jugador.cantBebida++;
            }
            Destroy(gameObject);
        }
    }
    
    void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                zonaActiva = true;
                panelInfo.SetActive(true);
                Debug.Log("Entró a zona activa");
            }
        }

        private void OnTriggerExit(Collider other) {
            if (other.CompareTag("Player"))
            {
                zonaActiva = false;
                panelInfo.SetActive(false);
                Debug.Log("Salió de zona activa");
            }
        }
}
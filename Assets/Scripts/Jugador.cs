using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Jugador : MonoBehaviour
{
    public float velCorrer;
    public float velRotacion;
    public Animator animatorJugador;
    private float ejeX, ejeY;

//Salto
    public Rigidbody rbJugador;
    public float alturaSalto;
    public Transform comprobacionSuelo;
    public float distanciaSuelo;
    public LayerMask mascaraSuelo;
    private bool enSuelo;

    // Update is called once per frame
    void Update()
    {
        MovimientoJugador();
        SaltoJugador();
    }

    public void MovimientoJugador(){
        ejeX = Input.GetAxis("Horizontal");
        ejeY = Input.GetAxis("Vertical");
        transform.Rotate(0, ejeX * Time.deltaTime * velRotacion, 0);
        transform.Translate(0, 0, ejeY * Time.deltaTime * velCorrer);
        animatorJugador.SetFloat("velGirar", ejeX);
        animatorJugador.SetFloat("velCorrer", ejeY);
    }

    public void SaltoJugador(){
        enSuelo = Physics.CheckSphere(comprobacionSuelo.position, distanciaSuelo, mascaraSuelo);
        if(Input.GetKey("space") && enSuelo){
            animatorJugador.Play("Saltar");
            rbJugador.AddForce(Vector3.up * alturaSalto, ForceMode.Impulse);
        }
    }
}

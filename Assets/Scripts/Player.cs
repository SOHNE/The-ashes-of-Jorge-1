using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
    Leandro Peres, FEV de 2019
    Código produzido para o jogo Jorge2.0  
*/


public class Player : CharacterBase
{

    void Update()
    {

    }

    void FixedUpdate()
    {
        if (!isDead)
        {
            MoveHandler(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            if (Input.GetButtonDown("Jump") && OnGround)
                Jump();

            if (Input.GetButtonDown("Fire1"))
                Attack();
        }
    }
}

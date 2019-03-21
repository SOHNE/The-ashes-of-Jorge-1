using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Leandro Peres, MAR de 2019
    Protótipo The Ashes of Jorge
*/


public class Player : CharacterBase
{

    void Update()
    {
        Debug.DrawLine(transform.position, groundCheck.transform.position, Color.red);
    }

    void FixedUpdate()
    {
        if (!Dead)
        {
            MoveHandler(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            if (Input.GetButtonDown("Jump") && OnGround)
                Jump();

            //if (Input.GetButtonDown("Fire1"))
            //    Attack();
        }
    }
}

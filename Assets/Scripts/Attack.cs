using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {

	public int damage;
    public string type = "P";

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerEnter(Collider other)
	{
		Enemy enemy = other.GetComponent<Enemy>();
		Player player = other.GetComponent<Player>();
        
		if(enemy != null && type != "E")
		{
			enemy.TookDamage(damage);
		}

		if (player != null && type != "P")
		{
			player.TookDamage(damage);
		}
	}
}

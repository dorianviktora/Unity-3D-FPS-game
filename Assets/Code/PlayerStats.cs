using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
	public int playerMaxHealth = 100;
	public int playerHealth;

	public HealthBar healthBar;
	public Gun gun;
	public AutoGun autoGun;

	public AudioSource heal;
	public AudioSource ammo;

	void Start()
	{
		playerHealth = playerMaxHealth;
		healthBar.SetMaxHealth(playerMaxHealth);
	}

	public void TakePlayerDamage(int damage)
	{
		playerHealth -= damage;

		healthBar.SetHealth(playerHealth);
	}

	void OnTriggerEnter(Collider collider)
	{
		if (collider.gameObject.tag == "medkit")
		{
			Debug.Log("Medkit picked up");
			Destroy(collider.gameObject);
			healthBar.SetMaxHealth(playerMaxHealth);
			playerHealth = playerMaxHealth;
			heal.Play();
		}
		if (collider.gameObject.tag == "ammo")
		{
			Debug.Log("Ammo picked up");
			Destroy(collider.gameObject);
			gun.AmmoRefill();
			autoGun.AmmoRefill();
			ammo.Play();
		}	
	}
}

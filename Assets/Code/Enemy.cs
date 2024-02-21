using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
	public float enemyHealth = 50f;
	public NavMeshAgent nav;
	public Transform target;

	public GameObject zombie;
	public Rigidbody rb;

	public float sightRange = 10f;
	public float attackRange = 1.5f;

	private float nextTimeToAttack = 0f;
	private float attackRate = 0.5f;

	public PlayerStats playerStats;

	void Start()
	{
		nav = GetComponent<NavMeshAgent>();
	}

	void Update()
	{
		float distance = Vector3.Distance(target.position, transform.position);

		//chase player
		if (enemyHealth > 0 && distance <= sightRange)
			nav.SetDestination(target.position);
		
		//attack player
		if(enemyHealth > 0 && distance <= attackRange && Time.time >= nextTimeToAttack)
		{
			//Debug.Log("funguje to");

			nextTimeToAttack = Time.time + 1f / attackRate;
			playerStats.TakePlayerDamage(20);
		}
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, sightRange);
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(transform.position, attackRange);
	}
	public void TakeDamage(float amount)
	{
		enemyHealth -= amount;
		if (enemyHealth <= 0f)
		{
			Die();
		}
	}
	void Die()
	{
		Debug.Log("Zombie je mrtvej");
		nav.enabled = false;

		rb = zombie.GetComponent<Rigidbody>();
		rb.isKinematic = false;

		rb.AddForce(-transform.forward * 150);

		Invoke(nameof(DeleteObject), 2);
	}
	void DeleteObject()
	{
		Destroy(gameObject);
	}
}

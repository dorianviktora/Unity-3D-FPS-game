using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ragdoll : MonoBehaviour
{
	public float enemyHealth = 50f;
	public NavMeshAgent nav;
	public Transform target;

	public GameObject zombie;

	public float sightRange = 10f;
	public float attackRange = 1.5f;

	private float nextTimeToAttack = 0f;
	private float attackRate = 0.5f;

	public PlayerStats playerStats;

	public Collider collider1;
	public Rigidbody rigidbody1;

	public Animator animator;
	public bool chasePlayer = false;
	public bool attackPlayer = false;

	void Start()
	{
		nav = GetComponent<NavMeshAgent>();
		SetKinematic(true);
		SetColliderTrigger(true);
		animator = GetComponent<Animator>();	
	}

	void SetKinematic(bool newValue)
	{
		Rigidbody[] bodies = GetComponentsInChildren<Rigidbody>();
		foreach (Rigidbody rb in bodies)
		{
			rb.isKinematic = newValue;
		}
	}

	void SetColliderTrigger(bool newValue)
	{
		Collider[] colliders = GetComponentsInChildren<Collider>();
		foreach (Collider coll in colliders)
		{
			coll.isTrigger = newValue;
		}
	}

	void Update()
	{
		float distance = Vector3.Distance(target.position, transform.position);

		//is in SightRange
		if (enemyHealth > 0 && distance <= sightRange)
		{
			chasePlayer = true;
		}

		//attack player
		if (enemyHealth > 0 && distance <= attackRange && Time.time >= nextTimeToAttack)
		{
			nextTimeToAttack = Time.time + 1f / attackRate;
			playerStats.TakePlayerDamage(20);

			animator.SetBool("isPunching", true);
			animator.SetBool("isWalking", false);
		}
		
		if(chasePlayer && enemyHealth > 0 && distance >= attackRange)
		{
			nav.SetDestination(target.position);

			//Debug.Log("Ragdoll is out of attack range");
			animator.SetBool("isPunching", false);
			animator.SetBool("isWalking", true);
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
		Debug.Log("Ragdoll je DEAD");
		nav.enabled = false;

		//rigidbody1 = zombie.GetComponent<Rigidbody>();
		//rigidbody1.isKinematic = false;
		//rigidbody1.AddForce(-transform.forward * 1500);

		Destroy(collider1);
		Destroy(rigidbody1);
		GetComponent<Animator>().enabled = false;

		SetKinematic(false);
		SetColliderTrigger(false);


		Invoke(nameof(DeleteObject), 5);
	}
	void DeleteObject()
	{
		Destroy(gameObject);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HeadShot : MonoBehaviour
{
    Ragdoll ragdoll;
    public ParticleSystem headBlood;

    // Start is called before the first frame update
    void Start()
    {
        ragdoll = transform.root.gameObject.GetComponent<Ragdoll>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OneTapHead()
	{
        headBlood.Play();
        ragdoll.TakeDamage(50f);
	}
}

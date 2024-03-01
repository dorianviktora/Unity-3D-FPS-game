using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// COPY OF AUTOGUN, TODO: Gun parent of autogun

public class Gun : MonoBehaviour
{
    public float damage = 20f;
    public float fireRate = 4f;

    public int maxClipAmmo = 10;
    public int maxBackAmmo = 25;
    public int currentBackAmmo;
    private int currentClipAmmo;
    public float reloadTime = 2f;
    private bool isReloading = false;
    public Text ammoDisplay;
    public Text allAmmoDisplay;

    public ParticleSystem muzzleFlash;
    public Camera fpsCam;

    private float nextTimeToFire = 0f;

    public AudioSource gunFire;
    public AudioSource gunReload;

	void Start()
	{
        currentClipAmmo = maxClipAmmo;
        currentBackAmmo = maxBackAmmo;
	}

    void OnEnable()
	{
        isReloading = false;
	}

	// Update is called once per frame
	void Update()
    {
		if (Input.GetKeyDown("r") && currentBackAmmo > 0 && currentClipAmmo != maxClipAmmo)
		{
            StartCoroutine(Reload());
		}
        
        ammoDisplay.text = currentClipAmmo.ToString();
        allAmmoDisplay.text = currentBackAmmo.ToString();

        if (isReloading)
            return;

        if(currentClipAmmo <= 0 && currentBackAmmo > 0)
		{
            StartCoroutine(Reload());
            return;
		}

        if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire && currentClipAmmo > 0)
        {
            gunFire.Play();
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        gunReload.Play();
        //Debug.Log("Reloading...");

        yield return new WaitForSeconds(reloadTime);

        int ammoToReload = maxClipAmmo - currentClipAmmo;
        if (currentBackAmmo < ammoToReload)
        {
            currentClipAmmo += currentBackAmmo;
            currentBackAmmo = 0;
        }
		else
		{
            currentClipAmmo = maxClipAmmo;
            currentBackAmmo -= ammoToReload;
        }
        
        isReloading = false;
	}
    public void AmmoRefill()
    {
        currentBackAmmo = maxBackAmmo;
    }

    void Shoot()
    {
        muzzleFlash.Play();

        currentClipAmmo--;

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit))
        {
            Debug.Log(hit.transform.name);

            Enemy enemy = hit.transform.GetComponent<Enemy>();
            if (enemy != null)
                enemy.TakeDamage(damage);
           
            Ragdoll ragdoll = hit.transform.GetComponent<Ragdoll>();
            if (ragdoll != null)
                ragdoll.TakeDamage(damage);

            HeadShot headshot = hit.transform.GetComponent<HeadShot>();
            if (headshot != null)
                headshot.OneTapHead();
        }
	}
}
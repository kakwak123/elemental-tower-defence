using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shooting : MonoBehaviour
{
    public float damage = 7f;
    public float range = 1000f;
    public float fireRate = 3f;
    public float impactForce = 30f;
    public float nextTimeToFire = 0f;
    public Camera fpsCam;
    public GameObject secImpact;
    public GameObject impact;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            GetComponent<FMODUnity.StudioEventEmitter>().Play();
            nextTimeToFire = Time.time + 1f / fireRate;
            if (CameraSwitch.isOnPlan == true)
            {
                topDownShoot();
            }
            else
            {
                fpsShoot();
            }
            
        }
    }
    void fpsShoot(){
        GameObject shootImpact = Instantiate(secImpact, this.transform);
        Destroy(shootImpact, 1f);
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range)){
            Debug.Log(hit.transform.name);
            Enemy target = hit.transform.GetComponent<Enemy>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }
            GameObject gunImpact = Instantiate(impact, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(gunImpact, 1f);
            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal*impactForce);
            }          
        }
    }

    void topDownShoot()
    {
        GameObject shootImpact = Instantiate(secImpact, this.transform);
        Destroy(shootImpact, 1f);
        RaycastHit hit;
        Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(_ray, out hit))
        {
            Debug.Log(hit.transform.name);
            Enemy target = hit.transform.GetComponent<Enemy>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }
            GameObject gunImpact = Instantiate(impact, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(gunImpact, 1f);
            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }
        }
    }
}

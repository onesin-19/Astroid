using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tir : MonoBehaviour
{
    private RaycastHit hit;
    public AudioClip soundTir;
    public GameObject bulletPrefab;
    public GameObject startBullet;
    public GameObject player;

    public float bulletspeed=5.0f;
    private Vector3 cible;
    AudioSource sound;
    bool peutTirer=true;
    bool recharge;
    float tpsEcoule;
    // Start is called before the first frame update
    void Start()
    {
        sound = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
     
        cible = transform.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y, 100));
        cible.z = 0;
        Vector3 diff = (cible - new Vector3(player.transform.position.x, player.transform.position.y)).normalized;
        if (Input.GetButtonDown("Fire1"))
        {
            if (!player.GetComponent<Player>().gameOver&&!player.GetComponent<Player>().estInactif)
            {
                if (peutTirer)
                {
                    sound.PlayOneShot(soundTir);
                    float distance = diff.magnitude;
                    Vector2 direction = diff.normalized;// /distance;
                                                        //direction.Normalize();
                    TirerBullet(direction);
                    peutTirer = false;
                    recharge = true;
                    tpsEcoule = 0;
                }
            }
           
        }

        if (recharge)
        {
            tpsEcoule += Time.deltaTime;
            if ((tpsEcoule >= 1))
            {
                peutTirer = true;
                recharge = false;
             
            }
        }
    }

    void TirerBullet(Vector2 dir)
    {
        float rotationZ;
        GameObject b = Instantiate(bulletPrefab) as GameObject;
        b.transform.position = startBullet.transform.position;
        if (startBullet.transform.position.x > cible.x)
            rotationZ = Vector2.Angle(Vector2.up, startBullet.transform.position - player.transform.position);
        else
            rotationZ = -Vector2.Angle(Vector2.up, startBullet.transform.position - player.transform.position);

        b.transform.eulerAngles = new Vector3(0, 0, 90+rotationZ); 
        b.GetComponent<Rigidbody2D>().velocity = dir * bulletspeed;
        Destroy(b, 2.0f);
    }

}

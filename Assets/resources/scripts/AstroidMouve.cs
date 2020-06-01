using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstroidMouve : MonoBehaviour
{
    public float maxForce;
    public float maxTorsion;
    Rigidbody2D rb;
    public int tailleAstroid;
    public GameObject astroidPetit;
    public int valeur;
    GameObject player;
    public GameObject explosion;
    // Start is called before the first frame update
    void Start()
    {
        Vector2 translation = new Vector2(Random.Range(-maxForce, maxForce), Random.Range(-maxForce, maxForce));
        float torsion = Random.Range(-maxTorsion, maxTorsion);
        rb = transform.GetComponent<Rigidbody2D>();
        rb.AddForce(translation);
        rb.AddTorque(torsion);
        player = GameObject.FindWithTag("Player");
        
        
    }

    // Update is called once per frame
    void Update()
    {
        ChangerPosition();
    }

    void ChangerPosition()
    {

        Vector2 newPosition = transform.position;
        if (transform.position.x >= GameFlow.floorX / 2 + transform.localScale.x / 2)
        {
            newPosition.x = -GameFlow.floorX / 2 - transform.localScale.x / 2;

        }
        else if (transform.position.x <= -GameFlow.floorX / 2 - transform.localScale.x / 2)
        {
            newPosition.x = GameFlow.floorX / 2 + transform.localScale.x / 2;
        }
        else if (transform.position.y >= GameFlow.floorY / 2 + transform.localScale.y / 2)
        {
            newPosition.y = -GameFlow.floorY / 2 - transform.localScale.y / 2;
        }
        else if (transform.position.y <= -GameFlow.floorY / 2 - transform.localScale.y / 2)
        {
            newPosition.y = GameFlow.floorY / 2 + transform.localScale.y / 2;
        }
        transform.position = newPosition;

    }
    bool estDansLaZoneDeJeu()
    {
        return (transform.position.x < GameFlow.floorX / 2 + transform.localScale.x / 2) &&
            (transform.position.x > -GameFlow.floorX / 2 - transform.localScale.x / 2) &&
            (transform.position.y < GameFlow.floorY / 2 + transform.localScale.y / 2) &&
            (transform.position.y > -GameFlow.floorY / 2 - transform.localScale.y / 2);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.transform.gameObject.CompareTag("bulletPlayer")|| collision.transform.gameObject.CompareTag("bulletEnemy"))&& estDansLaZoneDeJeu())
        {
            
            Destroy(collision.transform.gameObject);
            GameFlow.astroids.Remove(collision.transform.gameObject);
            if (tailleAstroid==4)
            {
                GameObject astroid1 = Instantiate(astroidPetit, transform.position, transform.rotation);
                GameObject astroid2 = Instantiate(astroidPetit, transform.position, transform.rotation);
                GameFlow.astroids.Add(astroid1);
                GameFlow.astroids.Add(astroid2);
                astroid1.GetComponent<AstroidMouve>().tailleAstroid = 2;
                astroid2.GetComponent<AstroidMouve>().tailleAstroid = 2;
                
            }
            else if (tailleAstroid==2)
            {

            }
            player.GetComponent<Player>().SetScore(valeur);

            GameObject newExplosion= Instantiate(explosion, transform.position, transform.rotation);
            Destroy(newExplosion, 1f);
            GameFlow.astroids.Remove(gameObject);
            Destroy(gameObject);
        }
       
    }


}

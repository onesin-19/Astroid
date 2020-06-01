using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float speed = 20.0f;
    public float speedRotation = 20.0f;
    float translation;
    float staffe;
    Rigidbody2D rb;
    [HideInInspector] public int score;
    [HideInInspector] public float PointDeVie=3;
    [HideInInspector] public float pointDeVieRestant;
    [HideInInspector] public int scoreMax;
    public GameObject explosion;
    [HideInInspector] public bool gameOver = false;
    [HideInInspector] public bool estInactif = false;
    public GameObject gameOverPanel;
    float tpsEcoule;
    public Color couleurNormal;
    public Color couleurInactif;
    public GameObject left;
    public GameObject right;
    // Start is called before the first frame update
    void Start()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        //tr = transform.GetComponent<TrailRenderer>();
        score = 0;
        pointDeVieRestant = PointDeVie;
    }

    // Update is called once per frame
    void Update()
    {
        translation = Input.GetAxis("Vertical")*speed;
        staffe = Input.GetAxis("Horizontal")* speedRotation;
        if ((translation > 0 || staffe > 0)&& !estInactif)
        {
            left.SetActive(true);
            right.SetActive(true);
        }
        rb.AddRelativeForce(Vector2.up * translation);
        transform.Rotate(-Vector3.forward * staffe * Time.deltaTime);
        ChangerPosition();
        if (gameOver)
        {
            tpsEcoule += Time.deltaTime;
            if ((tpsEcoule >= 5))
            {
                //charger le menu
                SceneManager.LoadScene("Menu");
            }
        }
        
       
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
            (transform.position.x > -GameFlow.floorX / 2 - transform.localScale.x / 2)&&
            (transform.position.y < GameFlow.floorY / 2 + transform.localScale.y / 2)&&
            (transform.position.y > -GameFlow.floorY / 2 - transform.localScale.y / 2);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.transform.gameObject.CompareTag("Astroid")|| collision.transform.gameObject.CompareTag("bulletEnemy"))&& estDansLaZoneDeJeu())
        {
           
            pointDeVieRestant--;
            GameObject newExplosion = Instantiate(explosion, transform.position, transform.rotation);
            Destroy(newExplosion, 2f);
            left.SetActive(false);
            right.SetActive(false);
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false; 
            estInactif = true;
            Invoke("Respawn", 5f);      
            if (score > scoreMax)
            {
                scoreMax = score; 
            }
            score = 0;
            if (pointDeVieRestant <= 0)
            {
                gameOver = true;
                GameOver();
            }
        }
        if (collision.transform.gameObject.CompareTag("bulletEnemy"))
        {
            Destroy(collision.transform.gameObject);
        }
        
    }

    void Respawn()
    {
        rb.velocity = Vector2.zero;
        transform.position = Vector2.zero;
        SpriteRenderer sr= GetComponent<SpriteRenderer>();
        sr.enabled = true;
        estInactif = true;
        sr.color = couleurInactif;
        Invoke("Inactif", 5f);
    }

    void Inactif()
    {
        GetComponent<Collider2D>().enabled = true;
        estInactif = false;
        GetComponent<SpriteRenderer>().color = couleurNormal;
    }
    void GameOver()
    {
        gameOverPanel.SetActive(true);
        CancelInvoke();
        
        
    }

    public void SetScore(int scoreAjouter)
    {
        score += scoreAjouter;

    }
}

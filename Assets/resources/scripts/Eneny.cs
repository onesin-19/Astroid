using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eneny : MonoBehaviour
{
    Rigidbody2D rb;
    Transform player;
    public float dernierTir=0f;
    public float delaiTir;
    Vector2 direction;
    public float speed;
    public float bulletSpeed;
    public GameObject bullet;
    public GameObject StartPointShoot;
    public GameObject explosion;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").transform;
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        direction = (player.position - transform.position).normalized;
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
        
        ChangerPosition();
    }

    private void Update()
    {
        if (Time.time > dernierTir + delaiTir)
        {
            float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Deg2Rad - 90f;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.right);
            GameObject newBullet = Instantiate(bullet, StartPointShoot.transform.position, q);
            newBullet.GetComponent<Rigidbody2D>().velocity = (direction.x==0 && direction.y == 0) ? new Vector2(23,1): direction * bulletSpeed;
            Destroy(newBullet, 2.0f);
            dernierTir = Time.time;
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
            (transform.position.x > -GameFlow.floorX / 2 - transform.localScale.x / 2) &&
            (transform.position.y < GameFlow.floorY / 2 + transform.localScale.y / 2) &&
            (transform.position.y > -GameFlow.floorY / 2 - transform.localScale.y / 2);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.tag.Equals("Astroid") || collision.transform.gameObject.CompareTag("bulletPlayer"))&&estDansLaZoneDeJeu())
        {
            GameObject newExplosion = Instantiate(explosion, transform.position, transform.rotation);
            Destroy(newExplosion, 2f);
            GameFlow.enemies.Remove(gameObject);
            Destroy(gameObject);
        }
        if (collision.transform.gameObject.CompareTag("bulletPlayer"))
        {
            Destroy(collision.transform.gameObject);
        }
    }
}

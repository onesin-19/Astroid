using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameFlow : MonoBehaviour
{
    
    public int numOfStartingEnemies;
    GameObject enemyPrefab;
    GameObject AstroidPrefab;
    Transform enemyParent;
    int nbEnemyActuel;
    int nbAstroidActuel;
    public static float floorX;
    public static float floorY;
    float duree;
    float tpsEcoule;
    float dureeSpawnNewAstroid;
    public static List<GameObject> enemies;
    public static List<GameObject> astroids; 

    public Text textLive;
    public Text textScore;
    public Text scoreMax;
    public Text nbEnemiText;
    public Image liveBar;
    GameObject player;
    public GameObject winPanel;
    public GameObject PausePAnel;
    bool win=false;
    bool estActive = true;

    // Start is called before the first frame update
    void Awake()
    {
        enemyPrefab = Resources.Load<GameObject>("prefabs/Extraterrestre");//cache the prefab
        AstroidPrefab = Resources.Load<GameObject>("prefabs/Astroid2");//cache the prefab
        enemies = new List<GameObject>();
        astroids = new List<GameObject>();
        enemyParent = (new GameObject()).transform;
        floorX = Camera.main.orthographicSize*Camera.main.aspect*2;
        floorY = Camera.main.orthographicSize*2;
        scoreMax.text = "Score Max : 0";
        player = GameObject.FindWithTag("Player");
        float pointDeVieRestant = player.GetComponent<Player>().pointDeVieRestant;
        textLive.text = "Vie " + pointDeVieRestant.ToString();


    }

    // Update is called once per frame
    void Update()
    {
        SpawnEnemy();
        SpawnAstroid();
        float PointDeVie=player.GetComponent<Player>().PointDeVie;
        float pointDeVieRestant = player.GetComponent<Player>().pointDeVieRestant;
        int score = player.GetComponent<Player>().score;
        int scorMax = player.GetComponent<Player>().scoreMax;

        textScore.text = "Score : " + score;
        scoreMax.text= "Score Max : "+ scorMax;
        textLive.text = "Vie " + pointDeVieRestant.ToString();
        nbEnemiText.text = "nombre enemis restant: "+(astroids.Count+enemies.Count)+"    ";
        liveBar.fillAmount = pointDeVieRestant/PointDeVie;
        liveBar.color = Color.Lerp(Color.red, Color.green, liveBar.fillAmount);

        if (astroids.Count <= 0&&enemies.Count<=0)
        {
            player.GetComponent<CapsuleCollider2D>().enabled = true;
            
            win = true;
            winPanel.SetActive(true);
        }
        if (win)
        {
            tpsEcoule += Time.deltaTime;
            if ((tpsEcoule >= 5))
            {
                //charger le menu
                SceneManager.LoadScene("Menu");
            }
        }
        ckeckPause();
    }

    void SpawnEnemy()
    {


        if (nbEnemyActuel < numOfStartingEnemies)
        {
            if (duree >= nbEnemyActuel * 15)
            {
                GameObject newEnemy = GameObject.Instantiate(enemyPrefab, enemyParent);

                newEnemy.transform.position = new Vector2(Random.Range(-floorX / 2 + newEnemy.transform.localScale.x / 2, floorX / 2 - newEnemy.transform.localScale.x / 2), Random.Range(-floorY / 2 + newEnemy.transform.localScale.y / 2, floorY / 2 - newEnemy.transform.localScale.y / 2));
                enemies.Add(newEnemy);
                nbEnemyActuel++;
            }

        }
        duree += Time.deltaTime;
        
    }

    void SpawnAstroid()
    {
        if (nbAstroidActuel < numOfStartingEnemies)
        {
            
            if (dureeSpawnNewAstroid >= nbAstroidActuel * 10)
            {
                /*GameObject newAstroid = GameObject.Instantiate(AstroidPrefab, enemyParent);
                newAstroid.transform.position = new Vector2 (Random.Range(-floorX / 2 + newAstroid.transform.localScale.x / 2, floorX / 2 - newAstroid.transform.localScale.x / 2), Random.Range(-floorY / 2 + newAstroid.transform.localScale.y / 2, floorY / 2 - newAstroid.transform.localScale.y / 2));
                astroids.Add(newAstroid);
                nbAstroidActuel++;*/

                Vector2 pos = new Vector2(Random.Range(-floorX / 2 + AstroidPrefab.transform.localScale.x / 2, floorX / 2 - AstroidPrefab.transform.localScale.x / 2), Random.Range(-floorY / 2 + AstroidPrefab.transform.localScale.y / 2, floorY / 2 - AstroidPrefab.transform.localScale.y / 2));

                Collider2D hit = Physics2D.OverlapCircle(player.transform.position, 5f, LayerMask.GetMask("Enemy"));
                
                if (hit!=null && hit.gameObject.transform.position.Equals(pos))
                {

                    /*GameObject newAstroid = GameObject.Instantiate(AstroidPrefab, enemyParent);
                    newAstroid.transform.position = pos;//new Vector2 (Random.Range(-floorX / 2 + newAstroid.transform.localScale.x / 2, floorX / 2 - newAstroid.transform.localScale.x / 2), Random.Range(-floorY / 2 + newAstroid.transform.localScale.y / 2, floorY / 2 - newAstroid.transform.localScale.y / 2));
                    astroids.Add(newAstroid);
                    nbAstroidActuel++;*/
                }
                if (Vector2.Distance(player.transform.position, pos) >= 15)
                {
                    GameObject newAstroid = GameObject.Instantiate(AstroidPrefab, enemyParent);
                    newAstroid.transform.position = pos;// new Vector2(Random.Range(-floorX / 2 + newAstroid.transform.localScale.x / 2, floorX / 2 - newAstroid.transform.localScale.x / 2), Random.Range(-floorY / 2 + newAstroid.transform.localScale.y / 2, floorY / 2 - newAstroid.transform.localScale.y / 2));
                    astroids.Add(newAstroid);
                    nbAstroidActuel++;
                }

            }

        }

        dureeSpawnNewAstroid += Time.deltaTime;
    }

    void ckeckPause()
    {
        if (estActive)
        {
            PausePAnel.SetActive(false);
            Time.timeScale = 1;
        }
        else
        {
            PausePAnel.SetActive(true);
            Time.timeScale = 0;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            estActive = !estActive;
        }
    }
}

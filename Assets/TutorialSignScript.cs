using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSignScript : MonoBehaviour
{
    private GameObject canvasGameObject;
    public bool isStart = false;
    private EnemySpawner enemySpawner;

    private void Awake()
    {
        canvasGameObject = transform.GetChild(0).gameObject;
        canvasGameObject.SetActive(false);
        enemySpawner = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<EnemySpawner>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            canvasGameObject.SetActive(true);
            /*if (isStart)
            {
                isStart = false;
                enemySpawner.isStart = true;
            }*/
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            canvasGameObject.SetActive(false);
        }
    }
}

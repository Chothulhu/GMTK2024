using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSignScript : MonoBehaviour
{
    private GameObject canvasGameObject;

    private void Awake()
    {
        canvasGameObject = transform.GetChild(0).gameObject;
        canvasGameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            canvasGameObject.SetActive(true);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObjectX : MonoBehaviour
{
    public GameManagerX gameManager;

    void Awake()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManagerX>();
    }

    void Start()
    {
        Invoke("DestroyObject", 2);
    }
    void OnEnable()
    {
    }


    void DestroyObject()
    {
        if(!this.gameObject.CompareTag("Bad"))
        {
            gameManager.GameOver();
        }
        Destroy(gameObject); // destroy particle after 2 seconds
    }
}

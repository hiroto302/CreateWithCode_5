using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ターゲットクラス
public class Target : MonoBehaviour
{
    private float _minSpeed = 12, _maxSpeed = 16 ;
    private float _maxTorque = 10;
    private float _xRange = 4, _ySpawnPos = -6;
    Rigidbody _targetRb;
    public int pointValue;
    public ParticleSystem explosionParticle;
    void Start()
    {
        // Creates a new vector with given x, y components and sets z to zero. So, z is always zero.
        transform.position = RandomSpawnPos();

        _targetRb = GetComponent<Rigidbody>();
        _targetRb.AddForce(RandomForce(), ForceMode.Impulse);
        _targetRb.AddTorque(RandomTorque(), RandomTorque(), RandomTorque(), ForceMode.Impulse);

        // オブジェクトの名前によって、pointValue を初期化するメソッド追加していいかも
        // Debug.Log(this.gameObject.name);
    }

    void Update()
    {
        
    }

    void OnMouseDown()
    {
        if(GameManager.Instance.isGameActive)
        {
            Destroy(gameObject);
            GameManager.UpdateScore(pointValue);
            Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation);
        }
    }

    // Sensor(存在範囲) に触れた時このオブジェクトを削除
    void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
        // Bad Object 以外の場合, Lives を減らしていく
        if(!gameObject.CompareTag("Bad"))
        {
            GameManager.Instance.UpdateLives(-1);
            // GameManager.Instance.GameOver(true);
        }
    }

    Vector3 RandomForce()
    {
        return Vector3.up * Random.Range(_minSpeed, _maxSpeed);
    }

    float RandomTorque()
    {
        return Random.Range(-_maxTorque, _maxTorque);
    }

    Vector3 RandomSpawnPos()
    {
        return new Vector3(Random.Range(-_xRange, _xRange), _ySpawnPos);
    }


}

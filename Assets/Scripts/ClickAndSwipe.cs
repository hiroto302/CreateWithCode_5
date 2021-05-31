using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Trail を利用して、Target を切り裂くように破壊することを可能とする機能を実装する

// This code will ensure that a TrailRenderer and BoxCollider are on the GameObject the script is attached to.
// このスクリプトをアタッチした時、下記に指定したComponent がなければ自動的にアタッチしてくれる
[RequireComponent(typeof(TrailRenderer), typeof(BoxCollider))]
public class ClickAndSwipe : MonoBehaviour
{
    private GameManager gameManager;
    private Camera cam;
    private Vector3 mousePos;
    private TrailRenderer trail;
    private BoxCollider col;
    private bool swiping = false;

    void Awake()
    {
        cam = Camera.main;
        trail = GetComponent<TrailRenderer>();
        col = GetComponent<BoxCollider>();
        trail.enabled = false;
        col.enabled = false;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        // Swipe 操作の制御
        if (gameManager.isGameActive)
        {
            if (Input.GetMouseButtonDown(0))
            {
                swiping = true;
                UpdateComponents();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                swiping = false;
                UpdateComponents();
            }

            if (swiping)
            {
                UpdateMousePosition();
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<Target>())
        {
            //Destroy the target
            collision.gameObject.GetComponent<Target>().DestroyTarget();
        }
    }

    // マウスのカーソルの位置に合わせて、object を移動させる
    void UpdateMousePosition()
    {
        // ScreenToWorldで、マウスのスクリーン位置をワールド位置に変換
        // カメラのz位置が-10.0fなので, z軸に 10.0fを指定
        mousePos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
        transform.position = mousePos;
    }

    // コンポーネント(trail & collier) の有効状態を切り替え
    void UpdateComponents()
    {
        trail.enabled = swiping;
        col.enabled = swiping;
    }
}

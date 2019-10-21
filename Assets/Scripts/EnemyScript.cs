using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{

    public float health = 3;

    private bool isShaking = false;
    private float shakeAmount = 0.5f;
    private Vector2 startPos;
    private PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        player = GetComponent<PlayerController>();

    }

    // Update is called once per frame
    private void Update()
    {
        if (isShaking)
        {
            transform.position = startPos + UnityEngine.Random.insideUnitCircle * shakeAmount;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!isShaking && collision.gameObject.name == "AttackHitbox")
        {
            TakeDamage(player.damage);
            if(health <= 0)
            {
                Destroy(gameObject);
            }
            else
            {
                isShaking = true;
                Invoke("StopShaking", .5f);
            }
        }

    }
    void StopShaking()
    {
        isShaking = false;

    }

    void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("damage taken");
    }
}

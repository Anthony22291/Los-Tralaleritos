using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Variables")]
    public float timeBtwSpawn = 1.5f;
    float timer = 0;
    [Header("Referencias")]
    public Transform leftPoint;
    public Transform rightPoint;
    public List<GameObject> enemy;

    void Start()
    {
        
    }

    void Update()
    {
        if(timer >= timeBtwSpawn)
        {
            timer = 0;
            float y = transform.position.y;
            float x = Random.Range(leftPoint.position.x, rightPoint.position.x);
            Vector2 pos = new Vector2(x, y);
            Instantiate(enemy[Random.Range(0, enemy.Count)], pos, Quaternion.Euler(0, 0, 180));
        }
        else
        {
            timer += Time.deltaTime;
        }
    }
}

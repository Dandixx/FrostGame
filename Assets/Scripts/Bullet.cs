using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int _damage;
    [SerializeField] private float _speed;


    private void Update()
    {
        gameObject.transform.Translate(Vector2.right * _speed * Time.deltaTime);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            print("Okay");
            collision.gameObject.GetComponent<Character>().TakeDamage(_damage);
            GameObject.Destroy(gameObject);
            
        }
    }


}


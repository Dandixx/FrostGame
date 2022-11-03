using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingCharacter : Character
{
    public Bullet bullet;


    public override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || _moving)
        {
            OnMove();


        }
    }


    private void Shoot()
    {
        Instantiate(bullet,gameObject.transform);
    }

    public override void OnMove()
    {
        base.OnMove();
        if (gameObject.transform.position == _targetPosition)
            Shoot();
    }

}



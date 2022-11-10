using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private float _movementSpeed = 3f;

    void Start()
    {
        _currentAnim = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        OnMove();
    }

    public override void OnMove()
    {
        if (Input.GetAxis("Horizontal") > 0)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            transform.Translate(Vector2.right * Input.GetAxis("Horizontal") * _movementSpeed * Time.deltaTime);
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            transform.Translate(Vector2.left * Input.GetAxis("Horizontal") * _movementSpeed * Time.deltaTime);
            transform.localRotation = Quaternion.Euler(0, 180, 0);

        }
    }
}

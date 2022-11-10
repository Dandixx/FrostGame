using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class Character : MonoBehaviour
{
    [SerializeField] private int _hp = 5;
    [SerializeField] private int _damage = 3;
    [SerializeField] private float _movingDistance = 1.4f;
    [SerializeField] protected RuntimeAnimatorController[] _currentController;
    private float _speed = 1f; // �������� �������� � �����

    protected Vector3 _targetPosition; // ������� ��� �����������
    protected Vector3 _startingPosition; // ��������� �������
    protected Vector3 _currentPosition = new Vector3(); // ������� �������


    protected bool _moving = false; // ����������, ����������� � ������ ������ ���������� �� � �������� ��������
    public Character enemy;

    protected Animator _currentAnim;
    
    


    public void TakeDamage( int damage) // �����, ������� ������� ����
    {
        if (_hp > 0)
        {
            _hp -= damage;
            if (_hp <= 0)
                GameObject.Destroy(gameObject);
        }
    }

    public virtual void Start()
    {
      _startingPosition = gameObject.transform.position;
      _targetPosition = new Vector3(_movingDistance, gameObject.transform.position.y);
       _currentAnim = gameObject.GetComponent<Animator>();
        
         
    }
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space) || _moving )
        {
            OnMove();
           

        }
    }
    public virtual void OnMove() // ����� �������� � �����
    {
        if (gameObject.transform.position != _targetPosition)
        {

            _moving = true;
           _currentAnim.SetInteger("Controller", 1);
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, _targetPosition, _speed * Time.deltaTime);

        }
        else if (_targetPosition == _startingPosition && gameObject.transform.position == _startingPosition)
        {
            _moving = false;
            _currentAnim.SetInteger("Controller", 0);
            _targetPosition = _currentPosition;

        }
        else if (gameObject.transform.position == _targetPosition )
        {
            _moving = false;
            _currentPosition = _targetPosition;
            _targetPosition = _startingPosition;
            _currentAnim.SetInteger("Controller", 0);

        }

    }

}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class Character : MonoBehaviour
{
    [SerializeField] private int _hp = 5;
    [SerializeField] private int _damage = 3;
    [SerializeField] private float _movingDistance = 1.4f;

    public CharacterAnimationController _animationController;

    protected CharacterInventory _inventory;

    private float _speed = 1f; // Скорость движение к врагу

    protected Vector3 _targetPosition; // позиция для перемещения
    protected Vector3 _startingPosition; // стартовая позиция
    protected Vector3 _currentPosition = new Vector3(); // текущая позиция


    protected bool _moving = false; // Переменная, проверяющая в данный момент находиться ли в движении персонаж
    public Character enemy;

    protected Animator _currentAnim;
    
    


    public void TakeDamage( int damage) // метод, который наносит урон
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
      _targetPosition = new Vector3(_movingDistance, gameObject.transform.position.y); // Позиция для перемещения
      _currentAnim = gameObject.GetComponent<Animator>();
        if (_animationController.characterWithMachine != null)
        {
            _currentAnim.runtimeAnimatorController = _animationController.characterWithMachine;
            gameObject.GetComponent<Animator>().runtimeAnimatorController = _currentAnim.runtimeAnimatorController;
            _currentAnim.SetInteger("Controller", 0);


        }
        else if (_animationController.characterWithShootgun != null)
        {
            _currentAnim.runtimeAnimatorController = _animationController.characterWithShootgun;
            gameObject.GetComponent<Animator>().runtimeAnimatorController = _currentAnim.runtimeAnimatorController;
            _currentAnim.SetInteger("Controller", 0);

        }
         else if (_animationController.characterWithPistol != null)
        {
            _currentAnim.runtimeAnimatorController = _animationController.characterWithPistol;
            gameObject.GetComponent<Animator>().runtimeAnimatorController = _currentAnim.runtimeAnimatorController;
            _currentAnim.SetInteger("Controller", 0);
        }
         else if (_animationController.characterWithSteelArms != null)
        {
            _currentAnim.runtimeAnimatorController = _animationController.characterWithSteelArms;
            gameObject.GetComponent<Animator>().runtimeAnimatorController = _currentAnim.runtimeAnimatorController;
            _currentAnim.SetInteger("Controller", 0);
        }

    }
    private void Update()
    {


        if (Input.GetKeyDown(KeyCode.Space) || _moving )
        {
            OnMove();
           

        }
        else
        {
            _currentAnim.SetInteger("Controller", 0);
        }
    }
    public virtual void OnMove() // Метод движения к врагу
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



[System.Serializable]
public struct CharacterAnimationController
{
    public RuntimeAnimatorController characterWithMachine; // Персонаж с автоматом

    public RuntimeAnimatorController characterWithoutWeapon; // Персонаж без оружия

    public RuntimeAnimatorController characterWithPistol; // Персонаж с пистолетом

    public RuntimeAnimatorController characterWithShootgun; // Персонаж с дробовиком

    public RuntimeAnimatorController characterWithSteelArms; // Персонаж с холодным оружием

}


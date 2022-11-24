using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class Character : MonoBehaviour
{
    [SerializeField] private int _hp = 5;
    [SerializeField] private int _damage;
    [SerializeField] private float _movingDistance = 1f;

    public CharacterAnimationController _animationController;

    private float _speed = 2.5f; // Скорость движение к врагу
    private float _timer = 2f;

    protected Vector3 _targetPosition; // позиция для перемещения
    protected Vector3 _startingPosition; // стартовая позиция
    protected Vector3 _currentPosition = new Vector3(); // текущая позиция


    protected bool _moving = false; // Переменная, проверяющая в данный момент находиться ли в движении персонаж

    
    private  Character[] _enemies; // массив врагов для присвоенмя в список
    private List<Character> _enemiesList = new List<Character>(); // список врагов, которых нужно атаковать

    protected Animator _currentAnim; // Текущая анимация персонажа
    


    public void TakeDamage( int damage) // метод, который наносит урон
    {
        if (_hp > 0)
        {
            _hp -= damage;

            if (_hp <= 0)
            {
                if (gameObject.tag == "Player")
                {
                    BattleController.playerPeople.Remove(gameObject.GetComponent<Character>());
                    Destroy(gameObject);

                }
                if (gameObject.tag == "Enemy")
                {
                    BattleController.enemiesPeople.Remove(gameObject.GetComponent<Character>());
                    Destroy(gameObject);

                }
                _enemiesList.Remove(this);
            }
        }
    }

    public virtual void Start()
    {
      _startingPosition = gameObject.transform.position;
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

      
        _enemies = FindObjectsOfType<Character>();

        if (gameObject.tag == "Player") // Если на данный момент персонаж - персонаж игрока, то находим врагов
        {
            for (int i = 0; i < _enemies.Length; i++)
            {

                if (_enemies[i].tag == "Enemy")
                {
                    _enemiesList.Add(_enemies[i]);
                    //print(_enemies[i]);

                }

            }

        }
        else if (gameObject.tag == "Enemy") // Если на данный момент персонаж - враг , то находим игроков
        {
            _movingDistance = -_movingDistance;

            for (int i = 0; i < _enemies.Length; i++)
            {
                if (_enemies[i].tag == "Player")
                {
                    _enemiesList.Add(_enemies[i]);

                }

            }

        }
        _targetPosition = new Vector3(_movingDistance, gameObject.transform.position.y); // Позиция для перемещения

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || _moving || BattleController.isAttack == true  )
        {                 
                BattleController.isAttack = false;
                BattleController.characterTern.OnMove();
        }
        if (Input.GetMouseButtonDown(0) && gameObject.GetComponent<SpriteRenderer>().color == Color.red && BattleController.hasClicked == true) // выбор вражеского персонажа
        {
            BattleController.currentCharacter = gameObject.GetComponent<Character>();
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
            _currentAnim.SetInteger("Controller", 2);
            StartCoroutine(Attackable());          
        }
    }
    private void OnMouseEnter()
    {
        if (BattleController.hasClicked == true && gameObject.tag !="Player")
        gameObject.GetComponent<SpriteRenderer>().color = Color.red;
    }
    private void OnMouseExit()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }

    IEnumerator Attackable()
    {       
        yield return new WaitForSeconds(_timer); // ожидание для проиграша анимации
       
        if (BattleController.battleState == BattleState.PLAYERTURN)
        {
            foreach (Character character in _enemiesList)
            {
                if (BattleController.currentCharacter == character)
                {
                    BattleController.currentCharacter.TakeDamage(BattleController.cardDamage);
                }
                if (BattleController.currentCharacter == null)
                {
                    BattleController.currentCharacter = BattleController.enemiesPeople[Random.Range(0, BattleController.enemiesPeople.Count)];
                    BattleController.currentCharacter.TakeDamage(BattleController.cardDamage);
                }
            }

            yield return new WaitForSeconds(_timer);
            _moving = true;

            BattleController.battleState = BattleState.ENEMYTURN;
            yield return new WaitForSeconds(_timer); // ожидание для того, чтобы персонаж вернулся на свою позицию

            BattleController.changeCharacterTern = true; // смена персонажа на противоположную команду

            yield return new WaitForSeconds(_timer); // ожидание для того, чтобы персонаж вернулся на свою позицию
            print("Next squad");
            BattleController.isAttack = true;
        }
        else if (BattleController.battleState == BattleState.ENEMYTURN)
        {

            BattleController.playerPeople[Random.Range(0, BattleController.playerPeople.Count)].TakeDamage(BattleController.cardDamage);
            
            yield return new WaitForSeconds(_timer);
            _moving = true;

            BattleController.battleState = BattleState.PLAYERTURN;      
           yield return new WaitForSeconds(_timer); // ожидание для того, чтобы персонаж вернулся на свою позици
            BattleController.changeCharacterTern = true; // смена персонажа на противоположную команду
            yield return new WaitForSeconds(_timer); // ожидание для того, чтобы персонаж вернулся на свою позицию
            print("Player squad");

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


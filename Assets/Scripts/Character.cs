using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class Character : MonoBehaviour
{
    [SerializeField] private int _hp = 5;
    [SerializeField] private int _damage;
    [SerializeField] private float _movingDistance = 1f;

    public CharacterAnimationController _animationController;

    private float _speed = 2.5f; // �������� �������� � �����
    private float _timer = 2f;

    protected Vector3 _targetPosition; // ������� ��� �����������
    protected Vector3 _startingPosition; // ��������� �������
    protected Vector3 _currentPosition = new Vector3(); // ������� �������


    protected bool _moving = false; // ����������, ����������� � ������ ������ ���������� �� � �������� ��������

    
    private  Character[] _enemies; // ������ ������ ��� ���������� � ������
    private List<Character> _enemiesList = new List<Character>(); // ������ ������, ������� ����� ���������

    protected Animator _currentAnim; // ������� �������� ���������
    


    public void TakeDamage( int damage) // �����, ������� ������� ����
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

        if (gameObject.tag == "Player") // ���� �� ������ ������ �������� - �������� ������, �� ������� ������
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
        else if (gameObject.tag == "Enemy") // ���� �� ������ ������ �������� - ���� , �� ������� �������
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
        _targetPosition = new Vector3(_movingDistance, gameObject.transform.position.y); // ������� ��� �����������

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || _moving || BattleController.isAttack == true  )
        {                 
                BattleController.isAttack = false;
                BattleController.characterTern.OnMove();
        }
        if (Input.GetMouseButtonDown(0) && gameObject.GetComponent<SpriteRenderer>().color == Color.red && BattleController.hasClicked == true) // ����� ���������� ���������
        {
            BattleController.currentCharacter = gameObject.GetComponent<Character>();
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
        yield return new WaitForSeconds(_timer); // �������� ��� ��������� ��������
       
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
            yield return new WaitForSeconds(_timer); // �������� ��� ����, ����� �������� �������� �� ���� �������

            BattleController.changeCharacterTern = true; // ����� ��������� �� ��������������� �������

            yield return new WaitForSeconds(_timer); // �������� ��� ����, ����� �������� �������� �� ���� �������
            print("Next squad");
            BattleController.isAttack = true;
        }
        else if (BattleController.battleState == BattleState.ENEMYTURN)
        {

            BattleController.playerPeople[Random.Range(0, BattleController.playerPeople.Count)].TakeDamage(BattleController.cardDamage);
            
            yield return new WaitForSeconds(_timer);
            _moving = true;

            BattleController.battleState = BattleState.PLAYERTURN;      
           yield return new WaitForSeconds(_timer); // �������� ��� ����, ����� �������� �������� �� ���� ������
            BattleController.changeCharacterTern = true; // ����� ��������� �� ��������������� �������
            yield return new WaitForSeconds(_timer); // �������� ��� ����, ����� �������� �������� �� ���� �������
            print("Player squad");

        }

    }
}



[System.Serializable]
public struct CharacterAnimationController
{
    public RuntimeAnimatorController characterWithMachine; // �������� � ���������

    public RuntimeAnimatorController characterWithoutWeapon; // �������� ��� ������

    public RuntimeAnimatorController characterWithPistol; // �������� � ����������

    public RuntimeAnimatorController characterWithShootgun; // �������� � ����������

    public RuntimeAnimatorController characterWithSteelArms; // �������� � �������� �������

}


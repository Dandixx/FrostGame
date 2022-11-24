using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    public static bool isAttack = false; // Переменная, которая говорит, что время для анимации атаки
    public static bool changeCharacterTern = false; // меняет ход персонажа в списке
    public static int cardDamage; // Атака текущей картой
    public static Character currentCharacter; // Выбор врага для атаки

    [HideInInspector] public static Character characterTern; // Текущий персонаж в команде

    public static bool hasClicked = false; // переменная, отвеающая за выбор врага перед уничтожением карты
    public static bool playerTern = true; // переменная, отвечающая за текущий ход игрока

    public static BattleState battleState = new(); // состояние хода(кто сейчас ходит)

    public static List<Character> playerPeople = new List<Character>(); // Список персонажей игрока
    public static List<Character> enemiesPeople = new List<Character>(); // список персонажей врага

    private void Start()
    {
        Character[] _enemies = FindObjectsOfType<Character>();
        Character[] _players = FindObjectsOfType<Character>();
        foreach (Character obj in _enemies)
        {
            if (obj.tag == "Enemy")
                enemiesPeople.Add(obj);
        }

        foreach (Character obj in _players)
        {
            if (obj.tag == "Player")
                playerPeople.Add(obj);
        }

        battleState = BattleState.PLAYERTURN;
        characterTern = playerPeople[0];
    }
    private void Update()
    {
        if (battleState == BattleState.PLAYERTURN && changeCharacterTern == true)
        {
            characterTern = playerPeople[0];                
            playerPeople.RemoveAt(0);                
            playerPeople.Add(characterTern);          
            changeCharacterTern = false;

        }

        if (battleState == BattleState.ENEMYTURN && changeCharacterTern == true )
        {                     
            characterTern = enemiesPeople[0];              
            enemiesPeople.RemoveAt(0);                
            enemiesPeople.Add(characterTern);


            
            changeCharacterTern = false;          
        }

    }
}
public enum BattleState { START, PLAYERTURN, ENEMYTURN, WIN, LOST };


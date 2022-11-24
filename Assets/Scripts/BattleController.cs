using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    public static bool isAttack = false; // ����������, ������� �������, ��� ����� ��� �������� �����
    public static bool changeCharacterTern = false; // ������ ��� ��������� � ������
    public static int cardDamage; // ����� ������� ������
    public static Character currentCharacter; // ����� ����� ��� �����

    [HideInInspector] public static Character characterTern; // ������� �������� � �������

    public static bool hasClicked = false; // ����������, ��������� �� ����� ����� ����� ������������ �����
    public static bool playerTern = true; // ����������, ���������� �� ������� ��� ������

    public static BattleState battleState = new(); // ��������� ����(��� ������ �����)

    public static List<Character> playerPeople = new List<Character>(); // ������ ���������� ������
    public static List<Character> enemiesPeople = new List<Character>(); // ������ ���������� �����

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


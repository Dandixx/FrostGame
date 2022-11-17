using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField] private Sprite _newSprite; // спрайт после клика мыши
    [SerializeField] private Sprite _oldSprite; // изначальный спрайт
    private Vector2 _startPosition; 
    private Vector2 _endPosition; // позиция после наведения курсора
    private Vector2 _afterClickPosition = new Vector2 (0f,0f);
    private float _movingDistance = 0.05f; // Дистанция перемещения при наведении на мышь
    private float _objectScale = 1.05f; // Размер карты при наведении на мышь
    private float _currentRotate = -15f;

    [SerializeField] int damage;

    



    private void Start()
    {
        _startPosition = gameObject.transform.position;
        _endPosition = new Vector2(transform.position.x + _movingDistance ,transform.position.y + _movingDistance);
        gameObject.transform.Rotate(0f, 0f, _currentRotate);
        
    }


    private void OnMouseEnter() // При наведении на карту.
    {
        if (gameObject.GetComponent<SpriteRenderer>().sprite != _newSprite)
        {
            transform.position = Vector2.Lerp(_endPosition, _startPosition, _movingDistance);
            transform.localScale = new Vector2(_objectScale, _objectScale);
        }
        
        
    }
    private void OnMouseExit() // При выходе из наведения карты
    {
        if (gameObject.GetComponent<SpriteRenderer>().sprite != _newSprite)
        {
            transform.position = Vector2.Lerp(_startPosition, _endPosition, _movingDistance);
            transform.localScale = new Vector2(1f, 1f);
        }
    }

    private void OnMouseDown()
    {
        if (_objectScale != 1.75f)
        {
            _objectScale = 1.75f;
            transform.position = Vector2.Lerp(_afterClickPosition, _startPosition, _movingDistance);
            gameObject.transform.Rotate(0f, 0f, -_currentRotate);
            transform.localScale = new Vector2(_objectScale, _objectScale);
            gameObject.GetComponent<SpriteRenderer>().sprite = _newSprite;
            Time.timeScale = 0;
        }
        else
        {
            Destroy(gameObject);
            GameController.isAttack = true;
            GameController.cardDamage = damage;
            
        }
        
    }



}

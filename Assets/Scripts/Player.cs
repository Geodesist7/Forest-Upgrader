using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float MovementSpeed = 1;
    public Animator AnimatorController;
    void Update()
    {
        Move();
    }
    private void Move() // метод перемещения персонажа
    {
        //if (attackingTimer > 0) return; // убираем возможность перемещения при атаке

        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        var targetLookPosition = new Vector3(horizontal, 0, vertical); // вектор направления движения

        if (targetLookPosition != Vector3.zero) // если мы куда-то хотим двигаться
        {
            transform.rotation = Quaternion.LookRotation(targetLookPosition); // поворачиваем персонажа на targetLookPosition
            transform.position += MovementSpeed * Time.deltaTime * targetLookPosition.normalized; // перемещаем персонажа в сторону нормализованного targetLookPosition
            AnimatorController.SetBool("isWalk", true); // анимируем (требует оптимизации? bool)
        }
        else
            AnimatorController.SetBool("isWalk", false); // выключаем анимацию (требует оптимизации? bool)
    }

}

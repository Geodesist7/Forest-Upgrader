using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private GameObject[] hearts; //Массив объектов (возможно, изображений) представляющих здоровье игрока.
    [SerializeField] private TextMeshProUGUI treeCountText; //Объект TextMeshProUGUI для отображения количества собранных деревьев.

    public void SetHealth(int health)
    //Принимает значение health, представляющее текущее количество здоровья игрока.
    //Проверяет, не превышает ли health количество элементов в массиве hearts.
    //Использует цикл для включения или выключения объектов в массиве hearts в зависимости от текущего здоровья.
    //Если health > i, то hearts[i] включается(отображается), иначе - выключается.
    {
        if (health > hearts.Length) return;

        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].SetActive(health > i); //SetActive вкл и выкл объект (в нашем случае хп)
        }
        HighScoreManager.instance.UpdateHighScore(_treeCount);
    }
    public int TreeCount
    //Позволяет устанавливать и получать значение _treeCount.
    //При установке значения _treeCount, обновляет текстовое поле для отображения количества собранных деревьев.
    {
        get => _treeCount;
        set
        {
            //Устанавливает значение _treeCount и обновляет текстовое поле treeCountText для отображения количества собранных деревьев.
            _treeCount = value;
            treeCountText.SetText(_treeCount.ToString());

            HighScoreManager.instance.UpdateHighScore(_treeCount);
        }
    }
    public int _treeCount; //Приватная переменная для хранения количества собранных деревьев.
    
}

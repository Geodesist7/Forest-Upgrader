using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private GameObject[] hearts;
    [SerializeField] private TextMeshProUGUI treeCountText;

    public void SetHealth(int health) 
    {
        if (health > hearts.Length) return;

        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].SetActive(health > i); //SetActive вкл и выкл объект (в нашем случае хп)
        }
    }
    public int TreeCount
    {
        get => _treeCount;
        set
        {
            _treeCount = value;
            treeCountText.SetText(_treeCount.ToString());
        }
    }
    public int _treeCount;
}

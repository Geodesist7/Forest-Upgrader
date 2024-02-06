using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private GameObject[] hearts; //������ �������� (��������, �����������) �������������� �������� ������.
    [SerializeField] private TextMeshProUGUI treeCountText; //������ TextMeshProUGUI ��� ����������� ���������� ��������� ��������.

    public void SetHealth(int health)
    //��������� �������� health, �������������� ������� ���������� �������� ������.
    //���������, �� ��������� �� health ���������� ��������� � ������� hearts.
    //���������� ���� ��� ��������� ��� ���������� �������� � ������� hearts � ����������� �� �������� ��������.
    //���� health > i, �� hearts[i] ����������(������������), ����� - �����������.
    {
        if (health > hearts.Length) return;

        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].SetActive(health > i); //SetActive ��� � ���� ������ (� ����� ������ ��)
        }
        HighScoreManager.instance.UpdateHighScore(_treeCount);
    }
    public int TreeCount
    //��������� ������������� � �������� �������� _treeCount.
    //��� ��������� �������� _treeCount, ��������� ��������� ���� ��� ����������� ���������� ��������� ��������.
    {
        get => _treeCount;
        set
        {
            //������������� �������� _treeCount � ��������� ��������� ���� treeCountText ��� ����������� ���������� ��������� ��������.
            _treeCount = value;
            treeCountText.SetText(_treeCount.ToString());

            HighScoreManager.instance.UpdateHighScore(_treeCount);
        }
    }
    public int _treeCount; //��������� ���������� ��� �������� ���������� ��������� ��������.
    
}

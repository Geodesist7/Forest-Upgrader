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
    private void Move() // ����� ����������� ���������
    {
        //if (attackingTimer > 0) return; // ������� ����������� ����������� ��� �����

        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        var targetLookPosition = new Vector3(horizontal, 0, vertical); // ������ ����������� ��������

        if (targetLookPosition != Vector3.zero) // ���� �� ����-�� ����� ���������
        {
            transform.rotation = Quaternion.LookRotation(targetLookPosition); // ������������ ��������� �� targetLookPosition
            transform.position += MovementSpeed * Time.deltaTime * targetLookPosition.normalized; // ���������� ��������� � ������� ���������������� targetLookPosition
            AnimatorController.SetBool("isWalk", true); // ��������� (������� �����������? bool)
        }
        else
            AnimatorController.SetBool("isWalk", false); // ��������� �������� (������� �����������? bool)
    }

}

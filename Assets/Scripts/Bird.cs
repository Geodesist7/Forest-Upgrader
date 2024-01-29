using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bird : MonoBehaviour
{
    [SerializeField] private float moveRangeXZ = 10f; //���������� ������������ ���������� �� �����������, �� ������� ����� ���������� ������� ����� ��� ����������� �����.
    [SerializeField] private float fixedHeight = 10f; //������������� ������, �� ������� ������ �����.
    [SerializeField] private float moveSpeed = 5f; //�������� ����������� ����� � ������� �����.
    // [SerializeField] private float rotationSpeed = 180f; //�������� �������� ����� � �������� � ������� 
    [SerializeField] private float duration = 5f; //����������������� �������� ����������� � ������� �����.

    private void Start()
    {
        MoveBird();
    }

    private void MoveBird()
    //����� ���������� ��������� ������� ������� � �������� ��������� ��������� � ������������� ������.
    //������������ ������������ �������� �� ������ ���������� ����� ������� � ������� ��������� � �������� ��������.
    //���������� DOTween ��� �������� �������� ����������� � �������� �����.
    //�� ���������� �������� �������� ��� ���� ��� ������� ����� ��������.
    {
        Vector3 targetPosition = new Vector3(
            Random.Range(-moveRangeXZ, moveRangeXZ),
            fixedHeight,
            Random.Range(-moveRangeXZ, moveRangeXZ)
        );

        float distance = Vector3.Distance(transform.position, targetPosition);
        float calculatedDuration = distance / moveSpeed;

        transform.DOMove(targetPosition, calculatedDuration)
            .SetEase(Ease.Linear)
            .OnComplete(MoveBird);

        // ������������ ���� ��������
        float angle = Vector3.SignedAngle(Vector3.forward, targetPosition - transform.position, Vector3.up);

        // ���������� DOTween ��� �������� �������� �������� ���� ����� � ������ ����������� ��������
        transform.DORotate(new Vector3(0f, angle, 0f), calculatedDuration, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear);

    }
}

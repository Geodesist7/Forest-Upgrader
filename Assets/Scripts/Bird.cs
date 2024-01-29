using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bird : MonoBehaviour
{
    [SerializeField] private float moveRangeXZ = 10f; //ќпредел€ет максимальное рассто€ние по горизонтали, на котором может находитьс€ целева€ точка дл€ перемещени€ птицы.
    [SerializeField] private float fixedHeight = 10f; //‘иксированна€ высота, на которой летают птицы.
    [SerializeField] private float moveSpeed = 5f; //—корость перемещени€ птицы к целевой точке.
    // [SerializeField] private float rotationSpeed = 180f; //—корость поворота птицы в градусах в секунду 
    [SerializeField] private float duration = 5f; //ѕродолжительность анимации перемещени€ к целевой точке.

    private void Start()
    {
        MoveBird();
    }

    private void MoveBird()
    //ћетод генерирует случайную целевую позицию в пределах заданного диапазона и фиксированной высоты.
    //–ассчитывает длительность анимации на основе рассто€ни€ между текущей и целевой позици€ми и скорости движени€.
    //»спользует DOTween дл€ создани€ анимации перемещени€ и поворота птицы.
    //ѕо завершении анимации вызывает сам себ€ дл€ запуска новой анимации.
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

        // –ассчитываем угол поворота
        float angle = Vector3.SignedAngle(Vector3.forward, targetPosition - transform.position, Vector3.up);

        // »спользуем DOTween дл€ создани€ анимации поворота тела птицы и задаем направление движени€
        transform.DORotate(new Vector3(0f, angle, 0f), calculatedDuration, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear);

    }
}

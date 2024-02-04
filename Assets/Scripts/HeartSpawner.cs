using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HeartSpawner : MonoBehaviour
{
    [SerializeField] private GameObject heartPrefab;
    [SerializeField] private float spawnIntervalMin = 5f;
    [SerializeField] private float spawnIntervalMax = 15f;
    [SerializeField] private int healAmount = 1;

    private void Start()
    {
        StartCoroutine(SpawnHearts());
    }

    private IEnumerator SpawnHearts()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(spawnIntervalMin, spawnIntervalMax));

            Vector3 spawnPosition = GetRandomSpawnPosition();
            GameObject heart = Instantiate(heartPrefab, spawnPosition, Quaternion.identity);
            // Set the HeartSpawner as the parent of the spawned heart
            heart.transform.parent = transform;

            // Используем DOTween для более эффектного появления
            heart.transform.localScale = Vector3.zero;
            heart.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);

            // Ждем немного перед началом вращения
            yield return new WaitForSeconds(0.5f);

            // Вызываем метод для вращения сердца
            RotateHearts(heart.transform);
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float spawnX = Random.Range(-30f, 30f);
        float spawnZ = Random.Range(-30f, 30f);
        return new Vector3(spawnX, 0.5f, spawnZ); // Чтобы сердце появлялось над поверхностью
    }

    private void RotateHearts(Transform heartTransform)
    {
        // Вращение сердца после спавна
        heartTransform.DORotate(new Vector3(0f, 360f, 0f), 1f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Incremental);
    }
    public int GetHealAmount()
    {
        return healAmount;
    }
   
}

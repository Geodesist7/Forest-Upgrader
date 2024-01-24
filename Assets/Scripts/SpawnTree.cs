using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTree : MonoBehaviour
{
    private float radius = 50f;
    [SerializeField] private int count = 20;
    [SerializeField] private GameObject treePrefab;
    private void Start()
    {
        for (int i = 0; i < count; i++)
        {
            if (Physics.Raycast(new Vector3(Random.Range(-radius, radius), 15, Random.Range(-radius, radius)), Vector3.down, out var hitInfo, 20f))
            {
                Instantiate(treePrefab, hitInfo.point + new Vector3(0, 2, 0), Quaternion.identity);
            }
        }
    }
}




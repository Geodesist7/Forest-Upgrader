using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;

public class Tree : MonoBehaviour
{
    public bool isRespawned = true;
    private Animator _anim;
    private NavMeshObstacle navMesh;
    private CapsuleCollider _collider;

    private void Start()
    {
        isRespawned = true;
        _anim = GetComponent<Animator>();
        navMesh = GetComponent<NavMeshObstacle>();
        _collider = GetComponent<CapsuleCollider>();
    }
    public void Destroy()
    {
        if (!isRespawned) return;
        //_anim.SetTrigger("onDestroy");
        //isRespawned = false;
        transform.DOKill();
        transform.DOScale(0, 0.5f);
        isRespawned = false;
        Invoke(nameof(Respawn),2f);
        navMesh.enabled = false;
        _collider.enabled = false;

    }
    private void Respawn() 
    {
        navMesh.enabled = true;
        _collider.enabled = true;
        transform.DOScale(1f, 2f).SetEase(Ease.OutElastic).onComplete = () => isRespawned = true;
    }
    
    
    

    
}

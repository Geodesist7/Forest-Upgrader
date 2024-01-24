using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    public bool isRespawned;
    private Animator _anim;

    private void Start()
    {
        isRespawned = true;
        _anim = GetComponent<Animator>();
    }
    public void Destroy()
    {
        _anim.SetTrigger("onDestroy");
        isRespawned = false;
    }
    public void OnAnimEnd() 
    {
        _anim.SetTrigger("startRespawn");
    }
    public void OnRespawnEnd() 
    {
        isRespawned = true;
    }
}

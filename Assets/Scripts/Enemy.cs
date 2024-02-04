using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform hitPoint;
    [SerializeField] private float radius;
    [SerializeField] private float reloadTime;
    [SerializeField] private int damage;
    [SerializeField] private AudioClip hitSound; // Ќова€ переменна€ дл€ аудиоклипа
    


    private NavMeshAgent _agent;
    private Transform _target;
    private Animator _anim;
    private bool _canHit;
    private bool _isReloaded = true;
    private bool _isDead;
    public float Hp;
    private AudioSource audioSource;

    private void Start()
    {
        _anim = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _target = FindObjectOfType<Player>().transform;

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = hitSound;
        audioSource.playOnAwake = false; // ќтключаем воспроизведение при активации объекта
    }
    private void Update()
    {
        if (_isDead) 
        {
            return;
        }
        _agent.SetDestination(_target.position); //задаем нашему агенту позицию до которой нужно дойти 
        _anim.SetBool("isRun", _agent.velocity.magnitude > 0.1f);
        bool isDistance = Vector3.Distance(transform.position, _target.position) < _agent.stoppingDistance;//если дистанци€ между врагом и играком будет меньше
        _canHit = isDistance;

        if (_canHit && _isReloaded)
        {
            Collider[] colliders = Physics.OverlapSphere(hitPoint.position,radius);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].TryGetComponent(out Player player)) 
                {
                    _anim.SetTrigger("hit");
                    player.GetDamage(damage);
                    PlayHitSound();
                }
            }
            StartCoroutine(Reload());
        }
    }
    private void PlayHitSound()
    {
        // ѕровер€ем, что есть аудио и не проигрываетс€
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
    public void Die()
    {
        GetComponent<Collider>().enabled = false;
        _isDead = true;
        _anim.SetTrigger("Die");
        Destroy(gameObject, 3.2f);
    }
    IEnumerator Reload() 
    {
        _isReloaded = false;
        yield return new WaitForSeconds(reloadTime);
        _isReloaded = true;
    }
}

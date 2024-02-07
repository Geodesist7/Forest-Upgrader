using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed; //���������� �������� �������� ������.
    [SerializeField] private float rotationSpeed; //���������� �������� �������� ������.
    [SerializeField] private float reloadTime; //����� ����������� ����� �������.
    [SerializeField] private int startHealth = 5; //��������� ���������� �������� ������.
    [SerializeField] private PlayerUI ui; //������ �� ��������� ���������� ������ (��������, UI, ������� ���������� �������� � ������ ����������).
    [SerializeField] private Transform hitPoint; //�����, ������ ���������� �����.
    [SerializeField] private float hitRadius; //������ ����� ������.
    [SerializeField] private AudioClip hitSound; // ����� ���������� ��� ����������
    [SerializeField] private AudioClip runSound;
    [SerializeField] private AudioClip pickupSound;// ���������� ��� ����� ������� ������


    private Rigidbody _rb; //������ �� ��������� Rigidbody ������.
    private Animator _anim; //������ �� ��������� Animator ������.
    private bool _canHit = true; //����, �����������, ����� �� ����� ��������� ����� � ������ ������.
    private int _health; //������� ���������� �������� ������.
    private AudioSource audioSource;
    private AudioSource runAudioSource;
    private AudioSource pickupAudioSource; // ��������� AudioSource ��� ��������������� �����

    private Vector3 startTouchPos; // ���������� ���� ��� �������� ������� ������ �������

    private void Start()
    // ����� ���������� ��� ������� �����.
    //�������������� ����������� ���������� � ����������.

    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();

        _health = startHealth;

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = hitSound;
        audioSource.playOnAwake = false; // ��������� ��������������� ��� ��������� �������

        runAudioSource = gameObject.AddComponent<AudioSource>();
        runAudioSource.clip = runSound;
        runAudioSource.playOnAwake = false;

        pickupAudioSource = gameObject.AddComponent<AudioSource>();
        pickupAudioSource.clip = pickupSound;
        pickupAudioSource.playOnAwake = false;
    }

    private void FixedUpdate()
    //���������� �� ������ ����� � ���������� ��������.
    //�������� ����� MovePlayer, ���������� �� �������� ������.
    {
        MovePlayer();
    }
    //private void MovePlayer()
    //// ������������ ���� �� ������ ��� �������� � ��������.
    ////���������� Quaternion.Lerp ��� �������� �������� ������ � ����������� ��������.
    ////��������� �������� �������� � ����������� �� �����.
    ////������������ ����� ��� ������� ������ ����, ���� _canHit ����� true.
    //{
    //    Vector3 moveInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
    //    bool isMoving = moveInput.magnitude > 0.1f;

    //    if (isMoving)
    //    {
    //        Quaternion rotation = Quaternion.LookRotation(moveInput);
    //        rotation.x = 0;
    //        rotation.z = 0;
    //        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

    //        _rb.velocity = moveInput * speed;
    //        _anim.SetBool("isWalk", true);

    //        // �������������� ���� ����, ���� ��� ��������������� �� ������
    //        if (!runAudioSource.isPlaying)
    //        {
    //            runAudioSource.Play();
    //        }
    //    }
    //    else
    //    {
    //        _rb.velocity = Vector3.zero;
    //        _anim.SetBool("isWalk", false);

    //        // ���������� ��������������� ����� ����, ���� �� ���������������
    //        if (runAudioSource.isPlaying)
    //        {
    //            runAudioSource.Stop();
    //        }
    //    }


    //    if (Input.GetMouseButton(0) && _canHit == true)
    //    {
    //        _anim.SetTrigger("attack");
    //        StartCoroutine(Reload());
    //        PlayHitSound();
    //    }
    //}

    private void MovePlayer()
    {
        // ���� ���� ������� �� ������
        if (Input.GetMouseButtonDown(0))
        {
            startTouchPos = Input.mousePosition; // ��������� ������� ������ �������
        }
        // ���� ������������ �������
        else if (Input.GetMouseButton(0))
        {
            Vector3 touchDelta = Input.mousePosition - startTouchPos; // ������� ����� ��������� � ������� ��������� �������
            Vector3 moveInput = new Vector3(touchDelta.x, 0, touchDelta.y); // ������� ������ ��������

            // ����������� ������ ��������, ����� ���������� ������������ ��������
            moveInput.Normalize();

            // ������������ ��������� � ����������� ��������
            Quaternion rotation = Quaternion.LookRotation(moveInput);
            rotation.x = 0;
            rotation.z = 0;
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

            // ������� ��������� � ����������� �������
            _rb.velocity = moveInput * speed;
            _anim.SetBool("isWalk", true);

            // ������������� ���� ����, ���� ��� ��������������� �� ������
            if (!runAudioSource.isPlaying)
            {
                runAudioSource.Play();
            }
        }
        else
        {
            // ���� ��� �������, ������������� �������� � ��������
            _rb.velocity = Vector3.zero;
            _anim.SetBool("isWalk", false);

            // ������������� ��������������� ����� ����, ���� �� ���������������
            if (runAudioSource.isPlaying)
            {
                runAudioSource.Stop();
            }
        }

        // ��������� ������� �� ������ ��� �����
        if (Input.GetMouseButtonDown(0) && _canHit)
        {
            // ��������� �����
            _anim.SetTrigger("attack");
            StartCoroutine(Reload());
            PlayHitSound();
        }
    }
    private void PlayHitSound()
    {
        // ���������, ��� ���� ����� � �� �������������
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
    IEnumerator Reload() //������������ ��� ��������� � ���������� ����������� ����� � �������� �������� �����������.
    {
        _canHit = false;
        yield return new WaitForSeconds(reloadTime);
        _canHit = true;
    }

    public void GetDamage(int damage)
    //����������, ����� ����� �������� ����.
    //��������� ���������� �������� � ��������� UI.
    //���� �������� ���������� ������ ��� ����� ����, ����������� ����� � �������� 0 (����������������, ��� ����� ������).
    {
        _health -= damage;
        ui.SetHealth(_health);

        if (_health <= 0)
        {
            SceneManager.LoadScene(0);
        }
    }

    private void Hit()
    {
        // ����������, ����� ����� �������.
        //���������� Physics.OverlapSphere ��� ����������� �������� � ������� �����.
        //���� ������ -������(Tree) � ��� �����������, ����������� ������� �������� � ���������� ������.
        //���� ������ -����(Enemy), �������� ����� Die �����.

        Collider[] colliders = Physics.OverlapSphere(hitPoint.position, hitRadius);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].TryGetComponent(out Tree tree) && tree.isRespawned)
            {
                ui.TreeCount++;
                tree.Destroy();
            }
            if (colliders[i].TryGetComponent<Enemy>(out var enemy))
            {
                enemy.Die();
                //Destroy(colliders[i].gameObject);
            }
        }


    }
    public void RestoreHealth(int amount)
    {
        _health += amount;
        // ���������� �������� ������������ ���������, ���� ��� ����������
        _health = Mathf.Clamp(_health, 0, startHealth);
        ui.SetHealth(_health);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Heart"))
        {
            HeartSpawner heart = other.GetComponentInParent<HeartSpawner>();
            if (heart != null)
            {
                RestoreHealth(heart.GetHealAmount());
                Destroy(other.gameObject);

                // ������������� ���� ������� ������
                PlayPickupSound();
            }
        }
    }

    private void PlayPickupSound()
    {
        if (pickupAudioSource != null)
        {
            pickupAudioSource.Play();
        }
    }
    public IEnumerator DelayedRestoreHealth(int amount, GameObject heartObject)
    {
        yield return null; // ���� ���� ����

        RestoreHealth(amount);
        Destroy(heartObject);
    }
}

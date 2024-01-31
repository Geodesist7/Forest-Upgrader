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
    [SerializeField] private int startHealth; //��������� ���������� �������� ������.
    [SerializeField] private PlayerUI ui; //������ �� ��������� ���������� ������ (��������, UI, ������� ���������� �������� � ������ ����������).
    [SerializeField] private Transform hitPoint; //�����, ������ ���������� �����.
    [SerializeField] private float hitRadius; //������ ����� ������.


    private Rigidbody _rb; //������ �� ��������� Rigidbody ������.
    private Animator _anim; //������ �� ��������� Animator ������.
    private bool _canHit = true; //����, �����������, ����� �� ����� ��������� ����� � ������ ������.
    private int _health; //������� ���������� �������� ������.

    private void Start()
    // ����� ���������� ��� ������� �����.
    //�������������� ����������� ���������� � ����������.
     
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();

        _health = startHealth;
    }

    private void FixedUpdate()
    //���������� �� ������ ����� � ���������� ��������.
    //�������� ����� MovePlayer, ���������� �� �������� ������.
    {
        MovePlayer();
    }
    private void MovePlayer()
    // ������������ ���� �� ������ ��� �������� � ��������.
    //���������� Quaternion.Lerp ��� �������� �������� ������ � ����������� ��������.
    //��������� �������� �������� � ����������� �� �����.
    //������������ ����� ��� ������� ������ ����, ���� _canHit ����� true.
    {
        Vector3 moveInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (moveInput.magnitude > 0.1f) //���� ����� ������� ������ 0.1 �� ��������� �������
        {
            //������� ������:
            Quaternion rotation = Quaternion.LookRotation(moveInput);
            rotation.x = 0;
            rotation.z = 0;
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime); //������� ����������� 

        }
        _rb.velocity = moveInput * speed;
        _anim.SetBool("isWalk", moveInput.magnitude > 0.1f);

        if (Input.GetMouseButton(0) && _canHit == true)
        {
            _anim.SetTrigger("attack");
            StartCoroutine(Reload());
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
            }
        }
    }
    public IEnumerator DelayedRestoreHealth(int amount, GameObject heartObject)
    {
        yield return null; // ���� ���� ����

        RestoreHealth(amount);
        Destroy(heartObject);
    }
}

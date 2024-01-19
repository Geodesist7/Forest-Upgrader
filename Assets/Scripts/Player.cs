using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float reloadTime;

    private Rigidbody _rb;
    private Animator _anim;
    private bool _canHit = true;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }
    private void MovePlayer() 
    {
        Vector3 moveInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (moveInput.magnitude > 0.1f) //если длина вектора больше 0.1 то выполняем поворот
        {
            //поворот игрока:
            Quaternion rotation = Quaternion.LookRotation(moveInput);
            rotation.x = 0;
            rotation.z = 0;
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime); //плавное перемещение 

        }
        _rb.velocity = moveInput * speed;
        _anim.SetBool("isWalk", moveInput.magnitude > 0.1f);

        if (Input.GetMouseButton(0) && _canHit == true) 
        {
            _anim.SetTrigger("attack");
            StartCoroutine(Reload());
        }
    }
     IEnumerator Reload() //куратина которая работает в потоке от других методов
    {
        _canHit = false;
        yield return new WaitForSeconds(reloadTime);
        _canHit = true;
    }
}

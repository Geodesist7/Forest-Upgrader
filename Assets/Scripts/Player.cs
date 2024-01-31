using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed; //Определяет скорость движения игрока.
    [SerializeField] private float rotationSpeed; //Определяет скорость поворота игрока.
    [SerializeField] private float reloadTime; //Время перезарядки между атаками.
    [SerializeField] private int startHealth; //Начальное количество здоровья игрока.
    [SerializeField] private PlayerUI ui; //Ссылка на компонент интерфейса игрока (вероятно, UI, который отображает здоровье и другую информацию).
    [SerializeField] private Transform hitPoint; //Точка, откуда начинается атака.
    [SerializeField] private float hitRadius; //Радиус атаки игрока.


    private Rigidbody _rb; //Ссылка на компонент Rigidbody игрока.
    private Animator _anim; //Ссылка на компонент Animator игрока.
    private bool _canHit = true; //Флаг, указывающий, может ли игрок совершить атаку в данный момент.
    private int _health; //Текущее количество здоровья игрока.

    private void Start()
    // Метод Вызывается при запуске сцены.
    //Инициализирует необходимые компоненты и переменные.
     
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();

        _health = startHealth;
    }

    private void FixedUpdate()
    //Вызывается на каждом кадре с постоянной частотой.
    //Вызывает метод MovePlayer, отвечающий за движение игрока.
    {
        MovePlayer();
    }
    private void MovePlayer()
    // Обрабатывает ввод от игрока для движения и поворота.
    //Использует Quaternion.Lerp для плавного поворота игрока в направлении движения.
    //Запускает анимацию движения в зависимости от ввода.
    //Обрабатывает атаку при нажатии кнопки мыши, если _canHit равен true.
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
    IEnumerator Reload() //Используется для включения и выключения возможности атаки с заданным временем перезарядки.
    {
        _canHit = false;
        yield return new WaitForSeconds(reloadTime);
        _canHit = true;
    }

    public void GetDamage(int damage)
    //Вызывается, когда игрок получает урон.
    //Уменьшает количество здоровья и обновляет UI.
    //Если здоровье становится меньше или равно нулю, загружается сцена с индексом 0 (предположительно, это сцена смерти).
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
        // Вызывается, когда игрок атакует.
        //Использует Physics.OverlapSphere для определения объектов в радиусе атаки.
        //Если объект -дерево(Tree) и оно возродилось, увеличивает счетчик деревьев и уничтожает дерево.
        //Если объект -враг(Enemy), вызывает метод Die врага.

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
        // Ограничьте здоровье максимальным значением, если это необходимо
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
        yield return null; // Ждем один кадр

        RestoreHealth(amount);
        Destroy(heartObject);
    }
}

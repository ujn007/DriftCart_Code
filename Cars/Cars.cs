using DG.Tweening;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class Cars : MonoBehaviour
{
    [Header("CarSet")]
    [SerializeField] protected float carSpeed;
    [SerializeField] protected float maxSpeed;
    public float MaxSpeed => maxSpeed;

    [SerializeField] protected float drag;
    [SerializeField] private Vector3 centerMass;
    [SerializeField] private float maxDirLenght;
    [SerializeField] private float force;
    public CarRankSO rankSO;
    protected GameDifficultSO difficultSO;
    public int rankPoint { get; private set; }
    public bool canGoal { get; private set; } = false;
    protected bool isGameStart;

    protected float currentMaxSpeed;
    protected float currentSpeed;

    [Header("Sprite")]
    public Sprite carSprite;
    [SerializeField] private ParticleSystem spawnEffect;

    [Header("MapSet")]
    public float rebirthDis = -20;

    protected Vector3 moveForce;
    protected Vector3 targetDir;

    protected Rigidbody rigid;
    [HideInInspector] public int rapCount;

    public virtual void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        difficultSO = DifficultInform.Instance.currentDifficultSO;
    }

    public virtual void Start()
    {
        rigid.centerOfMass = centerMass;
        moveForce = Vector3.zero;
    }

    public virtual void OnEnable()
    {
        currentMaxSpeed = maxSpeed;
        currentSpeed = carSpeed;

        GameManager.Instance.OnGameStart += HandleStartGame;
    }

    public virtual void OnDestroy()
    {
        GameManager.Instance.OnGameStart -= HandleStartGame;
    }

    public virtual void Update()
    {
        Rebirth();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    public void MoveDir(Vector3 targetPos)
    {
        Vector3 dir = targetPos - transform.position;
        dir = Vector3.ClampMagnitude(dir, maxDirLenght);

        targetDir = Quaternion.Euler(0, -transform.eulerAngles.y, 0) * dir;

        Rotation(dir);
    }

    public void Movement()
    {
        moveForce += transform.forward * targetDir.z * carSpeed * Time.deltaTime;
        moveForce = Vector3.ClampMagnitude(moveForce, maxSpeed);
        rigid.velocity = new Vector3(moveForce.x, rigid.velocity.y, moveForce.z);

        moveForce *= drag;
    }

    public virtual void Rotation(Vector3 dir)
    {
        Quaternion rot = Quaternion.LookRotation(dir);
        Quaternion slerpCar = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * 5);

        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, slerpCar.eulerAngles.y, transform.eulerAngles.z);
    }

    public void Rebirth()
    {
        if (rebirthDis >= transform.position.y)
        {
            Transform trm = CurrentPoint();

            int rand = Random.Range(-3, 4);
            Vector3 forwordPos = trm.position + trm.forward * 3;
            Vector3 rightPos = trm.right * rand;

            transform.SetPositionAndRotation(forwordPos + rightPos, Quaternion.Euler(0, trm.eulerAngles.y, 0));
            moveForce = Vector3.zero;
            rigid.velocity = Vector3.zero;

            spawnEffect.Play();
        }
    }

    public void SetPoint(int point) => rankPoint += point;

    public void ChangeCarLayer(bool isEnd)
    {
        if (isEnd)
            gameObject.layer = LayerMask.NameToLayer("GhostCar");
        else
        {
            if (transform.TryGetComponent(out CarController playerCar))
            {
                playerCar.gameObject.layer = LayerMask.NameToLayer("PlayerCar");
            }
            else
                gameObject.layer = LayerMask.NameToLayer("Default");
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Vector3 normal = collision.contacts[0].normal;
            Vector3 reflectDir = Vector3.Reflect(rigid.velocity.normalized * carSpeed, normal);
            rigid.velocity = Vector3.zero;
            rigid.AddForce(reflectDir * force, ForceMode.Impulse);
        }
    }

    public void GoStraight(Vector3 dir, float plusSpeed, float time)
    {
        isGameStart = false;

        if (transform.TryGetComponent(out CarController playerCar))
            playerCar.StopBoost();

        rigid.velocity = Vector3.zero;
        transform.rotation = Quaternion.LookRotation(dir);

        maxSpeed = plusSpeed;
        carSpeed = maxSpeed;

        GameManager.Instance.StartDelayCallback(time, () =>
       {
           print(maxSpeed);
           maxSpeed = currentMaxSpeed;
           carSpeed = currentSpeed;
           isGameStart = true;
       });
    }

    public void EndCarSet()
    {
        moveForce = Vector3.zero;
        rigid.velocity = Vector3.zero;
        MoveDir(Vector3.zero);
        transform.rotation = Quaternion.Euler(0, 90, 0);
        this.enabled = false;
    }

    public void CheckMiddleCollider(bool b = true) => canGoal = b;

    public abstract void MoveToNextTrm(CheckTurnCol checkCol);
    public abstract Transform CurrentPoint();
    public abstract void HandleStartGame();
}

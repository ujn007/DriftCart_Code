using Cinemachine;
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public enum CarMode
{
    Normal,
    Auto
}

public class CarController : Cars
{
    [Header("Boost")]
    [SerializeField] private float boostWeightSpeed;
    [SerializeField] private float maxFOV;

    [Header("Renderer")]
    [SerializeField] private List<TrailRenderer> trails;
    [SerializeField] private List<ParticleSystem> dusts;
    [SerializeField] private ParticleSystem boostEffect;

    [HideInInspector] public Transform currentPoint;

    private Transform startTrm;

    private UnityEngine.UI.Image boostIm;
    private InputAction inputAction;

    private bool isCooltime = true;
    private bool isFillBoost;

    private GroundCaster groundCaster;

    [HideInInspector] public CarMode carMode = CarMode.Normal;

    private Vector3 nextPos = Vector3.zero;
    private int moveCount = 1;

    private Sequence boostTween;

    public override void Awake()
    {
        base.Awake();
        inputAction = GetComponent<InputAction>();
        groundCaster = transform.Find("GroundCaster").GetComponent<GroundCaster>();
        boostIm = GameManager.Instance.boostImg;
    }

    public override void Start()
    {
        base.Start();
        currentMaxSpeed = maxSpeed;
        currentSpeed = carSpeed;
        startTrm = GameManager.Instance.transform;
        currentPoint = null;

        AudioManager.Instance.PlaySFX("Engine", 0.3f, true);
    }

    public override void OnEnable()
    {
        base.OnEnable();
        inputAction.OnMoveClickEvent += HandleMouseClick;
        inputAction.OnBoostEvent += HandleBoostEvent;
        inputAction.OnPauseEvent += HandlePauseEvent;
    }

    private new void OnDestroy()
    {
        base.OnDestroy();
        inputAction.OnMoveClickEvent -= HandleMouseClick;
        inputAction.OnBoostEvent -= HandleBoostEvent;
        inputAction.OnPauseEvent -= HandlePauseEvent;

    }

    public override void Update()
    {
        base.Update();

        if (isGameStart && carMode == CarMode.Normal)
            WeightSpeed();

        FillBoost();
        if (carMode == CarMode.Auto && nextPos != Vector3.zero)
            MoveDir(nextPos);
    }

    private void FixedUpdate()
    {
        Movement();

        transform.rotation = Quaternion.Euler(new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0));
    }

    private void HandleMouseClick(Vector3 targetPos, bool v)
    {
        DriftSkid(targetPos);

        if (isGameStart && carMode == CarMode.Normal)
            MoveDir(targetPos);
    }

    public override void Rotation(Vector3 dir)
    {
        base.Rotation(dir);
        if (dir.magnitude < 1.5f) return;
    }

    bool isPlay;
    public override void DriftSkid(Vector3 dir)
    {
        float driftDot = Vector3.Dot(rigid.velocity.normalized, transform.forward);
        if (driftDot >= 0.9f || !groundCaster.CheckGround())
        {
            trails.ForEach(x => x.emitting = false);
            PlaySkidSound(0);
        }
        else
        {
            dusts.ForEach(x => x.Play());
            trails.ForEach(x => x.emitting = true);

            PlaySkidSound(1);
        }
    }

    private void PlaySkidSound(float valume)
    {

       // AudioManager.Instance.PlaySFX("Drift", valume, true);
    }

    private void HandleBoostEvent()
    {
        if ((isGameStart && isCooltime) && carMode == CarMode.Normal)
            Boost();
    }

    private void Boost()
    {
        BoostSound();
        BoostFOV();
        BoostShake();

        isCooltime = false;

        boostEffect.gameObject.SetActive(true);

        float currentSpeed = 8;
        boostTween = DOTween.Sequence();
        boostTween.Append(DOTween.To(() => carSpeed = maxSpeed, x => carSpeed = maxSpeed = x, currentMaxSpeed + boostWeightSpeed, 0.5f));
        boostTween.Join(boostIm.DOFillAmount(0, 1));

        boostTween.AppendCallback(() =>
        {
            boostEffect.gameObject.SetActive(false);
            DOTween.To(() => maxSpeed, x => maxSpeed = x, currentMaxSpeed, 0.5f);
            DOTween.To(() => carSpeed, x => carSpeed = x, currentSpeed, 0.5f);
        });

        boostTween.AppendCallback(() => isFillBoost = true);
    }

    private void FillBoost()
    {
        if (!isFillBoost) return;

        boostIm.fillAmount += Time.deltaTime / 5;

        if (boostIm.fillAmount >= 1)
        {
            isCooltime = true;
            isFillBoost = false;
        }
    }

    private void BoostFOV()
    {
        GameManager gm = GameManager.Instance;
        float currentFOV = gm.cam.m_Lens.FieldOfView;

        DOTween.To(() => gm.cam.m_Lens.FieldOfView,
            x => gm.cam.m_Lens.FieldOfView = x, maxFOV, 1f).OnComplete(() =>
        {
            DOTween.To(() => gm.cam.m_Lens.FieldOfView,
                x => gm.cam.m_Lens.FieldOfView = x, currentFOV, 1.5f);
        });
    }

    public void StopBoost()
    {
        boostEffect.gameObject.SetActive(false);
        boostTween.Kill();
        isFillBoost = true;
        carSpeed = currentSpeed;
        maxSpeed = currentMaxSpeed;
    }

    private void BoostShake()
    {
        GameManager.Instance.impulseSource.GenerateImpulse();
    }

    private void BoostSound()
    {
        AudioManager.Instance.PlaySFX("Boost", 0.5f);
    }

    private void HandlePauseEvent(bool toggle)
    {
        GameManager.Instance.pausePanel.ShowPanel(toggle);
    }

    private void WeightSpeed()
    {
        if (!isCooltime) return;

        float rotationX = transform.rotation.eulerAngles.x;

        if (rotationX > 180f)
            rotationX = 0;

        maxSpeed = currentMaxSpeed + rotationX / 2;
    }

    public override void HandleStartGame()
    {
        isGameStart = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TurnCol"))
        {
            currentPoint = other.transform;
        }
    }

    #region MoveAuto
    public override void MoveToNextTrm(CheckTurnCol ckCol)
    {
        if (carMode != CarMode.Auto) return;

        Transform targetTrm = ckCol.checkCols[moveCount].transform;
        currentPoint = targetTrm;
        nextPos = targetTrm.position;

        if (ckCol.checkCols.Count - 1 <= moveCount)
        {
            moveCount = 0;
            return;
        }

        moveCount++;
    }

    public void AutoSetting()
    {
        maxSpeed = 30;
        carSpeed = 20;
        drag = 0.97f;
    }
    #endregion

    public void SetSpeed()
    {
        carSpeed = difficultSO.playerSpeed;
        maxSpeed = difficultSO.playerMaxSpeed;
    }

    public override Transform CurrentPoint()
    {
        if (currentPoint == null) return startTrm;
        return currentPoint;
    }
}

using UnityEngine;

public class EnemyCar : Cars
{
    private Vector3 moveDir = Vector3.zero;
    private int moveCount = 1;

    private Transform spawnTrm;
    private Transform startTrm;
    private Vector3 nextPos;

    public override void Awake()
    {
        base.Awake();
        maxSpeed = Random.Range(difficultSO.enemyMinSpeed, difficultSO.enemyMaxSpeed);
    }

    public override void Start()
    {
        base.Start();

        startTrm = transform;
    }

    public override void Update()
    {
        base.Update();
        Rotation90();

        if (moveDir != Vector3.zero)
            MoveDir(moveDir);

        if (!isGameStart) return;

        UpdateDir();
    }

    public override void MoveToNextTrm(CheckTurnCol ckCol)
    {
        Transform targetTrm = ckCol.checkCols[moveCount].transform;

        if (moveCount - 1 < 0)
            spawnTrm = ckCol.checkCols[ckCol.checkCols.Count - 1].transform;
        else
            spawnTrm = ckCol.checkCols[moveCount - 1].transform;

        float sizeX = ckCol.checkCols[moveCount].size.x;
        int posRange = Mathf.RoundToInt(sizeX / 5);

        float randX = Random.Range(-posRange, posRange + 1);

        Vector3 newPosition = targetTrm.position + targetTrm.right * randX;

        nextPos = newPosition;

        if (ckCol.checkCols.Count - 1 <= moveCount)
        {
            moveCount = 0;
            return;
        }

        moveCount++;
    }

    private void UpdateDir()
    {
        ChangeDir(nextPos);
    }

    public void ChangeDir(Vector3 dir)
    {
        moveDir = dir;
    }

    private void Rotation90()
    {
        Vector3 rot = transform.eulerAngles;

        if (rot.x >= 85 && rot.x <= 95)
        {
            rot.x = 0;
            transform.eulerAngles = rot;
        }

        if (rot.z >= 85 && rot.z <= 95)
        {
            rot.z = 0;
            transform.eulerAngles = rot;
        }
    }

    public override void HandleStartGame()
    {
        isGameStart = true;
        nextPos = transform.forward * 100;
    }

    public override Transform CurrentPoint()
    {
        if (spawnTrm == null) return startTrm;
        return spawnTrm;
    }
}

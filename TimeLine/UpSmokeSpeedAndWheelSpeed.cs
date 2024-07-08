using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class UpSmokeSpeedAndWheelSpeed : MonoBehaviour
{
    [SerializeField] private ParticleSystem smokeParticle;

    [SerializeField] private List<Transform> wheel;
    [SerializeField] private float wheelMaxSpeed;
    private float wheelSpeed = 0;
    private bool canSpin = false;

    public void UpSpeedAndWheel()
    {
        smokeParticle.Play();

        var main = smokeParticle.main;

        DOTween.To(() => main.simulationSpeed, x => main.simulationSpeed = x, 15, 0.8f);
        DOTween.To(() => main.startLifetime.constant, x => main.startLifetime = x, 5, 0.8f).From(0);

        canSpin = true;
    }

    public void StopSpeed()
    {
        canSpin = false;
        DOTween.To(() => wheelSpeed, x => wheelSpeed = x, 0.5f, 2);
    }

    private void Update()
    {
        if (canSpin)
            wheelSpeed += 0.1f;

        if (wheelSpeed > wheelMaxSpeed)
            wheelSpeed = wheelMaxSpeed;

        foreach (Transform w in wheel)
        {
            w.transform.Rotate(wheelSpeed, 0, 0);
        }
    }
}

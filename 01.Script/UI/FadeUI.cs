using DG.Tweening;
using System;
using UnityEngine;

public class FadeUI : MonoSingleton<FadeUI>
{
    private RectTransform RectTrm;

    private void Awake()
    {
        RectTrm = transform as RectTransform;
    }

    public void Fadeout(float time, Action CallBack)
    {
        print("dd");
        RectTrm.anchoredPosition = new Vector2(2400, 0);

        Sequence sq = DOTween.Sequence();
        sq.AppendInterval(0.5f);
        sq.Append(RectTrm.DOAnchorPosX(0, time/2).SetEase(Ease.Linear));
        sq.AppendCallback(() => CallBack?.Invoke());
        sq.Append(RectTrm.DOAnchorPosX(-2400, time / 2).SetEase(Ease.Linear));

    }
}

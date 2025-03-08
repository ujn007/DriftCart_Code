using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RapTween : MonoBehaviour
{
    private RectTransform rctTrm;

    private void Awake()
    {
        rctTrm = GetComponent<RectTransform>();
    }

    public void MoveTweenUI()
    {
        Sequence sq = DOTween.Sequence();
        sq.Append(rctTrm.DOAnchorPosX(0, 0.8f).SetEase(Ease.OutExpo));
        sq.Append(rctTrm.DOAnchorPosX(1200, 0.5f).SetEase(Ease.InQuint)).OnComplete(() =>
        {
            rctTrm.DOAnchorPosX(-1200, 0f);
        });
    }
}

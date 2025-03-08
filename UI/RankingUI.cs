using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RankingUI : MonoBehaviour
{
    private List<RankingBanner> bannerList = new List<RankingBanner>();

    [SerializeField] private RankingBanner rankBanner;
    [SerializeField] private float moveTime;
    [SerializeField] private float spacing;

    public void SetRanking(int rank, int plusPoint, int point, Cars car)
    {
        RankingBanner rankBnr = transform.GetChild(rank - 1).GetComponent<RankingBanner>();
        rankBnr.SetRankBanner(rank, plusPoint, point, car);
        bannerList.Add(rankBnr);
    }

    public async void ShowRanking()
    {
        OrderByCriteria(x => x.rank, false, 0);

        foreach (RankingBanner banner in bannerList)
        {
            banner.rctTrm.DOAnchorPosX(0, moveTime);
            AudioManager.Instance.PlaySFX("Wind", 0.8f, false);
            await Task.Delay(100);
        }

        StartCoroutine(OrderSequentially());
    }

    private IEnumerator OrderSequentially()
    {
        yield return new WaitForSeconds(2);
        IncreasePoint();
        IncreaseSound();
        yield return new WaitForSeconds(2);
        OrderByCriteria(x => x.point, true, 0.5f);
        yield return new WaitForSeconds(2);
        OrderRank();
        yield return new WaitForSeconds(1.5f);
        GameManager.Instance.nextPanel.ShowUI(true);
    }

    private void OrderByCriteria(Func<RankingBanner, float> keySelector, bool descending, float time)
    {
        bannerList = descending ? bannerList.OrderByDescending(keySelector).ToList()
                                : bannerList.OrderBy(keySelector).ToList();

        for (int i = 0; i < bannerList.Count; i++)
        {
            bannerList[i].rctTrm.DOAnchorPos(new Vector2(bannerList[i].rctTrm.anchoredPosition.x, i * spacing), time);
        }
    }

    private async void OrderRank()
    {
        int rank = 1;
        for (int i = 0; i < bannerList.Count ; i++)
        {
            bannerList[i].rankText.text = rank.ToString();
            await Task.Delay(100);

            if (bannerList[i].CurrentCar.rankSO.point == bannerList[i + 1].CurrentCar.rankSO.point) continue;   

            rank++;
        }
    }

    private void IncreasePoint()
    {
        foreach (RankingBanner banner in bannerList)
        {
            banner.UpDownPoint(0.7f);
        }
    }

    private async void IncreaseSound()
    {
        for (int i = 0; i < 6; i++)
        {
            AudioManager.Instance.PlaySFX("Point", 1f, false);
            await Task.Delay(116);
        }
    }

    public void ResetRankingUI()
    {
        foreach (RectTransform rct in transform)
        {
            rct.anchoredPosition = new Vector3(1000, 0, 0);
        }
        bannerList.Clear();
    }
}

using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RankingBanner : MonoBehaviour
{
    public TextMeshProUGUI rankText;
    [SerializeField] private TextMeshProUGUI plusPointText;
    [SerializeField] private TextMeshProUGUI pointText;
    [SerializeField] private Image rankImage;

    public int point { get; private set; }
    public int plusPoint { get; private set; }
    public int rank { get; private set; }
    public Cars currentCar { get; private set; }
    public Cars CurrentCar => currentCar;

    public RectTransform rctTrm { get; private set; }

    private void Awake()
    {
        rctTrm = transform as RectTransform;
    }

    public void SetRankBanner(int rankText, int plusPointText, int pointText, Cars car)
    {
        this.rankText.text = rankText.ToString();
        this.plusPointText.text = $"+{plusPointText}";
        this.pointText.text = pointText.ToString();
        this.rankImage.sprite = car.carSprite;

        point = pointText;
        plusPoint = plusPointText;
        rank = rankText;
        currentCar = car;
    }

    public void UpDownPoint(float duration)
    {
        DOTween.To(() => point, x => point = x, point + plusPoint, duration)
            .OnUpdate(() => pointText.text = Mathf.FloorToInt(point).ToString())
            .OnComplete(() => currentCar.rankSO.point = point);

        DOTween.To(() => plusPoint, x => plusPoint = x, 0, duration)
           .OnUpdate(() => plusPointText.text = $"+{Mathf.FloorToInt(plusPoint)}");

    }
}

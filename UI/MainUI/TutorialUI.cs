using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private RectTransform content;
    [SerializeField] private Button leftBtn, rightBtn;
    private bool canClick;

    private void Awake()
    {
        leftBtn.onClick.AddListener(() => MoveUIPosition(0));
        rightBtn.onClick.AddListener(() => MoveUIPosition(-1078));
    }

    private void MoveUIPosition(float posX)
    {
        if (canClick) return;
        canClick = true;

        content.DOAnchorPosX(posX, 0.5f).SetEase(Ease.Linear).OnComplete(() => canClick = false);    
    }
}

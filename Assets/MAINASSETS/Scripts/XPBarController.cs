using System;
using Core.Events;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class XPBarController : MonoBehaviour
{
    [SerializeField] private Image barImage;
    [SerializeField] private TMP_Text progressText;

    private int currentXPAmount = 0;
    private int maxXPAmountToCollect = 10;
    private void Start()
    {
        progressText.text = currentXPAmount + "/" + maxXPAmountToCollect;
        EventBus.Subscribe<XPCollectEvent>(OnXpCollected);
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe<XPCollectEvent>(OnXpCollected);
    }

    private void OnXpCollected(XPCollectEvent xpCollectEvent)
    {
        currentXPAmount += xpCollectEvent.XP;
        float targetProgress = (float)currentXPAmount / maxXPAmountToCollect;

        DOTween.Kill(barImage, true);
        progressText.text = currentXPAmount + "/" + maxXPAmountToCollect;
        barImage.DOFillAmount(targetProgress, 0.1f).OnComplete(() =>
        {
            if (currentXPAmount >= maxXPAmountToCollect)
            {
                Debug.Log("Need to Show Booster Screen");
                currentXPAmount = 0;
                barImage.fillAmount = 0.0f;
                progressText.text = currentXPAmount + "/" + maxXPAmountToCollect;


                EventBus.Publish(new EnableBoosterPanelEvent());
            }
        });

    }
}

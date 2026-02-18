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
    private int level = 1;
    private int maxXPAmountToCollect;

    private void Start()
    {
        // inizializza livello e XP richiesti
        level = 1;
        maxXPAmountToCollect = CalculateXpForLevel(level);

        // mostra il livello attuale con prefisso "LV."
        progressText.text = "LV. " + level.ToString();

        EventBus.Subscribe<XPCollectEvent>(OnXpCollected);
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe<XPCollectEvent>(OnXpCollected);
    }

    private void OnXpCollected(XPCollectEvent xpCollectEvent)
    {
        // accumula XP
        currentXPAmount += xpCollectEvent.XP;

        // aggiorna barra in base alla proporzione corrente
        float targetProgress = (float)currentXPAmount / maxXPAmountToCollect;
        DOTween.Kill(barImage, true);

        // Aggiorna visivamente la barra (la text mostra il livello, non il contatore)
        barImage.DOFillAmount(Mathf.Clamp01(targetProgress), 0.1f);

        // Gestisce uno o più level-up nel caso di overfill
        while (currentXPAmount >= maxXPAmountToCollect)
        {
            // spendi XP per salire di livello
            currentXPAmount -= maxXPAmountToCollect;
            level++;

            // ri-calcola XP necessari per il nuovo livello
            maxXPAmountToCollect = CalculateXpForLevel(level);

            // Aggiorna testo livello con prefisso
            progressText.text = "LV. " + level.ToString();

            // mostra il pannello booster per ogni upgrade
            EventBus.Publish(new EnableBoosterPanelEvent());

            // reset visivo della barra per il nuovo livello (animiamo verso il valore residuo)
            DOTween.Kill(barImage, true);
            float nextProgress = (float)currentXPAmount / maxXPAmountToCollect;
            barImage.fillAmount = 0f;
            barImage.DOFillAmount(Mathf.Clamp01(nextProgress), 0.1f);
        }

        // se non si è saliti di livello, assicura il testo (rimane livello corrente)
        if (currentXPAmount < maxXPAmountToCollect)
        {
            progressText.text = "LV. " + level.ToString();
        }
    }

    private int CalculateXpForLevel(int lvl)
    {
        // formula: Mathf.RoundToInt(5 * Mathf.Pow(1.35f, level));
        return Mathf.Max(1, Mathf.RoundToInt(5f * Mathf.Pow(1.35f, lvl)));
    }
}

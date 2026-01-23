using System;
using Core.Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoosterCard : MonoBehaviour
{
    [SerializeField] private TMP_Text boosterNameText;
    [SerializeField] private Image boosterImage;
    [SerializeField] private TMP_Text boosterDescriptionText;

    private BoosterData selectedBoosterData;
    private void Awake()
    {
        var button = GetComponent<Button>();
        button.onClick.AddListener(PickBooster);
    }

    public void SetCardInfo(BoosterData boosterData)
    {
        selectedBoosterData = boosterData;
        boosterNameText.text = boosterData.Name;
        boosterImage.sprite = boosterData.Icon;
        boosterDescriptionText.text = boosterData.Description;
    }

    private void PickBooster()
    {
        //Booster Collect Event
        EventBus.Publish(new BoosterCollectedEvent(selectedBoosterData));
    }
    

}

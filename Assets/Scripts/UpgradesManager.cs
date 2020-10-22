using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradesManager : MonoBehaviour
{
    [Header("Static constants")]
    public AudioSource audioSource;
    public GameManager thisGameManager;
    public AllMinersController thisMinersController;
    [Header("Time Upgrade")]
    public ClockController clock;
    public float perUpgradeDecreaseRotateTimeByPercentage;
    public AudioClip speedUpClip;
    public Text TimeUpgradeButtonText;
    public string timeUpgradeString;
    public float currentUpgradeCost;
    public AnimationCurve upgradeIncreaseCurve;
    public float upgradeStepLengthOnCurve;
    public int upgradeCount = 0;
    public float currentFloatMultiple;
    [Header("Add miner Upgrade")]
    public AudioClip addMinerSound;
    public Text addMinerButtonText;
    public string addMinerString;
    public float currentAddMinerCost;
    public AnimationCurve addMinerIncreaseCurve;
    public float addMinerStepLengthOnCurve;
    public float currentAddMinerFloatMultiple;
    [Header("upgrade miner Upgrade")]
    public int increaseMinedEachUpgradeBy;
    public AudioClip upgradeMinerSound;
    public Text upgradeMinerButtonText;
    public string upgradeMinerString;
    public float currentUpgradeMinerCost;
    public AnimationCurve upgradeMinerIncreaseCurve;
    public float upgradeMinerStepLengthOnCurve;
    public float currentUpgradeMinerFloatMultiple;
    public int upgradeMinerCount;

    private void Start()
    {
        TimeUpgradeButtonText.text = timeUpgradeString + currentUpgradeCost.ToString();
    }

    public void UpgradeTime()
    {
        //if reached lowest time clock
        if (clock.timePerRotation <= clock.lowestClockTime) return;

        //check if can afford
        if(thisGameManager.ChangeScore((int)-currentUpgradeCost))
        {
            audioSource.clip = speedUpClip;
            audioSource.Play();

            //increase stuff
            upgradeCount++;

            currentFloatMultiple = 1f + upgradeIncreaseCurve.Evaluate(upgradeCount * upgradeStepLengthOnCurve);

            currentUpgradeCost = currentUpgradeCost * currentFloatMultiple;

            //update stuff
            TimeUpgradeButtonText.text = timeUpgradeString + ((int)currentUpgradeCost).ToString();

            clock.ChangeTimerPerRotation(-perUpgradeDecreaseRotateTimeByPercentage * clock.timePerRotation);
        }
    }

    public void AddMiner()
    {
        //if max miners, return
        if (thisMinersController.miners.Count >= thisMinersController.minerCount.x * thisMinersController.minerCount.y) return;

        //check if can afford
        if (thisGameManager.ChangeScore((int)-currentAddMinerCost))
        {
            audioSource.clip = addMinerSound;
            audioSource.Play();

            //increase stuff

            currentAddMinerFloatMultiple = 1f + addMinerIncreaseCurve.Evaluate(thisMinersController.miners.Count * upgradeMinerStepLengthOnCurve);

            currentAddMinerCost = currentAddMinerCost * currentAddMinerFloatMultiple;

            //update stuff
            addMinerButtonText.text = addMinerString + ((int)currentAddMinerCost).ToString();

            thisMinersController.AddMiner();
        }
    }

    public void UpgradeMiner()
    {
        //can upgrade forever

        //check if can afford
        if (thisGameManager.ChangeScore((int)-currentUpgradeMinerCost))
        {
            audioSource.clip = upgradeMinerSound;
            audioSource.Play();

            //increase stuff
            upgradeMinerCount++;

            currentUpgradeMinerFloatMultiple = 1f + upgradeMinerIncreaseCurve.Evaluate(upgradeMinerCount * upgradeMinerStepLengthOnCurve);

            currentUpgradeMinerCost = currentUpgradeMinerCost * currentUpgradeMinerFloatMultiple;

            //update stuff
            upgradeMinerButtonText.text = upgradeMinerString + ((int)currentUpgradeMinerCost).ToString();

            thisGameManager.IncreaseGemsPerHit(increaseMinedEachUpgradeBy);
        }
    }
}

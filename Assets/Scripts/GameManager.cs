using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    public AllMinersController thisAllMinersController;
    [Header("Score")]
    public Text scoreObject;
    public string scoreText;
    [Header("Gems")]
    public GameObject gemContainer;
    public GameObject[] gems;
    public int[] gemValuesInOrder;
    public int gemsValueOnSpawn = 1;
    public Text gemsValueEachHitText;
    public string gemsValueEachHitString;
    [Header("Sound")]
    public AudioSource gemAudioSource;
    public float timeAcceptedAsCloseGemArrival = 0.1f;
    public float gemPitchLowest;
    public float gemPitchHighest;
    public float gemPitchStep;
    [Header("Chest")]
    public float percentageToAddOnHit;
    public float chestSizeScaleToAddWhenAtMax;
    public float chestSizeMaxAtScore;
    public RectTransform chestTransform;

    //privates
    private int score = 0;
    private float gemPitch;
    private float timeSinceLastGem = 0f;
    private int gemCount;
    //chest
    private float chestOriginalScale;
    private float chestPercentageFull;

    private void Start()
    {
        scoreObject.text = scoreText + score.ToString();

        chestOriginalScale = chestTransform.localScale.x;

        timeSinceLastGem = 10000f;

        gemCount = gems.Length;

        //possible generate gem value list on each start but cba
    }

    private void Update()
    {
        //update chest size percentage before, may be changed if chest is hit
        chestPercentageFull = score / chestSizeMaxAtScore;

        timeSinceLastGem += Time.deltaTime;
        //depending on this time, decrease or increase pitch
        if (timeSinceLastGem > timeAcceptedAsCloseGemArrival) gemPitch -= gemPitchStep;
        else gemPitch += gemPitchStep;
        //set limits
        if (gemPitch < gemPitchLowest) gemPitch = gemPitchLowest;
        else if (gemPitch > gemPitchHighest) gemPitch = gemPitchHighest;

        //update chest size if yes
        chestPercentageFull = score / chestSizeMaxAtScore;
        if (chestPercentageFull > 1f) chestPercentageFull = 1f;
        //CHECK IF HAS BEEN HIT THIS FRAME, IF YES, INCREASE SCALE, IF NOT, DECREASE
        if (timeSinceLastGem < 2f * Time.deltaTime) chestPercentageFull += percentageToAddOnHit;
        float newScale = (chestPercentageFull * chestSizeScaleToAddWhenAtMax) + chestOriginalScale;
        chestTransform.localScale = new Vector3(newScale, newScale);
    }

    public void AddGem(int change)
    {
        //reset value time since last
        timeSinceLastGem = 0f;

        //set score stuff
        ChangeScore(change);

        scoreObject.text = scoreText + score.ToString();

        //play audio
        //set and play
        gemAudioSource.pitch = gemPitch;
        gemAudioSource.Play();
    }

    public bool ChangeScore(int change)
    {
        //immediately return false if too little score for change, else do change
        if (score + change < 0) return false;

        score += change;
        scoreObject.text = scoreText + score.ToString();
        return true;
    }

    public void SpawnGem(int GemCountToSpawn)
    {
        int tempGemCountToSpawn = GemCountToSpawn;
        for (int k = 0; k < thisAllMinersController.allMinersPositions.Count; k++)
        {
            for (int i = 0; i < gemCount; i++)
            {
                //go backwards through list of gem values, if lets say one, will not do any loops on any of values above when, when reaches one, do one loop
                while (tempGemCountToSpawn >= gemValuesInOrder[gemCount - i - 1])
                {
                    Instantiate(gems[gemCount - i - 1], thisAllMinersController.allMinersPositions[k], Quaternion.identity, gemContainer.transform);
                    tempGemCountToSpawn -= gemValuesInOrder[gemCount - i - 1];
                }
            }
            tempGemCountToSpawn = GemCountToSpawn;
        }
    }

    public void OnClockTick()
    {
        //do miner stuff
        thisAllMinersController.AllMinersMine();

        //miners should determine this value
        SpawnGem(gemsValueOnSpawn);
    }

    public void IncreaseGemsPerHit(int change)
    {
        gemsValueOnSpawn += change;

        gemsValueEachHitText.text = gemsValueEachHitString + gemsValueOnSpawn;
    }
}

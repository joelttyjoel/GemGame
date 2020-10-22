using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllMinersController : MonoBehaviour
{
    public GameObject minerPrefab;
    public List<GameObject> miners;
    public Vector2 sizeMinerCell;
    public Vector2 minerCount;
    public Vector3 offsetMineSpawn;

    [System.NonSerialized]
    public List<Vector3> allMinersPositions = new List<Vector3>();
    private List<Animator> minersAnimations = new List<Animator>();

    // Start is called before the first frame update
    void Start()
    {
        UpdateAllMinerInfo();
    }

    private void UpdateAllMinerInfo()
    {
        minersAnimations.Clear();
        allMinersPositions.Clear();
        foreach (GameObject a in miners)
        {
            minersAnimations.Add(a.GetComponent<Animator>());
            allMinersPositions.Add(a.transform.position + offsetMineSpawn);
        }
    }

    //private void OnDrawGizmos()
    //{
    //    //draw four sides of miner cells
    //    //Gizmos.DrawLine(transform.position, transform.position + new Vector3(sizeMinerCell.x * minerCount.x, 0f, 0f));
    //    //Gizmos.DrawLine(transform.position, transform.position - new Vector3(0f, sizeMinerCell.y * minerCount.y, 0f));
    //    //Gizmos.DrawLine(transform.position + new Vector3(sizeMinerCell.x * minerCount.x, 0f, 0f), transform.position + new Vector3(sizeMinerCell.x * minerCount.x, -sizeMinerCell.y * minerCount.y, 0f));
    //    //Gizmos.DrawLine(transform.position - new Vector3(0f, sizeMinerCell.y * minerCount.y, 0f), transform.position + new Vector3(sizeMinerCell.x * minerCount.x, -sizeMinerCell.y * minerCount.y, 0f));
    //}

    public void AllMinersMine()
    {
        foreach (Animator a in minersAnimations)
        {
            a.SetTrigger("Strike");
        }
    }

    public void AddMiner()
    {
        //if (miners.Count >= minerCount.x * minerCount.y) return;

        int row = miners.Count / (int)minerCount.x;
        int collumn = miners.Count % (int)minerCount.x;
        Debug.Log("row" + row);
        Debug.Log("col" + collumn);
        GameObject newMiner = Instantiate(minerPrefab, this.gameObject.transform.position + new Vector3(collumn * sizeMinerCell.x, row * sizeMinerCell.y, 0), Quaternion.identity, this.transform);
        miners.Add(newMiner);
        UpdateAllMinerInfo();
    }
}

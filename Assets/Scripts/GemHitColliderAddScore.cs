using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemHitColliderAddScore : MonoBehaviour
{
    public GameManager thisGameManager;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        thisGameManager.AddGem(collision.gameObject.GetComponent<GemController>().gemValue);

        Destroy(collision.gameObject);
    }
}

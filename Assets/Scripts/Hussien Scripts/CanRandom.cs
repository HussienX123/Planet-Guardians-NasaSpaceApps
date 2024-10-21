using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanRandom : MonoBehaviour
{
    public GameObject[] cans;
    // Start is called before the first frame update
    void Start()
    {
        int random = Random.Range(0, cans.Length);
        cans[random].SetActive(true);

    }


}

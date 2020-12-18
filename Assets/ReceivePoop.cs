using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceivePoop : MonoBehaviour
{
    public TextMesh score;
    int scorevalue = 0;

    void Start() {
        score.text = scorevalue.ToString();
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<PoopScript>())
        {
            Debug.Log("POOP TOUCHING");
            Destroy(collision.gameObject);
            scorevalue++;
            score.text = scorevalue.ToString();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallTubesAnimation : MonoBehaviour
{
    public float maxY;
    public int positionInQueue;
    public bool goodToGo = false;
    public bool isDone = false;

    /// <summary>
    /// Funkcia starajúca sa o napĺňanie malých trubiek počas simulácie
    /// </summary>
    public void FillTheTube()
    {
        if (transform.localScale.y >= maxY)
        {
            return;
        }

        Vector3 vec = new Vector3(transform.localScale.x, transform.localScale.y + 2f, transform.localScale.z);
        transform.localScale = vec;

        if (transform.localScale.y >= maxY)
        {
            isDone = true;
            goodToGo = false;
        }
    }
}

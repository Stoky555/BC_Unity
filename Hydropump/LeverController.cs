using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeverController : MonoBehaviour
{
    private float _value;
    public float value {
        get
        {
            return _value;
        } 
        set
        {
            _value = value;
            if(_value > 100)
            {
                _value = 0;
            }
            handler(_value);
        } 
    }

    // Start is called before the first frame update
    void Start()
    {
        _value = 0;
        handler(_value);
    }

    /// <summary>
    /// Zapínanie levitujúceho textu nad ventilom
    /// </summary>
    /// <param name="value">Hodnota ventilu</param>
    public void handler(float value)
    {
        transform.parent.Find("Text (TMP)").gameObject.SetActive(true);
        transform.parent.Find("Text (TMP)").GetComponent<TextMeshPro>().text = value.ToString() + "%";
        StartCoroutine(RemoveAfterSeconds(5, transform.parent.Find("Text (TMP)").gameObject));
    }

    /// <summary>
    /// Zabezpečuje interakciu s ventilom, hodnota je uložena a nastavuje sa v triede, volanie spraví zvyšok
    /// </summary>
    public void LeverInteraction() {
        var angles = transform.localEulerAngles;
        value += 25f;

        if (transform.name == "V13" || transform.name == "V23")
        {
            if(_value > 100)
            {
                angles.Set(transform.localEulerAngles.x, transform.localEulerAngles.y, 90f);
                transform.localEulerAngles = angles;
                _value = 0;
                return;
            }
            angles.Set(transform.localEulerAngles.x, transform.localEulerAngles.y, 90f - (((float)(90f / 100f)) * _value));
            transform.localEulerAngles = angles;
        }
        else if (transform.name == "V1" || transform.name == "V2" || transform.name == "V3")
        {
            if (_value > 100)
            {
                angles.Set(transform.localEulerAngles.x, transform.localEulerAngles.y, 0f);
                transform.localEulerAngles = angles;
                _value = 0;
                return;
            }
            angles.Set(transform.localEulerAngles.x, transform.localEulerAngles.y, ((float)(90f / 100f)) * _value);
            transform.localEulerAngles = angles;
        }
    }

    /// <summary>
    /// Nastavenie ventilu podľa štandardných hodnôt regulátora
    /// </summary>
    /// <param name="v">Hodnota ventilu</param>
    public void LeverSet(float v)
    {
        var angles = transform.localEulerAngles;
        _value = v;

        if (_value == 0)
        {
            return;
        }
        if(transform.name == "V13" || transform.name == "V23")
        {
            angles.Set(transform.localEulerAngles.x, transform.localEulerAngles.y, 90f - ((float)(90f / 100f)) * _value);
            transform.localEulerAngles = angles;
        }
        else if(transform.name == "V1" || transform.name == "V2" || transform.name == "V3")
        {
            angles.Set(transform.localEulerAngles.x, transform.localEulerAngles.y, 0f + ((float)(90f / 100f)) * _value);
            transform.localEulerAngles = angles;
        }
    }

    /// <summary>
    /// Skrytie hodnoty levitujúceho textu nad ventilom
    /// </summary>
    /// <param name="seconds">Koľko sekúnd má byť zobrazený</param>
    /// <param name="obj">O ktorý objekt textu sa jedná</param>
    /// <returns></returns>
    IEnumerator RemoveAfterSeconds(int seconds, GameObject obj)
    {
        yield return new WaitForSeconds(seconds);
        obj.SetActive(false);
    }

}

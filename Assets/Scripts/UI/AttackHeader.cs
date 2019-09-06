using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AttackHeader : MonoBehaviour
{
    public TextMeshProUGUI text;

    public void UpdateText(string attackName)
    {
        text.text = attackName;
    }

}

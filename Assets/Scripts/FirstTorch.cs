﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstTorch : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<AudioManager>().Play("Flame");
    }
}

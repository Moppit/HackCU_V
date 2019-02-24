using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrintInstructions : MonoBehaviour
{
    public Text _instructions;
    int count = 0;
    // Start is called before the first frame update
    void Start()
    {
        _instructions.text = "Hello, trusty firewall!!!\nWe have entrusted you with the security of our website.\nBlock malicious traffic (red cubes) from getting to our network.\nBe careful with the green cubes though! We still want those to get through!\nPress the Space Bar to begin.";
    }

    // Update is called once per frame
    void Update()
    {
        if (count == 1) {
            _instructions.text = " ";
        }
        else if (Input.GetKeyDown("space")){
            count++;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffObjectTagger : MonoBehaviour
{
    [SerializeField] private BuffObject buffObjectType;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public BuffObject GetBuffObject() {
        return buffObjectType;
    }
}

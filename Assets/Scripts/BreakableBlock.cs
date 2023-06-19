using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableBlock : MonoBehaviour
{
    public void Break()
    { 
        // Destroy the block object.
        Destroy(this.gameObject);
    }
}

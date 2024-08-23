using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscoBall : MonoBehaviour, IHitable
{
    [Header(" Data ")]
    private DiscoBallManager _discoBallManager;
    private Flash _flash;



    private void Awake()
    {
        _flash = GetComponent<Flash>();
        _discoBallManager = FindFirstObjectByType<DiscoBallManager>();
    }


    public void TakeHit()
    {
        _discoBallManager.DiscoBallParty();

        _flash.StartFlash();
    }
}

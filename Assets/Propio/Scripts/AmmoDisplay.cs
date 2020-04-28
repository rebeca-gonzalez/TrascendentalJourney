using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoDisplay : MonoBehaviour
{
    private Text ammoText;
    private void ammoHasChanged()
    {
        ammoText.text = "Ammo: " + Player.getInstance().weapon.ammo + "/" + Player.getInstance().weapon.maxAmmo;
        //Debug.Log(Player.getInstance().weapon.ammo);
    }
    // Start is called before the first frame update
    void Start()
    {
        ammoText = GetComponent<Text>();
        Player.getInstance().GetComponent<Weapon>().ammoChange += ammoHasChanged;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

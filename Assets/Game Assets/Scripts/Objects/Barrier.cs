using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : InteractableObject {

    #region Private Members
    private BoxCollider2D _boxCollider;
    #endregion

    //Interaction: Get Focus
    public override void OnGetFocus() {
        Debug.Log(this.name + ":" + this.GetInstanceID().ToString() + " Now has focus");
        base.OnGetFocus();
    }

    //Interaction: Lose Focus
    public override void OnLoseFocus() {
        Debug.Log(this.name + ":" + this.GetInstanceID().ToString() + " No longer has focus");
        base.OnLoseFocus();
    }

    // Use this for initialization
    void Start () {
        _boxCollider = GetComponent<BoxCollider2D>();
	}
	
	// Update is called once per frame
	void Update () {
	    if (_hasFocus && Input.GetKeyDown(KeyCode.Space) && Vektor.getHandler().getEnergy() >= Vektor.getHandler().getEnergyRate()) {
            _boxCollider.isTrigger = !_boxCollider.isTrigger;
            Vektor.getHandler().setEnergy(Vektor.getHandler().getEnergy() - Vektor.getHandler().getEnergyRate());
        }
	}
}

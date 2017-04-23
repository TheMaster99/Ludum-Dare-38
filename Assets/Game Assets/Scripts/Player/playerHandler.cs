using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerHandler : MonoBehaviour {

    #region Unity Inspector
    [Header("Energy Config")]
    [SerializeField]
    private float _maxEnergy;
    [SerializeField]
    private bool _energyRecharge;
    [SerializeField]
    private float _energyRate;
    #endregion

    #region Private Members
    private GameObject _activeObject;
    private float _energyLevel;
    private RaycastHit2D _hit;
    #endregion

    // Use this for initialization
    void Start () {
        _maxEnergy = 100;
        _energyLevel = _maxEnergy;
        _energyRecharge = false; // Energy does not recharge by default
        _energyRate = 5; // The rate that energy is used/recharged
        Vektor.initHandler(this);
	}

    public void setEnergy(float _energy) {
        _energyLevel = _energy;
    }

    public float getEnergy() {
        return _energyLevel;
    }

    public float getEnergyRate() {
        return _energyRate;
    }
	
	// Update is called once per frame
	void Update () {

        print("Energy: " + _energyLevel);
        _energyLevel = Mathf.Clamp(_energyLevel, 0, 100);

        if (Input.GetMouseButtonDown(0)) {
            _hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            
            if (_hit.collider != null) {
                if (_hit.transform.gameObject.tag == "Interactable") {
                    if (_activeObject != null)
                        _activeObject.BroadcastMessage("OnLoseFocus");
                    if (_hit.transform.parent != null) // If the player is clicking on a platform, select the platform's parent (the physical platform isn't the logical platform)
                        _activeObject = _hit.transform.parent.gameObject;
                    else
                        _activeObject = _hit.transform.gameObject;
                    _activeObject.BroadcastMessage("OnGetFocus");
                    print(_activeObject.transform.name);
                }
            }
        }

	}
}

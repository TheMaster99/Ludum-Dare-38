using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerHandler : MonoBehaviour {

    #region Unity Inspector
    [Header("Energy Config")]
    [SerializeField]
    private int _maxEnergy;
    [SerializeField]
    private bool _energyRecharge;
    [SerializeField]
    private int _energyRate;
    #endregion

    #region Private Members
    private GameObject _activeObject;
    private int _energyLevel;
    private RaycastHit2D _hit;
    #endregion

    // Use this for initialization
    void Start () {
        _maxEnergy = 100;
        _energyLevel = _maxEnergy;
        _energyRecharge = false; // Energy does not recharge by default
        _energyRate = 5; // The rate that energy is used/recharged
	}

    public void setEnergy(int _energy) {
        _energyLevel = _energy;
    }

    public int getEnergy() {
        return _energyLevel;
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(0)) {
            _hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            
            if (_hit.collider != null) {
                if (_hit.transform.gameObject.tag == "Interactable") {
                    if (_activeObject != null)
                        _activeObject.BroadcastMessage("OnLoseFocus");
                    _activeObject = _hit.transform.parent.gameObject; // If the player is clicking on a platform, select the platform's parent (the physical platform isn't the logical platform)
                    _activeObject.BroadcastMessage("OnGetFocus");
                    print(_activeObject.transform.name);
                }
            }
        }

	}
}

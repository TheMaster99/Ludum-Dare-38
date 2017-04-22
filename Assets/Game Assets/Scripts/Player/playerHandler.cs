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
    private bool _buttonHold;
    #endregion

    // Use this for initialization
    void Start () {
        _maxEnergy = 100;
        _energyLevel = _maxEnergy;
        _energyRecharge = false; // Energy does not recharge by default
        _energyRate = 5; // The rate that energy is used/recharged
        _buttonHold = false; // Assume object has toggle interaction instead of hold interaction
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(0)) {
            _hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            
            if (_hit.collider != null) {
                if (_hit.transform.gameObject.tag == "Platform") {
                    _activeObject = _hit.transform.parent.gameObject; // If the player is clicking on a platform, select the platform's parent (the physical platform isn't the logical platform)
                }else if (_hit.transform.gameObject.tag == "Barrier") { // for later

                }
                print(_activeObject.transform.name);
            }
        }
		
        if (_activeObject != null) { // An object is selected

            if (_activeObject.tag == "Platform") {
                // figure out what kind of platform it is, then do stuff
            }else if (_activeObject.tag == "Barrier") {
                if (Input.GetKeyDown(KeyCode.Space)) {
                    // Make the door open
                    _energyLevel -= 5; // It costs 5 energy to open a barrier
                }
            }
            
        }

	}
}

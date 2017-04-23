using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract class for interactable objects
/// Important Methods:
/// OnGetFocus, OnLoseFocus
/// - VektorKnight
/// </summary>
public abstract class InteractableObject : MonoBehaviour {

    #region Private Members
    protected bool _hasFocus;
    #endregion

    //Initialization
    void Start () {
        _hasFocus = false;
	}
	
    /// <summary>
    /// Called to give focus to this object
    /// </summary>
    public virtual void OnGetFocus() {
        if (!_hasFocus) {
            _hasFocus = true;
        }
    }

    /// <summary>
    /// Called to take focus away from this object
    /// </summary>
    public virtual void OnLoseFocus() {
        if (_hasFocus) {
            _hasFocus = false;
        }
    }
}

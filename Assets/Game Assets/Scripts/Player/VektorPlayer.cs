using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class VektorPlayer : MonoBehaviour {

    #region Unity Inspector
    [Header("Movement Config")]
    [SerializeField]
    private float _moveSpeed;
    [SerializeField]
    private float _stopDistance;
    [SerializeField]
    private List<Transform> _checkPoints = new List<Transform>();
    #endregion

    #region Private Members
    private Rigidbody2D _rigidBody;
    private Vector3 _appliedMovement;
    private RaycastHit2D _rayHit;
    private bool _groundLeft, _groundRight;
    #endregion


    //Initialization
    void Start () {
        _rigidBody = this.GetComponent<Rigidbody2D>();
	}
	
	//Physics Update
	void FixedUpdate () {
		
	}
}

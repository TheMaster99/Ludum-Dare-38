using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls a platform which moves between a set of definied path nodes.
/// -VektorKnight
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class MovingPlatform : MonoBehaviour {

    #region Unity Inspector
    [Header("Path Config")]
    [SerializeField]
    private List<Vector3> _pathNodes = new List<Vector3>();

    [Header("Movement Config")]
    [SerializeField]
    private float _moveSpeed = 5.0f;
    [SerializeField]
    private float _pauseDelay = 1.5f;
    [SerializeField]
    private bool _pauseAtNodes = true;
    #endregion

    #region Private Members
    private Rigidbody2D _rigidBody;
    private Vector3 _initPosition;
    private Vector3 _velocity;
    private Vector3 _target;
    private int _currentNode;
    private bool _reverse;
    #endregion

    //Initialization
    void Start () {
        //Sanity Check
        if (_pathNodes.Count == 0) {
            Debug.LogError("Platform: " + this.name + " has no path nodes defined!");
            return;
        }

        //Initialize Members
        _rigidBody = this.GetComponent<Rigidbody2D>();
        _initPosition = transform.position;
        _currentNode = 0;

        //Enforce Kinematic State
        _rigidBody.isKinematic = true;

        //Start Coroutine
        StartCoroutine("PlatformLoop");
	}

    //Coroutine
	private IEnumerator PlatformLoop () {
        if (_pauseAtNodes) { yield return new WaitForSeconds(_pauseDelay); }
        while (true) {
            //Check if we're at the target node then choose the next node
            if (transform.position == (_initPosition + _pathNodes[_currentNode])) {
                if (_reverse) {
                    if (_currentNode - 1 < 0) {
                        _reverse = false;
                        _currentNode++;
                    }
                    else {
                        _currentNode--;
                    }
                }
                else {
                    if (_currentNode + 1 > _pathNodes.Count - 1) {
                        _reverse = true;
                        _currentNode--;
                    }
                    else {
                        _currentNode++;
                    }
                }
                //Pause at the node if needed
                if (_pauseAtNodes) { yield return new WaitForSeconds(_pauseDelay); }
            }

            //Move towards the current node
            _target = Vector3.MoveTowards(transform.position, _initPosition + _pathNodes[_currentNode], _moveSpeed * Time.deltaTime);
            _rigidBody.MovePosition(_target);

            yield return null;
        }
    }

	//Per-Frame Update
	void Update () {
		
	}
}

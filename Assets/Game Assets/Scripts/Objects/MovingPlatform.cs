﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls a platform which moves between a set of definied path nodes.
/// -VektorKnight
/// </summary>
public class MovingPlatform : MonoBehaviour {

    #region Unity Inspector
    [Header("Platform Config")]
    [SerializeField]
    private GameObject _platformObject;

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
    private GameObject _platformInstance;
    private Rigidbody2D _platformBody;
    private Vector3 _initPosition;
    private Vector3 _velocity;
    private Vector3 _target;
    private int _currentNode;
    private bool _reverse;
    #endregion

    //Debugging Gizmos
    void OnDrawGizmos() {
        //Sanity Check
        if (_pathNodes.Count == 0) {
            Debug.LogError("Platform: " + this.name + " has no path nodes defined!");
            return;
        }

        //Draw the Path
        Gizmos.DrawIcon(transform.TransformPoint(_pathNodes[0]), "StartGizmo"); //Start

        for (int i = 1; i < _pathNodes.Count - 1; i++) {
            Gizmos.DrawIcon(transform.TransformPoint(_pathNodes[i]), "PathGizmo"); //PathNodes
        }

        Gizmos.DrawIcon(transform.TransformPoint(_pathNodes[_pathNodes.Count - 1]), "EndGizmo"); //Start

        if (_pathNodes.Count > 0) {
            for (int i = 1; i < _pathNodes.Count; i++) {
                Gizmos.DrawLine(transform.TransformPoint(_pathNodes[i - 1]), transform.TransformPoint(_pathNodes[i]));
            }
            Gizmos.DrawLine(transform.TransformPoint(_pathNodes[_pathNodes.Count - 1]), transform.TransformPoint(_pathNodes[_pathNodes.Count -1]));
        }
        else {
            Gizmos.DrawLine(transform.TransformPoint(_pathNodes[0]), transform.TransformPoint(_pathNodes[_pathNodes.Count - 1]));
        }
    }

    //Initialization
    void Start () {
        //Sanity Checking
        if (_platformObject == null) {
            Debug.LogError("No platform prefab defined, stupid");
            return;
        }
        if (_pathNodes.Count == 0) {
            Debug.LogError("Platform: " + this.name + " has no path nodes defined!");
            return;
        }

        //Initialize Members
        _platformInstance = Instantiate(_platformObject, transform.TransformPoint(_pathNodes[0]), Quaternion.identity);
        _platformBody = _platformInstance.GetComponent<Rigidbody2D>();
        _currentNode = 0;

        //Enforce Kinematic State
        _platformBody.isKinematic = true;

        //Start Coroutine
        StartCoroutine("PlatformLoop");
	}

    //Coroutine
	private IEnumerator PlatformLoop () {
        if (_pauseAtNodes) { yield return new WaitForSeconds(_pauseDelay); }
        while (true) {
            //Check if we're at the target node then choose the next node
            if (_platformInstance.transform.position == (transform.TransformPoint(_pathNodes[_currentNode]))) {
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
            _target = Vector3.MoveTowards(_platformInstance.transform.position, transform.TransformPoint(_pathNodes[_currentNode]), _moveSpeed * Time.deltaTime);
            _platformBody.MovePosition(_target);

            yield return null;
        }
    }

	//Per-Frame Update
	void Update () {
		
	}
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls a platform which moves between a set of definied path nodes.
/// -VektorKnight
/// </summary>
public class MovingPlatform : InteractableObject {
    
    #region Unity Inspector
    [Header("Platform Config")]
    [SerializeField]
    private GameObject _platformObject;

    [Header("Path Config")]
    [SerializeField]
    private List<Vector3> _pathNodes = new List<Vector3>();

    [Header("Movement Config")]
    [SerializeField]
    private BehaviorMode _behavior;
    [SerializeField]
    private PathMode _pathMode;
    [SerializeField]
    private float _moveSpeed = 5.0f;
    [SerializeField]
    private float _pauseDelay = 1.5f;
    [SerializeField]
    private bool _pauseAtNodes = true;
    #endregion

    #region Private Members
    private Rigidbody2D _platformBody;
    private Vector3 _initPosition;
    private Vector3 _velocity;
    private Vector3 _target;
    private int _currentNode;
    private bool _reverse;
    private bool _doPathLoop;
    private bool _canMove;
    #endregion

    #region Properties
    public BehaviorMode PlatformMode {
        get { return _behavior; }
    }
    #endregion

    #region Debugging Gizmos
    void OnDrawGizmos() {
        //Sanity Check
        if (_pathNodes.Count == 0) {
            Debug.LogError("Platform: " + this.name + " has no path nodes defined!");
            return;
        }

        //Draw the Path
        Gizmos.DrawIcon(transform.position, "StartGizmo"); //Start

        for (int i = 0; i < _pathNodes.Count - 1; i++) {
            Gizmos.DrawIcon(transform.TransformPoint(_pathNodes[i]), "PathGizmo"); //PathNodes
        }

        Gizmos.DrawIcon(transform.TransformPoint(_pathNodes[_pathNodes.Count - 1]), "EndGizmo"); //Start

        if (_pathNodes.Count > 0) {
            Gizmos.DrawLine(transform.position, transform.TransformPoint(_pathNodes[0]));
            for (int i = 1; i < _pathNodes.Count; i++) {
                Gizmos.DrawLine(transform.TransformPoint(_pathNodes[i - 1]), transform.TransformPoint(_pathNodes[i]));
            }
            if (_pathMode == PathMode.Loop) {
                Gizmos.DrawLine(transform.TransformPoint(_pathNodes[_pathNodes.Count - 1]), transform.position);
            }
        }
        else {
            Gizmos.DrawLine(transform.TransformPoint(_pathNodes[0]), transform.TransformPoint(_pathNodes[_pathNodes.Count - 1]));
        }
    }
    #endregion

    //Initialization
    void Start () {
        //Sanity Checking
        if (_platformObject == null) {
            Debug.LogError("No Platform Defined");
            return;
        }
        if (_pathNodes.Count == 0) {
            Debug.LogError("Platform: " + this.name + " has no path nodes defined!");
            return;
        }

        //Insert the starting Node
        _pathNodes.Insert(0, transform.InverseTransformPoint(transform.position));

        //Initialize Members
        _platformBody = _platformObject.GetComponent<Rigidbody2D>();
        _currentNode = 0;

        //Set up conditions for behavior
        switch (_behavior) {
            case BehaviorMode.Autonomous:
                _doPathLoop = true;
                _canMove = true;
                break;

            case BehaviorMode.AdvanceOnEvent:
                _doPathLoop = false;
                _canMove = true;
                break;

            case BehaviorMode.AdvancedOnCondition:
                _doPathLoop = true;
                _canMove = false;
                break;
                
        }

        //Enforce Kinematic State
        _platformBody.isKinematic = true;

        //Start Coroutine
        StartCoroutine("PlatformLoop");
	}

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

    //Per-Frame Update
    void Update() {
        if (_hasFocus) {
            switch (_behavior) {
                //Advance on a key press or other input
                case BehaviorMode.AdvanceOnEvent:
                    if (Input.GetKeyDown(KeyCode.Space) && !_doPathLoop && Vektor.getHandler().getEnergy() >= Vektor.getHandler().getEnergyRate()) {
                        _doPathLoop = true;
                        Vektor.getHandler().setEnergy(Vektor.getHandler().getEnergy() - Vektor.getHandler().getEnergyRate());
                    }
                    break;

                //Advance while a key is held
                case BehaviorMode.AdvancedOnCondition:
                    if (Input.GetKey(KeyCode.Space) && Vektor.getHandler().getEnergy() > 0) {
                        _canMove = true;
                        Vektor.getHandler().setEnergy((Vektor.getHandler().getEnergy() - (Vektor.getHandler().getEnergyRate() * Time.deltaTime)));
                    }else if (_canMove) {
                        _canMove = false;
                    }
                    break;
            }
        }
    }

    //Coroutine
    private IEnumerator PlatformLoop () {
        if (_pauseAtNodes) { yield return new WaitForSeconds(_pauseDelay); }
        while (true) {
            if (_doPathLoop) {
                //Check if we're at the target node then choose the next node
                if (_platformObject.transform.position == (transform.TransformPoint(_pathNodes[_currentNode]))) {
                    switch (_pathMode) {
                        case PathMode.EndToEnd:
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
                            break;
                        case PathMode.Loop:
                            if (_currentNode + 1 > _pathNodes.Count - 1) {
                                _currentNode = 0;
                            }
                            else {
                                _currentNode++;
                            }
                            break;
                    }

                    //Stop advancing if the behavior mode is event based
                    if (_behavior == BehaviorMode.AdvanceOnEvent) {
                        _doPathLoop = false;
                        yield return null;
                    }

                    //Pause at the node if needed
                    if (_pauseAtNodes) { yield return new WaitForSeconds(_pauseDelay); }
                }

                //Move towards the current node
                if (_canMove) {
                    _target = Vector3.MoveTowards(_platformObject.transform.position, transform.TransformPoint(_pathNodes[_currentNode]), _moveSpeed * Time.deltaTime);
                    _platformBody.MovePosition(_target);
                }

                yield return null;
            }
            yield return null;
        }
    }
}

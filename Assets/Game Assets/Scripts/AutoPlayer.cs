using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* <summary>
 * Controls the "player" that paces back and forth.
 * It will avoid walking off an edge, unless there is a platform to walk onto.
 * Later, it will probably handle interactions with buttons and so on.
 * -TheMaster99
 * </summary>
 */

[RequireComponent(typeof(Rigidbody2D))]
public class AutoPlayer : MonoBehaviour {

    #region Unity Inspector
    [SerializeField]
    private float _moveSpeed = 1f;
    #endregion

    #region Private Members
    private Rigidbody2D _rigidBody;
    private BoxCollider2D _boxCollider;
    private bool _moveDirection; // true = move left, false = move right
    private RaycastHit2D _rayHit;
    private Vector2 _position;
    private Vector2 _movePos;
    private Vector2 _rayTargetLeft;
    private Vector2 _rayTargetRight;
    private float _rayDistance;
    #endregion

    //Initialization
    void Start () {
        _moveDirection = true; // move left initially

        // Define the basic stuff
        _rigidBody = this.GetComponent<Rigidbody2D>();
        _boxCollider = this.GetComponent<BoxCollider2D>();
        _position = new Vector2(transform.position.x, transform.position.y);
        _movePos = new Vector2(_position.x, _position.y);

        // Define raycasting stuff
        _rayTargetLeft = new Vector2(-_boxCollider.size.x / 2, -_boxCollider.size.y / 2);                       // Ray that should point to the bottom left corner, if I'm doing this correctly
        _rayTargetRight = new Vector2(_boxCollider.size.x / 2, -_boxCollider.size.y / 2);                       // Ray that should point to the bottom right corner, if I'm doing this correctly
        _rayDistance = Mathf.Sqrt(Mathf.Pow(_boxCollider.size.x, 2) + Mathf.Pow(_boxCollider.size.y, 2)) + .5f; // Distance from position (center of player) to corner of player
	}                                                                                                           // + a bit more, so it should be enough to find anything to walk on
    
	//Per-Frame Update
	void Update () {
        _position.x = transform.position.x;
        _position.y = transform.position.y;
        _movePos.y = _position.y;

        if (_moveDirection) { // if moving left
            _rayHit = Physics2D.Raycast(_position, _rayTargetLeft, _rayDistance);
        }else { // if moving right
            _rayHit = Physics2D.Raycast(_position, _rayTargetRight, _rayDistance);
        }

        if (_rayHit.collider == null) { // No more floor!
            _moveDirection = !_moveDirection; // Turn around.
        }

        if (_moveDirection) {
            _movePos.x = _position.x - (_moveSpeed * Time.deltaTime);
        }else {
            _movePos.x = _position.x + (_moveSpeed * Time.deltaTime);
        }

        print(_movePos);

        _rigidBody.MovePosition(_movePos);

	}
}

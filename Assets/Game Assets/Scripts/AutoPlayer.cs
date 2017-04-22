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
   // private RaycastHit2D _rayHit;
    private Vector2 _position;
    private Vector2 _move;
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
        _move = new Vector2(0,0);

        // Define raycasting stuff
        _rayTargetLeft = new Vector2(-_boxCollider.size.x / 2, -_boxCollider.size.y / 2);                       // Ray that should point to the bottom left corner, if I'm doing this correctly
        _rayTargetRight = new Vector2(_boxCollider.size.x / 2, -_boxCollider.size.y / 2);                       // Ray that should point to the bottom right corner, if I'm doing this correctly
        _rayDistance = Mathf.Sqrt(Mathf.Pow(_boxCollider.size.x, 2) + Mathf.Pow(_boxCollider.size.y, 2)) + 0.5f; // Distance from position (center of player) to corner of player
	}                                                                                                           // + a bit more, so it should be enough to find anything to walk on


    #region Gizmos
    public void OnDrawGizmos() {
        Gizmos.DrawRay(new Ray(transform.FindChild("rayLeft").position, _rayTargetLeft));
        Gizmos.DrawRay(new Ray(transform.FindChild("rayRight").position, _rayTargetRight));
    }
    #endregion


    //Per-Frame Update
    void FixedUpdate () {
        _position.x = transform.position.x;
        _position.y = transform.position.y;

        bool _rayHit;

        if (_moveDirection) { // if moving left
            _rayHit = Physics2D.Raycast(transform.FindChild("rayLeft").position, _rayTargetLeft, 0.25f);
        }else { // if moving right
            _rayHit = Physics2D.Raycast(transform.FindChild("rayRight").position, _rayTargetRight, 0.25f);
        }

        if (!_rayHit) { // No more floor!
            print("No more floor!");
            _moveDirection = !_moveDirection; // Turn around.
            _rigidBody.velocity = Vector2.zero;
        }

        if (_moveDirection) {
            _move.x = -_moveSpeed * Time.deltaTime;
        }else {
            _move.x = _moveSpeed * Time.deltaTime;
        }

        _rigidBody.AddForce(_move);

    }
}

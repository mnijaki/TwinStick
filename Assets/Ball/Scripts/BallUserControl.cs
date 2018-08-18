using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

// Namespace.
namespace UnityStandardAssets.Vehicles.Ball
{
  // This class is responsible for translating user input into motions, and then calling Ball's 'Move()' method.
  public class BallUserControl:MonoBehaviour
  {
    // ---------------------------------------------------------------------------------------------------------------\\
    //                                                  SERIALIZED FIELDS                                             \\
    // ---------------------------------------------------------------------------------------------------------------\\
    #region

    // Flag if draw debug gizmos for better understanding how 'AddTorque' works.
    // Serialized fields force Unity to serialize a private field (You can acces them via editor but the still
    // remain private. Thanks to that objt oriented encapsulation is preserved).
    [SerializeField]
    [Tooltip("Flag if draw debug gizmos for better understanding how 'AddTorque' works")]
    private bool is_debug_mode = false;

    #endregion


    // ---------------------------------------------------------------------------------------------------------------\\
    //                                                    PRIVATE FIELDS                                              \\
    // ---------------------------------------------------------------------------------------------------------------\\
    #region

    // Reference to the ball controller.
    private Ball ball;
    // Game manager.
    private GameManager game_manager;
    // World - relative desired move direction, calculated from the 'main_camera_direction' and user input.
    // The purpose of having the camera is to do motion from the frame reference of the camera, 
    // not that of the world space.
    // 
    // Movement vector.
    private Vector3 move;
    // A reference to the main camera transform.
    private Transform main_camera_trans;
    // Current forward direction of the camera.
    private Vector3 main_camera_forward_dir;
    // Flag if the jump button was pressed.
    private bool jump;
    
    #endregion


    // ---------------------------------------------------------------------------------------------------------------\\
    //                                                    PRIVATE METHODS                                             \\
    // ---------------------------------------------------------------------------------------------------------------\\
    #region

    // 'Awake()' (called before 'Start').
    private void Awake()
    {
      // Get ball.
      this.ball = GetComponent<Ball>();      
      // If there is camera tagged as 'MainCamera'.
      if(Camera.main != null)
      {
        // Get main camera transform.
        this.main_camera_trans=Camera.main.transform;
      }
      // If there is no camera tagged as 'MainCamera'.
      else
      {
        // Log warning.
        Debug.LogWarning("No main camera found. Ball needs a Camera tagged \"MainCamera\", "+
                         "for camera - relative controls.");
      }
    } // End of Awake

    // Initialization.
    private void Start()
    {
      // Get game manager.
      this.game_manager = GameObject.FindObjectOfType<GameManager>();
    } // End of Start

    // Update (called once per frame)
    private void Update()
    {
      // If replay mode then exit from function.
      if(!this.game_manager.is_recording)
      {
        return;
      }
      // Debug Horizontal and Vertical values.
      DebugAxis();
      // Debug RHorizontal and RVertical values.
      DebugRAxis();
      // Get the axis and jump input ('CrossPlatformInputManager' will provide correct input handling
      // for multiple platforms like PC, mobile, Xbox ect).
      float h = CrossPlatformInputManager.GetAxis("Horizontal");
      float v = CrossPlatformInputManager.GetAxis("Vertical");
      this.jump = CrossPlatformInputManager.GetButton("Jump");
      // If there is main camera.
      if(this.main_camera_trans!=null)
      {
        // 'Scale()' will multiply two Vectors (it is done to get rid of 'Y' axis value, the ball only moves in 'X' and 'Z').
        // Normalization is done to ensuer that this new vector will only have values like 0 or 1 (that way this Vector
        // will be only for puropses of recognizing with direction camera is facing).
        this.main_camera_forward_dir = Vector3.Scale(this.main_camera_trans.forward,new Vector3(1,0,1)).normalized;
        // Calculate movement.
        this.move = (this.main_camera_forward_dir*v + this.main_camera_trans.right*h).normalized;
      }
      // If there is no main camera.
      else
      {
        // If there is no main camera, just use world space instead of the camera's relative frame to calculate movement.
        this.move = (Vector3.forward*v + Vector3.right*h).normalized;
      }
    } // End of Update

    // 'FixedUpdate' works like 'Update', but it is called at a fixed framerate instead of on every frame. 
    // This makes it useful for physics actions, so that behaviour is repeatable (fro example, you can record
    // gameplay in code). 
    private void FixedUpdate()
    {
      // If replay mode then exit from function.
      if(!this.game_manager.is_recording)
      {
        return;
      }
      // Call the 'Move' function of the ball controller.
      this.ball.Move(this.move,this.jump);
      // Change 'jump' flag.
      this.jump = false;
    } // End of FixedUpdate

    // Draw gizmos of game object, even if game object is not selected.
    private void OnDrawGizmos()
    {
      // If not debug mode then exit from function.
      if(!this.is_debug_mode)
      {
        return;
      }
      // If no main camera then exit from function.
      if(this.main_camera_trans==null)
      {
        return;
      }
      // Draw main camera forward direction.
      Debug.DrawRay(Vector3.zero,this.main_camera_trans.forward,Color.red);
      // Draw vector that will help of getting rid of 'Y' value.
      Debug.DrawRay(Vector3.zero,new Vector3(1,0,1),Color.green);
      // Draw scaled vector (will get rid of 'Y' value).
      Debug.DrawRay(Vector3.zero,Vector3.Scale(this.main_camera_trans.forward,new Vector3(1,0,1)),Color.blue);
      // Draw scaled vector (will get rid of 'Y' value) and normalize it (make it directional, how far is not essential here).
      // If you want to see how normalization works change values of blue vector to (3,0,3) - same direction but greater value).
      Debug.DrawRay(Vector3.zero,Vector3.Scale(this.main_camera_trans.forward,new Vector3(1,0,1)).normalized,Color.cyan);
      // Draw final vector used in 'AddTorque' (it is perpendicullar to vector which was computed). 
      // To better understand 'AddTorque' see this: https://www.youtube.com/watch?v=De0PoxaKlww (remember left hand law).
      Vector3 vec = Vector3.Scale(this.main_camera_trans.forward,new Vector3(3,0,3)).normalized;
      Debug.DrawRay(Vector3.zero,new Vector3(vec.z,0,-vec.x),Color.yellow);
    } // End of OnDrawGizmos

    // Debug Horizontal and Vertical values.
    private void DebugAxis()
    {
      // If not debug mode then exit from function.
      if(!this.is_debug_mode)
      {
        return;
      }
      // Debug axis.
      Debug.Log("Horizontal="+CrossPlatformInputManager.GetAxis("Horizontal"));
      Debug.Log("Vertical="+CrossPlatformInputManager.GetAxis("Vertical"));
    } // End of DebugAxis

    // Debug RHorizontal and RVertical values.
    private void DebugRAxis()
    {
      // If not debug mode then exit from function.
      if(!this.is_debug_mode)
      {
        return;
      }
      // Debug axis.
      Debug.Log("RHorizontal="+CrossPlatformInputManager.GetAxis("RHorizontal"));
      Debug.Log("RVertical="+CrossPlatformInputManager.GetAxis("RVertical"));
    } // End of DebugRAxis

    #endregion

  } // End of BallUserControl
} // End of namespace
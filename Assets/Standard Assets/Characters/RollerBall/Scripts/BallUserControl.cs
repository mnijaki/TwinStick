using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Vehicles.Ball
{
  // This class is responsible for translating user input into motions, and then calling Ball's 'Move()' method.
  public class BallUserControl:MonoBehaviour
  {
    // ---------------------------------------------------------------------------------------------------------------\\
    //                                                    PRIVATE FIELDS                                              \\
    // ---------------------------------------------------------------------------------------------------------------\\
    #region

    // Reference to the ball controller.
    private Ball ball;
    // World - relative desired move direction, calculated from the 'main_camera_direction' and user input.
    // The purpose of having the camera is to do motion from the frame of reference of the camera, 
    // not of the world space.
    // 
    // Movement vector.
    private Vector3 move;
    // A reference to the main camera transform.
    private Transform main_camera_trans;
    // Current direction of the camera.
    private Vector3 main_camera_direction;
    // Flag if the jump button was pressed.
    private bool jump;

    #endregion


    // ---------------------------------------------------------------------------------------------------------------\\
    //                                                    PRIVATE METHODS                                             \\
    // ---------------------------------------------------------------------------------------------------------------\\
    #region

    // 'Awake()' is alled before 'Start'.
    private void Awake()
    {
      // Get ball.
      ball = GetComponent<Ball>();
      // If there is camera tagged as 'MainCamera'.
      if(Camera.main != null)
      {
        // Get main camera transform.
        main_camera_trans=Camera.main.transform;
      }
      // If there is no camera tagged as 'MainCamera'.
      else
      {
        // Log warning.
        Debug.LogWarning("No main camera found. Ball needs a Camera tagged \"MainCamera\", "+
                         "for camera - relative controls.");
      }
    } // End of Awake

    // Update (called once per frame)
    private void Update()
    {
      // Get the axis and jump input ('CrossPlatformInputManager' will provide correct input handling
      // for multiple platforms like PC, mobie, Xbox ect).
      float h = CrossPlatformInputManager.GetAxis("Horizontal");
      float v = CrossPlatformInputManager.GetAxis("Vertical");
      jump = CrossPlatformInputManager.GetButton("Jump");

      // Get the axis and jump input.
      Debug.Log("H="+CrossPlatformInputManager.GetAxis("Horizontal"));
      Debug.Log("V="+CrossPlatformInputManager.GetAxis("Vertical"));
      Debug.Log("main_camera_trans="+main_camera_trans);

      // If there is main camera.
      if(main_camera_trans!=null)
      {
        // Since there is a main camera, take the X and Z axis of the camera, throw away the y axis 
        // (the ball only moves in X and Z!), and let those be the X and Z axis for the ball's motion in response to the
        // X and Y axes. ???
        // 
        // 'Scale()' will multiply two Vectors (it is done to get rid of 'Y' axis value).
        // Normalization is done to ensuer that this new vector will only have values like 0 or 1 (that way this Vector
        // will be only for puropses of recognizing with direction camera is facing).
        main_camera_direction = Vector3.Scale(main_camera_trans.forward,new Vector3(1,0,1)).normalized;

        Debug.Log("main_camera_trans.forward="+main_camera_trans.forward);
        Debug.Log("new Vector3(1,0,1)="+new Vector3(1,0,1));
        Debug.Log("Vector3.Scale(main_camera_trans.forward,new Vector3(1,0,1))="+Vector3.Scale(main_camera_trans.forward,new Vector3(1,0,1)));
        Debug.Log("normalized="+Vector3.Scale(main_camera_trans.forward,new Vector3(1,0,1)).normalized);


        // Calculate movement.
        move = (main_camera_direction*v + main_camera_trans.right*h).normalized;
        //TDV: now, apply the motion.  cam.right is always horizontal 
        //TDV: regardless of camera rotion, so it doesn't need to be "Scale"'d. ???
      }
      // If there is no main camera.
      else
      {
        // If there is no main camera, just use world space instead of the camera's frame.
        // Calculate movement.
        move = (Vector3.forward*v + Vector3.right*h).normalized;
      }
    } // End of Update

    // 'FixedUpdate' works like 'Update', but it is called at a fixed framerate instead of on every frame. 
    // This makes it useful for physics actions so that behaviour is repeatable (so you can record for eg
    // gameplay with code). 
    private void FixedUpdate()
    {
      // Call the 'Move' function of the ball controller.
      ball.Move(move,jump);
      // Change 'jumpl' flag.
      jump = false;
    } // End of FixedUpdate

    #endregion

  } // End of BallUserControl
} // End of namespace
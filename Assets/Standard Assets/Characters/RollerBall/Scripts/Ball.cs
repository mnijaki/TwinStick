using System;
using UnityEngine;

namespace UnityStandardAssets.Vehicles.Ball
{
  // Ball.
  public class Ball:MonoBehaviour
  {
    // ---------------------------------------------------------------------------------------------------------------\\
    //                                                  SERIALIZED FIELDS                                             \\
    // ---------------------------------------------------------------------------------------------------------------\\
    #region

    // Apparently, the 'm_' part is ignored and a space is put between 'Move' and 'Power', and similarly for the other 
    // field names. This appears to happen automatically, unless there is a config file somewhere I haven't 
    // looked at that sets this up.
    //
    // The force added to the ball to move it.
    [SerializeField] private float m_MovePower = 5;
    // Whether or not to use torque to move the ball.
    [SerializeField] private bool m_UseTorque = true;
    // The maximum velocity the ball can rotate at.
    [SerializeField] private float m_MaxAngularVelocity = 25;
    // The force added to the ball when it jumps.
    [SerializeField] private float m_JumpPower = 2;

    #endregion


    // ---------------------------------------------------------------------------------------------------------------\\
    //                                                    PRIVATE FIELDS                                              \\
    // ---------------------------------------------------------------------------------------------------------------\\
    #region

    // The length of the ray to check if the ball is grounded.
    private const float k_GroundRayLength = 1f;
    // Ball rigidbody.
    private Rigidbody m_Rigidbody;

    #endregion


    // ---------------------------------------------------------------------------------------------------------------\\
    //                                                    PUBLIC METHODS                                              \\
    // ---------------------------------------------------------------------------------------------------------------\\
    #region

    // Method that handles moving and jumping. It is called by 'BallUserControl' in response to input.
    public void Move(Vector3 moveDirection,bool jump)
    {
      // If using torque to rotate the ball.
      if(m_UseTorque)
      {
        // 'AddTorque' - adds spin force to a rigid body.
        // In this case, the amount of forward motion becomes the amount of spin force around.
        // The X axis, and the amount of deviation along the X axis, becomes the amount of
        // spin force about the Z axis (with a negative sign to make it go the right way).
        //
        // Since torque is like a force, it works like gravity and causes the ball to "accelerate" its spin.
        //  
        // There is no need to add forward velocity, because once the ball spins, friction makes it roll the right 
        // direction!
        //
        // The ball accelerates because the torque "stays" on the ball and keeps accelerating it.  
        // The 'm_MaxAngularVelocity' was put onto the 'Rigidbody' component, and it keeps the ball from spinning 
        // too fast (thx to that angular speed cannot reach infinity).
        //        
        // We only have two axes of user control ('X' and 'Z') from keyboard or mouse or pad. 
        // The 'Y' axis (up) is controlled by jump which is why we always set the second member of Vector3() to zero.
        //
        // Since 'AddTorque is all about rotational forces, the force is perpendicular (90 degrees)
        // to the axis of movement, which is why 'Z' and 'X' are switched. 
        // I guess that's also why 'X' is negative - to make it move in the right direction relating to the
        // perspective of the camera.
        //
        // Add torque(spin force) around the axis defined by the move direction.
        m_Rigidbody.AddTorque(new Vector3(moveDirection.z,0,-moveDirection.x) * m_MovePower);
        //m_Rigidbody.AddTorque(new Vector3(moveDirection.x,0,moveDirection.z) * m_MovePower);

      }
      // If not using torque to rotate the ball.
      else
      {
        // Not using torque, so this can be used for a non-ball player
        // Just add ordinary force causing acceleration in the move direction.
        //
        // Add force in the move direction.
        m_Rigidbody.AddForce(moveDirection*m_MovePower);
      }

      // If on the ground and jump is pressed (so you cannot jump if ball is in the air).
      if((Physics.Raycast(transform.position,-Vector3.up,k_GroundRayLength)) && (jump))
      {
        // I'm not an expert but I guess 'ForceMode.Impulse' is a short cut for applying the force in an instant
        // where you would usually have to calculate the force based on the frame update duration 
        // which might vary with the frame rate.
        // Using regular apply force might vary with frame rate, but that's my naive guess.
        //
        // Add force in upwards. Impulse means the force is added once, as a shot, instead of continuously.
        m_Rigidbody.AddForce(Vector3.up*m_JumpPower,ForceMode.Impulse);
      }
    }

    #endregion


    // ---------------------------------------------------------------------------------------------------------------\\
    //                                                    PRIVATE METHODS                                             \\
    // ---------------------------------------------------------------------------------------------------------------\\
    #region

    // Initialization.
    private void Start()
    {
      // Get rigidbody.
      m_Rigidbody = GetComponent<Rigidbody>();
      // Set the maximum angular velocity (maximum spin rate of the ball). 
      GetComponent<Rigidbody>().maxAngularVelocity = m_MaxAngularVelocity;
    } // End of Start

    #endregion

  } // End of Ball
} // End of namespace
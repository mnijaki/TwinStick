using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class CameraPan : MonoBehaviour
{
  // ---------------------------------------------------------------------------------------------------------------\\
  //                                                   SERIALIZED FIELDS                                            \\
  // ---------------------------------------------------------------------------------------------------------------\\
  #region

  // Pan speed of camera.
  [SerializeField]
  [Tooltip("Pan speed of camera")]
  private float pan_speed = 1.0F;

  #endregion


  // ---------------------------------------------------------------------------------------------------------------\\
  //                                                     PRIVATE FIELDS                                             \\
  // ---------------------------------------------------------------------------------------------------------------\\
  #region

  // Player.
  private GameObject player;
  // Game manager.
  private GameManager game_manager;

  #endregion


  // ---------------------------------------------------------------------------------------------------------------\\
  //                                                    PRIVATE METHODS                                             \\
  // ---------------------------------------------------------------------------------------------------------------\\
  #region

  // Initialization.
  private void Start()
  {
    // Get Player.
    this.player = GameObject.FindGameObjectWithTag("Player");
    // Get game manager.
    this.game_manager = GameObject.FindObjectOfType<GameManager>();
  } // End of Start

  // LateUpdate is called after all Update functions have been called.
  private void LateUpdate()
  {
    // If replay mode then exit from function.
    if(!this.game_manager.is_recording)
    {
      return;
    }
    // Look camera at player.
    this.transform.LookAt(this.player.transform);
  } // End of LateUpdate

  // LateUpdate is called after all Update functions have been called.
  private void Update()
  {
    // If replay mode then exit from function.
    if(!this.game_manager.is_recording)
    {
      return;
    }
    // Get values.
    float h = CrossPlatformInputManager.GetAxis("RHorizontal");
    float v = CrossPlatformInputManager.GetAxis("RVertical");
    // Create directional vector.
    Vector3 dir = new Vector3(v,h,0);
    // Rotate camera aroun player
    this.transform.RotateAround(this.player.transform.position,dir,this.pan_speed);
    
    // TO_DO: should add code that handles continouse offest from camera to player.

  } // End of LateUpdate

  #endregion

} // End of CameraPan

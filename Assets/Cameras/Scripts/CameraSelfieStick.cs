using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class CameraSelfieStick : MonoBehaviour
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
  // Selfie stick rotation.
  private Vector3 rot;
  // Flag if playback was played.
  private bool was_playback = false;

  #endregion


  // ---------------------------------------------------------------------------------------------------------------\\
  //                                                    PRIVATE METHODS                                             \\
  // ---------------------------------------------------------------------------------------------------------------\\
  #region

  // Initialization.
  private void Start()
  {
    // Get player.
    this.player = GameObject.FindGameObjectWithTag("Player");
    // Get game manager.
    this.game_manager = GameObject.FindObjectOfType<GameManager>();
    // Get selfie stick rotation.
    this.rot = this.transform.rotation.eulerAngles;
  } // End of Start

  // Update (called once per frame).
  private void Update()
  {
    // If replay mode.
    if(!this.game_manager.is_recording)
    {
      // Change flag.
      this.was_playback=true;
      // Exit from function.
      return;
    }
    // If playback was played.
    if(this.was_playback)
    {
      // Get actual rotation (could be changed in playback mode).
      this.rot = this.transform.rotation.eulerAngles;
      // Reset flag.
      this.was_playback=false;
    }
    // Compute rotations.
    this.rot.y += CrossPlatformInputManager.GetAxis("RHorizontal") * this.pan_speed;
    this.rot.x += CrossPlatformInputManager.GetAxis("RVertical") * this.pan_speed;
    // Actualize position of selfie stick.
    this.transform.position = this.player.transform.position;
    // Actualize rotation of selfie stick.
    this.transform.rotation = Quaternion.Euler(this.rot);
  } // End of Update

  #endregion

} // End of CameraSelfieStick
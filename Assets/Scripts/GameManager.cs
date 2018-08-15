using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

// Game manager.
public class GameManager:MonoBehaviour
{
  // ---------------------------------------------------------------------------------------------------------------\\
  //                                                   PUBLIC FIELDS                                                \\
  // ---------------------------------------------------------------------------------------------------------------\\
  #region

  // Flag if recording is active.
  public bool is_recording = true;

  #endregion


  // ---------------------------------------------------------------------------------------------------------------\\
  //                                                  PRIVATE METHODS                                               \\
  // ---------------------------------------------------------------------------------------------------------------\\
  #region
    
  // Update (called once per frame).
  private void Update()
  {
    // If player hit 'Fire1' button (left control on default PC platform input).
    if(CrossPlatformInputManager.GetButton("Fire1"))
    {
      this.is_recording = false;
    }
    else
    {
      this.is_recording = true;
    }
  } // End of Update

  #endregion

} // End of GameManager
using UnityEngine;

// Replay manager.
public class ReplayManager:MonoBehaviour
{
  // ---------------------------------------------------------------------------------------------------------------\\
  //                                                   PRIVATE FIELDS                                               \\
  // ---------------------------------------------------------------------------------------------------------------\\
  #region
  
  // Maximum amount of frames that can be buffered.
  private const int frames_buffer_size = 500;
  // Array of frames.
  private MyKeyFrame[] frames = new MyKeyFrame[frames_buffer_size];
  // Rigidbody.
  private Rigidbody rigid_body;
  // Game manager.
  private GameManager game_manager;
  // Current frames index.
  private int frames_curr_idx = 0;
  // Number of recorded frames.
  private int frames_recorded = 0;

  #endregion


  // ---------------------------------------------------------------------------------------------------------------\\
  //                                                   PRIVATE METHODS                                              \\
  // ---------------------------------------------------------------------------------------------------------------\\
  #region
  
  // Initialization.
  private void Start()
  {
    // Get rigidbody.
    this.rigid_body = this.GetComponent<Rigidbody>();
    // Get game manager.
    this.game_manager = GameObject.FindObjectOfType<GameManager>();
    // Create initial values.
    float time = 0.0F;
    Vector3 pos = Vector3.zero;
    Quaternion rot = Quaternion.identity;
    // Initialize frames array (so garbage collector won't have too much work).
    for(int i=0; i<frames_buffer_size; i++)
    {
      this.frames[i]=new MyKeyFrame(time,pos,rot);
    }
  } // End of Start

  // Update (called once per frame).
  private void Update()
  {
    // If record mode.
    if(this.game_manager.is_recording)
    {
      Record();
    }
    // If playback mode.
    else
    {
      PlayBack();
    }
  } // End of Update

  // Record frames.
  private void Record()
  {
    // Change rigidbody to non kinematic (physics will calculate when recording is active). 
    this.rigid_body.isKinematic = false;
    // If recorded frames > 1;
    if(this.frames_recorded > 0)
    {
      // Increase current frame index.
      this.frames_curr_idx++;
    }
    // Actualize 'frame_curr_idx' to the size of buffer.
    this.frames_curr_idx=this.frames_curr_idx % frames_buffer_size;
    // If recorded frames is less than size of buffer.
    if(this.frames_recorded<frames_buffer_size)
    {
      // Actualize number of recorded frames.
      this.frames_recorded++;
    }
    // Save frame.
    this.frames[this.frames_curr_idx].MyKeyFrameSet(Time.time,this.transform.position,this.transform.rotation);
  } // End of Record

  // Playback saved frames.
  private void PlayBack()
  {
    // Change rigidbody to kinematic (physics won't calculate when playback is active).
    this.rigid_body.isKinematic = true;
    // If there was no recorded frames so far then exit from function.
    if(this.frames_recorded==0)
    {
      return;
    }    
    // Read frame values.
    this.transform.position = this.frames[this.frames_curr_idx].position;
    this.transform.rotation = this.frames[this.frames_curr_idx].rotation;
    // Decrease number of recorded frames.
    this.frames_recorded--;
    // If current frame index > 0.
    if(this.frames_curr_idx>0)
    {
      // Decrease current frame index.
      this.frames_curr_idx--;
    }
    // If current frame index <= 0.
    else
    {
      // If there are still some recorded frames.
      if(this.frames_recorded > 0)
      {
        // Set current frame index to last index in frames (frames buffer size - 1).   
        this.frames_curr_idx = frames_buffer_size - 1;
      }
    }
  } // End of PlayBack

  #endregion

} // End of ReplayManager



/// <summary>
/// A structure for storing time, rotation and position (structure store by values, not references like class).
/// </summary>
public struct MyKeyFrame
{
  // ---------------------------------------------------------------------------------------------------------------\\
  //                                                    PUBLIC FIELDS                                               \\
  // ---------------------------------------------------------------------------------------------------------------\\
  #region

  public float frame_time;
  public Vector3 position;
  public Quaternion rotation;

  #endregion


  // ---------------------------------------------------------------------------------------------------------------\\
  //                                                    PUBLIC METHODS                                              \\
  // ---------------------------------------------------------------------------------------------------------------\\
  #region

  // Constructor of 'MyKeyFrame' struct.
  public MyKeyFrame(float frame_time, Vector3 position, Quaternion rotation)
  {
    this.frame_time = frame_time;
    this.position = position;
    this.rotation = rotation;
  } // End of MyKeyFrame

  // Set values of 'MyKeyFrame'.
  public void MyKeyFrameSet(float frame_time,Vector3 position,Quaternion rotation)
  {
    this.frame_time = frame_time;
    this.position = position;
    this.rotation = rotation;
  } // End of MyKeyFrameSet

  #endregion

} // End of MyKeyFrame
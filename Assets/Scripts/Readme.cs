using UnityEngine;

// Only for purpose of readme.
public class Readme : MonoBehaviour
{

  //First scene should be used with cross platform input disabled(editor/mobile input/disable), because I
  //used there standard rig camera from standard assets to check how it works(so this
  //scene have camera without selfie stick).
  //There are some little glitches(I think that I should add replay system to rig camera and turn
  //off scripts responsible for camera movement when in rewind/replay mode)
  //Controls:
  //left control - rewind
  //wsad - movement
  //space - jump
  //mouse - camera angles

  //Second scene should be used with cross platform input enabled(editor/mobile input/enable) and is preetty much the same like version from courses.
  //There is one exception: I deleted camera pannign script because it is ussless (‘SelfieStick.cs’ is already
  //handling whole movement of camera. I belive it was omissioned in courses by coincidence).
  //Controls:
  //Whole stiring is made by UI buttons.

  //Third scene scene should be used with cross platform input enabled (editor/mobile input/enable).
  //This scene have camera without selfie stick(camera move is done by ‘LookAt()’ and ‘RotateAround()’
  //methods).
  //Controls:
  //Whole stiring is made by UI buttons.

  //I didnt add any seriouse game stucture becouse I think that main aim of this section was to
  //make own replay/rewind system.
  //I deleted code associated witch saving level and pausing game (in my opinion it was added a bit by force
  //and I think it is unnecessary in this project).

  //I made my own version of replay/rewind system, which works a little bit like in prince of persia.
  //Moreover it is overwriting existing elements without any problems that we have in courses build.
  //And at the end I added some initialisation of array to relieve garbage collector from unnecessery work.

} // End of Readme

Please feel free to contact me if you have any questions.
shopmaster@chobi-glass.com
http://chobi-glass.com/Unity/index.html


*** How to start the demo scenes ***
The demo scenes are placed under 'Kawaii_Tanks_Project' folder.
"Base_01" is a demo scene for "Web Player" and "PC, Mac & Linux Standalone" platforms.
"Base_01_Mobile" is a demo scene for "Android" and "iOS" platforms.
Make sure that your project's platform is set correctly in "Build Settings".


*** How to operate ***
"PC, Mac & Linux Standalone" platforms.
[Move] WASD
[Camera] Right Mouse Drag
[Zoom] Mouse Wheel
[Fire] Space key
[Aim with Sight] Left Mouse Drag
[Switch Tank] Enter (Return)

 "Android" and "iOS" platforms.
[Move] Touch "Move" pad.
[Camera] Swipe "Camera" pad.
[Zoom] Swipe "Zoom" pad.
[Lock On] Touch "Target" area.
[Aim with Sight] Drag "Gun Cam" pad.
[Fire] Releases a finger from "Gun Cam" pad.
[Switch Tank] Touch "Switch" pad.


*** How to use the tank in your scene ***
Drag and drop the tank prefab into your scene.
Drag and drop the "Canvas" prefab into your scene.
When your project's platform is movile device, also drag and drop the "Touch_Controls" prefab into your scene.
And make sure the following Layer Settings.


*** About Layer settings ***
This project uses User Layer 9, 10, 11 in order to control the collision of tank parts.
The wheels are placed in User Layer 9, and their colliders do not collide with MainBody and each other.
The suspensions are placed in User Layer 10, and their colliders do not collide with anything.
The MainBody is placed in User Layer 11, and its colliders do not collide with wheels.
The collision settings are written in "Wheel_Control_CS" script.


*** Contents ***
"Kawaii_Tanks_Assets" folder contains main contents of this asset.
	"Editor" and "Scripts" folders contain scripts for moving tanks.
	"Meshes" folder contains 3D models and textures.
	"Others" folder contains Physic Materials, sound files, Sprites for marker and reticle, terrain data and textures for terrain.
	"Prefabs" folder contains tank prefabs and other prefabs such as bullet and trees.
"Editor" and "Standard Assets" folders contain 'CrossPlatformInput' of Unity 5.


*** FAQ ***
Q1. How do I change the tank speed?
A1. You can change the maximum speed and the torque in 'Wheel_Control_CS(Script)' in the "MainBody".

Q2. How do I change the turret movement?
A2. The settings can be changed in the "Turret_Base" and the "Cannon_Base". Also you can change the recoil-brake settings in the "Barrel_Base".

Q3. How do I change the loading time of the gun?
A3. In the "Cannon_Base", you can change the reloading time and the recoil force.

Q4. How do I change the bullet?
A4. In the "Fire_Point", you can set Prefabs for the bullet and the muzzle fire.

Q5. Can I change the targeting marker and the reticle image?
A5. Set your images in Canvas in the scene. And input the marker's name in the "Turret_Base", and input the reticle's name in the "Gun_Camera".

Q6. I get the error message "There are 2 audio listeners in the scene........" in PlayMode.
A6. Give the ID number differing from other tanks in the top object of the tank.

Please feel free to contact me if you have any questions.
shopmaster@chobi-glass.com







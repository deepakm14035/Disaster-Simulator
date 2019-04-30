# Disaster-Simulator

There are two projects which contain different modules
1. Multi-user control + Unity based control + python based control, and
2. Multi-user control + Unity based control

The reason for this is that the libraries that are used to run the python code do not allow for an executable to be built in unity. So, if there is a need to create an executable, the second one can be used. Otherwise, the first one can be used which can be run only in the Unity Editor.

Tested with Unity 2017.4.10.

# To run the simulator in the editor -
1. Open project in Unity
2. Open the "mainmenu" scene, and hit "Play"


# To build the project -
1. Open project in Unity
2. Go to File -> Build Settings -> Build
3. Enter a name, and click save
4. The executable is now present in the location earlier specified. To move the executable to another location, the folder with the same name followed by the "_data" should also be moved.

MODES-

	Manual -
	In this mode, multiple users can connect to a single session and control different bots to complete the task of picking up people in an efficient manner. To do this,
		a. In the main menu, "Server" needs to be checked.
		b. Mode should be set to "Manual"
		b. the local IP address and an unused port number is to be entered
		c. If another user wants to connect to this session, "Client" checked, followed by the IP address and port number of the server.

	Auto -
	In this mode, an algorithm needs to be specified in the start and update python files. More details below. To run a simulation based on this,
		a. In the main menu, Mode should be set to "Manual". Ignore all the other input fields
		b. Click "start"
		
	Auto (without python) -
	In this mode, the function "moveBoat" of BoatController.cs C# script (located in Assets/scripts folder) is executed at every frame of the simulation for each boat. To run a simulation based on this,
		a. In the main menu, Mode should be set to "Auto (without python)". Ignore all the other input fields
		b. Click "start"
	
	
Python code - 

To modify the python code, there are two scripts, **start.py** and **update.py** in the Assets folder of the project.

**start.py :** It is executed once when the simulation starts. All the required variables are initialised here. It has some helper functions

**update.py :** It is executed at the rate same as the FPS (~ 30 times per second) of simulation. The functions declared in the start.py file is used here. A greedy approach for the allocation of tasks for the autonomous agents has been implemented here (where the closest autonomous boat is assigned the task of picking up the person).


For any queries, contact me at deepak14035@iiitd.ac.in

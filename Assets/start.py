#import clr
#clr.AddReference("UnityEngine")

import UnityEngine
from System.Collections.Generic import *
setBoatPositions=[]

def function_name(name):
	"""docstring"""
	print name
	return name+' is my name'
	
def getAllBoatObjects():
	boats = UnityEngine.GameObject.FindGameObjectsWithTag ("boat")
	#int[,] pos=new int[boats.Length,3];
	names=[]
	for b in boats:
		#UnityEngine.Debug.Log( b.name)
		names.append(b)
	return boats
	
def getBoatPositionFromObject(object):
	#object=UnityEngine.GameObject.Find('name')
	if(object):
		arr=[object.transform.position.x, object.transform.position.y, object.transform.position.z]
		return arr
	else:
		return null
		
def getAllDiscoveredPeopleObjects():
	people = UnityEngine.GameObject.FindGameObjectsWithTag ("detected")
	#UnityEngine.Debug.Log( people.Length)
	#for person in people:
	#	UnityEngine.Debug.Log(person.name)
	return people

def setDestinationForBoat(boatObj, position):
	boatObj.GetComponent<AutonomousBoat>().setDestination(position)

def distanceBetween(a,b):	
	return UnityEngine.Vector3.Distance(a.transform.position,b.transform.position)
	
def closestSafehouse(safehouses, object):
	mindist=9999
	closest_safehouse=safehouses[0]
	for safehouse in safehouses:
		if(distanceBetween(safehouse,object)<mindist):
			closest_safehouse=safehouse
			mindist=distanceBetween(safehouse,object)
			
	return closest_safehouse
	
def getBoatIndexFromName(name):
	for boat_index, boat in boats:
		if(boat.name.Equals(name)):
			return boat_index
		
	return -1

def movementForBoat(boat_index):
	people=getAllDiscoveredPeopleObjects()
	min_distance=99999
	min_index=-1
	for person_index, person in people:
		if(distanceBetween(boats[boat_index],person)<min_distance):
			min_distance=distanceBetween(boats[boat_index],person)
			min_index=person_index
	
	if(min_index>=0):
		setDestinationForBoat(boats[boat_index],people[min_index])
		setBoatPositions[boat_index]=True
		#UnityEngine.Debug.Log(boats[boat_index].name+" going to "+people[min_index].name)
	

def getSafehouses():
	return UnityEngine.GameObject.FindGameObjectsWithTag ("safehouse")
	
	
def getDestinations():
	return List[float](destinations)
	
safehouses=getSafehouses()
	
mname=function_name('deepak')

boats=getAllBoatObjects()

for boat in boats:
	setBoatPositions.append(False)

people=getAllDiscoveredPeopleObjects()
boat_index=0
destinations=[]
for b in boats:
	destinations.append(b.transform.position)

destination_people=[]
for b in boats:
	destination_people.append(b)

setBoatPositions=[]
	
for boat in boats:
	setBoatPositions.append(False)

for person in people:
	min_distance=99999
	min_index=-1
	#UnityEngine.Debug.Log( "a")
	boat_index=0
	for boat in boats:
		#UnityEngine.Debug.Log( "a")
		if(distanceBetween(boat,person)<min_distance and not setBoatPositions[boat_index]):
			if(distanceBetween(boat,person)<500):
				min_distance=distanceBetween(boat,person)
				min_index=boat_index
		boat_index=boat_index+1
	
	#UnityEngine.Debug.Log( "a")
	if(min_index>=0):
		#setDestinationForBoat(boat,people[min_index])
		setBoatPositions[min_index]=True
		destinations[min_index]=person.transform.position
		destination_people[min_index]=person

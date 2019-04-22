import UnityEngine
from System.Collections.Generic import *

boats=getAllBoatObjects()


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
			if(distanceBetween(boat,person)<1500):
				min_distance=distanceBetween(boat,person)
				min_index=boat_index
		boat_index=boat_index+1
	
	#UnityEngine.Debug.Log( "a")
	if(min_index>=0):
		#setDestinationForBoat(boat,people[min_index])
		setBoatPositions[min_index]=True
		destinations[min_index]=person.transform.position
		destination_people[min_index]=person

#UnityEngine.Debug.Log("ending here!")

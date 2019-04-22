
#setBoatPositions=[]

#boats=getAllBoatObjects()

for boat in boats:
	setBoatPositions.append(false)

people=getAllDiscoveredPeopleObjects()




for boat_index, boat in boats:
	min_distance=99999
	min_index=-1
	for person_index, person in people:
		if(distanceBetween(boat,person)<min_distance and not setBoatPositions[boat_index]):
			min_distance=distanceBetween(boat,person)
			min_index=person_index
	
	if(min_index>=0):
		setDestinationForBoat(boat,people[min_index])
		setBoatPositions[boat_index]=true

#for b in boatNames:
#	print(b)

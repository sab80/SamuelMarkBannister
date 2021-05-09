
#!/usr/bin/python

import pymongo
from bson import Code
import os
from pprint import pprint
import bson
import PySimpleGUI as sg 
import math
import matplotlib.pyplot as plt
import random

##This will not run, as I have removed my connection to the mongodb server.
##The most interesting class is the PossibleDestinations class in which I create a decision tree to attempt
##to predict a ships leaving port, when passed a joining port.
##It is trained on 95% of the dataset and tested on 5%, with an accuracy between 35%-43% with outliers.
##It's worth noting that this accuracy would be improved by training on a randomly split 95%, instead of the
##first 95% of the dataset.
connection_string = 'mongodb://'+user+':'+password+'@'+dbpath

client = pymongo.MongoClient(connection_string)

db = client.sab80

class SailorRecords():
    #Used to retrieve a dict of all the unique sailors based on name, year of birth and place of birth
    def GetIndividualSailors():
        pipeline = [{'$unwind': "$mariners"},
                        {"$sort": {"mariners.name": -1}},
                        {'$group': {'_id': {
                        "name" : "$mariners.name",
                        "place_of_birth" : "$mariners.place_of_birth",
                        "year_of_birth" : "$mariners.year_of_birth"}}},
                        {'$project': {'_id': 0,'name': "$_id.name",'place_of_birth': "$_id.place_of_birth",'year_of_birth': "$_id.year_of_birth"}}]
        
        result = db.sab80.aggregate(pipeline)
        return result
    #Used to search for a sailor when given name, year of birth and place of birth
    #Returns a dict including name, year of birth and place of birth and all of the attributes required for displaying for a sailors record.
    def SailorSearch(name, year_of_birth, place_of_birth):
        print(name, place_of_birth, year_of_birth)
        print("Testing in progress...")
        pipeline = [{'$unwind': '$mariners'},
                    {'$match': {'mariners.name': name, 'mariners.place_of_birth': place_of_birth, 'mariners.year_of_birth': year_of_birth}},
                    {"$sort": {"mariners.last_ship_leaving_date": 1}},
                    {'$group': {'_id': {
                        "name" : "$mariners.name",
                        "place_of_birth" : "$mariners.place_of_birth",
                        "year_of_birth" : "$mariners.year_of_birth"},
                        "sailorData": {"$push": {"last_ship_leaving_date" : "$mariners.last_ship_leaving_date", "last_ship_name" : "$mariners.last_ship_name","this_ship_leaving_port" : "$mariners.this_ship_leaving_port" , "this_ship_joining_date" : '$mariners.this_ship_joining_date', 'this_ship_joining_port': "$mariners.this_ship_joining_port", "this_ship_leaving_date" : "$mariners.this_ship_leaving_date","this_ship_leaving_port" : "$mariners.this_ship_leaving_port", "this_ship_leaving_cause" : "$mariners.this_ship_leaving_cause", "signed_with_mark" : "$mariners.signed_with_mark"}}}}  ]
                    
                    
        results = db.sab80.aggregate(pipeline)
 
        return results

    def printResults(self, results):
        for doc in results:
            print(" * {name}, {year_of_birth}, {place_of_birth}, {last_ship_name}".format(
                name = doc["name"],
                year_of_birth = doc['year_of_birth'],
                place_of_birth = doc['place_of_birth'],
                last_ship_name = doc['last_ship_name']
                #year_of_birth = doc['year_of_birth'],
                #home_address = doc['home_address'],
                #date_leaving = doc['this_ship_leaving_date'],
                #ship = doc["vessel name"],
                #notes = doc['mariners']['additional_notes']
                ))    
    #returns the length of a dict
    def countResults(self, results):
        count = 1
        for i in results:
            count = count + 1
            return count
    

class SailorsRanks():
    #returns a dict of sailors name and rank
    def DisplaySailorAndRank():
        pipeline = [{'$unwind': "$mariners"},
                    {"$sort": {"mariners.this_ship_capacity": 1}},
                    {'$group': {'_id': {
                        "name" : "$mariners.name",
                        "place_of_birth" : "$mariners.place_of_birth",
                        "year_of_birth" : "$mariners.year_of_birth"},
                        "rank": {"$push": {"this_ship_capacity": '$mariners.this_ship_capacity'} }}}]
                    
                   # {'$project': {'_id': 0,'name': "$_id.name", 'rank': "$_id.this_ship_capacity"}}]
        
        results = db.sab80.aggregate(pipeline)
        return results  
    #returns a dict containing rank and joining date and is sorted alphabetically
    def PromotionSearch(name, year_of_birth, place_of_birth):
        print(name, place_of_birth, year_of_birth)
        pipeline = [{'$unwind': '$mariners'},
                    {'$match': {'mariners.name': name, 'mariners.place_of_birth': place_of_birth, 'mariners.year_of_birth': year_of_birth}},
                    {"$sort": {"mariners.this_ship_joining_date": 1}},
                    {'$group': {'_id': {
                        "name" : "$mariners.name",
                        "place_of_birth" : "$mariners.place_of_birth",
                        "year_of_birth" : "$mariners.year_of_birth"},
                        "rankJoinDate": {"$push": {"this_ship_capacity": '$mariners.this_ship_capacity', "this_ship_joining_date" : "$mariners.this_ship_joining_date"}}}}  ]
        results = db.sab80.aggregate(pipeline)
 
        return results
    
    #not used in code
    def SailorsAtRank(this_ship_capacity):
        pipeline = [{'$unwind': '$mariners'},
                    {'$match': {'mariners.this_ship_capacity': this_ship_capacity}},    
                    {'$group': {'_id': "$mariners"}},
                    {"$count": "rank_count"}]
                
        results = db.sab80.aggregate(pipeline)
        return results  
    #not used in code
    def GetAllRanks(self):
        print("in ranks Func")
        result = db.sab80.distinct('mariners.this_ship_capacity')
        for doc in result:
            print(doc)
            
class NumberOfCrewOnShip():
    #Returns the official number of all unique ships
    def FindAllShipNames(self):
        result = db.sab80.distinct("official number")
        count = 0
        for doc in result:
            print(doc)
            count = count + 1   
            print(count)
        return result
    #counts the number of mariners on a ship       
    def CountOnShip(self, ship):
        count = 0
        result = db.sab80.find({"official number" : ship})
        for doc in result:
            #print(doc)
            count = count + 1 
        return count
    
    #creates a list of the crew size, including all the unique ships
    def ListVesselWithCount(self):
        shipsCount = []
        names_of_ships = NumberOfCrewOnShip().FindAllShipNames()
        for ship in names_of_ships:
            crew_count = NumberOfCrewOnShip().CountOnShip(ship)
            shipsCount.append(crew_count)
      
        return shipsCount
    #Draws a histogram of the number of crew on each ship    
    def DrawHistogram():
        shipCount = NumberOfCrewOnShip().ListVesselWithCount()
        n = len(shipCount)
        maxValue = max(shipCount)
        minValue = min(shipCount)
        range_of_values = maxValue - minValue
        intervals = float(math.sqrt(n))
        width_of_intervals = round(range_of_values / intervals)
        plt.hist(shipCount, bins = width_of_intervals)
        plt.show()


class PossibleDestinations():
    #returns the top ten most visited joining ports
    def FindTopTenJoiningPorts(self):
        print("In Func")
        joiningPortTree = {}

        pipeline = [{'$unwind': '$mariners'},
                      {'$group': {'_id': {
                        "name" : "$mariners.name"},
                        "ports": {"$last": {"this_ship_joining_port" : "$mariners.this_ship_joining_port"}}}}               
                    ]
        result = db.sab80.aggregate(pipeline)
        
        for doc in result:
            portInfo = doc["ports"]
            try:
                joiningPort = portInfo['this_ship_joining_port']
                #checks if the joining port is already in the dict
                #if it is then the value is incremented by one
                if joiningPort in joiningPortTree.keys():
                    joiningPortTree[joiningPort] += 1
                else:
                    joiningPortTree[joiningPort] = 1
            except:
                print("Missing Port") 
        
        #Orders the dict and transforms it to a list for use.
        joiningPortTree = dict(sorted(joiningPortTree.items(), key=lambda item: item[1]))
        topTen = []
        portTreeListKey = list(joiningPortTree)
        portTreeListValue = list(joiningPortTree.values())
        portTreeLength = len(portTreeListKey) - 1
        print(portTreeLength)
        #Starts at 1 to remove the blk field
        #populates the topTen list with the most visited joining ports
        for i in range(11):
            if(i == 0):
                print("blk removed!")
            else:
                topTenValues = (portTreeListKey[portTreeLength - i], portTreeListValue[portTreeLength - i])
                topTen.append(topTenValues)
        print(topTen)
        return topTen

                

    #Returns the 5 most visited leaving ports for a given joining port   
    def FindFiveMostVisitedLeavingPorts(self, joiningPort):
        leavingPortTree = {}
        pipeline = [{'$unwind': '$mariners'},
                {"$match": {"mariners.this_ship_joining_port" : joiningPort}},
                {'$group': {'_id': {"name" : "$mariners.name"},
                "ports": {"$last": {"this_ship_joining_port" : "$mariners.this_ship_joining_port", "this_ship_leaving_port" : "$mariners.this_ship_leaving_port"}}}}       
                ]
        result = db.sab80.aggregate(pipeline)
        
        for doc in result:
            portInfo = doc["ports"]
            
            try:
                leavingPort = portInfo['this_ship_leaving_port']
                if leavingPort in leavingPortTree.keys():
                    leavingPortTree[leavingPort] += 1
                else:
                    leavingPortTree[leavingPort] = 1
            except:
                print("Missing Port") 
                
        #Orders the dict and transforms it to a list for use.
        leavingPortTree = dict(sorted(leavingPortTree.items(), key=lambda item: item[1]))
        topFive = []
        portTreeListKey = list(leavingPortTree)
        portTreeListValue = list(leavingPortTree.values())
        portTreeLength = len(portTreeListKey) - 1
        
        #Starts at 1 to remove the blk field
        #populates the topFive list with the most visited leaving ports for a selected joining port
        for i in range(5):
            if(portTreeLength > 5):
 
                topFiveValues = (portTreeListKey[portTreeLength - i], portTreeListValue[portTreeLength - i])
                topFive.append(topFiveValues)
            else:
                return "remove"
      
        return topFive
    #counts docs in dict
    def countDoc(self, col):
        count = 0 
        for doc in col:
            count = count + 1
        return count
    
    #Splits the data into test and train, returns test data and the trained list to create the decision tree
    def SplittingTheData(self):
        joiningPortTree = {}
        allLeavingPorts = []
        testingData = []
        pipeline = [{'$unwind': '$mariners'},
                      {'$group': {'_id': {
                        "name" : "$mariners.name"},
                        "ports": {"$last": {"this_ship_joining_port" : "$mariners.this_ship_joining_port" , "this_ship_leaving_port" : "$mariners.this_ship_leaving_port"}}}}               
                    ]
        
        result = db.sab80.aggregate(pipeline)   
        #gets size of data
        dataSize = PossibleDestinations().countDoc(result)
        #Splits the data 95% training, 5% testing
        testingSize = round(dataSize / 20)
        trainingSize = dataSize - testingSize
        print("DATA SIZE:" ,dataSize)
        print("TRAINING SIZE: ",trainingSize)
        count = 0 
        data = db.sab80.aggregate(pipeline)
        for doc in data: 
            portInfo = doc["ports"]
            #Count is used to switch from training to creating the test data list
            if (count < trainingSize):
                count = count + 1
                print("c", count)
                try: 
                    #increments joining port value each time its visited
                    joiningPort = portInfo['this_ship_joining_port']
                    if joiningPort in joiningPortTree.keys():
                        joiningPortTree[joiningPort] += 1
                    else:
                        joiningPortTree[joiningPort] = 1
                except: 
                    print("Missing Port")
            else:
                print("Should be here")
                try: 
                    joiningPort = portInfo['this_ship_joining_port']
                    leavingPort = portInfo['this_ship_leaving_port']
                    #joining port and leaving port are added to testData list to allow for testing to be carried out.
                    testingData.append((joiningPort, leavingPort))
                except:
                    print("Missing Port")
                    
        joiningPortTree = dict(sorted(joiningPortTree.items(), key=lambda item: item[1]))
        joining_port_list = []
        portTreeListKey = list(joiningPortTree)
        portTreeListValue = list(joiningPortTree.values())
        portTreeLength = len(portTreeListKey) - 1
        print(portTreeLength)
        
        i = 0
        for port in joiningPortTree:
            joiningPortCurrent = (portTreeListKey[i], portTreeListValue[i])
            joining_port_list.append(joiningPortCurrent)
            i = i+1
            currentFive = PossibleDestinations().FindFiveMostVisitedLeavingPorts(port)
            # This is here to allow the indexes of the joining ports and the leaving ports to still match up
            if(currentFive != "remove"): 
                allLeavingPorts.append(currentFive)
            else: 
                allLeavingPorts.append(("","","","",""))
            
            
        return joining_port_list, allLeavingPorts, testingData
    #Runs the tests on the testing data and provides accuracy
    def TestingTheData(self):
        joining_port_list, allLeavingPorts, testingData = PossibleDestinations().SplittingTheData()
        correct_answers = 0
        wrong_answers = 0
        totalTests = 0
        for test in testingData:
            actual_leaving_port = test[1]
            print("ACTUAL OUTCOME: ",actual_leaving_port)
            #Runs prediction function for each test
            prediction = PossibleDestinations().PredictALeavingPort(test[0], joining_port_list, allLeavingPorts)
            
            #This is used to determine a fair test has occured, if so the results are updated
            if(prediction == actual_leaving_port):
                correct_answers = correct_answers + 1
                totalTests = totalTests + 1
            elif(prediction == "unknown"):
                print("not counted")
                #actual_leaving_port == "blk" or 
            elif(actual_leaving_port == "Blk" or actual_leaving_port == " blk" or actual_leaving_port == " Blk" or actual_leaving_port == "Continued" or test[0] == "blk" or test[0] == "Blk" or test[0] == "Continued"):
                print("not counted")
            else:
                wrong_answers = wrong_answers + 1
                totalTests = totalTests + 1
        #Accuracy of the tests are produced
        accuracy = (correct_answers/totalTests) * 100
        print(correct_answers, "/", totalTests)
        print("Accuracy: ",accuracy,"%")
        
        
    #Predicts the leaving port when given a test joining port, and the decision tree
    def PredictALeavingPort(self, testJoiningPort, joiningPortTree, allLeavingPorts):
        print(joiningPortTree)
        print(testJoiningPort)
      
        i = 0
        index_joining_port = -1
        for port in joiningPortTree:
            #This finds if the joining port is in the decision tree
            if(port[0] == testJoiningPort):
                index_joining_port = i
                i = i + 1
                break
            else:
                i = i + 1
        #If its not in the tree then unknown is returned and the test is deemed to be unfair
        if(index_joining_port == -1): 
            print(testJoiningPort, "is not in the dataset!")
            return "unknown"

        relevent_leaving_ports = allLeavingPorts[index_joining_port]
        if(relevent_leaving_ports == (("","","","",""))):
            return "unknown"
        print("RELEVENT:", relevent_leaving_ports)
        totalWeight = 0
        accumalativeWeight = 0
        #multiplier was tested to create more seperation between leaving ports, so if there are lots of small values, they are less likely to be chosen
        #It was removed for the results section of the report
        multiplier = 2
        #Finds total weight
        for i in range(len(relevent_leaving_ports) -1):
            totalWeight = totalWeight + relevent_leaving_ports[i][1]
        #Randomly selects a value within the total weight
        randomSection = random.randrange(totalWeight)#multiplier can be tested here
        
        #Used to select the section of weight that the random number lies within.
        #This implements the weights of each of the ports to find a leaving port with probablity
        for i in range(len(relevent_leaving_ports) -1):
            accumalativeWeight = accumalativeWeight + relevent_leaving_ports[i][1]# multiplier can be tested here
            if (accumalativeWeight >= randomSection):
                selection = i
                break
        #the selected leaving port is returned
        prediction = relevent_leaving_ports [selection][0]
        
        print("prediction: ", prediction)
        return prediction
    
        

        
        
              
    
    
def CreateTree():
    allLeavingPorts = []
    topTenJoiningPorts = PossibleDestinations().FindTopTenJoiningPorts()
    for port in topTenJoiningPorts:
        currentFive = PossibleDestinations().FindFiveMostVisitedLeavingPorts(port[0])
        allLeavingPorts.append(currentFive)
        
    # The decision tree is put into a format in order to be displayed
    listOfTrees =  {'Aberystwyth': {0: {topTenJoiningPorts[0]: {0: allLeavingPorts[0][0], 1: allLeavingPorts[0][1], 2: allLeavingPorts[0][2], 3: allLeavingPorts[0][3], 4: allLeavingPorts[0][4]}},
                                     1: {topTenJoiningPorts[1]: {0: allLeavingPorts[1][0], 1: allLeavingPorts[1][1], 2: allLeavingPorts[1][2], 3: allLeavingPorts[1][3], 4: allLeavingPorts[1][4]}},
                                     2: {topTenJoiningPorts[2]: {0: allLeavingPorts[2][0], 1: allLeavingPorts[2][1], 2: allLeavingPorts[2][2], 3: allLeavingPorts[2][3], 4: allLeavingPorts[2][4]}},
                                     3: {topTenJoiningPorts[3]: {0: allLeavingPorts[3][0], 1: allLeavingPorts[3][1], 2: allLeavingPorts[3][2], 3: allLeavingPorts[3][3], 4: allLeavingPorts[3][4]}},
                                     4: {topTenJoiningPorts[4]: {0: allLeavingPorts[4][0], 1: allLeavingPorts[4][1], 2: allLeavingPorts[4][2], 3: allLeavingPorts[4][3], 4: allLeavingPorts[4][4]}},
                                     5: {topTenJoiningPorts[5]: {0: allLeavingPorts[5][0], 1: allLeavingPorts[5][1], 2: allLeavingPorts[5][2], 3: allLeavingPorts[5][3], 4: allLeavingPorts[5][4]}},
                                     6: {topTenJoiningPorts[6]: {0: allLeavingPorts[6][0], 1: allLeavingPorts[6][1], 2: allLeavingPorts[6][2], 3: allLeavingPorts[6][3], 4: allLeavingPorts[6][4]}},
                                     7: {topTenJoiningPorts[7]: {0: allLeavingPorts[7][0], 1: allLeavingPorts[7][1], 2: allLeavingPorts[7][2], 3: allLeavingPorts[7][3], 4: allLeavingPorts[7][4]}},
                                     8: {topTenJoiningPorts[8]: {0: allLeavingPorts[8][0], 1: allLeavingPorts[8][1], 2: allLeavingPorts[8][2], 3: allLeavingPorts[8][3], 4: allLeavingPorts[8][4]}},
                                     9: {topTenJoiningPorts[9]: {0: allLeavingPorts[9][0], 1: allLeavingPorts[9][1], 2: allLeavingPorts[9][2], 3: allLeavingPorts[9][3], 4: allLeavingPorts[9][4]}}, 
                                     }}
                               
              
    return listOfTrees 

        





##NONE OF THE FOLLOWING IS MY OWN CODE TO PRESENT THE TREE DIAGRAM --> ref: https://www.programmersought.com/article/93024638825/
#vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv
decisionNode=dict(boxstyle="sawtooth",fc="0.8")
leafNode=dict(boxstyle="round4",fc="0.8")
arrow_args=dict(arrowstyle="<-")

def plotNode(nodeText,centerPt,parentPt,nodeType):
    createPlot.ax1.annotate(nodeText, fontsize=4 ,xy=parentPt,xycoords='axes fraction',xytext=centerPt,textcoords='axes fraction',
                           va='center',ha='center',bbox=nodeType,arrowprops=arrow_args)
    # This parameter is a bit scary. did not understand

    
def getNumLeafs(myTree):
    numLeafs=0
    firstList=list(myTree.keys())
    firstStr=firstList[0]
    secondDict=myTree[firstStr]# Read the value of the key value
    for key in secondDict.keys():# Monitor if there is a dictionary collection
        if type(secondDict[key]).__name__=='dict':
            numLeafs+=getNumLeafs(secondDict[key])
        else:
            numLeafs+=1
    return numLeafs

# depths function
def getTreeDepth(myTree):
    maxDepth=0
    firstList=list(myTree.keys())
    firstStr=firstList[0]
    secondDict=myTree[firstStr]
    for key in secondDict.keys():
        if type(secondDict[key]).__name__=='dict':
            thisDepth=1+getTreeDepth(secondDict[key])
        else: thisDepth=1
        if thisDepth>maxDepth:
            maxDepth=thisDepth
    return maxDepth    


def plotMidText(cntrPt,parentPt,txtString):
    xMid=(parentPt[0]-cntrPt[0])/2.0+cntrPt[0]
    yMid=(parentPt[1]-cntrPt[1])/2.0+cntrPt[1]
    createPlot.ax1.text(xMid,yMid,txtString,fontsize=8)

# define the main functions, plotTree
def plotTree(myTree, parentPt, nodeTxt):#if the first key tells you what feat was split on
    numLeafs = getNumLeafs(myTree)  #this determines the x width of this tree
    depth = getTreeDepth(myTree)
    firstList = list(myTree.keys())
    firstStr=firstList[0] #the text label for this node should be this
    cntrPt = ((plotTree.xOff + (1.0 + float(numLeafs))/2.0/plotTree.totalW), plotTree.yOff)
    plotMidText(cntrPt, parentPt, nodeTxt)
    plotNode(firstStr, cntrPt, parentPt, decisionNode)
    secondDict = myTree[firstStr]
    plotTree.yOff = plotTree.yOff - 1.0/plotTree.totalD
    for key in secondDict.keys():
        if type(secondDict[key]).__name__=='dict':#test to see if the nodes are dictonaires, if not they are leaf nodes   
            plotTree(secondDict[key],cntrPt,str(key))        #recursion
        else:   #it's a leaf node print the leaf node
            plotTree.xOff = plotTree.xOff + 1.0/plotTree.totalW
            plotNode(secondDict[key], (plotTree.xOff, plotTree.yOff), cntrPt, leafNode)
            plotMidText((plotTree.xOff, plotTree.yOff), cntrPt, str(key))
    plotTree.yOff = plotTree.yOff + 1.0/plotTree.totalD
#if you do get a dictonary you know it's a tree, and the first element    

# Perform graphic display
def createPlot(inTree):
    fig=plt.figure(1,facecolor='white')
    fig.clf()
    axprops=dict(xticks=[],yticks=[])
    createPlot.ax1=plt.subplot(111,frameon=False,**axprops)
    plotTree.totalW=float(getNumLeafs(inTree))
    plotTree.totalD=float(getTreeDepth(inTree))
    plotTree.xOff=-0.5/plotTree.totalW
    plotTree.yOff=1.0
    plotTree(inTree,(0.5,1.0),'')
    plt.show()

#^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
##NONE OF THE ABOVE IS MY OWN CODE TO PRESENT THE TREE DIAGRAM --> ref: https://www.programmersought.com/article/93024638825/   

#calls the functions to draw a tree
def drawTree():
        tree = CreateTree()
        getNumLeafs(tree)
        createPlot(tree)    
    
    
class Windows():
    #Holds the windows infomation
    def Window():
        #Window populating
        sailors = SailorRecords.GetIndividualSailors()
        mariners = []
        records = []
        newrecords =[]
    
        #populates the mariners list to be displayed in the ListBox
        for record in sailors:   
            try:
                name = record["name"]
                year_of_birth = record['year_of_birth']
                place_of_birth = record['place_of_birth']
                mariners.append((name, year_of_birth, place_of_birth))
            
            except:
                print("A missing value")
    
    
        #Window2 populating
        sailor_and_rank = SailorsRanks.DisplaySailorAndRank()
        promotionSearch1 = SailorsRanks.PromotionSearch("James Jones", 1836, "New Quay")
        promotionSearch2 = SailorsRanks.PromotionSearch("John Evans", "blk", "Aberayron")
        
        sailorRankArr = []
        promotion1 = []
        promotion2 = []
        #populates the sailors rank list to be displayed in the ListBox
        for doc in sailor_and_rank:
        
            try:
            
                name = doc["_id"]["name"]
                rank = doc["rank"][0]
                
                sailorRankArr.append((name, rank))
          
            except:
                print("A missing value")
        #populates the promotion1 list to be displayed in the ListBox
        for doc in promotionSearch1:
          
            name = doc["_id"]["name"]
            for record in doc["rankJoinDate"]:
                rank = record["this_ship_capacity"]
                joinDate = record["this_ship_joining_date"]
                promotion1.append((name,joinDate,rank))
               
        
        #populates the promotion2 list to be displayed in the ListBox
        for doc in promotionSearch2:
            
            name = doc["_id"]["name"]
            for record in doc["rankJoinDate"]:
                rank = record["this_ship_capacity"]
                joinDate = record["this_ship_joining_date"]
                promotion2.append((name,joinDate,rank))
                
            
        
        
   
        #These are the layouts for each window
        layout = [[
            sg.Frame('Sailors',[[
                sg.Text('Sailors Records'),
                sg.Listbox(values=(mariners), size=(50, 15), key='_sailors_'),
                sg.Button('Show'),
                sg.Listbox(values=(records), size=(100, 15), key='_records_')
                ]])
            ]]
        
        layout2 = [[
            sg.Frame('Sailors Ranks',[[
            sg.Text('Sailors and Ranks'),
            sg.Listbox(values=(sailorRankArr), size=(75, 15), key='_sailors_'),
            sg.Text('Two individuals Rank History'),
            sg.Listbox(values=(promotion1), size=(50, 15), key='_promotion1_'),
            sg.Listbox(values=(promotion2), size=(50, 15), key='_promotion2_')
            ]])
        ]]
        layout3 = [[
            sg.Frame('Section 2, window',[[
            sg.Button('Draw Histogram'),
            sg.Button('Draw Tree'),
            sg.Button('Run prediction'),
            sg.Text('Prediction only shows in the command line')
      
            ]])
        ]]
        
        #Each window is initialised
        window = sg.Window('App', layout)
        window2 = sg.Window('App2', layout2)
        window3 = sg.Window('App3', layout3)
        
        
        while True:
        
            event, values = window.read()
     
            #Event handling for each window
            if event == sg.WIN_CLOSED or event == 'Exit':
                break
            if event == 'Show':
                newrecords = []
                #set = to the value selected by the user when the button "show" is pressed
                mariner_selected = values['_sailors_']  
                showMarinerRecord = SailorRecords.SailorSearch(mariner_selected[0][0],mariner_selected[0][1], mariner_selected[0][2])
                for doc in showMarinerRecord:           
                    for record in doc["sailorData"]:
                        try:
                            last_ship_leaving_date = record["last_ship_leaving_date"]
                            last_ship_name = record["last_ship_name"]
                            this_ship_joining_date = record["this_ship_joining_date"]
                            this_ship_joining_port = record['this_ship_joining_port']
                            this_ship_leaving_date = record["this_ship_leaving_date"]
                            this_ship_leaving_port = record["this_ship_leaving_port"]
                            this_ship_leaving_cause = record["this_ship_leaving_cause"]
                            signed_with_mark = record["signed_with_mark"]
                            newrecords.append((last_ship_leaving_date, last_ship_name,this_ship_joining_date,this_ship_joining_port, this_ship_leaving_date,this_ship_leaving_port,this_ship_leaving_cause, signed_with_mark))
                        except:
                            print("missing records")
                #The records listBox is updated with the selected sailor records
                window.Element('_records_').Update(newrecords)
        event, values = window.Read()
        window.Close()
        while True:
            event, values = window2.read()
        #print(event, values)
            if event == sg.WIN_CLOSED or event == 'Exit':
                break
        event, values = window2.Read()
        window2.Close()
        
        while True:
            event, values = window3.read()
        #print(event, values)
            if event == sg.WIN_CLOSED or event == 'Exit':
                break
            if(event == 'Draw Histogram'):
                NumberOfCrewOnShip.DrawHistogram()
            if(event == 'Draw Tree'):
                drawTree()
            if(event == 'Run prediction'):
                PossibleDestinations().TestingTheData()
                
        event, values = window3.Read()
        window3.Close()
    
if __name__ == "__main__":
    #FixingTheData().ReplaceBlanks()
    window = Windows.Window()

  
    
    
    #SailorsRanks().GetAllRanks()
    #sailor_search = SailorSearch()
    #unique_results = sailor_search.GetIndividualSailors()
    #search_results = sailor_search.SailorSearch("David Jones", "Liverpool", 1855)
    #sailor_search.countResults(unique_results)
    #sailor_search.printResults(search_results)



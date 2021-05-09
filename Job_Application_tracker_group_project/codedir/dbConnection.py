# -*- coding: utf-8 -*-
"""
Created on Tue Mar  9 12:57:47 2021
#Comments:
    Needs to be put into useable functions, example: insertJob, DeleteJob, create_connection, ect...
@author: SMBtr
"""
import sqlite3


def create_connection(db_file):
    global conn
    """ create a database connection to the SQLite database
        specified by the db_file
    :param db_file: database file
    :return: Connection object or None
    """
    try:
        conn = sqlite3.connect(db_file)
    except Error as e:
        print(e)
        conn = None

    return conn


def AddJob(skills, company, jobTitle, description, location, status):
    cur = conn.cursor()
    nextRow = 1
    nextRow = FindNextRow()
    nextRow = nextRow + 1
    print(skills)
    skills.append(nextRow)

    print(len(skills))

    skillquery = "INSERT into Skills(creativity, relationship_building, critical_thinking, problem_solving, public_speaking, Positive_attitude, Complaint_resolution, Patience, Persuasion_and_influencing_skills, Respectfulness, Reliability, Tolerance, Improving_customer_experience, Attention_to_detail, Teamwork_skills, Communication, Collaboration, Accounting, Active_listening, Adaptability, Negotiation, Conflict_resolution, Decision_making, Empathy, Bilingual_customer_support, Management, Organization, skillID) VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)"
    appquery = "INSERT into applications(appID,Company,JobTitle,Description,Location,status, SkillID) VALUES (?,?,?,?,?,?,?)"

    cur.execute(appquery, (nextRow, company, jobTitle, description, location, status, nextRow))
    cur.execute(skillquery, (skills))

    conn.commit()


def FindNextRow():
    query = "SELECT MAX(appID) FROM Applications"
    cur = conn.cursor()
    cur.execute(query)
    results = cur.fetchall()
    # print(results)
    result = results[0][0]
    # print(result)
    if (result == None):
        result = 0

    return result


def DeleteJob(company, job):
    print("company:",company)
    print("job:",job)
    cur = conn.cursor()
    queryApp = "DELETE from Applications WHERE appID = ?"
    querySkill= "DELETE from Skills WHERE SkillID = ?"
    queryFindSkills = "SELECT appID from Applications Where company = ? AND jobTitle = ?"
    
    cur.execute(queryFindSkills, (company, job))
    Results = cur.fetchall()
    givenAppID = Results[0][0]
    givenAppID = str(givenAppID)
    
    print(givenAppID)
    
    cur.execute(queryApp, givenAppID)
    cur.execute(querySkill, givenAppID)
    conn.commit()


def ViewApplication(givenappID):
    cur = conn.cursor()
    queryApp = "SELECT company,jobTitle,description,Location,Status from Applications WHERE appID = ?"
    cur.execute(queryApp, givenappID)
    appResults = cur.fetchall()
    conn.commit()
    return appResults


def ViewSkills(givenappID):
    cur = conn.cursor()
    querySkill = "SELECT * from Skills WHERE SkillID = ?"
    cur.execute(querySkill, givenappID)
    skillResults = cur.fetchall()
    conn.commit()
    return skillResults
# Simular to delete but with a query that gets the relative information from an ID instead of deleting it.

def searchByLocation(location):
    cur = conn.cursor()
    queryLocation = f"SELECT * from Applications WHERE Location LIKE ?"
    location = (location,)
    cur.execute(queryLocation,location)
    appByLocation = cur.fetchall()
    conn.commit()
    #print(appByLocation)
    return appByLocation
# skill_to_add = [1,1,1,1,0]
# AddJob(create_connection("applications.db"),skill_to_add,"Morrisons", "Assistant", "Loooks like a mad good job butty")
# Tests
# create_connection("applications.db")

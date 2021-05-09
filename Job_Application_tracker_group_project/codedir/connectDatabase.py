import sqlite3

def profileToDatabase(name1, address1, date_of_birth1, country1, linkedIn1, email1, phone_number1, skillsChecked1, education1, experience1):
    #place the following line at the end of the save_profile function in the profile.py
    #connectDatabase.profileToDatabase(name, address, date_of_birth, country, linkedIn, email, phone_number, skillsChecked, education, experience)

    conn = sqlite3.connect('db.sqlite')
    cur = conn.cursor()

    cur.execute("CREATE TABLE IF NOT EXISTS profile (name text, address text, dob text, country text, linkedIn text, email text, phoneNo text, skillsChecked text, education text, experience text);")
    cur.execute("INSERT INTO profile (name , address , dob , country , linkedIn , email , phoneNo , skillsChecked, education , experience ) VALUES (?,?,?,?,?,?,?,?,?,?);", (name1, address1, date_of_birth1, country1,linkedIn1, email1, phone_number1, skillsChecked1, education1, experience1))

    conn.commit()
    cur.close()
    conn.close()

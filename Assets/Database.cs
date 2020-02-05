using System.Data;
using Mono.Data.Sqlite;
using UnityEngine;
using Venus.Utilities;




public class Database : Singleton<Database>
{

    //script strings stores the sql script
    private string script = "";
    private string script2 = "";
    private string intOrVarchar = "";

    //db filepath
    private string dbPath;

    private string insert;


    private int arraySize = 0;
    //ArrayShrinker must be arraySize - 2
    private int arrayShrinker = 0;

    string[] stringTypeOfArray;
    int[] intTypeOfArray;

    string tableArraySizeId;
    protected override void Awake()
    {
        base.Awake();

        //setting Database root
        dbPath = "URI=file:" + Application.persistentDataPath + "/GameData.db";


        Debug.Log(Application.persistentDataPath);

    }

    //Create schema creates table of wanted name. It also puts wanted amount  of columns which are named slots. It also creates necessary tables for saving critical values/items. Those are used for fetching game information 
    public void CreateSchema(string nameOfTable, int arraySize, string intOrVarchar, string tableArraySizeId)
    {
        this.tableArraySizeId = tableArraySizeId;
        this.arraySize = arraySize;
        
        /*if(arraySize <= 2)
        {
            arrayShrinker += (this.arraySize - 1);
        }
        else
        {
            arrayShrinker += (this.arraySize - 2);
        }*/
        
        this.intOrVarchar += intOrVarchar;
        //creating connection to database
        using (var conn = new SqliteConnection(dbPath))
        {
            //Opening Database
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {


                //adding neccessary text/script to the string "script" which creates later when executed slot columns and column for id
                cmd.CommandType = CommandType.Text;

                //insert = "'id' INT PRIMARY KEY, ";
                // script += insert;
                Debug.Log(arraySize + "size of array");
                Debug.Log(arrayShrinker + " arrayshrinker");
                for (int i = 0; i < arraySize; i++)
                {
                    Debug.Log("inside for loop");
                   
                    if(i == 0)
                    {
                        insert = "'slot" + i + "' "+intOrVarchar+"";
                        script += insert;
                    }
                    else
                    {
                        insert = ", 'slot" + i + "' "+intOrVarchar+"";
                        script += insert;
                    }

                }
                Debug.Log(script);

                //Inserting sql command to commandText which makes table 

                cmd.CommandText = "DROP TABLE IF EXISTS " + nameOfTable + ";" +
                                      "CREATE TABLE IF NOT EXISTS '" + nameOfTable + "' ( " +
                                      "" + script + "" +
                                      ");" +
                                      "DROP TABLE IF EXISTS '"+tableArraySizeId+"';" +
                                      "CREATE TABLE IF NOT EXISTS '"+tableArraySizeId+"' ( " +
                                      " 'size' INT NOT NULL" +
                                      ");" +
                                      "DROP TABLE IF EXISTS 'VarType';" +
                                      "CREATE TABLE IF NOT EXISTS 'VarType' ( " +
                                      " 'type' VARCHAR NOT NULL" +
                                      ");";


                //cmd.Excmd.ExecuteNonQuery executes the script.
                var result = cmd.ExecuteNonQuery();
                Debug.Log("tablet luotiin");

                //Erasing text from script string so it is possible to use somewhere else with new text
                script = "";
                intOrVarchar = "";
            }
        }
    }

    //InsertArray stores given slots array to the given table.
    public void InsertArray(string[] items, string nameOfTable)
    {
        //creating connection to database
        using (var conn = new SqliteConnection(dbPath))
        {
            //Opening Database
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                //Adding needed script/text to the variable script

                for (int i = 0; i < arraySize; i++)
                {
                    if(i == 0)
                    {
                        insert = "slot" + i + "";
                        script += insert;
                    }
                    else
                    {
                        insert = ", slot" + i + "";
                        script += insert;
                    }
                    
                }

                //Adding needed script/text to the variable script2. Inserting requires value for assigning value to parameter.
                Debug.Log(script);
                for (int z = 0; z < arraySize; z++)
                {
                    if(z == 0)
                    {
                        insert = "@Slot" + z + "";
                        script2 += insert;
                    }
                    else
                    {
                        insert = ", @Slot" + z + " ";
                        script2 += insert;
                    }
                   
                }
                Debug.Log(script2);

                //Inserting Value commands to commandText which makes inserts values to tables. Array size stores the size of array

                cmd.CommandText = "INSERT INTO " + nameOfTable + " (" + script + ")" +
                                      "VALUES (" + script2 + ");" +
                                  "INSERT INTO '"+tableArraySizeId+"' (size) " +
                                      "VALUES (@Size);" +
                                  "INSERT INTO 'VarType' (type) " +
                                      "VALUES (@Type);";
                //slot0, slot1, slot2, slot3, slot4, slot5, slot6, slot7, slot8, slot9, slot10, slot11
                //@Slot0, @Slot1, @Slot2, @Slot3, @Slot4, @Slot5, @Slot6, @Slot7, @Slot8, @Slot9, @Slot10, @Slot11

                //Adding needed parameters for script
                for (int z = 0; z < arraySize; z++)
                {
                    cmd.Parameters.Add(new SqliteParameter
                    {
                        ParameterName = "Slot" + z + "",
                        Value = items[z]

                    });
                }

                //Parameter for storing the size of array
                cmd.Parameters.Add(new SqliteParameter
                {
                    ParameterName = "Size",
                    Value = arraySize

                });

                //Parameter for storing the type of array
                cmd.Parameters.Add(new SqliteParameter
                {
                    ParameterName = "Type",
                    Value = intOrVarchar

                });

                //executing Insert
                var result = cmd.ExecuteNonQuery();

                Debug.Log("" + nameOfTable + ": " + result);


                Debug.Log("whole array was added");
                script = "";
                script2 = "";
            }
        }
    }

    //function for getting array out of db. limit specifies how many rows we want from the table. nameoftable specifies which table we are looking for
    public void FindArray(int limit, string nameOfTable, string tableArraySizeId)
    {
        //creating connection to database
        using (var conn = new SqliteConnection(dbPath))
        {
            //Opening Database
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                //search script which is assigned to commanText

                //gets size of saved array
                Debug.Log("menee");
                GetSizeOfArray(tableArraySizeId);
                
                cmd.CommandType = CommandType.Text;

                arrayShrinker = arraySize - 1;
               
        
                
                //For loop creates script for getting stuff out of database. 

                for (int i = 0; i < arraySize; i++)
                {
                    
                   
                    if (i < arrayShrinker)
                    {
                        insert = "slot" + i + ", ";
                        script += insert;
                    }
                    else
                    {
                        insert = "slot" + i + " ";
                        script += insert;
                    }
                }
                Debug.Log(script + "Found array script");
                cmd.CommandText = "SELECT "+ script +" FROM "+nameOfTable+" DESC LIMIT @Count;";
           
                //slot0, slot1, slot2, slot3, slot4, slot5, slot6, slot7, slot8, slot9, slot10, slot11

                //Adding line limiter parameter
                cmd.Parameters.Add(new SqliteParameter
                {
                    ParameterName = "Count",
                    Value = limit
                });

                Debug.Log("" + nameOfTable + " (begin)");

                //executes script   
                var reader = cmd.ExecuteReader();


                //gets variable type of saved array
                //GetVarType();

                //Check if database has int or string variables
                
                    string[] readerArray = new string[arraySize];

                    //while loop reads the table and ads values to readerArray

                    //for loop for displaying fetched array
                   
                    

                    while (reader.Read())
                    {
                        string test = reader.GetString(0);
                        for (int i = 0; i < arraySize; i++)
                        {
                            readerArray[i] = reader.GetString(i);
                        }
                    }      
                    
                    this.stringTypeOfArray = readerArray;
                   
                    for (int z = 0; z < arraySize; z++)
                    {
                        Debug.Log(readerArray[z]);
                    }
                

               /* else if (intOrVarchar == "INT")
                {
                    int[] readerArray = new int[arraySize];

                    while (reader.Read())
                    {

                        //var id = reader.GetInt32(0);
                        for (int i = 0; i < arraySize; i++)
                        {
                            readerArray[i] = reader.GetInt32(i);
                        }

                    }
                    this.intTypeOfArray = readerArray;
                    for (int z = 0; z < arraySize; z++)
                    {
                        Debug.Log(readerArray[z]);
                    }
                } */

                Debug.Log("" + nameOfTable + " (end)");

                script = "";
            }
        }
    }

    public void GetSizeOfArray(string tableArraySizeId)
    {

        //creating connection to database
        using (var conn = new SqliteConnection(dbPath))
        {
            //Opening Database
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;

                cmd.CommandText = "SELECT size FROM "+tableArraySizeId+";";

                var reader1 = cmd.ExecuteReader();
                while (reader1.Read())
                {
                    this.arraySize = reader1.GetInt32(0);
                    Debug.Log(arraySize + " :size of array");
                }
                reader1.Close();

            }

        }
    }

    /*public void GetVarType()
    {
        //creating connection to database
        using (var conn = new SqliteConnection(dbPath))
        {
            //Opening Database
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;

                cmd.CommandText = "SELECT type FROM VarType;";

                var reader2 = cmd.ExecuteReader();
                while (reader2.Read())
                {
                    intOrVarchar = reader2.GetString(0);
                    Debug.Log(intOrVarchar + " :type of array");
                }
                reader2.Close();
            }

        }
    }*/

    public string[] GetStringArray()
    {
        
        return this.stringTypeOfArray;
    }

    public int[] GetIntArray()
    {
        return this.intTypeOfArray;
    }


    //Insert player gets two column names and types. 
    


    
}




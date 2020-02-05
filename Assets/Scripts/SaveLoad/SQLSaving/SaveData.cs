using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Venus.Utilities;

public class SaveData : MonoBehaviour {

    private int arrayLength = 0;

    string[] test;

    public bool Save(string[] arrayToSave, string tableName, string tableArraySizeId)
    {
        
        //Creates table where string is name of the table.
        //Given int value creates table big enough for the array. If Array length is 5 this int value must be 5.

        Database.S_INSTANCE.CreateSchema(tableName, arrayToSave.Length, "VARCHAR", tableArraySizeId);
        //Database.S_INSTANCE.CreateSchema("Final3", 19);

        //Inserts slots array to table of given name.
        
        Database.S_INSTANCE.InsertArray(arrayToSave, tableName);
        PauseMenu pauseMenu = new PauseMenu();
        return true;

    }
    public string[] Load(string tableName, string tableArraySizeId)
    {
        //Finds array from database. first int = limit(how many rows u want from table) and table name.
        Database.S_INSTANCE.FindArray(1, tableName, tableArraySizeId);
        //after array found function Get "variable" Array returns wanted array
        
        if (Database.S_INSTANCE.GetStringArray() != null)
        {
            return Database.S_INSTANCE.GetStringArray();
        }
        else
            return null;
        
        
    }


}

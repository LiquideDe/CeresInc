using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.Collections.Generic;
using System.Globalization;

public class DataBase
{
    string dbConnectionString;
    main mainClass;

    public DataBase(main main)
    {
        mainClass = main;
    }

    public void CreateDB(string path)
    {
        dbConnectionString = "URI=file:" + path;
        IDbConnection dbConnection = new SqliteConnection(dbConnectionString);
        dbConnection.Open();
        IDbCommand dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = "CREATE TABLE Prices (Day INTEGER PRIMARY KEY NOT NULL, H FLOAT NOT NULL, He FLOAT NOT NULL, Li FLOAT NOT NULL, Be FLOAT NOT NULL, C FLOAT NOT NULL, " +
            "N FLOAT NOT NULL, Mg FLOAT NOT NULL, Al FLOAT NOT NULL, Si FLOAT NOT NULL, Ne FLOAT NOT NULL, Ti FLOAT NOT NULL, Cr FLOAT NOT NULL, Fe FLOAT NOT NULL, Ni FLOAT NOT NULL, Cu FLOAT NOT NULL, " +
            "Xe FLOAT NOT NULL, Ir FLOAT NOT NULL, Pt FLOAT NOT NULL, Po FLOAT NOT NULL, Th FLOAT NOT NULL, U FLOAT NOT NULL, Pu FLOAT NOT NULL, Ea FLOAT NOT NULL); ";
        dbCommand.ExecuteNonQuery();
        
        dbCommand.CommandText = "CREATE TABLE Production (Day INTEGER PRIMARY KEY NOT NULL, H FLOAT NOT NULL, He FLOAT NOT NULL, Li FLOAT NOT NULL, Be FLOAT NOT NULL, C FLOAT NOT NULL, " +
            "N FLOAT NOT NULL, Mg FLOAT NOT NULL, Al FLOAT NOT NULL, Si FLOAT NOT NULL, Ne FLOAT NOT NULL, Ti FLOAT NOT NULL, Cr FLOAT NOT NULL, Fe FLOAT NOT NULL, Ni FLOAT NOT NULL, Cu FLOAT NOT NULL, " +
            "Xe FLOAT NOT NULL, Ir FLOAT NOT NULL, Pt FLOAT NOT NULL, Po FLOAT NOT NULL, Th FLOAT NOT NULL, U FLOAT NOT NULL, Pu FLOAT NOT NULL, Ea FLOAT NOT NULL); ";
        dbCommand.ExecuteNonQuery();
        dbCommand.CommandText = "CREATE TABLE AmountOnEarth (Day INTEGER PRIMARY KEY NOT NULL, H FLOAT NOT NULL, He FLOAT NOT NULL, Li FLOAT NOT NULL, Be FLOAT NOT NULL, C FLOAT NOT NULL, " +
            "N FLOAT NOT NULL, Mg FLOAT NOT NULL, Al FLOAT NOT NULL, Si FLOAT NOT NULL, Ne FLOAT NOT NULL, Ti FLOAT NOT NULL, Cr FLOAT NOT NULL, Fe FLOAT NOT NULL, Ni FLOAT NOT NULL, Cu FLOAT NOT NULL, " +
            "Xe FLOAT NOT NULL, Ir FLOAT NOT NULL, Pt FLOAT NOT NULL, Po FLOAT NOT NULL, Th FLOAT NOT NULL, U FLOAT NOT NULL, Pu FLOAT NOT NULL, Ea FLOAT NOT NULL); ";
        dbCommand.ExecuteNonQuery();
        dbCommand.CommandText = "CREATE TABLE AmountOnCeres (Day INTEGER PRIMARY KEY NOT NULL, H FLOAT NOT NULL, He FLOAT NOT NULL, Li FLOAT NOT NULL, Be FLOAT NOT NULL, C FLOAT NOT NULL, " +
            "N FLOAT NOT NULL, Mg FLOAT NOT NULL, Al FLOAT NOT NULL, Si FLOAT NOT NULL, Ne FLOAT NOT NULL, Ti FLOAT NOT NULL, Cr FLOAT NOT NULL, Fe FLOAT NOT NULL, Ni FLOAT NOT NULL, Cu FLOAT NOT NULL, " +
            "Xe FLOAT NOT NULL, Ir FLOAT NOT NULL, Pt FLOAT NOT NULL, Po FLOAT NOT NULL, Th FLOAT NOT NULL, U FLOAT NOT NULL, Pu FLOAT NOT NULL, Ea FLOAT NOT NULL); ";
        dbCommand.ExecuteNonQuery();
        dbCommand.CommandText = "CREATE TABLE Consumption (Day INTEGER PRIMARY KEY NOT NULL, H FLOAT NOT NULL, He FLOAT NOT NULL, Li FLOAT NOT NULL, Be FLOAT NOT NULL, C FLOAT NOT NULL, " +
            "N FLOAT NOT NULL, Mg FLOAT NOT NULL, Al FLOAT NOT NULL, Si FLOAT NOT NULL, Ne FLOAT NOT NULL, Ti FLOAT NOT NULL, Cr FLOAT NOT NULL, Fe FLOAT NOT NULL, Ni FLOAT NOT NULL, Cu FLOAT NOT NULL, " +
            "Xe FLOAT NOT NULL, Ir FLOAT NOT NULL, Pt FLOAT NOT NULL, Po FLOAT NOT NULL, Th FLOAT NOT NULL, U FLOAT NOT NULL, Pu FLOAT NOT NULL, Ea FLOAT NOT NULL); ";
        dbCommand.ExecuteNonQuery();
        dbCommand.CommandText = "CREATE TABLE LvlIndustry (Day INTEGER PRIMARY KEY NOT NULL, Energy FLOAT NOT NULL, Heavy FLOAT NOT NULL, Light FLOAT NOT NULL, High FLOAT NOT NULL, Space FLOAT NOT NULL);";
        dbCommand.ExecuteNonQuery();
        dbCommand.CommandText = "CREATE TABLE LvlConsumptionIndustry (Day INTEGER PRIMARY KEY NOT NULL, Energy FLOAT NOT NULL, Heavy FLOAT NOT NULL, Light FLOAT NOT NULL, High FLOAT NOT NULL, Space FLOAT NOT NULL);";
        dbCommand.ExecuteNonQuery();
        dbCommand.CommandText = "CREATE TABLE Population (Day INTEGER PRIMARY KEY NOT NULL, PopulationOnEarth INTEGER NOT NULL);";
        dbCommand.ExecuteNonQuery();
        dbCommand.CommandText = "CREATE TABLE GDP (Day INTEGER PRIMARY KEY NOT NULL, GdpEarth FLOAT NOT NULL);";
        dbCommand.ExecuteNonQuery();
        dbCommand.CommandText = GenerateTextForShareTable();
        dbCommand.ExecuteNonQuery();
        dbConnection.Close();
    }

    private string GenerateTextForShareTable()
    {
        string sql = "CREATE TABLE Shareprice (Day INTEGER PRIMARY KEY NOT NULL";
        for(int i=0; i< 5; i++)
        {
            sql += $", '{mainClass.Earth.GetEarthCorp(i).CorpName}' FLOAT NOT NULL";
        }
        for(int i = 0; i < mainClass.Corporates.CountMiningCorp(); i++)
        {
            sql += $", '{mainClass.Corporates.GetMiningCorporates(i).CorpName}' FLOAT NOT NULL";
        }
        sql += $");";
        return sql;
    }

    public void Save(string command)
    {
        IDbConnection dbConnection = new SqliteConnection(dbConnectionString);
        dbConnection.Open();
        IDbCommand dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = command;
        dbCommand.ExecuteNonQuery();
        dbConnection.Close();
    }

    public SqliteDataReader Load(string command, string path = "")
    {
        if(path !="")
            dbConnectionString = "URI=file:" + path + "Ceres.db";
        SqliteConnection dbConnection = new SqliteConnection(dbConnectionString);
        dbConnection.Open();
        SqliteCommand sqlite = new SqliteCommand(command, dbConnection);
        SqliteDataReader reader = sqlite.ExecuteReader();
        return reader;
    }

    public void GetTableFromCommand(string command, List<List<float>> table)
    {
        table.Clear();
        SqliteConnection dbConnection = new SqliteConnection(dbConnectionString);
        dbConnection.Open();
        SqliteCommand sqlite = new SqliteCommand(command, dbConnection);
        SqliteDataReader reader = sqlite.ExecuteReader();
        if (reader.HasRows)
        {
            while (reader.Read())
            {
                table.Add(new List<float> { reader.GetFloat(0), reader.GetFloat(1) });
            }
        }
        dbConnection.Close();
    }

    public void RestorationDbPath(string path)
    {
        dbConnectionString = "URI=file:" + path + "Ceres.db";
    }

    public void SaveInSql()
    {
        Save(GenerateCommandToSavePrice());
        Save(GenerateCommandToSaveAmountOnEarth());
        Save(GenerateCommandToSaveAmountOnCeres());
        Save(GenerateCommandToSaveProduction());
        Save(GenerateCommandToSaveProm());
        Save(GenerateCommandToSaveConsumptionProm());
        Save(GenerateCommandToSaveConsumption());
        Save("INSERT OR REPLACE INTO Population (Day, PopulationOnEarth) VALUES (" + (int)mainClass.CeresTime + ", " + mainClass.Earth.Population + ");");
        Save("INSERT OR REPLACE INTO GDP (Day, GdpEarth) VALUES (" + (int)mainClass.CeresTime + ", " + mainClass.Earth.GDP + ");");
        Save(GenerateCommandToSavePriceShare()); 
    }

    private string GenerateCommandToSavePrice()
    {
        string command = "INSERT OR REPLACE INTO Prices (Day";
        for (int i = 0; i < mainClass.Materials.MaterialsCount(); i++)
        {
            command += ", " + mainClass.Materials.GetMaterial(i).ElementName;
            
        }
        command += $") VALUES ({(int)mainClass.CeresTime}";

        for (int i = 0; i < 23; i++)
        {
            command += ", " + mainClass.Materials.GetMaterial(i).Price.ToString(CultureInfo.InvariantCulture);
        }
        command += $");";
        //Debug.Log(command);
        return command;
    }

    string GenerateCommandToSaveAmountOnEarth()
    {
        string command = "INSERT OR REPLACE INTO AmountOnEarth (Day";
        for (int i = 0; i < mainClass.Materials.MaterialsCount(); i++)
        {
            command += ", " + mainClass.Materials.GetMaterial(i).ElementName;
        }
        command += $") VALUES ({(int)mainClass.CeresTime}";

        for (int i = 0; i < mainClass.Materials.MaterialsCount(); i++)
        {
            command += ", " + mainClass.Earth.GetAmountRes(i).ToString(CultureInfo.InvariantCulture);
        }
        command += $");";
        return command;
    }

    string GenerateCommandToSaveAmountOnCeres()
    {
        string command = "INSERT OR REPLACE INTO AmountOnCeres (Day";
        for (int i = 0; i < mainClass.Materials.MaterialsCount(); i++)
        {
            command += ", " + mainClass.Materials.GetMaterial(i).ElementName;
        }
        command += $") VALUES ({(int)mainClass.CeresTime}";

        for (int i = 0; i < mainClass.Materials.MaterialsCount(); i++)
        {
            command += ", " + mainClass.Player.GetAmountRes(i).ToString(CultureInfo.InvariantCulture);
        }
        command += $");";
        return command;
    }

    string GenerateCommandToSaveProduction()
    {
        string command = "INSERT OR REPLACE INTO Production (Day";
        for (int i = 0; i < mainClass.Materials.MaterialsCount(); i++)
        {
            command += ", " + mainClass.Materials.GetMaterial(i).ElementName;
        }
        command += $") VALUES ({(int)mainClass.CeresTime}";

        for (int i = 0; i < mainClass.Materials.MaterialsCount(); i++)
        {
            command += ", " + mainClass.Materials.GetMaterial(i).Production.ToString(CultureInfo.InvariantCulture);
        }
        command += $");";
        return command;
    }

    string GenerateCommandToSaveProm()
    {
        string command = "INSERT OR REPLACE INTO LvlIndustry (Day";
        command += ", Energy, Heavy, Light, High, Space) VALUES (" + (int)mainClass.CeresTime;
        for (int i = 0; i < 5; i++)
        {
            command += ", " + mainClass.Earth.GetProm(i).ToString(CultureInfo.InvariantCulture);
        }
        command += $");";
        return command;
    }

    string GenerateCommandToSaveConsumptionProm()
    {
        string command = "INSERT OR REPLACE INTO LvlConsumptionIndustry (Day";
        command += ", Energy, Heavy, Light, High, Space) VALUES (" + (int)mainClass.CeresTime;
        for (int i = 0; i < 5; i++)
        {
            command += ", " + mainClass.Earth.GetConsumptionIndustry(i).ToString(CultureInfo.InvariantCulture);
        }
        command += $");";
        return command;
    }

    string GenerateCommandToSaveConsumption()
    {
        string command = "INSERT OR REPLACE INTO Consumption (Day";
        for (int i = 0; i < mainClass.Materials.MaterialsCount(); i++)
        {
            command += ", " + mainClass.Materials.GetMaterial(i).ElementName;
        }
        command += $") VALUES ({(int)mainClass.CeresTime}";

        for (int i = 0; i < mainClass.Materials.MaterialsCount(); i++)
        {
            command += ", " + mainClass.Materials.GetMaterial(i).Consumption.ToString(CultureInfo.InvariantCulture);
        }
        command += $");";
        return command;
    }

    string GenerateCommandToSavePriceShare()
    {
        string command = $"INSERT OR REPLACE INTO Shareprice (Day {GetNamesIncs()}) VALUES";
        command += $"({(int)mainClass.CeresTime}";

        for (int i=0;i< mainClass.CorpPanel.CountCorp(); i++)
        {
            command += $",{mainClass.CorpPanel.GetPriceShare(i).ToString(CultureInfo.InvariantCulture)}";
        }
        command += $");";
        return command;
    }

    private string GetNamesIncs()
    {
        string sql = "";
        for (int i = 0; i < 5; i++)
        {
            sql += $", '{mainClass.Earth.GetEarthCorp(i).CorpName}'";
        }
        for (int i = 0; i < mainClass.Corporates.CountMiningCorp(); i++)
        {
            sql += $", '{mainClass.Corporates.GetMiningCorporates(i).CorpName}' ";
        }        

        return sql;
    }
}

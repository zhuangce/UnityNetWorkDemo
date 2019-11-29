using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class JsonManager : MonoSingleton<JsonManager> {


    string path;


    private void Awake()
    {
        path = Application.streamingAssetsPath;
    }
    void Start () {
       
        // CorrctJsonForm();
       //  ReadFromJson_Jsonutility();
       // WriteAJsonFile();
    }
    void WriteAJsonFile()
    {
        for (int i = 0; i < 10; i++)
        {
            JsonData js = new JsonData();
            js.id = i.ToString();
            FlightsInfo._Instance.jsondata.Add(js);
           
        }
        string str =   JsonUtility.ToJson(FlightsInfo._Instance);

         File.WriteAllText(path + "/newjsonFile.json",str);
    }

    public void InitalJsonData()
    {
        ReadFromJsonData_FlightInfo();
        ReadFromJsonData_AirLineFlightInfo();
        ReadFromJsonData_AirLines();

        //for (int i = 0; i < AirLineFlightInfo._Instance.airlineflightData.Count; i++)
        //{
        //   Debug.Log( AirLineFlightInfo._Instance.airlineflightData[i].id);
        //}

    }

    private void ReadFromJsonData_FlightInfo() 
    {
       
        var data = File.ReadAllText(path+ "/jsonFile_Copy.json");

        FlightsInfo._Instance  = JsonUtility.FromJson<FlightsInfo>(data as string);

       
    }
    private void ReadFromJsonData_AirLineFlightInfo()
    {

        var data = File.ReadAllText(path + "/AirLineFlight.json");

        AirLineFlightInfo._Instance = JsonUtility.FromJson<AirLineFlightInfo>(data as string);

        for (int i = 0; i < AirLineFlightInfo._Instance.airlineflightData.Count; i++)
        {
            AirLineFlightInfo._Instance.airlineflightDic.Add(AirLineFlightInfo._Instance.airlineflightData[i].id, AirLineFlightInfo._Instance.airlineflightData[i]);
        }
        AirLineFlightInfo._Instance.airlineflightData.Clear();
    }
    private void ReadFromJsonData_AirLines()
    {

        var data = File.ReadAllText(path + "/airlines.json");

        AirLineInfo._Instance = JsonUtility.FromJson<AirLineInfo>(data as string);

        for (int i = 0; i < AirLineInfo._Instance.airlineData.Count; i++)
        {
            AirLineInfo._Instance.airlineDic.Add(AirLineInfo._Instance.airlineData[i].ICAO, AirLineInfo._Instance.airlineData[i]);
        }
        AirLineInfo._Instance.airlineData.Clear();
    }
    //void CorrctJsonForm()
    //{

    //    string[] datas = File.ReadAllLines(path + "/jsonFile.json");
    //    if (File.Exists(path + "/jsonFile_Copy.json"))
    //    {
    //        File.Delete(path + "/jsonFile_Copy.json");
    //        Debug.Log("delete copy josn file");
    //    }
    //     StreamWriter sw = new StreamWriter(path + "/jsonFile_Copy.json");
    //    sw.WriteLine("{  \"jsondata\" : [  ");
    //    for (int i = 0; i < datas.Length; i++)
    //    {
    //        if (i == datas.Length-1)
    //        {
    //            sw.WriteLine(datas[i]);
    //        }
    //        else
    //        {
    //            sw.WriteLine(datas[i] + ",");

    //        }

    //    }
    //    sw.WriteLine("   ]  }");

    //    sw.Flush();
    //    sw.Close();
    //    sw.Dispose();
    //   // Debug.Log("");
    //}
    // Update is called once per frame
    void Update () {
		
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class JsonData  {

    public string id;
    //经度
    public float lat;

    //纬度
    public float lon;
    public int heading;
    public float alt;
    public int info1;
    public int info2;
    public int time;
    

    public string flightid;

   
}
[Serializable]
public class FlightsInfo:mSingleton<FlightsInfo> {

    public List< JsonData> jsondata = new List<JsonData>();
   
   // public List<AirLineData> airlinedata = new List<AirLineData>();
   // public List<AirPortData> airportdata = new List<AirPortData>();
}

[Serializable]
public class AirLineFlightInfo : mSingleton<AirLineFlightInfo>
{
    public List<AirLineFlightData> airlineflightData = new List<AirLineFlightData>();

    public Dictionary<string, AirLineFlightData> airlineflightDic = new Dictionary<string, AirLineFlightData>();
}

[Serializable]
public class AirLineInfo : mSingleton<AirLineInfo>
{
    public List<AirLineData> airlineData = new List<AirLineData>();

    public Dictionary<string, AirLineData> airlineDic = new Dictionary<string, AirLineData>();
}


[Serializable]
public class   AirLineData
{
    public string Name;
                  
    public string Code;
                  
    public string ICAO;
}

[Serializable]
public class AirPortData
{
    public string name;

    public string nameforshort;

    public string icaoname;

    public float latitude;
    public float lontitude;

    public string country;

    public float altitude;

}

[Serializable]
public class AirLineFlightData
{
    public string id;

    public string ICAOAddress;

    public string item1;

    public string airCraftType;

    public string registration;

    public string startAirport;

    public string endAirport;

    public string airLineFlightNumberShort;

    public string item2;

    public string item3;

    public string airLineFlightNumber;

    public string item4;

    public string airLineForShort;
   
}





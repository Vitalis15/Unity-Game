using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BluePrint 
{
   
    public string ItemName;
    public string Req1;
    public string Req2;
    public string Req3;

    public int Req1amount;
    public int Req2amount;
    public int Req3amount;

    public int numOfRequirements;
    public int numOfItemsToProduce;

    public BluePrint(string name, int producedItem, int reqNum, string R1, int R1num,string R2, int R2num, string R3, int R3num)
    {
        ItemName = name;
        numOfRequirements = reqNum;

        Req1 = R1;
        Req2 = R2;
        Req3 = R3;

        numOfItemsToProduce = producedItem;

        Req1amount = R1num;        
        Req2amount = R2num;
        Req3amount = R3num;
    }

}

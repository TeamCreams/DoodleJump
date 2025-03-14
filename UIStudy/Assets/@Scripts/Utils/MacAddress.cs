using UnityEngine;
using System.Net.NetworkInformation;
public static class MacAddress
{
    public static string Get()
    {
        string macAddress = "";
        int count = 0;
        foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
        {
            if(5 < count)
            {
                break;
            }
            if (nic.OperationalStatus == OperationalStatus.Up)
            {
                string tempMacAddress = nic.GetPhysicalAddress().ToString();
                if (!string.IsNullOrEmpty(tempMacAddress))
                {
                    macAddress += tempMacAddress;
                    //Debug.Log($"macAddress{count} : {macAddress}");
                    count++;
                }
            }
        }
        return macAddress;
    }
}

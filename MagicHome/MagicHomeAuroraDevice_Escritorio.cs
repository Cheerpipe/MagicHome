using Aurora;
using Aurora.Devices;
using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Diagnostics;
using MagicHomeAurora;

public class MagicHomeAuroraDevice_Escritorio
{
    public string devicename = "MagicHome - Tira escritorio";
    public string ipAddress = "192.168.1.33";	
    public bool enabled = true;
    private Color device_color = Color.Black;
    private MagicHomeAuroraDevice _magicHomeAurora;
    public bool Initialize()
    {
        try
        {
			_magicHomeAurora = new MagicHomeAuroraDevice();
			_magicHomeAurora.Init(ipAddress);
            return true;
        }
        catch(Exception exc)
        {
            return false;
        }
    }
    
    public void Reset()
    {
		Shutdown();
		Initialize();		
    }
    
    public void Shutdown()
    {
		_magicHomeAurora.Stop();
    }
    
    public bool UpdateDevice(Dictionary<DeviceKeys, Color> keyColors, bool forced)
    {
        try
        {
            foreach (KeyValuePair<DeviceKeys, Color> key in keyColors)
            {
                //Iterate over each key and color and send them to your device
				if(key.Key == DeviceKeys.G3)
                {
                    //For example if we're basing our device color on Peripheral colors
                    SendColorToDevice(key.Value, forced);
                }
            }
            
            return true;
        }
        catch(Exception exc)
        {
            return false;
        }
    }

    //Custom method to send the color to the device
    private void SendColorToDevice(Color color, bool forced)
    {
        //Check if device's current color is the same, no need to update if they are the same
        if (!device_color.Equals(color) || forced)
        {
			//SendArgs(new string[] { string.Format("--sc:{0}:{1}:{2}", color.R.ToString(), color.G.ToString(), color.B.ToString()) });
			
			_magicHomeAurora = new MagicHomeAuroraDevice();
			_magicHomeAurora.Init(ipAddress);
			_magicHomeAurora.SetColor(color);
			_magicHomeAurora.Stop();
			device_color=color;			
			Global.logger.LogLine(string.Format("[C# Script] Sent a color, {0} to MagicHomeLive", color));
        }
	}
}

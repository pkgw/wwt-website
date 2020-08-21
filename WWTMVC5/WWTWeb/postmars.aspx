<%@ Page Language="C#" ContentType="text/plain" %>
<%@ Import Namespace="System.Drawing" %>
<%@ Import Namespace="System.Drawing.Text" %>
<%@ Import Namespace="System.Drawing.Imaging" %>
<%@ Import Namespace="System.Drawing.Drawing2D" %>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="WWTWebservices" %>
<%
	Response.Write("OK");	
return;

        string query = Request.Params["Q"];
        string[] values = query.Split(',');   
        int level = Convert.ToInt32(values[0]);
        int tileX = Convert.ToInt32(values[1]);
        int tileY = Convert.ToInt32(values[2]);
	string dataset = values[3];
        string file = "mars";
	string wwtTilesDir = ConfigurationManager.AppSettings["WWTTilesDir"];
  	string DSSTileCache = WWTUtil.GetCurrentConfigShare("DSSTileCache", true);

        string filename = String.Format( DSSTileCache  + "\\wwtcache\\mars\\{3}\\{0}\\{2}\\{1}_{2}.png", level, tileX, tileY, dataset);
        string path = String.Format(DSSTileCache + "\\wwtcache\\mars\\{3}\\{0}\\{2}", level, tileX, tileY, dataset);


	if (!Directory.Exists(path))
        {
        	Directory.CreateDirectory(path);
        }
	//Response.Write("len = "+Request.ContentLength.ToString());
	if (Request.ContentLength > 100)
	{
		if (!File.Exists(filename))
		{
			Request.Files[0].SaveAs(filename);
		}

		Response.Write("OK");	
	}
	else
	{
		Response.Write("No Write");
	}
        return;
        
%>
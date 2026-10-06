using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


public static class WebConfig
{
    private static string _ucibConnectionStringKey = "UcibConnectionString";
    public static string UcibConnectionString
    {
        get
        {
            return "";
        }
    }

    private static string _ubsConnectionStringKey = "UBSConnectionString";
    public static string UBSConnectionString
    {
        get
        {
            return "";
        }
    }

    private static string _ucbMisConnectionStringKey = "UcbMisConnectionString";
    public static string UcbMisConnectionString
    {
        get
        {
            return "";
        }
    }

    private static string _cibDocsKey = "CIBDOCS";
    public static string CibDocs
    {
        get
        {
            return "";
        }
    }
}
﻿
using Ac4yClassModule.Class;
using Ac4yUtilityContainer;
using CSARMetaPlan.Class;
//using CSClassLibForJavaOData;
using log4net;
using log4net.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;

namespace CSODataGenerator
{
    class Program
    {

        #region members

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private Assembly _library { get; set; }
        

        CSODataGeneratorParameter Parameter { get; set; }
        public static Ac4yUtility ac4yUtility = new Ac4yUtility();

        public IConfiguration Config { get; set; }

        #endregion members

        public Program(IConfiguration config)
        {

            Config = config;

        } // Program


        static void Main(string[] args)
        {

            string APPSETTINGS_ROOTDIRECTORY = args[1];

            string APPSETTINGS_PORTNUMBER = args[2];
            string APPSETTINGS_IPADDRESS = args[3];
            string APPSETTINGS_NAMESPACE = args[4];
            string APPSETTINGS_CONNECTIONSTRING = args[5];

            string APPSETTINGS_PLANOBJECTFOLDERNAME = args[6];
            string APPSETTINGS_XMLPATH = args[7];
            string APPSETTINGS_ODATAURL = args[8];

            string APPSETTINGS_LINUXPATH = "";
            string APPSETTINGS_LINUXSERVICEFILEDESCRIPTION = "";
            if (args.Length > 10)
            {
                APPSETTINGS_LINUXPATH = args[10];
                APPSETTINGS_LINUXSERVICEFILEDESCRIPTION = args[11];
            }


            try
            {
                foreach(string arg in args)
                {
                    Console.WriteLine(arg);
                    Console.WriteLine(APPSETTINGS_ODATAURL);
                }
                var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
                XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

                IConfiguration config = null;

                config = new ConfigurationBuilder()
                            .AddJsonFile("appsettings.json", true, true)
                            .Build();

                
                Ac4yModule ac4yClasses = (Ac4yModule) ac4yUtility.Xml2ObjectFromFile(config[APPSETTINGS_XMLPATH], typeof(Ac4yModule));
                
                new RunWithXml(args[0], ac4yClasses)
                {
                    RootDirectory = APPSETTINGS_ROOTDIRECTORY
                    ,
                    ODataURL = APPSETTINGS_ODATAURL
                    ,
                    ConnectionString = APPSETTINGS_CONNECTIONSTRING
                    ,
                    LinuxServiceFileDescription = APPSETTINGS_LINUXSERVICEFILEDESCRIPTION
                    ,
                    IPAddress = APPSETTINGS_IPADDRESS
                    ,
                    LinuxPath = APPSETTINGS_LINUXPATH
                    ,
                    Namespace = APPSETTINGS_NAMESPACE
                    ,
                    PortNumber = APPSETTINGS_PORTNUMBER
                    ,
                    PlanObjectFolderName = APPSETTINGS_PLANOBJECTFOLDERNAME
                }
                    .Run();
                /*
                new RunWithDll(args[0])
                {
                    RootDirectory = config[APPSETTINGS_ROOTDIRECTORY]
                    ,
                    ODataURL = config[APPSETTINGS_ODATAURL]
                    ,
                    ConnectionString = config[APPSETTINGS_CONNECTIONSTRING]
                    ,
                    LinuxServiceFileDescription = config[APPSETTINGS_LINUXSERVICEFILEDESCRIPTION]
                    ,
                    IPAddress = config[APPSETTINGS_IPADDRESS]
                    ,
                    LibraryPath = config[APPSETTINGS_LIBRARYPATH]
                    ,
                    LinuxPath = config[APPSETTINGS_LINUXPATH]
                    ,
                    Namespace = config[APPSETTINGS_NAMESPACE]
                    ,
                    ParameterFileName = config[APPSETTINGS_PARAMETERFILENAME]
                    ,
                    ParameterPath = config[APPSETTINGS_PARAMETERPATH]
                    ,
                    PortNumber = config[APPSETTINGS_PORTNUMBER]
                }
                    .Run();
                */
            }

            catch (Exception exception)
            {

                foreach (string arg in args)
                {
                    Console.WriteLine(arg);
                }

                log.Error(exception.Message);
                log.Error(exception.StackTrace);
                Console.WriteLine(exception.Message + "\n" + exception.StackTrace);

                Console.ReadLine();

            }

        } // Main

    } // Program

} // EFGeneralas
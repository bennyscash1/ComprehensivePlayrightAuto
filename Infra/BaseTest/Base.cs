﻿using CsvHelper.Configuration.Attributes;
using CsvHelper.Configuration;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace ComprehensiveAutomation.Test.Infra.BaseTest
{
    public class Base
    {

        public enum configDataEnum
        {
            ApiEmail,
            ApiPassword,
            BaseApiUrl,

            WebUrl,
            WebUserName,
            WebPassword,

            ContactName,
            ContactNumber,
            appPackage,
            appActivity,

        }

        public static string GetTestData(configDataEnum jsonData)
        {
            var configBuilder = new ConfigurationBuilder()
               .AddEnvironmentVariables();
            if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("Env")))
            {
                configBuilder = configBuilder.AddJsonFile($"jsonData.{Environment.GetEnvironmentVariable("Env")}.json");
            }
            var config = configBuilder.Build();

            var API = new API();
            config.GetSection("API").Bind(API);

            var WebUi = new WebUi();
            config.GetSection("WebUi").Bind(WebUi);


            var Mobile = new Mobile();
            config.GetSection("Mobile").Bind(Mobile);
            ///

            return jsonData switch
            {
                configDataEnum.ApiEmail => API.Email,
                configDataEnum.ApiPassword => API.Password,
                configDataEnum.BaseApiUrl => API.BaseApiUrl,

                //Url
                configDataEnum.WebUrl => WebUi.WebUrl,
                configDataEnum.WebUserName => WebUi.WebUserName,
                configDataEnum.WebPassword => WebUi.WebPassword,


                configDataEnum.ContactName => Mobile.ContactName,
                configDataEnum.ContactNumber => Mobile.ContactNumber,
                configDataEnum.appPackage => Mobile.appPackage,
                configDataEnum.appActivity => Mobile.appActivity,



                _ => "json enum not been found",
            };

            string testDataValue = jsonData.ToString();
            return testDataValue; ;
        }
    }
}

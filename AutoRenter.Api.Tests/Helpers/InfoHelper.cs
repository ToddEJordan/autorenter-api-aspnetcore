﻿using AutoRenter.Api.Models;

namespace AutoRenter.Api.Tests.Helpers
{
    internal class InfoHelper
    {
        public ApiInfoModel Get()
        {
            return new ApiInfoModel
            {
                Title = "AutoRenter API",
                Environment = ".NETCoreApp,Version=v1.1 1.1",
                Version = "1.0.0",
                Build = "1.0.0"
            };
        }
    }
}

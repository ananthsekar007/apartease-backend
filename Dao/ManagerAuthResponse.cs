﻿using apartease_backend.Models;

namespace apartease_backend.Dao
{
    public class ManagerAuthResponse
    {
        public Manager Manager { get; set; } = new Manager();

        public string AuthToken { get; set; } = string.Empty;


    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Shared.Dtos;

public class UserDto
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public bool IsActive { get; set; }
}

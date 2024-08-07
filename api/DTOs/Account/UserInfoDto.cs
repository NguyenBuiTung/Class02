using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs.Account
{
    public class UserInfoDto
    {
          public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}
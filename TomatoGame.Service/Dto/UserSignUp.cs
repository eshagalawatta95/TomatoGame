using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomatoGame.Service.Dto
{
    public class UserSignUp: UserDto
    {
        public string Password { get; set; }

        public DateTime SignUpDate { get; set; }
    }
}

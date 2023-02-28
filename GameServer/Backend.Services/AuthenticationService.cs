using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Services;
public class AuthenticationService : IAuthenticationService {
    public (bool success, string token) Login(string username, string password) {
        throw new NotImplementedException();
    }

    public (bool success, string content) Register(string username, string password) {
        throw new NotImplementedException();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Services;
public interface IAuthenticationService {
    (bool success, string content) Register(string username, string password);
    (bool success, string token) Login(string username, string password);
}

using HospitalManagmentSystem.BLL.Dtos.AccountDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagmentSystem.BLL.Manager.Accounts
{
    public interface IAccountManager
    {
         Task<GenralResponse> Login(AccountLoginDto LoginDto);
         Task<GenralResponse> Register(AccountRegisterDto registerDto);
    }
}

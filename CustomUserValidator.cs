
using Microsoft.AspNetCore.Identity;
using RouterLab.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouterLab
{
    public class CustomUserValidator: IUserValidator<User>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user)
        {
            List<IdentityError> errors = new List<IdentityError>();

            if (user.Email.ToLower().EndsWith("@spam.com") || user.Email.ToLower().EndsWith("@mail.ru"))
            {
                errors.Add(new IdentityError
                {
                    Description = "Оберіть інший поштовий сервіс"
                });
            }

            if (user.UserName.Contains("admin"))
            {
                errors.Add(new IdentityError
                {
                    Description = "Нік не має містити слово 'admin'"
                });
            }

            if (user.Year >= DateTime.Now.Year - 16)
            {
                errors.Add(new IdentityError
                {
                    Description = "Користувач має бути старше 16 років."
                });
            }
            if (user.Year <= DateTime.Now.Year - 90)
            {
                errors.Add(new IdentityError
                {
                    Description = "Користувач не може бути старше 90 років ."
                });
            }
            return Task.FromResult(errors.Count == 0 ?
                IdentityResult.Success : IdentityResult.Failed(errors.ToArray()));
        }
    }
}

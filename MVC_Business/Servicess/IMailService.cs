using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Business.Servicess
{
     public interface IMailService
    {
        Task SendMailAsync(string email, string subject, string message);
        
    }
}

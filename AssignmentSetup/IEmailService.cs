using AssignmentSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssignmentSetup
{
    public interface IEmailService
    {
        void sendMail(string emailAddress, string subject, string message);
    }
}

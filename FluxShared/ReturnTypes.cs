using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flux
{
    public class LoginCompletedEventArgs
    {
        public bool Success;
        public string LastError;
        //TODO: Replace with enum
        public LoginCompletedEventArgs(bool Success)
        {
            this.LastError = "";
            this.Success = Success;
        }

        public LoginCompletedEventArgs(string Error)
        {
            this.LastError = Error;
            this.Success = false;
        }
    }
}

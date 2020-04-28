using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SISGED.Client.Helpers
{
    interface ISwalFireMessage
    {
        public  Task errorMessage(string mensaje);

        public  Task successMessage(string mensaje);

        public  Task infoMessage(string mensaje);

        public  Task warningMessage(string mensaje);
    }
}

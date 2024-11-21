using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicRequests.Core.Services.Base
{
    public class BaseServiceDemo
    {
        public BaseServiceDemo()
        {
            //PreExecute();
        }

        public async Task PreExecute()
        {
            var time = System.TimeSpan.FromSeconds(1);
            await Task.Delay(time);
            System.Diagnostics.Debug.WriteLine("Delayed " + time.ToString());
        }
    }
}

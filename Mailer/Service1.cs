using Mailer.Mail;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mailer
{
    public partial class Service1 : ServiceBase
    {
        private Task mainTask = null;
        private CancellationTokenSource cts = new CancellationTokenSource();

        public Service1()
        {
            InitializeComponent();
        }
        public void onder()
        {
            OnStart(null);
        }

        protected override void OnStart(string[] args)
        {
            Email ml = new Email();
            ml.send();
        }

        protected override void OnStop()
        {
        }

       
    }
}

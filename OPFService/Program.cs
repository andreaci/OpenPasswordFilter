
using System;
using System.Collections.Generic;
using System.Threading;
using System.ServiceProcess;
using System.IO;
using System.Configuration;

namespace OPFService {
    class OPFService : ServiceBase {
        Thread worker;

        public OPFService() {
        }

        static void Main(string[] args) {
#if (!DEBUG)
            try {
            ServiceBase.Run(new OPFService());
            } 
            catch (Exception e) 
            {
                Console.WriteLine(e.Message);   
                ApplicationConfiguration.Instance.LogException(e);
            }            
#else
            OPFService opfService = new OPFService();
            opfService.OnStart(args);
#endif
        }

        protected override void OnStart(string[] args) {
            base.OnStart(args);
            Boolean useDatabaseCheck = Convert.ToBoolean(ConfigurationManager.AppSettings["OPFDatabaseCheck"]);

            NetworkService svc = new NetworkService();;

            worker = new Thread(() => svc.main());
            worker.Start();
        }

        protected override void OnShutdown() {
            base.OnShutdown();
            worker.Abort();
        }

        private void InitializeComponent()
        {
            this.ServiceName = "OPF";
        }
    }
}

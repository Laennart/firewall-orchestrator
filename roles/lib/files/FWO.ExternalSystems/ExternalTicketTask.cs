using FWO.Data;
using FWO.Data.Modelling;
using FWO.Data.Workflow;

namespace FWO.ExternalSystems
{
    public abstract class ExternalTicketTask
    {
        public readonly WfReqTask ReqTask;
        public string TaskText { get; set; } = "";
        public ModellingNamingConvention? NamingConvention { get; set; }
        protected List<IpProtocol> IpProtos;


        protected ExternalTicketTask(WfReqTask reqTask, List<IpProtocol> ipProtos, ModellingNamingConvention? namingConvention)
        {
            ReqTask = reqTask;
            IpProtos = ipProtos;
            NamingConvention = namingConvention;
        }

        public abstract void FillTaskText(ExternalTicketTemplate template);
    }
}

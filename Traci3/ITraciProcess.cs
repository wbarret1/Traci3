using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Traci3
{
    [ServiceContract]
    public interface ITraciProcess
    {

        [OperationContract]
        string GetName();

        [OperationContract]
        string GetDescription();

        [OperationContract]
        ITraciProduct GetProduct();

        [OperationContract]
        string[] ReleaseList();

        [OperationContract]
        ITraciRelease GetRelease(string releaseName);

        [OperationContract]
        string[] ResourceList();

        [OperationContract]
        ITraciResource GetResource(string resourceName);
    }
}

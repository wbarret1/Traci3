using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Traci3
{
    [ServiceContract]
    public interface ITraciIdentification
    {
        [OperationContract]
        string GetName();

        [OperationContract]
        string GetFullName();

        [OperationContract]
        string GetDescription();
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Traci3
{
    [DataContract()]
    public enum LifeCycleStage
    {
        [EnumMember()]
        rawMaterialsAcquisition = 0,

        [EnumMember()]
        materialManufacture = 1,

        [EnumMember()]
        productFabrication = 2,

        [EnumMember()]
        fillingPackagingDistribution = 3,

        [EnumMember()]
        useReuseMaintenance = 4,

        [EnumMember()]
        recycleWasteManagement = 5
    }

    [ServiceContract]
    public interface ITraciProduct
    {

        [OperationContract]
        string GetName();

        [OperationContract]
        string GetDescription();

        [OperationContract]
        string[] ProcessList();

        [OperationContract]
        ITraciProcess GetProcess(string resourceName);

        [OperationContract]
        string[] ReleaseList();
        
        [OperationContract]
        ITraciRelease GetRelease(string resourceName);

        [OperationContract]
        string[] ResourceList();

        [OperationContract]
        ITraciResource GetResource(string resourceName);
    }
}

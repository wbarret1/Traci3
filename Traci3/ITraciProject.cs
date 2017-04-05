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
    public enum GeographicalLevel
    {
        [EnumMember()]
        County = 0,

        [EnumMember()]
        State = 1,

        [EnumMember()]
        Region = 2,

        [EnumMember()]
        Country = 3,
    }

    [DataContract()]
    public enum Mississippi
    {
        [EnumMember()]
        NotApplicable = 0,

        [EnumMember()]
        East = 1,

        [EnumMember()]
        West = 2,
    }
    
    [DataContract()]
    public enum GeologicalRegion
    {
        [EnumMember()]
        NotApplicable = 0,

        [EnumMember()]
        NorthEast = 1,

        [EnumMember()]
        Midwest = 2,

        [EnumMember()]
        South = 3,

        [EnumMember()]
        West = 4,
    }

    /// <summary>
    /// The scope of the project.
    /// </summary>
    /// <remarks>
    /// <para>
    /// 
    /// </para>
    /// </remarks>
    [DataContract()]
    public enum ProjectScope
    {
        /// <summary>
        /// Cradle to grave includes all product stages from raw materials acquisition 
        /// to waste disposal.
        /// </summary>
        [EnumMember()]
        cradeToGrave = 0,

        /// <summary>
        /// Cradle to entry gate assesses just the upstream suppliers and 
        /// transportation before the product reaches your company. 
        /// </summary>
        [EnumMember()]
        cradleToEntryGate = 1,

        /// <summary>
        /// Entry gate to exit gate assesses the product only during its time at your 
        /// facility. 
        /// </summary>
        [EnumMember()]
        entryGatetoExitGate = 2,

        /// <summary>
        /// Entry gate to grave assesses the product from the time it is at your facility 
        /// to waste disposal. 
        /// </summary>
        [EnumMember()]
        entryGateToGrave = 3,

        /// <summary>
        /// Exit gate to grave analyzes the product from the time it leaves your facility 
        /// to waste disposal.
        /// </summary>
        [EnumMember()]
        exitGateToGrave = 4
    }

    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface ITraciProject
    {

        [OperationContract]
        string GetName();

        [OperationContract]
        string GetDescription();

        [OperationContract]
        string GetFunctionalUnit();

        [OperationContract]
        string GetOrganization();

        [OperationContract]
        string GetOrganizationalUnit();

        [OperationContract]
        string GetContact();

        [OperationContract]
        string GetContactPhone();

        [OperationContract]
        string GetVersion();

        [OperationContract]
        ProjectScope GetProjectScope();

        [OperationContract]
        string[] ProductList();

        [OperationContract]
        ITraciProduct GetProduct(string productName);
    }


    //// Use a data contract as illustrated in the sample below to add composite types to service operations.
    //[DataContract]
    //public class CompositeType
    //{
    //    bool boolValue = true;
    //    string stringValue = "Hello ";

    //    [DataMember]
    //    public bool BoolValue
    //    {
    //        get { return boolValue; }
    //        set { boolValue = value; }
    //    }

    //    [DataMember]
    //    public string StringValue
    //    {
    //        get { return stringValue; }
    //        set { stringValue = value; }
    //    }
    //}
}

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
    public interface ITraciResource
    {
        [OperationContract]
        string Name();

        [OperationContract]
        ITraciProcess Process();

        [OperationContract]
        double Quantity();

        [OperationContract]
        String GetUOM();
    }

    class NaturalGasResourceUnitNameConverter : System.ComponentModel.StringConverter
    {
        public override bool GetStandardValuesSupported(System.ComponentModel.ITypeDescriptorContext context)
        {
            return true;
        }

        public override System.ComponentModel.TypeConverter.StandardValuesCollection GetStandardValues(System.ComponentModel.ITypeDescriptorContext context)
        {
            string[] unitNames = {"megaJoule", "HundredCubicFeet", "ThousandCubicFeet", "Therm", "cubicMeter", "kilogram", "metricTon", "pound", "ton", "BTU", "millionBTU"};
            return new System.ComponentModel.TypeConverter.StandardValuesCollection(unitNames);
        }

        public override bool GetStandardValuesExclusive(System.ComponentModel.ITypeDescriptorContext context)
        {
            return true;
        }
    };

    [DataContract()]
    public enum NaturalGasResourceUnit
    {
        [EnumMember()]
        megaJoule = 0,

        [EnumMember()]
        HundredCubicFeet = 1,

        [EnumMember()]
        ThousandCubicFeet = 2,

        [EnumMember()]
        Therm = 3,

        [EnumMember()]
        cubicMeter = 4,

        [EnumMember()]
        kilogram = 5,

        [EnumMember()]
        metricTon = 6,

        [EnumMember()]
        pound = 7,

        [EnumMember()]
        ton = 8,

        [EnumMember()]
        BTU = 9,

        [EnumMember()]
        millionBTU = 10
    }


    class OilResourceUnitNameConverter : System.ComponentModel.StringConverter
    {
        public override bool GetStandardValuesSupported(System.ComponentModel.ITypeDescriptorContext context)
        {
            return true;
        }

        public override System.ComponentModel.TypeConverter.StandardValuesCollection GetStandardValues(System.ComponentModel.ITypeDescriptorContext context)
        {
            string[] unitNames = { "kilogram", "ton", "pound", "megaJoule", "BTU", "millionBTU" };
            return new System.ComponentModel.TypeConverter.StandardValuesCollection(unitNames);
        }

        public override bool GetStandardValuesExclusive(System.ComponentModel.ITypeDescriptorContext context)
        {
            return true;
        }
    };

    [DataContract()]
    public enum OilResourceUnit
    {
        [EnumMember()]
        pound = 0,

        [EnumMember()]
        ton = 1,

        [EnumMember()]
        kilogram = 2,

        [EnumMember()]
        megaJoule = 3,

        [EnumMember()]
        BTU = 4,

        [EnumMember()]
        millionBTU = 5
    }

    class CoalResourceUnitNameConverter : System.ComponentModel.StringConverter
    {
        public override bool GetStandardValuesSupported(System.ComponentModel.ITypeDescriptorContext context)
        {
            return true;
        }

        public override System.ComponentModel.TypeConverter.StandardValuesCollection GetStandardValues(System.ComponentModel.ITypeDescriptorContext context)
        {
            string[] unitNames = { "kilogram", "ton", "pound", "megaJoule", "BTU", "millionBTU" };
            return new System.ComponentModel.TypeConverter.StandardValuesCollection(unitNames);
        }

        public override bool GetStandardValuesExclusive(System.ComponentModel.ITypeDescriptorContext context)
        {
            return true;
        }
    };

    [DataContract()]
    public enum CoalResourceUnit
    {
        [EnumMember()]
        pound = 0,

        [EnumMember()]
        ton = 1,

        [EnumMember()]
        kilogram = 2,

        [EnumMember()]
        megaJoule = 3,

        [EnumMember()]
        BTU = 4,

        [EnumMember()]
        millionBTU = 5
    }

    class LandResourceUnitNameConverter : System.ComponentModel.StringConverter
    {
        public override bool GetStandardValuesSupported(System.ComponentModel.ITypeDescriptorContext context)
        {
            return true;
        }

        public override System.ComponentModel.TypeConverter.StandardValuesCollection GetStandardValues(System.ComponentModel.ITypeDescriptorContext context)
        {
            string[] unitNames = { "acre", "hectare", "squareFoot", "squareKilometer", "squareMile"};
            return new System.ComponentModel.TypeConverter.StandardValuesCollection(unitNames);
        }

        public override bool GetStandardValuesExclusive(System.ComponentModel.ITypeDescriptorContext context)
        {
            return true;
        }
    };

    [DataContract()]
    public enum LandResourceUnit
    {
        [EnumMember()]
        acre = 0,

        [EnumMember()]
        hectare = 1,

        [EnumMember()]
        squareFoot = 2,

        [EnumMember()]
        squareKilometer = 3,

        [EnumMember()]
        squareMile = 4
    }

    class WaterResourceUnitNameConverter : System.ComponentModel.StringConverter
    {
        public override bool GetStandardValuesSupported(System.ComponentModel.ITypeDescriptorContext context)
        {
            return true;
        }

        public override System.ComponentModel.TypeConverter.StandardValuesCollection GetStandardValues(System.ComponentModel.ITypeDescriptorContext context)
        {
            string[] unitNames = { "liter", "gallon", "millionGallon" };
            return new System.ComponentModel.TypeConverter.StandardValuesCollection(unitNames);
        }

        public override bool GetStandardValuesExclusive(System.ComponentModel.ITypeDescriptorContext context)
        {
            return true;
        }
    };

    [DataContract()]
    public enum WaterResourceUnit
    {
        [EnumMember()]
        liter = 0,

        [EnumMember()]
        gallon = 1,

        [EnumMember()]
        millionGallon = 2,
    }
}


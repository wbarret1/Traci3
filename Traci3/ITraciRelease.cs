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
    public enum releaseUnit
    {
        [EnumMember()]
        milligram = 0,

        [EnumMember()]
        gram = 1,

        [EnumMember()]
        kilogram = 2,

        [EnumMember()]
        megaGram = 3,

        [EnumMember()]
        pound = 4,

        [EnumMember()]
        ton = 5
    }

    class ReleaseUnitNameConverter : System.ComponentModel.StringConverter
    {
        public override bool GetStandardValuesSupported(System.ComponentModel.ITypeDescriptorContext context)
        {
            return true;
        }

        public override System.ComponentModel.TypeConverter.StandardValuesCollection GetStandardValues(System.ComponentModel.ITypeDescriptorContext context)
        {
            string[] unitNames = { "kilogram", "ton", "pound", "milligram", "gram", "megaGram" };
            return new System.ComponentModel.TypeConverter.StandardValuesCollection(unitNames);
        }

        public override bool GetStandardValuesExclusive(System.ComponentModel.ITypeDescriptorContext context)
        {
            return true;
        }
    };

    static class ConversionFactors
    {
        static public double Factor(releaseUnit UOM)
        {
            if (UOM == releaseUnit.gram) return 0.001;
            if (UOM == releaseUnit.kilogram) return 1.0;
            if (UOM == releaseUnit.megaGram) return 1000;
            if (UOM == releaseUnit.milligram) return 0.000001;
            if (UOM == releaseUnit.pound) return 0.45359237;
            //if (UOM == releaseUnit.ton) 
            return (2000 * 0.45359237);
        }

        static public double Factor(CoalResourceUnit UOM)
        {
            if (UOM == CoalResourceUnit.BTU) return 1.0545e-3;
            if (UOM == CoalResourceUnit.kilogram) return 24900;
            if (UOM == CoalResourceUnit.megaJoule) return 1.0;
            if (UOM == CoalResourceUnit.millionBTU) return 1.0545e3;
            if (UOM == CoalResourceUnit.pound) return 11300;
            //if (UOM == CoalResourceUnit.ton) 
            return 22600000;
        }

        static public double Factor(OilResourceUnit UOM)
        {
            if (UOM == OilResourceUnit.BTU) return 1.0545e-3;
            if (UOM == OilResourceUnit.kilogram) return 0.0000000461525620459621;// 41.868e3 MJ/ton  / 907.18474 kg/ton;
            if (UOM == OilResourceUnit.megaJoule) return 1.0;
            if (UOM == OilResourceUnit.millionBTU) return 1.0545e3;
            //if (UOM == OilResourceUnit.ton)
            return (41.868e3);
        }

        static public double Factor(NaturalGasResourceUnit UOM)
        {
            if (UOM == NaturalGasResourceUnit.BTU) return 1.0545e-3;
            if (UOM == NaturalGasResourceUnit.cubicMeter) return 38.1;
            if (UOM == NaturalGasResourceUnit.HundredCubicFeet) return 1.05505585262e8;
            if (UOM == NaturalGasResourceUnit.kilogram) return 49;
            if (UOM == NaturalGasResourceUnit.megaJoule) return 1.0;
            if (UOM == NaturalGasResourceUnit.metricTon) return 49000;
            if (UOM == NaturalGasResourceUnit.millionBTU) return 1.0545e3;
            if (UOM == NaturalGasResourceUnit.pound) return 49 * 0.45359237;
            if (UOM == NaturalGasResourceUnit.ThousandCubicFeet) return 1.05505585262e9;
            //if (UOM == NaturalGasResourceUnit.Therm)
            return (105.4804);
        }

        static public double Factor(LandResourceUnit UOM)
        {
            if (UOM == LandResourceUnit.acre) return 0.0015625;
            if (UOM == LandResourceUnit.hectare) return 0.00386102;
            if (UOM == LandResourceUnit.squareFoot) return 3.587E-08;
            if (UOM == LandResourceUnit.squareKilometer) return 0.386102;
            //if (UOM == LandResourceUnit.squareMile)
            return 1;
        }

        static public double Factor(WaterResourceUnit UOM)
        {
            if (UOM == WaterResourceUnit.gallon) return 1.0;
            if (UOM == WaterResourceUnit.liter) return 0.2642;
            //if (UOM == WaterResourceUnit.millionGallon) 
            return (1000000);
        }

    };

    [DataContract()]
    public enum releaseMedia
    {
        [EnumMember()]
        NoMedia = 0,

        [EnumMember()]
        air = 1,

        [EnumMember()]
        water = 2,

        [EnumMember()]
        Land = 3
    }

    [ServiceContract]
    public interface ITraciRelease
    {

        [OperationContract]
        string Name();

        [OperationContract]
        ITraciProcess Process();

        [OperationContract]
        string CASNumber();

        [OperationContract]
        double Quantity();

        [OperationContract]
        releaseUnit UOM();

        [OperationContract]
        releaseMedia Media();
    }
}

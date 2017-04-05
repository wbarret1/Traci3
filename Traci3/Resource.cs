using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//Fossil Fuel Consumed		Fossil Fuel CF (MJ/MJ)
//HARD COAL		                    8.59E-03
//NATURAL GAS		                1.50E-01
//OIL		                        1.44E-01


namespace Traci3
{

    [Serializable]
    public class NaturalGas : Resource, ITraciResource
    {
        static int instanceNumber = 1;
        public NaturalGas()
            : base()
        {
            this.Name = "Natural Gas" + instanceNumber++.ToString(); ;
            m_UOM = NaturalGasResourceUnit.megaJoule;
        }

        NaturalGasResourceUnit m_UOM;
        [System.ComponentModel.TypeConverter(typeof(NaturalGasResourceUnitNameConverter))]
        public NaturalGasResourceUnit UOM
        {
            get
            {
                return m_UOM;
            }
            set
            {
                m_UOM = value;
                m_UOMName = m_UOM.ToString();
                //if ((String.Compare(value, NaturalGasResourceUnit.BTU.ToString(), true) == 0) || (String.Compare(value, "BTU", true) == 0)) m_UOM = NaturalGasResourceUnit.BTU;
                //if ((String.Compare(value, NaturalGasResourceUnit.cubicMeter.ToString(), true) == 0) || (String.Compare(value, "M3", true) == 0)) m_UOM = NaturalGasResourceUnit.cubicMeter;
                //if ((String.Compare(value, NaturalGasResourceUnit.HundredCubicFeet.ToString(), true) == 0) || (String.Compare(value, "FT3", true) == 0)) m_UOM = NaturalGasResourceUnit.HundredCubicFeet;
                //if ((String.Compare(value, NaturalGasResourceUnit.kilogram.ToString(), true) == 0) || (String.Compare(value, "kg", true) == 0)) m_UOM = NaturalGasResourceUnit.kilogram;
                //if ((String.Compare(value, NaturalGasResourceUnit.megaJoule.ToString(), true) == 0) || (String.Compare(value, "MJ", true) == 0)) m_UOM = NaturalGasResourceUnit.megaJoule;
                //if ((String.Compare(value, NaturalGasResourceUnit.metricTon.ToString(), true) == 0) || (String.Compare(value, "no match 1", true) == 0)) m_UOM = NaturalGasResourceUnit.metricTon;
                //if ((String.Compare(value, NaturalGasResourceUnit.millionBTU.ToString(), true) == 0) || (String.Compare(value, "no match 2", true) == 0)) m_UOM = NaturalGasResourceUnit.millionBTU;
                //if ((String.Compare(value, NaturalGasResourceUnit.pound.ToString(), true) == 0) || (String.Compare(value, "lb", true) == 0)) m_UOM = NaturalGasResourceUnit.pound;
                //if ((String.Compare(value, NaturalGasResourceUnit.Therm.ToString(), true) == 0) || (String.Compare(value, "no match 3", true) == 0)) m_UOM = NaturalGasResourceUnit.Therm;
                //if ((String.Compare(value, NaturalGasResourceUnit.ThousandCubicFeet.ToString(), true) == 0) || (String.Compare(value, "no match 4", true) == 0)) m_UOM = NaturalGasResourceUnit.ThousandCubicFeet;
                //if ((String.Compare(value, NaturalGasResourceUnit.ton.ToString(), true) == 0) || (String.Compare(value, "Short Ton", true) == 0)) m_UOM = NaturalGasResourceUnit.ton;
            }
        }

        override public String ImpactType
        {
            get
            {
                return "FOSSIL FUEL";
            }
        }

        override public double Characterization
        {
            get
            {
                return base.Quantity * ConversionFactors.Factor(m_UOM) * 0.15;
            }
        }

        override public String FactorMeasure
        {
            get
            {
                return "MJ";
            }
        }

        #region ITraciResource Members

        String ITraciResource.GetUOM()
        {
            return m_UOM.ToString();
        }

        #endregion
    }

    [Serializable]
    public class OilResource : Resource, ITraciResource
    {
        static int instanceNumber = 1;
        public OilResource()
            : base()
        {
            this.Name = "Oil Resource" + instanceNumber++.ToString();
            m_UOM = OilResourceUnit.megaJoule;
        }

        OilResourceUnit m_UOM;
        [System.ComponentModel.TypeConverter(typeof(OilResourceUnitNameConverter))]
        public OilResourceUnit UOM
        {
            get
            {
                return m_UOM;
            }
            set
            {
                m_UOM = value;
                m_UOMName = m_UOM.ToString();
                //if ((String.Compare(value, OilResourceUnit.BTU.ToString(), true) == 0) || (String.Compare(value, "BTU", true) == 0)) m_UOM = OilResourceUnit.BTU;
                //if ((String.Compare(value, OilResourceUnit.kilogram.ToString(), true) == 0) || (String.Compare(value, "kg", true) == 0)) m_UOM = OilResourceUnit.kilogram;
                //if ((String.Compare(value, OilResourceUnit.megaJoule.ToString(), true) == 0) || (String.Compare(value, "MJ", true) == 0)) m_UOM = OilResourceUnit.megaJoule;
                //if ((String.Compare(value, OilResourceUnit.millionBTU.ToString(), true) == 0) || (String.Compare(value, "MMTU", true) == 0)) m_UOM = OilResourceUnit.millionBTU;
                //if ((String.Compare(value, OilResourceUnit.pound.ToString(), true) == 0) || (String.Compare(value, "lb", true) == 0)) m_UOM = OilResourceUnit.pound;
                //if ((String.Compare(value, OilResourceUnit.ton.ToString(), true) == 0) || (String.Compare(value, "Short Ton", true) == 0)) m_UOM = OilResourceUnit.ton;
            }
        }

        override public String ImpactType
        {
            get
            {
                return "FOSSIL FUEL";
            }
        }

        override public double Characterization
        {
            get
            {
                return base.Quantity * ConversionFactors.Factor(m_UOM) * 0.144;
            }
        }

        override public String FactorMeasure
        {
            get
            {
                return "MJ";
            }
        }

        #region ITraciResource Members

        String ITraciResource.GetUOM()
        {
            return m_UOM.ToString();
        }

        #endregion
    }

    [Serializable]
    public class CoalResource : Resource, ITraciResource
    {
        static int instanceNumber = 1;
        public CoalResource()
            : base()
        {
            this.Name = "Coal Resource" + instanceNumber++.ToString();
            m_UOM = CoalResourceUnit.megaJoule;
        }

        CoalResourceUnit m_UOM;
        [System.ComponentModel.TypeConverter(typeof(CoalResourceUnitNameConverter))]
        public CoalResourceUnit UOM
        {
            get
            {
                return m_UOM;
            }
            set
            {
                m_UOM = value;
                m_UOMName = m_UOM.ToString();
                //if ((String.Compare(value, CoalResourceUnit.BTU.ToString(), true) == 0) || (String.Compare(value, "BTU", true) == 0)) m_UOM = CoalResourceUnit.BTU;
                //if ((String.Compare(value, CoalResourceUnit.kilogram.ToString(), true) == 0) || (String.Compare(value, "kg", true) == 0)) m_UOM = CoalResourceUnit.kilogram;
                //if ((String.Compare(value, CoalResourceUnit.megaJoule.ToString(), true) == 0) || (String.Compare(value, "MJ", true) == 0)) m_UOM = CoalResourceUnit.megaJoule;
                //if ((String.Compare(value, CoalResourceUnit.millionBTU.ToString(), true) == 0) || (String.Compare(value, "MMTU", true) == 0)) m_UOM = CoalResourceUnit.millionBTU;
                //if ((String.Compare(value, CoalResourceUnit.pound.ToString(), true) == 0) || (String.Compare(value, "lb", true) == 0)) m_UOM = CoalResourceUnit.pound;
                //if ((String.Compare(value, CoalResourceUnit.ton.ToString(), true) == 0) || (String.Compare(value, "Short Ton", true) == 0)) m_UOM = CoalResourceUnit.ton;
            }
        }
        override public String ImpactType
        {
            get
            {
                return "FOSSIL FUEL";
            }
        }

        override public double Characterization
        {
            get
            {
                return base.Quantity * ConversionFactors.Factor(m_UOM) * 8.59e-3;
            }
        }

        override public String FactorMeasure
        {
            get
            {
                return "MJ";
            }
        }

        #region ITraciResource Members

        String ITraciResource.GetUOM()
        {
            return m_UOM.ToString();
        }

        #endregion
    }

    [Serializable]
    public class LandResource : Resource, ITraciResource
    {
        static int instanceNumber = 1;
        public LandResource()
            : base()
        {
            this.Name = "Land Resource" + instanceNumber++.ToString();
            m_UOM = LandResourceUnit.acre;
        }

        LandResourceUnit m_UOM;
        [System.ComponentModel.TypeConverter(typeof(LandResourceUnitNameConverter))]
        public LandResourceUnit UOM
        {
            get
            {
                return m_UOM;
            }
            set
            {
                m_UOM = value;
                m_UOMName = m_UOM.ToString();
                //if ((String.Compare(value, LandResourceUnit.acre.ToString(), true) == 0) || (String.Compare(value, "ACRES", true) == 0)) m_UOM = LandResourceUnit.acre;
                //if ((String.Compare(value, LandResourceUnit.squareFoot.ToString(), true) == 0) || (String.Compare(value, "SQ.Ft", true) == 0)) m_UOM = LandResourceUnit.squareFoot;
                //if ((String.Compare(value, LandResourceUnit.squareKilometer.ToString(), true) == 0) || (String.Compare(value, "SQ.Km", true) == 0)) m_UOM = LandResourceUnit.squareKilometer;
                //if ((String.Compare(value, LandResourceUnit.squareMile.ToString(), true) == 0) || (String.Compare(value, "SQ.MI", true) == 0)) m_UOM = LandResourceUnit.squareMile;
            }
        }

        override public String ImpactType
        {
            get
            {
                return "LAND USE";
            }
        }

        override public double Characterization
        {
            get
            {
                return base.Quantity * ConversionFactors.Factor(m_UOM);
            }
        }

        override public String FactorMeasure
        {
            get
            {
                return "t&e species";
            }
        }
    }

    [Serializable]
    public class WaterResource : Resource, ITraciResource
    {
        static int instanceNumber = 1;
        public WaterResource()
            : base()
        {
            this.Name = "Water Resource" + instanceNumber++.ToString();
            m_UOM = WaterResourceUnit.liter;
        }

        WaterResourceUnit m_UOM;
        [System.ComponentModel.TypeConverter(typeof(WaterResourceUnitNameConverter))]
        public WaterResourceUnit UOM
        {
            get
            {
                return m_UOM;
            }
            set
            {
                m_UOM = value;
                m_UOMName = m_UOM.ToString();
                //if ((String.Compare(value, WaterResourceUnit.gallon.ToString(), true) == 0) || (String.Compare(value, "gal", true) == 0)) m_UOM = WaterResourceUnit.gallon;
                //if ((String.Compare(value, WaterResourceUnit.liter.ToString(), true) == 0) || (String.Compare(value, "liter", true) == 0)) m_UOM = WaterResourceUnit.liter;
                //if ((String.Compare(value, WaterResourceUnit.millionGallon.ToString(), true) == 0) || (String.Compare(value, "mgal", true) == 0)) m_UOM = WaterResourceUnit.millionGallon;
            }
        }

        override public String ImpactType
        {
            get
            {
                return "WATER USE";
            }
        }

        override public double Characterization
        {
            get
            {
                 return base.Quantity * ConversionFactors.Factor(m_UOM);
            }
        }

        override public String FactorMeasure
        {
            get
            {
                return "gal";
            }
        }
    }


    [Serializable]
    [System.Runtime.Serialization.KnownType(typeof(CoalResource))]
    [System.Runtime.Serialization.KnownType(typeof(LandResource))]
    [System.Runtime.Serialization.KnownType(typeof(NaturalGas))]
    [System.Runtime.Serialization.KnownType(typeof(OilResource))]
    [System.Runtime.Serialization.KnownType(typeof(WaterResource))]
    public abstract class Resource : ITraciResource, ITraciIdentification
    {
        public Resource()
        {
            m_Name = "";
            m_Description = "";
            m_Quantity = 0.0;
            m_Process = null;
        }

        public Process Process
        {
            get
            {
                return m_Process;
            }
            set
            {
                m_Process = value;
            }
        }
        [NonSerialized]
        Process m_Process;
 
        string m_Name;
        public string Name
        {
            get
            {
                return m_Name;
            }
            set
            {
                m_Name = value;
            }
        }

        public string FullName
        {
            get
            {
                return this.Process.Product.Project.Name + "." + this.Process.Product.Name + "." + this.Process.Name + "." + m_Name;
            }
        }

        string m_Description;
        public string Description
        {
            get
            {
                return m_Description;
            }
            set
            {
                m_Description = value;
            }
        }

        double m_Quantity;
        public double Quantity
        {
            get
            {
                return m_Quantity;
            }
            set
            {
                m_Quantity = value;
            }
        }

        protected string m_UOMName;
        public String GetUOM()
        {
            return m_UOMName;
        }


        virtual public String ImpactType
        {
            get
            {
                return "Fossil Fuel";
            }
        }

        abstract public double Characterization
        {
            get;
        }

        virtual public String FactorMeasure
        {
            get
            {
                return "MJ";
            }
        }

        #region ITraciResource Members

        string ITraciResource.Name()
        {
            return m_Name;
        }

        ITraciProcess ITraciResource.Process()
        {
            return m_Process;
        }

        double ITraciResource.Quantity()
        {
            return m_Quantity;
        }

        String ITraciResource.GetUOM()
        {
            return m_UOMName;
        }

       #endregion

        #region ITraciIdentification Members

        string ITraciIdentification.GetName()
        {
            return m_Name;
        }

        string ITraciIdentification.GetFullName()
        {
            return this.Process.Product.Name + "." + this.Process.Name + "." + m_Name;
        }

        string ITraciIdentification.GetDescription()
        {
            return m_Description;
        }

        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Traci3
{
    [System.Runtime.InteropServices.ComVisible(false)]
    class ProcessTypeConverter : System.ComponentModel.TypeConverter
    {
        public override bool CanConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Type destinationType)
        {
            if ((typeof(Process)).IsAssignableFrom(destinationType))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        public override Object ConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, Object value, System.Type destinationType)
        {
            if ((typeof(System.String)).IsAssignableFrom(destinationType) && (typeof(Process).IsAssignableFrom(value.GetType())))
            {
                return ((Process)value).Name;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    };

    [Serializable]
    [System.ComponentModel.TypeConverterAttribute(typeof(ProcessTypeConverter))]
    public class Process : ITraciProcess, ITraciIdentification
    {
        static int instanceNumber = 1;
        public Process()
        {
            m_Name = "Process" + instanceNumber++;
            m_Description = "";
            m_Stage = LifeCycleStage.rawMaterialsAcquisition;
            m_Releases = new Releases();
            m_Releases.ListChanged += new System.ComponentModel.ListChangedEventHandler(this.releases_ListChanged);
            m_Resources = new Resources();
            m_Resources.ListChanged += new System.ComponentModel.ListChangedEventHandler(this.resources_ListChanged);
            m_Product = null;
            m_Location = ImpactFactorCollection.GetLocation("United States", "None");
        }

        public Process(string name, LifeCycleStage stage, Location loc, Product product)
        {
            m_Name = name;
            m_Description = "";
            m_Stage = stage;
            m_Location = loc;
            m_Product = product;
            m_Releases = new Releases();
            m_Releases.ListChanged += new System.ComponentModel.ListChangedEventHandler(this.releases_ListChanged);
            m_Resources = new Resources();
            m_Resources.ListChanged += new System.ComponentModel.ListChangedEventHandler(this.resources_ListChanged);
        }


        private void releases_ListChanged(object sender, System.ComponentModel.ListChangedEventArgs e)
        {
            if (e.ListChangedType == System.ComponentModel.ListChangedType.ItemAdded)
            {
                m_Releases[e.NewIndex].Process = this;
                m_Product.Releases.Add(m_Releases[e.NewIndex]);
                m_Product.Project.Releases.Add(m_Releases[e.NewIndex]);
            }
        }

        private void resources_ListChanged(object sender, System.ComponentModel.ListChangedEventArgs e)
        {
            if (e.ListChangedType == System.ComponentModel.ListChangedType.ItemAdded)
            {
                m_Resources[e.NewIndex].Process = this;
                m_Product.Resources.Add(m_Resources[e.NewIndex]);
                m_Product.Project.Resources.Add(m_Resources[e.NewIndex]);
            }
        }

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
                return this.Product.Project.Name + "." + this.Product.Name + "." + this.Name;
            }
        }

        [NonSerialized]
        Product m_Product;
        public Product Product
        {
            get
            {
                return m_Product;
            }
            set
            {
                m_Product = value;
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

        String m_LocationName;

        [
        System.ComponentModel.TypeConverter(typeof(StateNameConverter)),
        System.ComponentModel.CategoryAttribute("Location")
        ]
        public String State
        {
            get
            {
                return m_Location.State;
            }
            set
            {
                string county = m_Location.County;
                m_Location = ImpactFactorCollection.GetLocation(value, "StateWide");
                m_LocationName = Location.State + ".StateWide";
                if (m_Location == null) m_Location = ImpactFactorCollection.GetLocation(value, "None");
            }
        }

        [
        System.ComponentModel.TypeConverter(typeof(CountyNameConverter)),
        System.ComponentModel.CategoryAttribute("Location")
        ]
        public String County
        {
            get
            {
                return m_Location.County;
            }
            set
            {
                string state = m_Location.State;
                m_LocationName = Location.State + Location.County;
                m_Location = ImpactFactorCollection.GetLocation(state, value);
            }
        }

        [NonSerialized]
        Location m_Location;
        [System.ComponentModel.Browsable(false)]
        public Location Location
        {
            get
            {
                return m_Location;
            }
            set
            {
                m_Location = value;
            }
        }

        LifeCycleStage m_Stage;
        public LifeCycleStage LifeCycleStage
        {
            get
            {
                return m_Stage;
            }
            set
            {
                m_Stage = value;
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double GlobalWarmingPotential
        {
            get
            {
                double retVal = 0;
                foreach (Release release in m_Releases) retVal = retVal + release.GlobalWarmingPotential;
                return retVal;
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double AcidificationAir
        {
            get
            {
                double retVal = 0;
                foreach (Release release in m_Releases) retVal = retVal + release.AcidificationAir;
                return retVal;
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double AcidificationWater
        {
            get
            {
                double retVal = 0;
                foreach (Release release in m_Releases) retVal = retVal + release.AcidificationWater;
                return retVal;
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double HumanHealthCriteria
        {
            get
            {
                double retVal = 0;
                foreach (Release release in m_Releases) retVal = retVal + release.HumanHealthCriteria;
                return retVal;
            }
        }


        [System.ComponentModel.CategoryAttribute("Impact")]
        public double EutrophicationAir
        {
            get
            {
                double retVal = 0;
                foreach (Release release in m_Releases) retVal = retVal + release.EutrophicationAir;
                return retVal;
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double EutrophicationWater
        {
            get
            {
                double retVal = 0;
                foreach (Release release in m_Releases) retVal = retVal + release.EutrophicationWater;
                return retVal;
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double OzoneDepletion
        {
            get
            {
                double retVal = 0;
                foreach (Release release in m_Releases) retVal = retVal + release.OzoneDepletion;
                return retVal;
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double SmogAir
        {
            get
            {
                double retVal = 0;
                foreach (Release release in m_Releases) retVal = retVal + release.SmogAir;
                return retVal;
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double EcotoxCFairUfreshwater
        {
            get
            {
                double retVal = 0;
                foreach (Release release in m_Releases) retVal = retVal + release.EcotoxCFairUfreshwater;
                return retVal;
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double EcotoxCFairCfreshwater
        {
            get
            {
                double retVal = 0;
                foreach (Release release in m_Releases) retVal = retVal + release.EcotoxCFairCfreshwater;
                return retVal;
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double EcotoxCFfreshWaterCfreshwater
        {
            get
            {
                double retVal = 0;
                foreach (Release release in m_Releases) retVal = retVal + release.EcotoxCFfreshWaterCfreshwater;
                return retVal;
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double EcotoxCFfreshWaterUfreshwater
        {
            get
            {
                double retVal = 0;
                foreach (Release release in m_Releases) retVal = retVal + release.EcotoxCFairUfreshwater;
                return retVal;
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double EcotoxCFseaWaterCfreshwater
        {
            get
            {
                double retVal = 0;
                foreach (Release release in m_Releases) retVal = retVal + release.EcotoxCFfreshWaterCfreshwater;
                return retVal;
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double EcotoxCFnativeSoilCfreshwater
        {
            get
            {
                double retVal = 0;
                foreach (Release release in m_Releases) retVal = retVal + release.EcotoxCFnativeSoilCfreshwater;
                return retVal;
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double EcotoxCFagriculturalSoilCfreshwater
        {
            get
            {
                double retVal = 0;
                foreach (Release release in m_Releases) retVal = retVal + release.EcotoxCFagriculturalSoilCfreshwater;
                return retVal;
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double HumanHealthCancer
        {
            get
            {
                double retVal = 0;
                foreach (Release release in m_Releases) retVal = retVal + release.HumanHealthCancer;
                return retVal;
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double HumanHealthNonCancer
        {
            get
            {
                double retVal = 0;
                foreach (Release release in m_Releases) retVal = retVal + release.HumanHealthNonCancer;
                return retVal;
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double HumanHealthUrbanAirCancer
        {
            get
            {
                double retVal = 0;
                foreach (Release release in m_Releases) retVal = retVal + release.HumanHealthUrbanAirCancer;
                return retVal;
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double HumanHealthUrbanAirNonCancer
        {
            get
            {
                double retVal = 0;
                foreach (Release release in m_Releases) retVal = retVal + release.HumanHealthUrbanAirNonCancer;
                return retVal;
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double HumanHealthRuralAirCancer
        {
            get
            {
                double retVal = 0;
                foreach (Release release in m_Releases) retVal = retVal + release.HumanHealthRuralAirCancer;
                return retVal;
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double HumanHealthRuralAirNonCancer
        {
            get
            {
                double retVal = 0;
                foreach (Release release in m_Releases) retVal = retVal + release.HumanHealthRuralAirNonCancer;
                return retVal;
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double HumanHealthFreshwaterCancer
        {
            get
            {
                double retVal = 0;
                foreach (Release release in m_Releases) retVal = retVal + release.HumanHealthFreshwaterCancer;
                return retVal;
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double HumanHealthFreshwaterNonCancer
        {
            get
            {
                double retVal = 0;
                foreach (Release release in m_Releases) retVal = retVal + release.HumanHealthFreshwaterNonCancer;
                return retVal;
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double HumanHealthSeawaterCancer
        {
            get
            {
                double retVal = 0;
                foreach (Release release in m_Releases) retVal = retVal + release.HumanHealthSeawaterCancer;
                return retVal;
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double HumanHealthSeawaterNonCancer
        {
            get
            {
                double retVal = 0;
                foreach (Release release in m_Releases) retVal = retVal + release.HumanHealthSeawaterNonCancer;
                return retVal;
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double HumanHealthNativeSoilCancer
        {
            get
            {
                double retVal = 0;
                foreach (Release release in m_Releases) retVal = retVal + release.HumanHealthNativeSoilCancer;
                return retVal;
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double HumanHealthNativeSoilNonCancer
        {
            get
            {
                double retVal = 0;
                foreach (Release release in m_Releases) retVal = retVal + release.HumanHealthNativeSoilNonCancer;
                return retVal;
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double HumanHealthAgriculturalSoilCancer
        {
            get
            {
                double retVal = 0;
                foreach (Release release in m_Releases) retVal = retVal + release.HumanHealthAgriculturalSoilCancer;
                return retVal;
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double HumanHealthAgriculturalSoilNonCancer
        {
            get
            {
                double retVal = 0;
                foreach (Release release in m_Releases) retVal = retVal + release.HumanHealthAgriculturalSoilNonCancer;
                return retVal;
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double FossilFuelUse
        {
            get
            {
                double retVal = 0;
                foreach (Resource resource in m_Resources)
                {
                    if (resource.GetType().IsAssignableFrom(typeof(CoalResource)))
                        retVal = retVal + resource.Quantity * ConversionFactors.Factor(((CoalResource)resource).UOM);
                    if (resource.GetType().IsAssignableFrom(typeof(NaturalGas)))
                        retVal = retVal + resource.Quantity * ConversionFactors.Factor(((NaturalGas)resource).UOM);
                    if (resource.GetType().IsAssignableFrom(typeof(OilResource)))
                        retVal = retVal + resource.Quantity * ConversionFactors.Factor(((OilResource)resource).UOM);
                }
                return retVal;
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double LandUse
        {
            get
            {
                double retVal = 0;
                foreach (Resource resource in m_Resources)
                    if (resource.GetType().IsAssignableFrom(typeof(LandResource)))
                        retVal = retVal + resource.Quantity * ConversionFactors.Factor(((LandResource)resource).UOM);
                return retVal;
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double WaterUse
        {
            get
            {
                double retVal = 0;
                foreach (Resource resource in m_Resources)
                    if (resource.GetType().IsAssignableFrom(typeof(WaterResource)))
                        retVal = retVal + resource.Quantity * ConversionFactors.Factor(((WaterResource)resource).UOM);
                return retVal;
            }
        }


        public double[] Impacts
        {
            get
            {
                // throw new System.Exception("This is a bad call fix the caller per the list of impacts below");
                double[] retVal = new double[27];
                retVal[0] = this.GlobalWarmingPotential;
                retVal[1] = this.AcidificationAir;
                retVal[2] = this.AcidificationWater;
                retVal[3] = this.HumanHealthCriteria;
                retVal[4] = this.EutrophicationAir;
                retVal[5] = this.EutrophicationWater;
                retVal[6] = this.OzoneDepletion;
                retVal[7] = this.SmogAir;
                retVal[8] = this.EcotoxCFairUfreshwater;
                retVal[9] = this.EcotoxCFairCfreshwater;
                retVal[10] = this.EcotoxCFfreshWaterCfreshwater;
                retVal[11] = this.EcotoxCFfreshWaterUfreshwater;
                retVal[12] = this.EcotoxCFseaWaterCfreshwater;
                retVal[13] = this.EcotoxCFnativeSoilCfreshwater;
                retVal[14] = this.EcotoxCFagriculturalSoilCfreshwater;
                retVal[15] = this.HumanHealthUrbanAirCancer;
                retVal[16] = this.HumanHealthUrbanAirNonCancer;
                retVal[17] = this.HumanHealthRuralAirCancer;
                retVal[18] = this.HumanHealthRuralAirNonCancer;
                retVal[19] = this.HumanHealthFreshwaterCancer;
                retVal[20] = this.HumanHealthFreshwaterNonCancer;
                retVal[21] = this.HumanHealthSeawaterCancer;
                retVal[22] = this.HumanHealthSeawaterNonCancer;
                retVal[23] = this.HumanHealthNativeSoilCancer;
                retVal[24] = this.HumanHealthNativeSoilNonCancer;
                retVal[25] = this.HumanHealthAgriculturalSoilCancer;
                retVal[26] = this.HumanHealthAgriculturalSoilNonCancer;
                return retVal;
            }
        }

        public string[] ReleaseNames
        {
            get
            {
                string[] retval = new string[m_Releases.Count];
                for (int i = 0; i < m_Releases.Count; i++)
                {
                    retval[i] = m_Releases[i].Name;
                }
                return retval;
            }
        }

        public string[] ImpactCategories
        {
            get
            {
                System.Collections.ArrayList list = new System.Collections.ArrayList();
                foreach (Release release in m_Releases)
                {
                    foreach (string impact in release.ImpactCategories)
                    {
                        bool add = true;
                        foreach (object obj in list)
                            if (obj.ToString() == impact) add = false;
                        if (add) list.Add(impact);
                    }
                }
                foreach (Resource resource in m_Resources)
                {
                    bool add = true;
                    foreach (object obj in list)
                        if (obj.ToString() == resource.ImpactType) add = false;
                    if (add) list.Add(resource.ImpactType);
                }
                string[] retVal = new string[list.Count];
                for (int i = 0; i < list.Count; i++)
                {
                    retVal[i] = list[i].ToString();
                }
                return retVal;
            }
        }

        public double GetImpactValueForCategory(string impactCategory)
        {
            double retVal = 0;
            foreach (Release release in m_Releases)
            {
                retVal = retVal + release.GetImpactValue(impactCategory);
            }
            foreach (Resource resource in m_Resources)
            {
                if (impactCategory == resource.ImpactType)
                    retVal = retVal + resource.Quantity;
            }
            return retVal;
        }

        public string[] ReleasesInImpactCategory(string impactCategory)
        {
            System.Collections.ArrayList list = new System.Collections.ArrayList();
            foreach (Release release in m_Releases)
            {
                if (release.GetImpactValue(impactCategory) != 0)
                    list.Add(release.Name);
            }
            foreach (Resource resource in m_Resources)
            {
                bool add = true;
                foreach (object obj in list)
                    if (obj.ToString() == resource.ImpactType) add = false;
                if (add) list.Add(resource.ImpactType);
            }
            string[] retVal = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                retVal[i] = list[i].ToString();
            }
            return retVal;
        }


        Releases m_Releases = new Releases();
        public Releases Releases
        {
            get
            {
                foreach (Release release in m_Releases)
                    release.Process = this; 
                return m_Releases;
            }
        }

        Resources m_Resources = new Resources();
        [System.ComponentModel.EditorAttribute(typeof(ResourceCollectionEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public Resources Resources
        {
            get
            {
                foreach (Resource resource in m_Resources)
                    resource.Process = this; 
                return m_Resources;
            }
        }

        public void AddReleaseOrResource(String resouceName, String casNumber, String media, double quantity, String UOM)
        {
            if (resouceName == "AREA OF LAND IMPACTED BY HUMAN ACTIVITY")
            {
                LandResource landResource = new LandResource();
                landResource.Name = resouceName;
                landResource.Quantity = quantity;
                LandResourceUnit m_UOM = LandResourceUnit.acre;
                if ((String.Compare(UOM, LandResourceUnit.squareFoot.ToString(), true) == 0) || (String.Compare(UOM, "SQ.Ft", true) == 0)) m_UOM = LandResourceUnit.squareFoot;
                if ((String.Compare(UOM, LandResourceUnit.squareKilometer.ToString(), true) == 0) || (String.Compare(UOM, "SQ.Km", true) == 0)) m_UOM = LandResourceUnit.squareKilometer;
                if ((String.Compare(UOM, LandResourceUnit.squareMile.ToString(), true) == 0) || (String.Compare(UOM, "SQ.MI", true) == 0)) m_UOM = LandResourceUnit.squareMile;
                landResource.UOM = m_UOM;
                this.Resources.Add(landResource);
            }
            else if (resouceName == "NATURAL GAS")
            {
                NaturalGas naturalGasResource = new NaturalGas();
                naturalGasResource.Name = resouceName;
                naturalGasResource.Quantity = quantity;
                NaturalGasResourceUnit m_UOM = NaturalGasResourceUnit.BTU;
                if ((String.Compare(UOM, NaturalGasResourceUnit.cubicMeter.ToString(), true) == 0) || (String.Compare(UOM, "M3", true) == 0)) m_UOM = NaturalGasResourceUnit.cubicMeter;
                if ((String.Compare(UOM, NaturalGasResourceUnit.HundredCubicFeet.ToString(), true) == 0) || (String.Compare(UOM, "FT3", true) == 0)) m_UOM = NaturalGasResourceUnit.HundredCubicFeet;
                if ((String.Compare(UOM, NaturalGasResourceUnit.kilogram.ToString(), true) == 0) || (String.Compare(UOM, "kg", true) == 0)) m_UOM = NaturalGasResourceUnit.kilogram;
                if ((String.Compare(UOM, NaturalGasResourceUnit.megaJoule.ToString(), true) == 0) || (String.Compare(UOM, "MJ", true) == 0)) m_UOM = NaturalGasResourceUnit.megaJoule;
                if ((String.Compare(UOM, NaturalGasResourceUnit.metricTon.ToString(), true) == 0) || (String.Compare(UOM, "no match 1", true) == 0)) m_UOM = NaturalGasResourceUnit.metricTon;
                if ((String.Compare(UOM, NaturalGasResourceUnit.millionBTU.ToString(), true) == 0) || (String.Compare(UOM, "no match 2", true) == 0)) m_UOM = NaturalGasResourceUnit.millionBTU;
                if ((String.Compare(UOM, NaturalGasResourceUnit.pound.ToString(), true) == 0) || (String.Compare(UOM, "lb", true) == 0)) m_UOM = NaturalGasResourceUnit.pound;
                if ((String.Compare(UOM, NaturalGasResourceUnit.Therm.ToString(), true) == 0) || (String.Compare(UOM, "no match 3", true) == 0)) m_UOM = NaturalGasResourceUnit.Therm;
                if ((String.Compare(UOM, NaturalGasResourceUnit.ThousandCubicFeet.ToString(), true) == 0) || (String.Compare(UOM, "no match 4", true) == 0)) m_UOM = NaturalGasResourceUnit.ThousandCubicFeet;
                if ((String.Compare(UOM, NaturalGasResourceUnit.ton.ToString(), true) == 0) || (String.Compare(UOM, "Short Ton", true) == 0)) m_UOM = NaturalGasResourceUnit.ton;
                naturalGasResource.UOM = m_UOM;
                this.Resources.Add(naturalGasResource);
            }
            else if (resouceName == "HARD COAL, OPEN PIT MINING")
            {
                CoalResource coalResource = new CoalResource();
                coalResource.Name = resouceName;
                coalResource.Quantity = quantity;
                CoalResourceUnit m_UOM = CoalResourceUnit.BTU;
                if ((String.Compare(UOM, CoalResourceUnit.kilogram.ToString(), true) == 0) || (String.Compare(UOM, "kg", true) == 0)) m_UOM = CoalResourceUnit.kilogram;
                if ((String.Compare(UOM, CoalResourceUnit.megaJoule.ToString(), true) == 0) || (String.Compare(UOM, "MJ", true) == 0)) m_UOM = CoalResourceUnit.megaJoule;
                if ((String.Compare(UOM, CoalResourceUnit.millionBTU.ToString(), true) == 0) || (String.Compare(UOM, "MMTU", true) == 0)) m_UOM = CoalResourceUnit.millionBTU;
                if ((String.Compare(UOM, CoalResourceUnit.pound.ToString(), true) == 0) || (String.Compare(UOM, "lb", true) == 0)) m_UOM = CoalResourceUnit.pound;
                if ((String.Compare(UOM, CoalResourceUnit.ton.ToString(), true) == 0) || (String.Compare(UOM, "Short Ton", true) == 0)) m_UOM = CoalResourceUnit.ton;
                coalResource.UOM = m_UOM;
                this.Resources.Add(coalResource);
            }
            else if (resouceName == "OIL")
            {
                OilResource oilResource = new OilResource();
                oilResource.Name = resouceName;
                oilResource.Quantity = quantity;
                OilResourceUnit m_UOM = OilResourceUnit.BTU;
                if ((String.Compare(UOM, OilResourceUnit.kilogram.ToString(), true) == 0) || (String.Compare(UOM, "kg", true) == 0)) m_UOM = OilResourceUnit.kilogram;
                if ((String.Compare(UOM, OilResourceUnit.megaJoule.ToString(), true) == 0) || (String.Compare(UOM, "MJ", true) == 0)) m_UOM = OilResourceUnit.megaJoule;
                if ((String.Compare(UOM, OilResourceUnit.millionBTU.ToString(), true) == 0) || (String.Compare(UOM, "MMTU", true) == 0)) m_UOM = OilResourceUnit.millionBTU;
                if ((String.Compare(UOM, OilResourceUnit.pound.ToString(), true) == 0) || (String.Compare(UOM, "lb", true) == 0)) m_UOM = OilResourceUnit.pound;
                if ((String.Compare(UOM, OilResourceUnit.ton.ToString(), true) == 0) || (String.Compare(UOM, "Short Ton", true) == 0)) m_UOM = OilResourceUnit.ton;
                oilResource.UOM = m_UOM;
                this.Resources.Add(oilResource);
            }
            else if (resouceName == "WATER USE")
            {
                WaterResource waterResource = new WaterResource();
                waterResource.Name = resouceName;
                waterResource.Quantity = quantity;
                WaterResourceUnit m_UOM = WaterResourceUnit.gallon;
                if ((String.Compare(UOM, WaterResourceUnit.liter.ToString(), true) == 0) || (String.Compare(UOM, "liter", true) == 0)) m_UOM = WaterResourceUnit.liter;
                if ((String.Compare(UOM, WaterResourceUnit.millionGallon.ToString(), true) == 0) || (String.Compare(UOM, "mgal", true) == 0)) m_UOM = WaterResourceUnit.millionGallon;
                waterResource.UOM = m_UOM;
                this.Resources.Add(waterResource);
            }
            else
            {
                Release release = new Release();
                release.Name = resouceName;
                release.Quantity = quantity;
                release.UOM = UOM;
                this.Releases.Add(release);
            }
        }

        #region ITraciProcess Members

        string ITraciProcess.GetName()
        {
            return m_Name;
        }

        ITraciProduct ITraciProcess.GetProduct()
        {
            return m_Product;
        }

        string ITraciProcess.GetDescription()
        {
            return m_Description;
        }


        string[] ITraciProcess.ResourceList()
        {
            string[] retval = new string[Resources.Count];
            for (int n = 0; n < Resources.Count; n++)
            {
                retval[n] = Resources[n].Name;
            }
            return retval;
        }

        ITraciResource ITraciProcess.GetResource(string resourceName)
        {
            for (int n = 0; n < Resources.Count; n++)
            {
                if (resourceName == Resources[n].Name) return Resources[n];
            }
            throw new ArgumentException(resourceName + "was not found in the list of Products");
        }

        string[] ITraciProcess.ReleaseList()
        {
            string[] retval = new string[Releases.Count];
            for (int n = 0; n < Releases.Count; n++)
            {
                retval[n] = Releases[n].Name;
            }
            return retval;
        }

        ITraciRelease ITraciProcess.GetRelease(string releaseName)
        {
            for (int n = 0; n < Releases.Count; n++)
            {
                if (releaseName == Releases[n].Name) return Releases[n];
            }
            throw new ArgumentException(releaseName + "was not found in the list of Products");
        }
        #endregion

        #region ITraciIdentification Members

        string ITraciIdentification.GetName()
        {
            return m_Name;
        }

        string ITraciIdentification.GetFullName()
        {
            return this.FullName;
        }

        string ITraciIdentification.GetDescription()
        {
            return m_Description;
        }

        #endregion
    }
}

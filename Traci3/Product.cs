using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Traci3
{
    [System.Runtime.InteropServices.ComVisible(false)]
    class ProductTypeConverter : System.ComponentModel.TypeConverter
    {
        public override bool CanConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Type destinationType)
        {
            if ((typeof(Product)).IsAssignableFrom(destinationType))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        public override Object ConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, Object value, System.Type destinationType)
        {
            if ((typeof(System.String)).IsAssignableFrom(destinationType) && (typeof(Product).IsAssignableFrom(value.GetType())))
            {
                return ((Product)value).Name;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    };

    /// <summary>
    /// This class represents one of the products that TRACI will compare the Life Cycle Imapcts of.
    /// </summary>
    [Serializable]
    [System.ComponentModel.TypeConverterAttribute(typeof(ProductTypeConverter))]
    public class Product : ITraciProduct, ITraciIdentification
    {
        [NonSerialized]
        static int instanceNumber = 1;


        public Product()
        {
            m_Name = "Product" + instanceNumber++.ToString();
            m_Description = "";
            m_Processes = new Processes();
            m_Processes.ListChanged += new System.ComponentModel.ListChangedEventHandler(this.processes_ListChanged);
            m_Releases = new Releases();
            m_Resources = new Resources();
        }

        public Product(String name, TraciProject project)
        {
            m_Name = name;
            m_Description = "";
            m_Processes = new Processes();
            m_Processes.ListChanged += new System.ComponentModel.ListChangedEventHandler(this.processes_ListChanged);
            m_Releases = new Releases();
            m_Resources = new Resources();
            m_Project = project;
        }

        [System.Runtime.Serialization.OnDeserializing]
        private void ResetObjectGraphOnDeserialized(System.Runtime.Serialization.StreamingContext context)
        {
            m_Releases = new Releases();
            m_Resources = new Resources();
        }

        private void processes_ListChanged(object sender, System.ComponentModel.ListChangedEventArgs e)
        {
            if (e.ListChangedType == System.ComponentModel.ListChangedType.ItemAdded)
            {
                m_Processes[e.NewIndex].Product = this;
                Process process = m_Processes[e.NewIndex];
                m_Project.Processes.Add(process);
                //if (process.LifeCycleStage == LifeCycleStage.fillingPackagingDistribution) this.m_LifeCycleStages.FillingPackagingDistribution.Processes.Add(process);
                //if (process.LifeCycleStage == LifeCycleStage.materialManufacture) this.m_LifeCycleStages.MaterialManufacture.Processes.Add(process);
                //if (process.LifeCycleStage == LifeCycleStage.productFabrication) this.m_LifeCycleStages.ProductFabrication.Processes.Add(process);
                //if (process.LifeCycleStage == LifeCycleStage.rawMaterialsAcquisition) this.m_LifeCycleStages.RawMaterialsAcquisition.Processes.Add(process);
                //if (process.LifeCycleStage == LifeCycleStage.recycleWasteManagement) this.m_LifeCycleStages.RecycleWasteManagement.Processes.Add(process);
                //if (process.LifeCycleStage == LifeCycleStage.useReuseMaintenance) this.m_LifeCycleStages.UseReuseMaintenance.Processes.Add(process);
            }
        }

        string m_Name;
        [System.ComponentModel.CategoryAttribute("Identification")]
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

        [System.ComponentModel.CategoryAttribute("Identification")]
        public string FullName
        {
            get
            {
                return this.m_Project.Name + "." + this.Name;
            }
        }

        string m_Description;
        [System.ComponentModel.CategoryAttribute("Identification")]
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

        [NonSerialized]
        TraciProject m_Project;
        [System.ComponentModel.CategoryAttribute("Identification")]
        public TraciProject Project
        {
            get
            {
                return m_Project;
            }
            set
            {
                m_Project = value;
            }
        }

        Processes m_Processes = new Processes();
        public Processes Processes
        {
            get
            {
                foreach (Process process in m_Processes)
                    process.Product = this; 
                return m_Processes;
            }
        }

        //LifeCycleStageCollection m_LifeCycleStages;
        //public LifeCycleStageCollection LifeCycleStages
        //{
        //    get
        //    {
        //        return m_LifeCycleStages;
        //    }
        //}


        //[System.ComponentModel.CategoryAttribute("Life Cycle Stages")]
        //public LifeCycleStage FillingPackagingDistribution
        //{
        //    get
        //    {
        //        return this.m_LifeCycleStages.FillingPackagingDistribution;
        //    }
        //}

        //[System.ComponentModel.CategoryAttribute("Life Cycle Stages")]
        //public LifeCycleStage MaterialManufacture
        //{
        //    get
        //    {
        //        return this.m_LifeCycleStages.MaterialManufacture;
        //    }
        //}

        //[System.ComponentModel.CategoryAttribute("Life Cycle Stages")]
        //public LifeCycleStage ProductFabrication
        //{
        //    get
        //    {
        //        return this.m_LifeCycleStages.ProductFabrication;
        //    }
        //}

        //[System.ComponentModel.CategoryAttribute("Life Cycle Stages")]
        //public LifeCycleStage RawMaterialsAcquisition
        //{
        //    get
        //    {
        //        return this.m_LifeCycleStages.RawMaterialsAcquisition;
        //    }
        //}

        //[System.ComponentModel.CategoryAttribute("Life Cycle Stages")]
        //public LifeCycleStage RecycleWasteManagement
        //{
        //    get
        //    {
        //        return this.m_LifeCycleStages.RecycleWasteManagement;
        //    }
        //}

        //[System.ComponentModel.CategoryAttribute("Life Cycle Stages")]
        //public LifeCycleStage UseReuseMaintenance
        //{
        //    get
        //    {
        //        return this.m_LifeCycleStages.UseReuseMaintenance;
        //    }
        //}

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double GlobalWarmingPotential
        {
            get
            {
                double retVal = 0;
                foreach (Process process in m_Processes) retVal = retVal + process.GlobalWarmingPotential;
                return retVal;
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double AcidificationAir
        {
            get
            {
                double retVal = 0;
                foreach (Process process in m_Processes) retVal = retVal + process.AcidificationAir;
                return retVal;
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double AcidificationWater
        {
            get
            {
                double retVal = 0;
                foreach (Process process in m_Processes) retVal = retVal + process.AcidificationWater;
                return retVal;
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double HumanHealthCriteria
        {
            get
            {
                double retVal = 0;
                foreach (Process process in m_Processes) retVal = retVal + process.HumanHealthCriteria;
                return retVal;
            }
        }


        [System.ComponentModel.CategoryAttribute("Impact")]
        public double EutrophicationAir
        {
            get
            {
                double retVal = 0;
                foreach (Process process in m_Processes) retVal = retVal + process.EutrophicationAir;
                return retVal;
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double EutrophicationWater
        {
            get
            {
                double retVal = 0;
                foreach (Process process in m_Processes) retVal = retVal + process.EutrophicationWater;
                return retVal;
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double OzoneDepletion
        {
            get
            {
                double retVal = 0;
                foreach (Process process in m_Processes) retVal = retVal + process.OzoneDepletion;
                return retVal;
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double SmogAir
        {
            get
            {
                double retVal = 0;
                foreach (Process process in m_Processes) retVal = retVal + process.SmogAir;
                return retVal;
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double EcotoxCFairUfreshwater
        {
            get
            {
                double retVal = 0;
                foreach (Process process in m_Processes) retVal = retVal + process.EcotoxCFairUfreshwater;
                return retVal;
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double EcotoxCFairCfreshwater
        {
            get
            {
                double retVal = 0;
                foreach (Process process in m_Processes) retVal = retVal + process.EcotoxCFairCfreshwater;
                return retVal;
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double EcotoxCFfreshWaterCfreshwater
        {
            get
            {
                double retVal = 0;
                foreach (Process process in m_Processes) retVal = retVal + process.EcotoxCFfreshWaterCfreshwater;
                return retVal;
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double EcotoxCFfreshWaterUfreshwater
        {
            get
            {
                double retVal = 0;
                foreach (Process process in m_Processes) retVal = retVal + process.EcotoxCFairUfreshwater;
                return retVal;
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double EcotoxCFseaWaterCfreshwater
        {
            get
            {
                double retVal = 0;
                foreach (Process process in m_Processes) retVal = retVal + process.EcotoxCFfreshWaterCfreshwater;
                return retVal;
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double EcotoxCFnativeSoilCfreshwater
        {
            get
            {
                double retVal = 0;
                foreach (Process process in m_Processes) retVal = retVal + process.EcotoxCFnativeSoilCfreshwater;
                return retVal;
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double EcotoxCFagriculturalSoilCfreshwater
        {
            get
            {
                double retVal = 0;
                foreach (Process process in m_Processes) retVal = retVal + process.EcotoxCFagriculturalSoilCfreshwater;
                return retVal;
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double HumanHealthCancer
        {
            get
            {
                double retVal = 0;
                foreach (Process process in m_Processes) retVal = retVal + process.HumanHealthCancer;
                return retVal;
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double HumanHealthNonCancer
        {
            get
            {
                double retVal = 0;
                foreach (Process process in m_Processes) retVal = retVal + process.HumanHealthNonCancer;
                return retVal;
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double HumanHealthUrbanAirCancer
        {
            get
            {
                double retVal = 0;
                foreach (Process process in m_Processes) retVal = retVal + process.HumanHealthUrbanAirCancer;
                return retVal;
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double HumanHealthUrbanAirNonCancer
        {
            get
            {
                double retVal = 0;
                foreach (Process process in m_Processes) retVal = retVal + process.HumanHealthUrbanAirNonCancer;
                return retVal;
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double HumanHealthRuralAirCancer
        {
            get
            {
                double retVal = 0;
                foreach (Process process in m_Processes) retVal = retVal + process.HumanHealthRuralAirCancer;
                return retVal;
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double HumanHealthRuralAirNonCancer
        {
            get
            {
                double retVal = 0;
                foreach (Process process in m_Processes) retVal = retVal + process.HumanHealthRuralAirNonCancer;
                return retVal;
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double HumanHealthFreshwaterCancer
        {
            get
            {
                double retVal = 0;
                foreach (Process process in m_Processes) retVal = retVal + process.HumanHealthFreshwaterCancer;
                return retVal;
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double HumanHealthFreshwaterNonCancer
        {
            get
            {
                double retVal = 0;
                foreach (Process process in m_Processes) retVal = retVal + process.HumanHealthFreshwaterNonCancer;
                return retVal;
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double HumanHealthSeawaterCancer
        {
            get
            {
                double retVal = 0;
                foreach (Process process in m_Processes) retVal = retVal + process.HumanHealthSeawaterCancer;
                return retVal;
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double HumanHealthSeawaterNonCancer
        {
            get
            {
                double retVal = 0;
                foreach (Process process in m_Processes) retVal = retVal + process.HumanHealthSeawaterNonCancer;
                return retVal;
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double HumanHealthNativeSoilCancer
        {
            get
            {
                double retVal = 0;
                foreach (Process process in m_Processes) retVal = retVal + process.HumanHealthNativeSoilCancer;
                return retVal;
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double HumanHealthNativeSoilNonCancer
        {
            get
            {
                double retVal = 0;
                foreach (Process process in m_Processes) retVal = retVal + process.HumanHealthNativeSoilNonCancer;
                return retVal;
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double HumanHealthAgriculturalSoilCancer
        {
            get
            {
                double retVal = 0;
                foreach (Process process in m_Processes) retVal = retVal + process.HumanHealthAgriculturalSoilCancer;
                return retVal;
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double HumanHealthAgriculturalSoilNonCancer
        {
            get
            {
                double retVal = 0;
                foreach (Process process in m_Processes) retVal = retVal + process.HumanHealthAgriculturalSoilNonCancer;
                return retVal;
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double LandUse
        {
            get
            {
                double retVal = 0;
                foreach (Process process in m_Processes) retVal = retVal + process.LandUse;
                return retVal;
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double FossilFuelUse
        {
            get
            {
                double retVal = 0;
                foreach (Process process in m_Processes) retVal = retVal + process.FossilFuelUse;
                return retVal;
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double WaterUse
        {
            get
            {
                double retVal = 0;
                foreach (Process process in m_Processes) retVal = retVal + process.WaterUse;
                return retVal;
            }
        }

        public double GetReleaseValue(string impact)
        {
            if (impact == "GlobalWarmingPotential") return this.GlobalWarmingPotential;
            if (impact == "AcidificationAir") return this.AcidificationAir;
            if (impact == "AcidificationWater") return this.AcidificationWater;
            if (impact == "HumanHealthCriteria") return this.HumanHealthCriteria;
            if (impact == "Eutrophication") return this.EutrophicationAir + this.EutrophicationAir;
            if (impact == "EutrophicationAir") return this.EutrophicationAir;
            if (impact == "EutrophicationWater") return this.EutrophicationWater;
            if (impact == "OzoneDepletion") return this.OzoneDepletion;
            if (impact == "SmogAir") return this.SmogAir;
            if (impact == "EcoToxicity")
            {
                double retVal = this.EcotoxCFagriculturalSoilCfreshwater + this.EcotoxCFairCfreshwater + this.EcotoxCFairUfreshwater + this.EcotoxCFfreshWaterCfreshwater;
                retVal = retVal + this.EcotoxCFfreshWaterUfreshwater + this.EcotoxCFnativeSoilCfreshwater + this.EcotoxCFseaWaterCfreshwater;
                return retVal;
            }
            if (impact == "EcotoxCFairUfreshwater") return this.EcotoxCFairUfreshwater;
            if (impact == "EcotoxCFairCfreshwater") return this.EcotoxCFairCfreshwater;
            if (impact == "EcotoxCFfreshWaterCfreshwater") return this.EcotoxCFfreshWaterCfreshwater;
            if (impact == "EcotoxCFfreshWaterUfreshwater") return this.EcotoxCFfreshWaterUfreshwater;
            if (impact == "EcotoxCFseaWaterCfreshwater") return this.EcotoxCFseaWaterCfreshwater;
            if (impact == "EcotoxCFnativeSoilCfreshwater") return this.EcotoxCFnativeSoilCfreshwater;
            if (impact == "EcotoxCFagriculturalSoilCfreshwater") return this.EcotoxCFagriculturalSoilCfreshwater;
            if (impact == "HumanHealthCancer")
            {
                return this.HumanHealthUrbanAirCancer + this.HumanHealthSeawaterCancer + this.HumanHealthRuralAirCancer + this.HumanHealthNativeSoilCancer + this.HumanHealthFreshwaterCancer + this.HumanHealthAgriculturalSoilCancer;
            }
            if (impact == "HumanHealthNonCancer")
            {
                return this.HumanHealthUrbanAirNonCancer + this.HumanHealthSeawaterNonCancer + this.HumanHealthRuralAirNonCancer + this.HumanHealthNativeSoilNonCancer + this.HumanHealthFreshwaterNonCancer + this.HumanHealthAgriculturalSoilNonCancer;
            }
            if (impact == "HumanHealthUrbanAirCancer") return this.HumanHealthUrbanAirCancer;
            if (impact == "HumanHealthUrbanAirNonCancer") return this.HumanHealthUrbanAirNonCancer;
            if (impact == "HumanHealthRuralAirCancer") return this.HumanHealthRuralAirCancer;
            if (impact == "HumanHealthRuralAirNonCancer") return this.HumanHealthRuralAirNonCancer;
            if (impact == "HumanHealthFreshwaterCancer") return this.HumanHealthFreshwaterCancer;
            if (impact == "HumanHealthFreshwaterNonCancer") return this.HumanHealthFreshwaterNonCancer;
            if (impact == "HumanHealthSeawaterCancer") return this.HumanHealthSeawaterCancer;
            if (impact == "HumanHealthSeawaterNonCancer") return this.HumanHealthSeawaterNonCancer;
            if (impact == "HumanHealthNativeSoilCancer") return this.HumanHealthNativeSoilCancer;
            if (impact == "HumanHealthNativeSoilNonCancer") return this.HumanHealthNativeSoilNonCancer;
            if (impact == "HumanHealthAgriculturalSoilCancer") return this.HumanHealthAgriculturalSoilCancer;
            return this.HumanHealthAgriculturalSoilNonCancer;

        }

        //public double[] ChemicalSubstances
        //{
        //    get
        //    {
        //        double[] retVal = new double[25];
        //        retVal[0] = this.GlobalWarmingPotential;
        //        retVal[0] = this.Acidification;
        //        retVal[1] = this.HumanHealthCriteria;
        //        retVal[2] = this.EutrophicationAir;
        //        retVal[3] = this.EutrophicationWater;
        //        retVal[4] = this.OzoneDepletion;
        //        retVal[5] = this.SmogAir;
        //        retVal[6] = this.EcotoxCFairUfreshwater;
        //        retVal[7] = this.EcotoxCFairCfreshwater;
        //        retVal[8] = this.EcotoxCFfreshWaterCfreshwater;
        //        retVal[9] = this.EcotoxCFfreshWaterUfreshwater;
        //        retVal[10] = this.EcotoxCFseaWaterCfreshwater;
        //        retVal[11] = this.EcotoxCFnativeSoilCfreshwater;
        //        retVal[12] = this.EcotoxCFagriculturalSoilCfreshwater;
        //        retVal[13] = this.HumanHealthUrbanAirCancer;
        //        retVal[14] = this.HumanHealthUrbanAirNonCancer;
        //        retVal[15] = this.HumanHealthRuralAirCancer;
        //        retVal[16] = this.HumanHealthRuralAirNonCancer;
        //        retVal[17] = this.HumanHealthFreshwaterCancer;
        //        retVal[18] = this.HumanHealthFreshwaterNonCancer;
        //        retVal[19] = this.HumanHealthSeawaterCancer;
        //        retVal[20] = this.HumanHealthSeawaterNonCancer;
        //        retVal[21] = this.HumanHealthNativeSoilCancer;
        //        retVal[22] = this.HumanHealthNativeSoilNonCancer;
        //        retVal[23] = this.HumanHealthAgriculturalSoilCancer;
        //        retVal[24] = this.HumanHealthAgriculturalSoilNonCancer;
        //        return retVal;
        //    }
        //}

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
                {
                    bool add = true;
                    foreach (Object obj in list)
                        if (obj.ToString() == release.Name) add = false;                 
                    if (add) list.Add(release.Name);
                }
            }
            if (impactCategory == "FOSSIL FUEL" && this.FossilFuelUse != 0) list.Add("FOSSIL FUEL");
            if (impactCategory == "LAND USE" && this.LandUse != 0) list.Add("LAND USE");
            if (impactCategory == "WATER USE" && this.WaterUse != 0) list.Add("WATER USE");
            string[] retVal = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                retVal[i] = list[i].ToString();
            }
            return retVal;
        }

        public double GetImpactValueForRelaseAndImpactCategory(string releaseName, String impactCategory)
        {
            foreach (Release release in m_Releases)
            {
                if (releaseName == release.Name)
                    return release.GetImpactValue(impactCategory);
            }
            if (impactCategory == "FOSSIL FUEL") return this.FossilFuelUse;
            if (impactCategory == "LAND USE") return this.LandUse;
            if (impactCategory == "WATER USE") return this.WaterUse;
            return 0;
        }

        public string[] ReleaseNames
        {
            get 
            {
                return new string[0];
            }
        }

        [NonSerialized]
        Releases m_Releases = new Releases();
        [System.ComponentModel.Browsable(false)]
        public Releases Releases
        {
            get
            {
                return m_Releases;
            }
        }

        [NonSerialized]
        Resources m_Resources = new Resources();
        [System.ComponentModel.Browsable(false)]
        public Resources Resources
        {
            get
            {
                return m_Resources;
            }
        }


        public bool ContainsProcess(String processName)
        {
            foreach (Process process in m_Processes)
            {
                if (process.Name == processName) return true;
            }
            return false;
        }

        public void AddProcess(string name, LifeCycleStage stage, Location loc)
       {
            m_Processes.Add(new Process(name, stage, loc, this)); 
        }

        public Process GetProcess(String processName)
        {
            foreach (Process process in m_Processes)
            {
                if (process.Name == processName) return process;
            }
            return null;
        }

        #region ITraciProduct Members

        string ITraciProduct.GetName()
        {
            return m_Name;
        }

        string ITraciProduct.GetDescription()
        {
            return m_Description;
        }

        string[] ITraciProduct.ProcessList()
        {
            string[] retval = new string[Processes.Count];
            for (int n = 0; n < Processes.Count; n++)
            {
                retval[n] = Processes[n].Name;
            }
            return retval;
        }

        ITraciProcess ITraciProduct.GetProcess(string processName)
        {
            for (int n = 0; n < Processes.Count; n++)
            {
                if (processName == Processes[n].Name) return Processes[n];
            }
            throw new ArgumentException(processName + "was not found in the list of Products");
        }


        string[] ITraciProduct.ResourceList()
        {
            string[] retval = new string[Resources.Count];
            for (int n = 0; n < Resources.Count; n++)
            {
                retval[n] = Resources[n].Name;
            }
            return retval;
        }

        ITraciResource ITraciProduct.GetResource(string resourceName)
        {
            for (int n = 0; n < Resources.Count; n++)
            {
                if (resourceName == Resources[n].Name) return Resources[n];
            }
            throw new ArgumentException(resourceName + "was not found in the list of Products");
        }

        string[] ITraciProduct.ReleaseList()
        {
            string[] retval = new string[Releases.Count];
            for (int n = 0; n < Releases.Count; n++)
            {
                retval[n] = Releases[n].Name;
            }
            return retval;
        }

        ITraciRelease ITraciProduct.GetRelease(string releaseName)
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

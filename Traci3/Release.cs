using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Traci3
{
    class ReleaseNameConverter : System.ComponentModel.StringConverter
    {
        public override bool GetStandardValuesSupported(System.ComponentModel.ITypeDescriptorContext context)
        {
            return true;
        }

        public override System.ComponentModel.TypeConverter.StandardValuesCollection GetStandardValues(System.ComponentModel.ITypeDescriptorContext context)
        {
            return new System.ComponentModel.TypeConverter.StandardValuesCollection(ImpactFactorCollection.ChemicalSubstances);
        }

        public override bool GetStandardValuesExclusive(System.ComponentModel.ITypeDescriptorContext context)
        {
            return true;
        }
    };

    //class ReleaseCASnumberConverter : System.ComponentModel.StringConverter
    //{
    //    public override bool GetStandardValuesSupported(System.ComponentModel.ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }

    //    public override System.ComponentModel.TypeConverter.StandardValuesCollection GetStandardValues(System.ComponentModel.ITypeDescriptorContext context)
    //    {
    //        return new System.ComponentModel.TypeConverter.StandardValuesCollection(ImpactFactorCollection.casNumbers);
    //    }

    //    public override bool GetStandardValuesExclusive(System.ComponentModel.ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //};

    class ReleaseProcessConverter : System.ComponentModel.StringConverter
    {
        public override bool GetStandardValuesSupported(System.ComponentModel.ITypeDescriptorContext context)
        {
            return true;
        }

        public override System.ComponentModel.TypeConverter.StandardValuesCollection GetStandardValues(System.ComponentModel.ITypeDescriptorContext context)
        {
            return new System.ComponentModel.TypeConverter.StandardValuesCollection(((Release)context.Instance).Process.Product.Processes.Names);
        }

        public override bool GetStandardValuesExclusive(System.ComponentModel.ITypeDescriptorContext context)
        {
            return true;
        }
    };

    /// <summary>
    /// This class represents the release of a chemical to the environment.
    /// </summary>
    /// <remarks>
    /// <para>
    /// </para>
    /// </remarks>
    [Serializable]
    public class Release : ITraciRelease, ITraciIdentification
    {
        public Release()
        {
            m_Description = String.Empty;
            m_Media = releaseMedia.air;
            m_Quantity = 0.0;
            m_UOM = releaseUnit.kilogram;
            m_Process = null;
        }

        [NonSerialized]
        ReleaseImpactFactor m_factor;
        string m_factorName = string.Empty;

        /// <summary>
        /// The name of the Release.
        /// </summary>
        /// <remarks>
        /// </remarks>
        [System.ComponentModel.TypeConverter(typeof(ReleaseNameConverter))]
        [System.ComponentModel.CategoryAttribute("Input")]
        public string Name
        {
            get
            {
                //if (m_factor == null) return String.Empty;
                //string[] impacts = ImpactFactorCollection.ChemicalSubstances;
                //if (m_Name == "SILVER COMPOUNDS") m_Name = "SILVER(I)";
                //if (m_Name == "CFC 113") m_Name = "CFC-113";
                //if (m_Name == "NICKEL COMPOUNDS") m_Name = "NICKEL(II)";
                //if (m_Name == "NICKEL") m_Name = "NICKEL(II)";
                //if (m_Name == "HCFC 123") m_Name = "HCFC-123";
                //if (m_Name == "BARIUM") m_Name = "BARIUM(II)";
                //if (m_Name == "NITROGEN OXIDES (NOX)") m_Name = "NITROGEN OXIDES";
                //if (m_Name == "HCFC 225CA") m_Name = "HCFC-225CA";
                //if (m_Name == "HCFC 141B") m_Name = "HCFC-141B";
                //if (m_Name == "1,1-DIMETHYLHYDRAZINE") m_Name = "1,1-DIMETHYL HYDRAZINE";
                //if (m_Name == "XYLENE (MIXED ISOMERS)") m_Name = "XYLENES";
                //if (m_Name == "HCFC 124") m_Name = "HCFC-124";
                //if (m_Name == "CFC 11") m_Name = "CFC-11";
                //if (m_Name == "MERCURY") m_Name = "MERCURY(II)";
                //if (m_Name == "LEAD COMPOUNDS") m_Name = "LEAD(II)";
                //if (m_Name == "LEAD") m_Name = "LEAD(II)";
                //if (m_Name == "CHROMIUM") m_Name = "CHROMIUM(VI)";
                //if (m_Name == "HFC-4310MEE") m_Name = "HFC-43-10MEE";
                //if (m_Name == "CFC 12") m_Name = "CFC-12";
                //if (m_Name == "3,5,5-TRIMETHYL-2-CYCLOHEXENE-1-ONE") m_Name = "CYCLOHEXANOL, 3,3,5-TRIMETHYL-";
                //if (m_Name == "ZINC") m_Name = "ZINC(II)";
                //if (m_Name == "HCFC 142B") m_Name = "HCFC-142B";
                //if (m_Name == "DDT") m_Name = "O,P'-DDT";
                //if (m_Name	== "CFC 114") m_Name = "CFC-114";
                //if (m_Name == "METHYLENE CHLORIDE") m_Name = "METHYLENECHLORIDE ";
                //m_factor = ImpactFactorCollection.GetImpact(m_Name);
                //m_factorName = m_factor.SubstanceName;
                m_factor = ImpactFactorCollection.GetImpact(m_factorName);
                return m_factorName;
            }
            set
            {
                if (value == "SILVER COMPOUNDS") value = "SILVER(I)";
                if (value == "CFC 113") value = "CFC-113";
                if (value == "NICKEL COMPOUNDS") value = "NICKEL(II)";
                if (value == "NICKEL") value = "NICKEL(II)";
                if (value == "HCFC 123") value = "HCFC-123";
                if (value == "BARIUM") value = "BARIUM(II)";
                if (value == "NITROGEN OXIDES (NOX)") value = "NITROGEN OXIDES";
                if (value == "HCFC 225CA") value = "HCFC-225CA";
                if (value == "HCFC 141B") value = "HCFC-141B";
                if (value == "1,1-DIMETHYLHYDRAZINE") value = "1,1-DIMETHYL HYDRAZINE";
                if (value == "XYLENE (MIXED ISOMERS)") value = "XYLENES";
                if (value == "HCFC 124") value = "HCFC-124";
                if (value == "CFC 11") value = "CFC-11";
                if (value == "MERCURY") value = "MERCURY(II)";
                if (value == "LEAD COMPOUNDS") value = "LEAD(II)";
                if (value == "LEAD") value = "LEAD(II)";
                if (value == "CHROMIUM") value = "CHROMIUM(VI)";
                if (value == "HFC-4310MEE") value = "HFC-43-10MEE";
                if (value == "CFC 12") value = "CFC-12";
                if (value == "3,5,5-TRIMETHYL-2-CYCLOHEXENE-1-ONE") value = "CYCLOHEXANOL, 3,3,5-TRIMETHYL-";
                if (value == "ZINC") value = "ZINC(II)";
                if (value == "HCFC 142B") value = "HCFC-142B";
                if (value == "DDT") value = "O,P'-DDT";
                if (value == "CFC 114") value = "CFC-114";
                if (value == "METHYLENE CHLORIDE") value = "METHYLENECHLORIDE ";
                m_factor = ImpactFactorCollection.GetImpact(value);
                m_factorName = m_factor.SubstanceName;
            }
        }

        /// <summary>
        /// The full name of the Release object.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The full name is in the form of <code>Project Name.Product Name.Process Name.Release Name</code>.
        /// </para>
        /// </remarks>
        /// <value>The full name of the Release.</value>
        [System.ComponentModel.CategoryAttribute("Identification")]
        public string FullName
        {
            get
            {
                if (m_factor == null) return String.Empty;
                return this.Process.Product.Project.Name + "." + this.Process.Product.Name + "." + this.Process.Name + "." + this.m_factor.SubstanceName;
            }
        }

        /// <summary>
        /// The <see cref="Process"/> that generates this Release
        /// </summary>
        [System.ComponentModel.Browsable(false)]
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

        /// <summary>
        /// A description of the Release.
        /// </summary>
        string m_Description;
        [System.ComponentModel.CategoryAttribute("Input")]
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


        releaseMedia m_Media;
        [System.ComponentModel.CategoryAttribute("Media")]
        public releaseMedia Media
        {
            get
            {
                return m_Media;
            }
            set
            {
                m_Media = value;
            }
        }

        [System.ComponentModel.CategoryAttribute("Identification")]
        public string CASNumber
        {
            get
            {
                if (m_factor == null) return String.Empty;
                return m_factor.casNumber;
            }
            //set
            //{
            //    m_CASNo = value;
            //}
        }

        double m_Quantity;
        [System.ComponentModel.CategoryAttribute("Input")]
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

        releaseUnit m_UOM;
        [
        //System.ComponentModel.TypeConverter(typeof(ReleaseNameConverter)),
        System.ComponentModel.CategoryAttribute("Input")
        ]
        public String UOM
        {
            get
            {
                return m_UOM.ToString();
            }
            set
            {
                if ((String.Compare(value, releaseUnit.gram.ToString(), true) == 0) || (String.Compare(value, "gm", true) == 0)) m_UOM = releaseUnit.gram;
                if ((String.Compare(value, releaseUnit.kilogram.ToString(), true) == 0) || (String.Compare(value, "kg", true) == 0)) m_UOM = releaseUnit.kilogram;
                if ((String.Compare(value, releaseUnit.megaGram.ToString(), true) == 0) || (String.Compare(value, "NoUnit", true) == 0)) m_UOM = releaseUnit.megaGram;
                if ((String.Compare(value, releaseUnit.milligram.ToString(), true) == 0) || (String.Compare(value, "mg", true) == 0)) m_UOM = releaseUnit.milligram;
                if ((String.Compare(value, releaseUnit.pound.ToString(), true) == 0) || (String.Compare(value, "lb", true) == 0)) m_UOM = releaseUnit.pound;
                if ((String.Compare(value, releaseUnit.ton.ToString(), true) == 0) || (String.Compare(value, "ton", true) == 0)) m_UOM = releaseUnit.ton;
            }
        }

        [
        //System.ComponentModel.TypeConverter(typeof(ReleaseNameConverter)),
        System.ComponentModel.CategoryAttribute("Input")
        ]
        public LifeCycleStage LifeCycleStage
        {
            get
            {
                return m_Process.LifeCycleStage;
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double GlobalWarmingPotential
        {
            get
            {
                //String location = this.Process.Location.GeogID.ToString();
                //System.Data.DataSet data = this.Process.Product.Project.Traci2Data;
                //System.Data.DataTable chemicals = data.Tables["tblkupChem"];
                //String chemID = String.Empty;
                //foreach (System.Data.DataRow row in chemicals.Rows)
                //{
                //    if (row["Name"].ToString() == this.Name)
                //    {
                //        chemID = row["CHEMID"].ToString();
                //        break;
                //    }
                //}
                //System.Data.DataTable impacts = data.Tables["tblImpacts"];
                //string value = String.Empty;
                //foreach (System.Data.DataRow row in impacts.Rows)
                //{
                //    if ((row["GeoLocationID"].ToString() == location) && (row["CHEMID"].ToString() == location) || (row["FactorID"].ToString() == "1"))
                //    {
                //        chemID = row["Factor Value"].ToString();
                //        break;
                //    }
                //}
                //if (value != string.Empty)
                //{
                //    return double.Parse(value) * this.m_Quantity * ConversionFactors.Factor(this.m_UOM);
                //}
                if (Double.IsNaN(this.m_factor.GlobalWarmingPotential)) return 0;
                return this.m_factor.GlobalWarmingPotential * this.m_Quantity * ConversionFactors.Factor(this.m_UOM);
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double AcidificationAir
        {
            get
            {
                if (Double.IsNaN(this.m_factor.AcidificationAir)) return 0;
                return this.m_factor.AcidificationAir * this.m_Quantity * ConversionFactors.Factor(this.m_UOM);
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double AcidificationWater
        {
            get
            {
                if (Double.IsNaN(this.m_factor.AcidificationWater)) return 0;
                return this.m_factor.AcidificationWater * this.m_Quantity * ConversionFactors.Factor(this.m_UOM);
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double HumanHealthCriteria
        {
            get
            {
                if (Double.IsNaN(this.m_factor.HumanHealthCriteria)) return 0;
                return this.m_factor.HumanHealthCriteria * this.m_Quantity * ConversionFactors.Factor(this.m_UOM);
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double EutrophicationAir
        {
            get
            {
                if (Double.IsNaN(this.m_factor.EutrophicationAir)) return 0;
                return this.m_factor.EutrophicationAir * this.m_Quantity * ConversionFactors.Factor(this.m_UOM);
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double EutrophicationWater
        {
            get
            {
                if (Double.IsNaN(this.m_factor.EutrophicationWater)) return 0;
                return this.m_factor.EutrophicationWater * this.m_Quantity * ConversionFactors.Factor(this.m_UOM);
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double OzoneDepletion
        {
            get
            {
                if (Double.IsNaN(this.m_factor.OzoneDepletion)) return 0;
                return this.m_factor.OzoneDepletion * this.m_Quantity * ConversionFactors.Factor(this.m_UOM);
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double SmogAir
        {
            get
            {
                if (Double.IsNaN(this.m_factor.SmogAir)) return 0;
                return this.m_factor.SmogAir * this.m_Quantity * ConversionFactors.Factor(this.m_UOM);
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double EcotoxCFairUfreshwater
        {
            get
            {
                if (Double.IsNaN(this.m_factor.EcotoxCFairUfreshwater)) return 0;
                return this.m_factor.EcotoxCFairUfreshwater * this.m_Quantity * ConversionFactors.Factor(this.m_UOM);
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double EcotoxCFairCfreshwater
        {
            get
            {
                if (Double.IsNaN(this.m_factor.EcotoxCFairCfreshwater)) return 0;
                return this.m_factor.EcotoxCFairCfreshwater * this.m_Quantity * ConversionFactors.Factor(this.m_UOM);
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double EcotoxCFfreshWaterCfreshwater
        {
            get
            {
                if (Double.IsNaN(this.m_factor.EcotoxCFfreshWaterCfreshwater)) return 0;
                return this.m_factor.EcotoxCFfreshWaterCfreshwater * this.m_Quantity * ConversionFactors.Factor(this.m_UOM);
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double EcotoxCFfreshWaterUfreshwater
        {
            get
            {
                if (Double.IsNaN(this.m_factor.EcotoxCFfreshWaterUfreshwater)) return 0;
                return this.m_factor.EcotoxCFfreshWaterUfreshwater * this.m_Quantity * ConversionFactors.Factor(this.m_UOM);
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double EcotoxCFseaWaterCfreshwater
        {
            get
            {
                if (Double.IsNaN(this.m_factor.EcotoxCFseaWaterCfreshwater)) return 0;
                return this.m_factor.EcotoxCFseaWaterCfreshwater * this.m_Quantity * ConversionFactors.Factor(this.m_UOM);
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double EcotoxCFnativeSoilCfreshwater
        {
            get
            {
                if (Double.IsNaN(this.m_factor.EcotoxCFnativeSoilCfreshwater)) return 0;
                return this.m_factor.EcotoxCFnativeSoilCfreshwater * this.m_Quantity * ConversionFactors.Factor(this.m_UOM);
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double EcotoxCFagriculturalSoilCfreshwater
        {
            get
            {
                if (Double.IsNaN(this.m_factor.EcotoxCFagriculturalSoilCfreshwater)) return 0;
                return this.m_factor.GlobalWarmingPotential * this.m_Quantity * ConversionFactors.Factor(this.m_UOM);
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public DataQualityFlag EcoToxFlag
        {
            get
            {
                return this.m_factor.EcoToxFlag;
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double HumanHealthCancer
        {
            get
            {
                return this.HumanHealthAgriculturalSoilCancer + this.HumanHealthFreshwaterCancer + this.HumanHealthNativeSoilCancer + this.HumanHealthRuralAirCancer + this.HumanHealthSeawaterCancer + this.HumanHealthUrbanAirCancer;
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double HumanHealthNonCancer
        {
            get
            {
                return this.HumanHealthAgriculturalSoilNonCancer + this.HumanHealthFreshwaterNonCancer + this.HumanHealthNativeSoilNonCancer + this.HumanHealthRuralAirNonCancer + this.HumanHealthSeawaterNonCancer + this.HumanHealthUrbanAirNonCancer;
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double HumanHealthUrbanAirCancer
        {
            get
            {
                if (Double.IsNaN(this.m_factor.HumanHealthUrbanAirCancer)) return 0;
                return this.m_factor.HumanHealthUrbanAirCancer * this.m_Quantity * ConversionFactors.Factor(this.m_UOM);
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double HumanHealthUrbanAirNonCancer
        {
            get
            {
                if (Double.IsNaN(this.m_factor.HumanHealthUrbanAirNonCancer)) return 0;
                return this.m_factor.HumanHealthUrbanAirNonCancer * this.m_Quantity * ConversionFactors.Factor(this.m_UOM);
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double HumanHealthRuralAirCancer
        {
            get
            {
                if (Double.IsNaN(this.m_factor.HumanHealthRuralAirCancer)) return 0;
                return this.m_factor.HumanHealthRuralAirCancer * this.m_Quantity * ConversionFactors.Factor(this.m_UOM);
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double HumanHealthRuralAirNonCancer
        {
            get
            {
                if (Double.IsNaN(this.m_factor.HumanHealthRuralAirNonCancer)) return 0;
                return this.m_factor.HumanHealthRuralAirNonCancer * this.m_Quantity * ConversionFactors.Factor(this.m_UOM);
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double HumanHealthFreshwaterCancer
        {
            get
            {
                if (Double.IsNaN(this.m_factor.HumanHealthFreshwaterCancer)) return 0;
                return this.m_factor.HumanHealthFreshwaterCancer * this.m_Quantity * ConversionFactors.Factor(this.m_UOM);
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double HumanHealthFreshwaterNonCancer
        {
            get
            {
                if (Double.IsNaN(this.m_factor.HumanHealthFreshwaterNonCancer)) return 0;
                return this.m_factor.HumanHealthFreshwaterNonCancer * this.m_Quantity * ConversionFactors.Factor(this.m_UOM);
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double HumanHealthSeawaterCancer
        {
            get
            {
                if (Double.IsNaN(this.m_factor.HumanHealthSeawaterCancer)) return 0;
                return this.m_factor.HumanHealthSeawaterCancer * this.m_Quantity * ConversionFactors.Factor(this.m_UOM);
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double HumanHealthSeawaterNonCancer
        {
            get
            {
                if (Double.IsNaN(this.m_factor.HumanHealthSeawaterNonCancer)) return 0;
                return this.m_factor.HumanHealthSeawaterNonCancer * this.m_Quantity * ConversionFactors.Factor(this.m_UOM);
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double HumanHealthNativeSoilCancer
        {
            get
            {
                if (Double.IsNaN(this.m_factor.HumanHealthNativeSoilCancer)) return 0;
                return this.m_factor.HumanHealthNativeSoilCancer * this.m_Quantity * ConversionFactors.Factor(this.m_UOM);
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double HumanHealthNativeSoilNonCancer
        {
            get
            {
                if (Double.IsNaN(this.m_factor.HumanHealthNativeSoilNonCancer)) return 0;
                return this.m_factor.HumanHealthNativeSoilNonCancer * this.m_Quantity * ConversionFactors.Factor(this.m_UOM);
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double HumanHealthAgriculturalSoilCancer
        {
            get
            {
                if (Double.IsNaN(this.m_factor.HumanHealthAgriculturalSoilCancer)) return 0;
                return this.m_factor.HumanHealthAgriculturalSoilCancer * this.m_Quantity * ConversionFactors.Factor(this.m_UOM);
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public double HumanHealthAgriculturalSoilNonCancer
        {
            get
            {
                if (Double.IsNaN(this.m_factor.HumanHealthAgriculturalSoilNonCancer)) return 0;
                return this.m_factor.HumanHealthAgriculturalSoilNonCancer * this.m_Quantity * ConversionFactors.Factor(this.m_UOM);
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public DataQualityFlag CFHumanHealthCancerFlag
        {
            get
            {
                return this.m_factor.CFHumanHealthCancerFlag;
            }
        }

        [System.ComponentModel.CategoryAttribute("Impact")]
        public DataQualityFlag CFHumanHealthNonCancerFlag
        {
            get
            {
                return this.m_factor.CFHumanHealthNonCancerFlag;
            }
        }

        public String[] ImpactCategories
        {
            get
            {
                bool[] impacts = new bool[29];
                int count = 28;
                impacts[0] = true;
                if (this.GlobalWarmingPotential == 0)
                {
                    impacts[0] = false;
                    count--;
                }
                impacts[1] = true;
                if (this.AcidificationAir == 0)
                {
                    impacts[1] = false;
                    count--;
                }
                impacts[2] = true;
                if (this.AcidificationWater == 0)
                {
                    impacts[2] = false;
                    count--;
                }
                impacts[3] = true;
                if (this.HumanHealthCriteria == 0)
                {
                    impacts[3] = false;
                    count--;
                }
                impacts[4] = true;
                if (this.EutrophicationAir == 0)
                {
                    impacts[4] = false;
                    count--;
                }
                impacts[5] = true;
                if (this.EutrophicationWater == 0)
                {
                    impacts[5] = false;
                    count--;
                }
                impacts[6] = true;
                if (this.OzoneDepletion == 0)
                {
                    impacts[6] = false;
                    count--;
                }
                impacts[7] = true;
                if (this.SmogAir == 0)
                {
                    impacts[7] = false;
                    count--;
                }
                impacts[8] = true;
                if (this.EcotoxCFairUfreshwater == 0)
                {
                    impacts[8] = false;
                    count--;
                }
                impacts[9] = true;
                if (this.EcotoxCFairCfreshwater == 0)
                {
                    impacts[9] = false;
                    count--;
                }
                impacts[10] = true;
                if (this.EcotoxCFfreshWaterCfreshwater == 0)
                {
                    impacts[10] = false;
                    count--;
                }
                impacts[11] = true;
                if (this.EcotoxCFfreshWaterUfreshwater == 0)
                {
                    impacts[11] = false;
                    count--;
                }
                impacts[12] = true;
                if (this.EcotoxCFseaWaterCfreshwater == 0)
                {
                    impacts[12] = false;
                    count--;
                }
                impacts[13] = true;
                if (this.EcotoxCFnativeSoilCfreshwater == 0)
                {
                    impacts[13] = false;
                    count--;
                }
                impacts[14] = true;
                if (this.EcotoxCFagriculturalSoilCfreshwater == 0)
                {
                    impacts[14] = false;
                    count--;
                }
                impacts[15] = true;
                if (this.HumanHealthCancer == 0)
                {
                    impacts[15] = false;
                    count--;
                }
                impacts[16] = true;
                if (this.HumanHealthNonCancer == 0)
                {
                    impacts[16] = false;
                    count--;
                }
                impacts[17] = true;
                if (this.HumanHealthUrbanAirCancer == 0)
                {
                    impacts[17] = false;
                    count--;
                }
                impacts[18] = true;
                if (this.HumanHealthUrbanAirNonCancer == 0)
                {
                    impacts[18] = false;
                    count--;
                }
                impacts[19] = true;
                if (this.HumanHealthRuralAirCancer == 0)
                {
                    impacts[19] = false;
                    count--;
                }
                impacts[20] = true;
                if (this.HumanHealthRuralAirNonCancer == 0)
                {
                    impacts[20] = false;
                    count--;
                }
                impacts[21] = true;
                if (this.HumanHealthFreshwaterCancer == 0)
                {
                    impacts[21] = false;
                    count--;
                }
                impacts[22] = true;
                if (this.HumanHealthFreshwaterNonCancer == 0)
                {
                    impacts[22] = false;
                    count--;
                }
                impacts[23] = true;
                if (this.HumanHealthSeawaterCancer == 0)
                {
                    impacts[23] = false;
                    count--;
                }
                impacts[24] = true;
                if (this.HumanHealthSeawaterNonCancer == 0)
                {
                    impacts[24] = false;
                    count--;
                }
                impacts[25] = true;
                if (this.HumanHealthNativeSoilCancer == 0)
                {
                    impacts[25] = false;
                    count--;
                }
                impacts[26] = true;
                if (this.HumanHealthNativeSoilNonCancer == 0)
                {
                    impacts[26] = false;
                    count--;
                }
                impacts[27] = true;
                if (this.HumanHealthAgriculturalSoilCancer == 0)
                {
                    impacts[27] = false;
                    count--;
                }
                impacts[28] = true;
                if (this.HumanHealthAgriculturalSoilNonCancer == 0)
                {
                    impacts[28] = false;
                    count--;
                }
                string[] retVal = new string[count+1];
                int i = 0;
                if (impacts[0]) retVal[i++] = "GlobalWarmingPotential";
                if (impacts[1]) retVal[i++] = "AcidificationAir";
                if (impacts[2]) retVal[i++] = "AcidificationWater";
                if (impacts[3]) retVal[i++] = "HumanHealthCriteria";
                if (impacts[4]) retVal[i++] = "EutrophicationAir";
                if (impacts[5]) retVal[i++] = "EutrophicationWater";
                if (impacts[6]) retVal[i++] = "OzoneDepletion";
                if (impacts[7]) retVal[i++] = "SmogAir";
                if (impacts[8]) retVal[i++] = "EcotoxCFairUfreshwater";
                if (impacts[9]) retVal[i++] = "EcotoxCFairCfreshwater";
                if (impacts[10]) retVal[i++] = "EcotoxCFfreshWaterCfreshwater";
                if (impacts[11]) retVal[i++] = "EcotoxCFfreshWaterUfreshwater";
                if (impacts[12]) retVal[i++] = "EcotoxCFseaWaterCfreshwater";
                if (impacts[13]) retVal[i++] = "EcotoxCFnativeSoilCfreshwater";
                if (impacts[14]) retVal[i++] = "EcotoxCFagriculturalSoilCfreshwater";
                if (impacts[15]) retVal[i++] = "HumanHealthUrbanCancer";
                if (impacts[16]) retVal[i++] = "HumanHealthUrbanNonCancer";
                if (impacts[17]) retVal[i++] = "HumanHealthUrbanAirCancer";
                if (impacts[18]) retVal[i++] = "HumanHealthUrbanAirNonCancer";
                if (impacts[19]) retVal[i++] = "HumanHealthRuralAirCancer";
                if (impacts[20]) retVal[i++] = "HumanHealthRuralAirNonCancer";
                if (impacts[21]) retVal[i++] = "HumanHealthFreshwaterCancer";
                if (impacts[22]) retVal[i++] = "HumanHealthFreshwaterNonCancer";
                if (impacts[23]) retVal[i++] = "HumanHealthSeawaterCancer";
                if (impacts[24]) retVal[i++] = "HumanHealthSeawaterNonCancer";
                if (impacts[25]) retVal[i++] = "HumanHealthNativeSoilCancer";
                if (impacts[26]) retVal[i++] = "HumanHealthNativeSoilNonCancer";
                if (impacts[27]) retVal[i++] = "HumanHealthAgriculturalSoilCancer";
                if (impacts[28]) retVal[i++] = "HumanHealthAgriculturalSoilNonCancer";
                return retVal;
            }
        }

        public double GetImpactValue(string impact)
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
            if (impact == "FOSSIL FUEL") return 0;
            if (impact == "LAND USE") return 0;
            if (impact == "WATER USE") return 0;
            return this.HumanHealthAgriculturalSoilNonCancer;
        }

        public String GetImpactFactor(string impact)
        {
            if (impact == "GlobalWarmingPotential") return "kg CO2";
            if (impact == "AcidificationAir") return "kg SO2 equiv";
            if (impact == "AcidificationWater") return "kg SO2 equiv";
            if (impact == "HumanHealthCriteria") return "total DALYs";
            if (impact == "EutrophicationAir") return "kg N";
            if (impact == "EutrophicationWater") return "kg N";
            if (impact == "OzoneDepletion") return "kg CFC-11";
            if (impact == "SmogAir") return "g NOX equiv";
            if (impact == "EcotoxCFairUfreshwater") return "lbs 2,4-D equiv";
            if (impact == "EcotoxCFairCfreshwater") return "lbs 2,4-D equiv";
            if (impact == "EcotoxCFfreshWaterCfreshwater") return "lbs 2,4-D equiv";
            if (impact == "EcotoxCFfreshWaterUfreshwater") return "lbs 2,4-D equiv";
            if (impact == "EcotoxCFseaWaterCfreshwater") return "lbs 2,4-D equiv";
            if (impact == "EcotoxCFnativeSoilCfreshwater") return "lbs 2,4-D equiv";
            if (impact == "EcotoxCFagriculturalSoilCfreshwater") return "lbs 2,4-D equiv";
            if (impact == "HumanHealthUrbanAirCancer") return "lbs C6H6 equiv";
            if (impact == "HumanHealthUrbanAirNonCancer") return "lbs C7H7 equiv";
            if (impact == "HumanHealthRuralAirCancer") return "lbs C6H6 equiv";
            if (impact == "HumanHealthRuralAirNonCancer") return "lbs C7H7 equiv";
            if (impact == "HumanHealthFreshwaterCancer") return "lbs C6H6 equiv";
            if (impact == "HumanHealthFreshwaterNonCancer") return "lbs C7H7 equiv";
            if (impact == "HumanHealthSeawaterCancer") return "lbs C6H6 equiv";
            if (impact == "HumanHealthSeawaterNonCancer") return "lbs C7H7 equiv";
            if (impact == "HumanHealthNativeSoilCancer") return "lbs C6H6 equiv";
            if (impact == "HumanHealthNativeSoilNonCancer") return "lbs C7H7 equiv";
            if (impact == "HumanHealthAgriculturalSoilCancer") return "lbs C6H6 equiv";
            return "lbs C7H7 equiv";
        }

        #region ITraciRelease Members

        string ITraciRelease.Name()
        {
            return m_factor.SubstanceName;
        }

        ITraciProcess ITraciRelease.Process()
        {
            return m_Process;
        }

        double ITraciRelease.Quantity()
        {
            return m_Quantity;
        }

        releaseUnit ITraciRelease.UOM()
        {
            return m_UOM;
        }

        string ITraciRelease.CASNumber()
        {
            if (m_factor == null) return String.Empty;
            return m_factor.casNumber;
        }

        releaseMedia ITraciRelease.Media()
        {
            return m_Media;
        }

        #endregion

        #region ITraciIdentification Members

        string ITraciIdentification.GetName()
        {
            return this.m_factor.SubstanceName;
        }

        string ITraciIdentification.GetFullName()
        {
            return this.Process.Product.Name + "." + this.Process.Name + "." + this.m_factor.SubstanceName;
        }

        string ITraciIdentification.GetDescription()
        {
            return m_Description;
        }

        #endregion
    }
}
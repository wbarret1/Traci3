using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Traci3
{
    [DataContract()]
    public enum DataQualityFlag
    {
        [EnumMember()]
        NotApplicable = 0,

        [EnumMember()]
        Interim = 1,

        [EnumMember()]
        Recommended = 2
    }

    [System.Runtime.InteropServices.ComVisible(false)]
    class ReleaseImpactFactorTypeConverter : System.ComponentModel.TypeConverter
    {
        public override bool CanConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Type destinationType)
        {
            if ((typeof(ReleaseImpactFactor)).IsAssignableFrom(destinationType))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        public override Object ConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, Object value, System.Type destinationType)
        {
            if ((typeof(System.String)).IsAssignableFrom(destinationType) && (typeof(ReleaseImpactFactor).IsAssignableFrom(value.GetType())))
            {
                return ((ReleaseImpactFactor)value).SubstanceName;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    };

    class StateNameConverter : System.ComponentModel.StringConverter
    {
        public override bool GetStandardValuesSupported(System.ComponentModel.ITypeDescriptorContext context)
        {
            return true;
        }

        public override System.ComponentModel.TypeConverter.StandardValuesCollection GetStandardValues(System.ComponentModel.ITypeDescriptorContext context)
        {
            return new System.ComponentModel.TypeConverter.StandardValuesCollection(ImpactFactorCollection.States);
        }

        public override bool GetStandardValuesExclusive(System.ComponentModel.ITypeDescriptorContext context)
        {
            return true;
        }
    };

    class CountyNameConverter : System.ComponentModel.StringConverter
    {
        public override bool GetStandardValuesSupported(System.ComponentModel.ITypeDescriptorContext context)
        {
            return true;
        }

        public override System.ComponentModel.TypeConverter.StandardValuesCollection GetStandardValues(System.ComponentModel.ITypeDescriptorContext context)
        {
            return new System.ComponentModel.TypeConverter.StandardValuesCollection(ImpactFactorCollection.Counties(((Release)(context.Instance)).Process.State));
        }

        public override bool GetStandardValuesExclusive(System.ComponentModel.ITypeDescriptorContext context)
        {
            return true;
        }
    };

    class ResourceCountyNameConverter : System.ComponentModel.StringConverter
    {
        public override bool GetStandardValuesSupported(System.ComponentModel.ITypeDescriptorContext context)
        {
            return true;
        }

        public override System.ComponentModel.TypeConverter.StandardValuesCollection GetStandardValues(System.ComponentModel.ITypeDescriptorContext context)
        {
            return new System.ComponentModel.TypeConverter.StandardValuesCollection(ImpactFactorCollection.Counties(((Resource)(context.Instance)).Process.State));
        }

        public override bool GetStandardValuesExclusive(System.ComponentModel.ITypeDescriptorContext context)
        {
            return true;
        }
    };

    [Serializable]
    [System.ComponentModel.TypeConverterAttribute(typeof(ReleaseImpactFactorTypeConverter))]
    public class ReleaseImpactFactor
    {
 
        public ReleaseImpactFactor()
        {
        }

        string m_casNumber;
        public string casNumber
        {
            get
            {
                return m_casNumber;
            }
            set
            {
                // remove any "-" delimeters from CAS Number
                while (value.Contains("-"))
                {
                    value.Remove(value.IndexOf("-"));
                }
                // Check to see if a numnber was submitted...
                int checksum = 0;
                if (!int.TryParse(value, out checksum))
                {
                    // if not, set the CAS number to an empty string.
                    // Not throwing an exception here because a number of nn-numbers are in the
                    // CAS field in the spreadsheet.
                    m_casNumber = String.Empty;
                    return;
                }

                // get each character in the string
                string[] digits = new String[value.Length];
                // checksum set to zero for validation of number
                checksum = 0;
                // get digits and add up checksum
                for (int i = 0; i < value.Length; i++)
                {
                    digits[i] = value.Substring(value.Length - i - 1, 1);
                    checksum = checksum + i * int.Parse(digits[i]);

                }

                // number is valid if the last digit equals the remaider from dividing 
                // the checksum by 10.
                if (checksum % 10 != int.Parse(digits[0]))
                {
                    // throw exception if the checksum does not work...
                    throw new System.ArgumentException("The CAS Number Checksum does not match. Invalid CAS Number");
                }

                // format the CAS Number 
                m_casNumber = digits[0];
                m_casNumber = m_casNumber.Insert(0, "-");
                m_casNumber = m_casNumber.Insert(0, digits[1]);
                m_casNumber = m_casNumber.Insert(0, digits[2]);
                m_casNumber = m_casNumber.Insert(0, "-");
                for (int j = 3; j < digits.Length; j++)
                {
                    m_casNumber = m_casNumber.Insert(0, digits[j]);
                }
            }
        }

        string m_SubstanceName;
        public string SubstanceName
        {
            get
            {
                return m_SubstanceName;
            }
            set
            {
                m_SubstanceName = value;
            }
        }


        string m_AltSubstanceName;
        public string AlternativeSubstanceNames
        {
            get
            {
                return m_AltSubstanceName;
            }
            set
            {
                m_AltSubstanceName = value;
            }
        }

        double m_globalWarming;
        public double GlobalWarmingPotential
        {
            get
            {
                return m_globalWarming;
            }
            set
            {
                m_globalWarming = value;
            }
        }

        double m_AcidificationAir;
        public double AcidificationAir
        {
            get
            {
                return m_AcidificationAir;
            }
            set
            {
                m_AcidificationAir = value;
            }
        }

        double m_AcidificationWater;
        public double AcidificationWater
        {
            get
            {
                return m_AcidificationWater;
            }
            set
            {
                m_AcidificationWater = value;
            }
        }

        double m_HumanHealthCriteria;
        public double HumanHealthCriteria
        {
            get
            {
                return m_HumanHealthCriteria;
            }
            set
            {
                m_HumanHealthCriteria = value;
            }
        }


        double m_EutrophicationAir;
        public double EutrophicationAir
        {
            get
            {
                return m_EutrophicationAir;
            }
            set
            {
                m_EutrophicationAir = value;
            }
        }

        double m_EutrophicationWater;
        public double EutrophicationWater
        {
            get
            {
                return m_EutrophicationWater;
            }
            set
            {
                m_EutrophicationWater = value;
            }
        }

        double m_OzoneDepletion;
        public double OzoneDepletion
        {
            get
            {
                return m_OzoneDepletion;
            }
            set
            {
                m_OzoneDepletion = value;
            }
        }

        double m_SmogAir;
        public double SmogAir
        {
            get
            {
                return m_SmogAir;
            }
            set
            {
                m_SmogAir = value;
            }
        }

        double m_EcotoxCFairUfreshwater;
        public double EcotoxCFairUfreshwater
        {
            get
            {
                return m_EcotoxCFairUfreshwater;
            }
            set
            {
                m_EcotoxCFairUfreshwater = value;
            }
        }

        double m_EcotoxCFairCfreshwater;
        public double EcotoxCFairCfreshwater
        {
            get
            {
                return m_EcotoxCFairCfreshwater;
            }
            set
            {
                m_EcotoxCFairCfreshwater = value;
            }
        }

        double m_EcotoxCFfreshWaterCfreshwater;
        public double EcotoxCFfreshWaterCfreshwater
        {
            get
            {
                return m_EcotoxCFfreshWaterCfreshwater;
            }
            set
            {
                m_EcotoxCFfreshWaterCfreshwater = value;
            }
        }

        double m_EcotoxCFfreshWaterUfreshwater;
        public double EcotoxCFfreshWaterUfreshwater
        {
            get
            {
                return m_EcotoxCFfreshWaterUfreshwater;
            }
            set
            {
                m_EcotoxCFfreshWaterUfreshwater = value;
            }
        }

        double m_EcotoxCFseaWaterCfreshwater;
        public double EcotoxCFseaWaterCfreshwater
        {
            get
            {
                return m_EcotoxCFseaWaterCfreshwater;
            }
            set
            {
                m_EcotoxCFseaWaterCfreshwater = value;
            }
        }

        double m_EcotoxCFnativeSoilCfreshwater;
        public double EcotoxCFnativeSoilCfreshwater
        {
            get
            {
                return m_EcotoxCFnativeSoilCfreshwater;
            }
            set
            {
                m_EcotoxCFnativeSoilCfreshwater = value;
            }
        }

        double m_EcotoxCFagriculturalSoilCfreshwater;
        public double EcotoxCFagriculturalSoilCfreshwater
        {
            get
            {
                return m_EcotoxCFagriculturalSoilCfreshwater;
            }
            set
            {
                m_EcotoxCFagriculturalSoilCfreshwater = value;
            }
        }

        DataQualityFlag m_CFFlagEcotox;
        public DataQualityFlag EcoToxFlag
        {
            get
            {
                return m_CFFlagEcotox;
            }
            set
            {
                m_CFFlagEcotox = value;
            }
        }

        double m_HumanHealthUrbanAirCancer;
        public double HumanHealthUrbanAirCancer
        {
            get
            {
                return m_HumanHealthUrbanAirCancer;
            }
            set
            {
                m_HumanHealthUrbanAirCancer = value;
            }
        }

        double m_HumanHealthUrbanAirNonCancer;
        public double HumanHealthUrbanAirNonCancer
        {
            get
            {
                return m_HumanHealthUrbanAirNonCancer;
            }
            set
            {
                m_HumanHealthUrbanAirNonCancer = value;
            }
        }

        double m_HumanHealthRuralAirCancer;
        public double HumanHealthRuralAirCancer
        {
            get
            {
                return m_HumanHealthRuralAirCancer;
            }
            set
            {
                m_HumanHealthRuralAirCancer = value;
            }
        }

        double m_HumanHealthRuralAirNonCancer;
        public double HumanHealthRuralAirNonCancer
        {
            get
            {
                return m_HumanHealthRuralAirNonCancer;
            }
            set
            {
                m_HumanHealthRuralAirNonCancer = value;
            }
        }

        double m_HumanHealthFreshwaterCancer;
        public double HumanHealthFreshwaterCancer
        {
            get
            {
                return m_HumanHealthFreshwaterCancer;
            }
            set
            {
                m_HumanHealthFreshwaterCancer = value;
            }
        }

        double m_HumanHealthFreshwaterNonCancer;
        public double HumanHealthFreshwaterNonCancer
        {
            get
            {
                return m_HumanHealthFreshwaterNonCancer;
            }
            set
            {
                m_HumanHealthFreshwaterNonCancer = value;
            }
        }

        double m_HumanHealthSeawaterCancer;
        public double HumanHealthSeawaterCancer
        {
            get
            {
                return m_HumanHealthSeawaterCancer;
            }
            set
            {
                m_HumanHealthSeawaterCancer = value;
            }
        }

        double m_HumanHealthSeawaterNonCancer;
        public double HumanHealthSeawaterNonCancer
        {
            get
            {
                return m_HumanHealthSeawaterNonCancer;
            }
            set
            {
                m_HumanHealthSeawaterNonCancer = value;
            }
        }

        double m_HumanHealthNativeSoilCancer;
        public double HumanHealthNativeSoilCancer
        {
            get
            {
                return m_HumanHealthNativeSoilCancer;
            }
            set
            {
                m_HumanHealthNativeSoilCancer = value;
            }
        }

        double m_HumanHealthNativeSoilNonCancer;
        public double HumanHealthNativeSoilNonCancer
        {
            get
            {
                return m_HumanHealthNativeSoilNonCancer;
            }
            set
            {
                m_HumanHealthNativeSoilNonCancer = value;
            }
        }

        double m_HumanHealthAgriculturalSoilCancer;
        public double HumanHealthAgriculturalSoilCancer
        {
            get
            {
                return m_HumanHealthAgriculturalSoilCancer;
            }
            set
            {
                m_HumanHealthAgriculturalSoilCancer = value;
            }
        }

        double m_HumanHealthAgriculturalSoilNonCancer;
        public double HumanHealthAgriculturalSoilNonCancer
        {
            get
            {
                return m_HumanHealthAgriculturalSoilNonCancer;
            }
            set
            {
                m_HumanHealthAgriculturalSoilNonCancer = value;
            }
        }


        DataQualityFlag m_CFHumanHealthCancerFlag;
        public DataQualityFlag CFHumanHealthCancerFlag
        {
            get
            {
                return m_CFHumanHealthCancerFlag;
            }
            set
            {
                m_CFHumanHealthCancerFlag = value;
            }
        }

        DataQualityFlag m_CFHumanHealthNonCancerFlag;
        public DataQualityFlag CFHumanHealthNonCancerFlag
        {
            get
            {
                return m_CFHumanHealthNonCancerFlag;
            }
            set
            {
                m_CFHumanHealthNonCancerFlag = value;
            }
        }

    }
}
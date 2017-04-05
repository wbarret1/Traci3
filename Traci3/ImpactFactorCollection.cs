using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Traci3
{

    /// <summary>
    /// This class represents a geographical location in the TRACI project.
    /// </summary>
    /// <remarks>
    /// TRACI is built specifically to conduct Life Cycle Impact Assessment for locations in 
    /// the United States. As a result, the locations used in TRACI are United States-centric.
    /// Specifically, TRACI locations are primarily states and counties. If the location considers
    /// the entire state, the location's county is "Statewide". Categorical variables are used to
    /// further refine the location. <see cref="Mississippi"/> refers to whether 
    /// the location is east or west of the Mississippi River. <see cref="GeologicalRegion"/>
    /// indicates what pert of the US the place is located (Northeast, Midwest, South or West). 
    /// <see cref="GeographicalLevel"/> also refers to political division levels common in the 
    /// US (country, region, state, and county).
    /// </remarks>
    [Serializable]
    public class Location
    {
        /// <summary>
        /// Constructor for the Location class.
        /// </summary>
        /// <param name="GeogID">A nuique integer ID for the instance.</param>
        /// <param name="LocName">The name of the location.</param>
        /// <param name="county">The county name of the location.</param>
        /// <param name="state">The state containing the location.</param>
        /// <param name="LocAbb">An abbreviation for the location.</param>
        /// <param name="missEastWest">A <see cref="Mississippi"/> value showing the locations relationship to the Mississippi River.</param>
        /// <param name="region">A <see cref="GeologicalRegion"/> value indicating the geological region of the location.</param>
        /// <param name="level">A <see cref="GeographicalLevel"/>value indicating the political division level of the location.</param>
        /// <param name="isInUS">Indicates whether the location is in the United States.</param>
        /// <param name="stateId">value cross referencing counties to states.</param>
        public Location(int GeogID, String LocName, String county, String state, String LocAbb, Mississippi missEastWest, GeologicalRegion region, GeographicalLevel level, bool isInUS, int stateId)
        {
            m_GeogId = GeogID;
            m_LocationName = LocName;
            m_County = county;
            m_State = state;
            m_Abbreviation = LocAbb;
            m_Mississippi = missEastWest;
            m_Region = region;
            m_GeoLevelId = level;
            m_isInUS = isInUS;
            m_StateId = stateId;
        }

        /// <summary>
        /// Unique identifier for the location.
        /// </summary>
        [System.ComponentModel.Browsable(false)]
        public int GeogID
        {
            get
            {
                return m_GeogId;
            }
        }
        int m_GeogId;

        /// <summary>
        /// Name of the location.
        /// </summary>
        public String Name
        {
            get
            {
                return m_LocationName;
            }
        }
        String m_LocationName;

        /// <summary>
        /// The state of the location.
        /// </summary>
        /// <remarks>
        /// Counties and states in the United States have a state value assigned. For regions
        /// and countries, the value of the state is set to "None."
        /// </remarks>
        public String State
        {
            get
            {
                return m_State;
            }
        }
        String m_State;


        /// <summary>
        /// The county of the location.
        /// </summary>
        /// <remarks>
        /// Counties have a county value assigned. If the entire state is desired, the 
        /// county is set to "Statewide." Otherwise, the county value is set to "None."
        /// </remarks>
        String m_County;
        public String County
        {
            get
            {
                return m_County;
            }
        }

        /// <summary>
        /// An abbreviation for the location, if available.
        /// </summary>
        /// <remarks>Postal code is used for states in the United States.</remarks>
        public String Abbreviation
        {
            get
            {
                return m_Abbreviation;
            }
        }
        String m_Abbreviation;

        /// <summary>
        /// A <see cref="Mississippi"/> value indicating whether the location is east or west
        /// of the Mississippi River.
        /// </summary>
        public Mississippi EastWestofMississippi
        {
            get
            {
                return m_Mississippi;
            }
        }
        Mississippi m_Mississippi;

        /// <summary>
        /// A <see cref="GeologicalRegion"/> value indicating whether the location inside the 
        /// United States is located in the Northeast, Midwest, South or West.
        /// </summary>
        public GeologicalRegion Region
        {
            get
            {
                return m_Region;
            }
        }
        GeologicalRegion m_Region;

        /// <summary>
        /// A <see cref="GeologicalLevel"/> value indicating the level of the political subdivision
        /// of the location.
        /// </summary>
        public GeographicalLevel GeoLevelId
        {
            get
            {
                return m_GeoLevelId;
            }
        }
        GeographicalLevel m_GeoLevelId;

        /// <summary>
        /// Indicates whether the location is within the United States.
        /// </summary>
        public bool IsInUS
        {
            get
            {
                return m_isInUS;
            }
        }
        bool m_isInUS;

        /// <summary>
        /// Integer value to reference counties to states.
        /// </summary>
        [System.ComponentModel.Browsable(false)]
        public int StateId
        {
            get
            {
                return m_StateId;
            }
        }
        int m_StateId;
    }

    /// <summary>
    /// Static class containing impact factor data for the chemicals in TRACI and location data.
    /// </summary>
    static public class ImpactFactorCollection
    {
        // 
        static System.Collections.Generic.List<ReleaseImpactFactor> factors = new List<ReleaseImpactFactor>();
        static System.Collections.Generic.List<Location> locations = new System.Collections.Generic.List<Location>();

        static ImpactFactorCollection()
        {
            // Get the current application domain to load the data file.
            //System.AppDomain domain = System.AppDomain.CurrentDomain;
            //// create the filename of the data file.
            //String fileName = domain.BaseDirectory + "TRACI 2.1.xlsx";
            try
            {

                String file = Properties.Resources.Substances;
                string[] lines = file.Split('\n');
                foreach (string line in lines)
                {

                    ReleaseImpactFactor factor = new ReleaseImpactFactor();
                    string[] values = line.Split('\t');


                    // Substance Name
                    factor.SubstanceName = values[1].ToString();
                    if (factor.SubstanceName != "") factors.Add(factor);

                    //SpellAidService.SpellAidClient chemSpell = new SpellAidService.SpellAidClient("", );
                    //string synonyms = chemSpell.getSugList("BENZENE", "All databases");

                    // CAS Number
                    factor.casNumber = values[0].ToString();

                    // Alternate Substance Name
                    factor.AlternativeSubstanceNames = values[2].ToString();
                    try
                    {
                        // Global Warming Air (kg CO2 eq / kg substance)
                        factor.GlobalWarmingPotential = values[3] != "n/a" ? Convert.ToDouble(values[3]) : Double.NaN;// ImpactFactorCollection.GetDoubleValue(r, "Global Warming Air (kg CO2 eq / kg substance)");
                    }
                    catch (System.Exception)
                    {

                    }
                    // Acidification Air (kg SO2 eq / kg substance)
                    factor.AcidificationAir = values[4] != "n/a" ? Convert.ToDouble(values[4]) : Double.NaN;// ImpactFactorCollection.GetDoubleValue(r, "Acidification Air (kg SO2 eq / kg substance)");

                    // Acidification Water (kg SO2 eq / kg substance)
                    factor.AcidificationWater = values[5] != "n/a" ? Convert.ToDouble(values[5]) : Double.NaN;// ImpactFactorCollection.GetDoubleValue(r, "Acidification Water (kg SO2 eq / kg substance)");

                    // HH Particulate Air (PM2.5 eq / kg substance)
                    factor.HumanHealthCriteria = values[6] != "n/a" ? Convert.ToDouble(values[6]) : Double.NaN;// ImpactFactorCollection.GetDoubleValue(r, "HH Particulate Air (PM2#5 eq / kg substance)");

                    // Eutrophication Air (kg N eq / kg substance)
                    factor.EutrophicationAir = values[7] != "n/a" ? Convert.ToDouble(values[7]) : Double.NaN;// ImpactFactorCollection.GetDoubleValue(r, "Eutrophication Air (kg N eq / kg substance)");

                    // Eutrophication Water (kg N eq / kg substance)
                    factor.EutrophicationWater = values[8] != "n/a" ? Convert.ToDouble(values[8]) : Double.NaN;// ImpactFactorCollection.GetDoubleValue(r, "Eutrophication Water (kg N eq / kg substance)");

                    //Ozone Depletion Air (kg CFC-11 eq / kg substance)
                    factor.OzoneDepletion = values[9] != "n/a" ? Convert.ToDouble(values[9]) : Double.NaN;// ImpactFactorCollection.GetDoubleValue(r, "Ozone Depletion Air (kg CFC-11 eq / kg substance)");

                    //Smog Air (kg O3 eq / kg substance)
                    factor.SmogAir = values[10] != "n/a" ? Convert.ToDouble(values[10]) : Double.NaN;// ImpactFactorCollection.GetDoubleValue(r, "Smog Air (kg O3 eq / kg substance)");

                    //"Ecotox# CF (CTUeco/kg), Em#airU, freshwater"
                    factor.EcotoxCFairUfreshwater = values[11] != "n/a" ? Convert.ToDouble(values[11]) : Double.NaN;// ImpactFactorCollection.GetDoubleValue(r, "Ecotox# CF (CTUeco/kg), Em#airU, freshwater");

                    //"Ecotox# CF (CTUeco/kg), Em#airC, freshwater"
                    factor.EcotoxCFairCfreshwater = values[12] != "n/a" ? Convert.ToDouble(values[12]) : Double.NaN;// ImpactFactorCollection.GetDoubleValue(r, "Ecotox# CF (CTUeco/kg), Em#airC, freshwater");

                    //"Ecotox# CF (CTUeco/kg), Em#fr#waterC, freshwater"
                    factor.EcotoxCFfreshWaterCfreshwater = values[13] != "n/a" ? Convert.ToDouble(values[13]) : Double.NaN;// ImpactFactorCollection.GetDoubleValue(r, "Ecotox# CF (CTUeco/kg), Em#fr#waterC, freshwater");

                    //"Ecotox# CF (CTUeco/kg), Em#sea waterC, freshwater"
                    factor.EcotoxCFseaWaterCfreshwater = values[14] != "n/a" ? Convert.ToDouble(values[14]) : Double.NaN;// ImpactFactorCollection.GetDoubleValue(r, "Ecotox# CF (CTUeco/kg), Em#sea waterC, freshwater");

                    //"Ecotox# CF (CTUeco/kg), Em#nat#soilC, freshwater"
                    factor.EcotoxCFnativeSoilCfreshwater = values[15] != "n/a" ? Convert.ToDouble(values[15]) : Double.NaN;// ImpactFactorCollection.GetDoubleValue(r, "Ecotox# CF (CTUeco/kg), Em#nat#soilC, freshwater");

                    //"Ecotox# CF (CTUeco/kg), Em#agr#soilC, freshwater"
                    factor.EcotoxCFagriculturalSoilCfreshwater = values[16] != "n/a" ? Convert.ToDouble(values[16]) : Double.NaN;// ImpactFactorCollection.GetDoubleValue(r, "Ecotox# CF (CTUeco/kg), Em#agr#soilC, freshwater");

                    //CF Flag Ecotox
                    factor.EcoToxFlag = DataQualityFlag.NotApplicable;// ImpactFactorCollection.GetFlagValue(r, 17);
                    if (values[17] == "Interim") factor.EcoToxFlag = DataQualityFlag.Interim;
                    if (values[17] == "Recommended") factor.EcoToxFlag = DataQualityFlag.Recommended;

                    //HH Criteria Air (kg PM10 eq / kg substance)
                    factor.HumanHealthUrbanAirCancer = values[18] != "n/a" ? Convert.ToDouble(values[18]) : Double.NaN;// ImpactFactorCollection.GetDoubleValue(r, 18);

                    //Human health CF  [cases/kgemitted], Emission to urban air, non-canc.
                    factor.HumanHealthUrbanAirNonCancer = values[19] != "n/a" ? Convert.ToDouble(values[19]) : Double.NaN;// ImpactFactorCollection.GetDoubleValue(r, 19);

                    //Human health CF  [cases/kgemitted], Emission to cont. rural air, cancer
                    factor.HumanHealthRuralAirCancer = values[20] != "n/a" ? Convert.ToDouble(values[20]) : Double.NaN;// ImpactFactorCollection.GetDoubleValue(r, 20);

                    //Human health CF  [cases/kgemitted], Emission to cont. rural air, non-canc.
                    factor.HumanHealthRuralAirNonCancer = values[21] != "n/a" ? Convert.ToDouble(values[21]) : Double.NaN;// ImpactFactorCollection.GetDoubleValue(r, 21);

                    //Human health CF  [cases/kgemitted], Emission to cont. freshwater, cancer
                    factor.HumanHealthFreshwaterCancer = values[22] != "n/a" ? Convert.ToDouble(values[22]) : Double.NaN;// ImpactFactorCollection.GetDoubleValue(r, 22);

                    //Human health CF  [cases/kgemitted], Emission to cont. freshwater, non-canc.
                    factor.HumanHealthFreshwaterNonCancer = values[23] != "n/a" ? Convert.ToDouble(values[23]) : Double.NaN;// ImpactFactorCollection.GetDoubleValue(r, 23);

                    //Human health CF  [cases/kgemitted], Emission to cont. sea water, cancer
                    factor.HumanHealthSeawaterCancer = values[24] != "n/a" ? Convert.ToDouble(values[24]) : Double.NaN;// ImpactFactorCollection.GetDoubleValue(r, 24);

                    //Human health CF  [cases/kgemitted], Emission to cont. sea water, non-canc.
                    factor.HumanHealthSeawaterNonCancer = values[25] != "n/a" ? Convert.ToDouble(values[25]) : Double.NaN;// ImpactFactorCollection.GetDoubleValue(r, 25);

                    //Human health CF  [cases/kgemitted], Emission to cont. natural soil, cancer
                    factor.HumanHealthNativeSoilCancer = values[26] != "n/a" ? Convert.ToDouble(values[26]) : Double.NaN;// ImpactFactorCollection.GetDoubleValue(r, 26);

                    //Human health CF  [cases/kgemitted], Emission to cont. natural soil, non-canc.
                    factor.HumanHealthNativeSoilNonCancer = values[27] != "n/a" ? Convert.ToDouble(values[27]) : Double.NaN;// ImpactFactorCollection.GetDoubleValue(r, 27);

                    //Human health CF  [cases/kgemitted], Emission to cont. agric. Soil, cancer
                    factor.HumanHealthAgriculturalSoilCancer = values[28] != "n/a" ? Convert.ToDouble(values[28]) : Double.NaN;// ImpactFactorCollection.GetDoubleValue(r, 28);

                    //Human health CF  [cases/kgemitted], Emission to cont. agric. Soil, non-canc.
                    factor.HumanHealthAgriculturalSoilNonCancer = values[29] != "n/a" ? Convert.ToDouble(values[29]) : Double.NaN;// ImpactFactorCollection.GetDoubleValue(r, 29);

                    //CF Flag HH carcinogenic
                    factor.CFHumanHealthCancerFlag = DataQualityFlag.NotApplicable;// ImpactFactorCollection.GetFlagValue(r, 30);
                    if (values[30] == "Interim") factor.CFHumanHealthCancerFlag = DataQualityFlag.Interim;
                    if (values[30] == "Recommended") factor.CFHumanHealthCancerFlag = DataQualityFlag.Recommended;

                    //CF Flag HH non-carcinogenic
                    factor.CFHumanHealthNonCancerFlag = DataQualityFlag.NotApplicable;//ImpactFactorCollection.GetFlagValue(r, 31);
                    if (values[31] == "Interim") factor.CFHumanHealthNonCancerFlag = DataQualityFlag.Interim;
                    if (values[31] == "Recommended") factor.CFHumanHealthNonCancerFlag = DataQualityFlag.Recommended;
                }

                file = Properties.Resources.Locations;
                lines = file.Split('\n');

                //table = ImpactFactorCollection.ReadExcel2007toDataTable(fileName, "Sheet1");
                System.Collections.Generic.List<string> stateNames = new System.Collections.Generic.List<string>();
                foreach (string line in lines)
                {
                    try
                    {
                        string[] values = line.Split('\t');
                        //GeogID
                        int GeogID = Convert.ToInt32(values[0]);

                        //Location
                        string LocName = values[1];

                        //Location2
                        string LocAbb = values[2];

                        //EWID
                        int temp = Convert.ToInt32(values[3]);
                        Mississippi missEastWest = Mississippi.NotApplicable;
                        if (temp == 57 || GeogID == 57) missEastWest = Mississippi.East;
                        if (temp == 58 || GeogID == 58) missEastWest = Mississippi.West;

                        //RegionID
                        temp = Convert.ToInt32(values[4]);
                        GeologicalRegion region = GeologicalRegion.NotApplicable;
                        if (temp == 53 || GeogID == 53) region = GeologicalRegion.NorthEast;
                        if (temp == 54 || GeogID == 54) region = GeologicalRegion.Midwest;
                        if (temp == 55 || GeogID == 55) region = GeologicalRegion.South;
                        if (temp == 56 || GeogID == 56) region = GeologicalRegion.West;

                        //GeoLevelId
                        // Political division level. 
                        int geoLevel = Convert.ToInt32(values[5]);
                        GeographicalLevel level = GeographicalLevel.County;
                        string county = LocName;
                        string state = LocName;
                        bool isInUS = true;
                        if (geoLevel == 2)
                        {
                            county = "StateWide";
                            level = GeographicalLevel.State;
                            stateNames.Add(LocName);
                        }
                        if (geoLevel == 3)
                        {
                            county = "None";
                            state = LocName;
                            level = GeographicalLevel.Region;
                        }
                        if (geoLevel == 4)
                        {
                            county = "None";
                            state = LocName;
                            level = GeographicalLevel.Region;
                        }
                        if (geoLevel == 5)
                        {
                            county = "None";
                            state = LocName;
                            level = GeographicalLevel.Country;
                        }
                        if (geoLevel == 6)
                        {
                            county = "None";
                            state = LocName;
                            level = GeographicalLevel.Country;
                            isInUS = false;
                        }

                        //StateId
                        // Integer value specifying the state.
                        int StateId = Convert.ToInt32(values[6]);
                        if (level == GeographicalLevel.State) state = LocName;
                        if (level == GeographicalLevel.County) state = stateNames[StateId - 1].ToString();

                        // Now add the location to the collection.
                        locations.Add(new Location(GeogID, LocName, county, state, LocAbb, missEastWest, region, level, isInUS, StateId));
                    }
                    catch(System.Exception ex)
                    {

                    }
                }
                m_States = stateNames.ToArray<string>();
            }
            catch (System.Exception ex)
            {

            }
        }

        static void ImportLocations()
        {
            string locations = Properties.Resources.Locations;

        }

        /// <summary>
        /// The method reads data from an Excel 2007 spreadsheet into a data table for use in
        /// importing the spreadsheet. Currently not used.
        /// a <see cref="System.Data.DataTable"/>.
        /// </summary>
        /// <param name="filename">The name of the EXCEL 2007 file to be opened.</param>
        static System.Data.DataTable ReadExcel2007toDataTable(string filename, string tableName)
        {
            // Create a new DataTable, the database command, and the data adapter.
            System.Data.DataTable dt = new System.Data.DataTable();

            // Open the connection to the EXCEL spreadsheet unsing the connection string 
            // in the OLE data connection...
            String excelConnStr = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + filename + "; Extended Properties=Excel 16.0;";
            System.Data.OleDb.OleDbConnection excelConn = new System.Data.OleDb.OleDbConnection(excelConnStr);
            excelConn.Open();

            // Create a OLE database command to select all data from the TRACI export table...
            System.Data.OleDb.OleDbCommand excelCommand = new System.Data.OleDb.OleDbCommand("SELECT * FROM [" + tableName + "$]", excelConn);

            // Create a OLE data adapter for the command...
            System.Data.OleDb.OleDbDataAdapter excelDataAdapter = new System.Data.OleDb.OleDbDataAdapter(excelCommand);

            // fill the data table...
            excelDataAdapter.Fill(dt);
            dt.AcceptChanges();
            return dt;
        }

        //  
        /// <summary>
        /// Method used to get the <see cref="DataQualityFlag"/> value from a row in the data table.
        /// </summary>
        /// <param name="row">The current data row to get the flag for.</param>
        /// <param name="columnNumber">The column number where the flag is located.</param>
        /// <returns></returns>
        static DataQualityFlag GetFlagValue(System.Data.DataRow row, int columnNumber)
        {
            DataQualityFlag retVal = DataQualityFlag.NotApplicable;
            string columnName = row.Table.Columns[columnNumber].ColumnName;
            if (DBNull.Value.Equals(row[columnName])) return DataQualityFlag.NotApplicable;
            String text = row[columnName].ToString();
            if (text == "Recommended") retVal = DataQualityFlag.Recommended;
            if (text == "Interim") retVal = DataQualityFlag.Interim;
            return retVal;
        }

        /// <summary>
        /// Method used to get the value of a characterization factor from a row in the data table.
        /// </summary>
        /// <param name="row">The current data row to get the characterization factor for.</param>
        /// <param name="columnName">The column name where the characterization factor is located.</param>
        /// <returns></returns>
        static double GetDoubleValue(System.Data.DataRow row, string columnName)
        {
            if (DBNull.Value.Equals(row[columnName])) return Double.NaN;
            String retVal = row[columnName].ToString();
            if (retVal == "n/a") return Double.NaN;
            if (retVal == String.Empty) return Double.NaN;
            return double.Parse(retVal);
        }

        /// <summary>
        /// Method used to get the value of a characterization factor from a row in the data table.
        /// </summary>
        /// <param name="row">The current data row to get the characterization factor for.</param>
        /// <param name="columnNumber">The column numnber where the characterization factor is located.</param>
        /// <returns></returns>
        static double GetDoubleValue(System.Data.DataRow row, int columnNumber)
        {
            string columnName = row.Table.Columns[columnNumber].ColumnName;
            if (DBNull.Value.Equals(row[columnName])) return Double.NaN;
            String retVal = row[columnName].ToString();
            if (retVal == "n/a") return Double.NaN;
            if (retVal == String.Empty) return Double.NaN;
            return double.Parse(retVal);
        }

        /// <summary>
        /// Method used to get the value from a row in the data table.
        /// </summary>
        /// <param name="row">The current data row to get the value.</param>
        /// <param name="columnNumber">The column numnber where the value is located.</param>
        /// <returns></returns>
        static int GetIntegerValue(System.Data.DataRow row, string columnName)
        {
            if (DBNull.Value.Equals(row[columnName])) return Int32.MaxValue;
            String retVal = row[columnName].ToString();
            if (retVal == "n/a") return Int32.MaxValue;
            if (retVal == String.Empty) return Int32.MaxValue;
            return int.Parse(retVal);
        }

        /// <summary>
        /// Returns a list of all county names in the state.
        /// </summary>
        /// <param name="state">State to get the list of counties</param>
        /// <returns></returns>
        static public String[] Counties(String state)
        {
            System.Collections.ArrayList counties = new System.Collections.ArrayList();
            foreach (Location place in locations)
            {
                if (state == place.State) counties.Add(place.County);
            }
            String[] retVal = new String[counties.Count];
            for (int i = 0; i < counties.Count; i++)
            {
                retVal[i] = counties[i].ToString();
            }
            return retVal;

        }

        /// <summary>
        /// Gets the array of <see cref="Location"/> in the location table.
        /// </summary>
        static public Location[] Locations
        {
            get
            {
                Location[] retVal = new Location[locations.Count];
                for (int i = 0; i < locations.Count; i++)
                {
                    retVal[i] = (Location)locations[i];
                }
                return retVal;
            }
        }

        /// <summary>
        /// Gets the <see cref="Location"/> for a state and county.
        /// </summary>
        /// <param name="State">State of interest.</param>
        /// <param name="County">County of interest.</param>
        /// <returns></returns>
        static public Location GetLocation(String State, String County)
        {
            foreach (Location place in locations)
            {
                if (State == place.State && County == place.County) return place;
            }
            return null;
        }

        /// <summary>
        /// Gets the <see cref="Location"/> for a location by name.
        /// </summary>
        /// <param name="locationName">Name of the location.</param>
        /// <returns></returns>
        static public Location GetLocation(String locationName)
        {
            foreach (Location place in locations)
            {
                if (locationName == place.Name) return place;
            }
            return null;
        }

        /// <summary>
        /// Returns a list of all states.
        /// </summary>
        static public String[] States
        {
            get
            {
                System.Collections.ArrayList states = new System.Collections.ArrayList();
                foreach (Location place in locations)
                {
                    if (place.GeoLevelId == GeographicalLevel.Country || place.GeoLevelId == GeographicalLevel.Region || place.GeoLevelId == GeographicalLevel.State) states.Add(place.Name);
                }
                String[] retVal = new String[states.Count];
                for (int i = 0; i < states.Count; i++)
                {
                    retVal[i] = states[i].ToString();
                }
                return retVal;
            }
        }
        static String[] m_States;

        /// <summary>
        /// Returns a list of chemical substances.
        /// </summary>
        static public String[] ChemicalSubstances
        {
            get
            {
               System.Collections.Generic.List<string> retVal = new System.Collections.Generic.List<string>();
                for (int i = 0; i < factors.Count; i++)
                {
                    retVal.Add(factors[i].SubstanceName);
                }
                retVal.Sort();
                return retVal.ToArray();
            }
        }

        ///// <summary>
        ///// Returns the CAS Numbers of all the chemcial substances.
        ///// </summary>
        //static public String[] casNumbers
        //{
        //    get
        //    {
        //        String[] retVal = new String[factors.Count];
        //        for (int i = 0; i < factors.Count; i++)
        //        {
        //            retVal[i] = factors[i].casNumber;
        //        }
        //        return retVal;
        //    }
        //}


        //static public string[] ImpactNames
        //{
        //    get
        //    {
        //        string[] retVal = new string[25];
        //        retVal[0] = "GlobalWarmingPotential";
        //        retVal[0] = "Acidification";
        //        retVal[1] = "HumanHealthCriteria";
        //        retVal[2] = "EutrophicationAir";
        //        retVal[3] = "EutrophicationWater";
        //        retVal[4] = "OzoneDepletion";
        //        retVal[5] = "SmogAir";
        //        retVal[6] = "EcotoxCFairUfreshwater";
        //        retVal[7] = "EcotoxCFairCfreshwater";
        //        retVal[8] = "EcotoxCFfreshWaterCfreshwater";
        //        retVal[9] = "EcotoxCFfreshWaterUfreshwater";
        //        retVal[10] = "EcotoxCFseaWaterCfreshwater";
        //        retVal[11] = "EcotoxCFnativeSoilCfreshwater";
        //        retVal[12] = "EcotoxCFagriculturalSoilCfreshwater";
        //        retVal[13] = "HumanHealthUrbanAirCancer";
        //        retVal[14] = "HumanHealthUrbanAirNonCancer";
        //        retVal[15] = "HumanHealthRuralAirCancer";
        //        retVal[16] = "HumanHealthRuralAirNonCancer";
        //        retVal[17] = "HumanHealthFreshwaterCancer";
        //        retVal[18] = "HumanHealthFreshwaterNonCancer";
        //        retVal[19] = "HumanHealthSeawaterCancer";
        //        retVal[20] = "HumanHealthSeawaterNonCancer";
        //        retVal[21] = "HumanHealthNativeSoilCancer";
        //        retVal[22] = "HumanHealthNativeSoilNonCancer";
        //        retVal[23] = "HumanHealthAgriculturalSoilCancer";
        //        retVal[24] = "HumanHealthAgriculturalSoilNonCancer";
        //        return retVal;
        //    }
        //}

        /// <summary>
        /// Returns the <see cref="ReleaseImpactFactor"/> for a chemcial by name.
        /// </summary>
        /// <param name="factorName">The name of the characterization facor.</param>
        /// <returns>the value of the characterication factor.</returns>
        static public ReleaseImpactFactor GetImpact(String factorName)
        {
            foreach (ReleaseImpactFactor factor in factors)
            {
                if (factor.SubstanceName.ToUpper() == factorName.ToUpper())
                    return factor;
                if (factor.casNumber == factorName.ToUpper())
                    return factor;
                if (factor.AlternativeSubstanceNames.ToUpper() == factorName.ToUpper())
                    return factor;
            }
            return null;
        }
    }
}


// Miscellaneous stuff... works, but slow.


//static ImpactFactorCollection()
// {
//    // Get the current application domain to load the data file.
//     System.AppDomain domain = System.AppDomain.CurrentDomain;
//     // create the filename of the data file.
//     String fileName = domain.BaseDirectory + "TRACI 2.0.xlsx";
//     System.IO.Stream myStream = new System.IO.FileStream(fileName, System.IO.FileMode.Open);
//     //try
//     //{
//     // Open a SpreadsheetDocument for read-only access based on a filepath.

//     // open the file as an Excel 2007 file.
//     SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(myStream, true);

//     // get the workbooks.
//     WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;

//     // and an enumerator for the workbooks.
//     IEnumerator<WorksheetPart> worksheetParts = workbookPart.WorksheetParts.GetEnumerator();

//     // The release fac
//     worksheetParts.MoveNext();
//     worksheetParts.MoveNext();
//     SheetData sheetData = worksheetParts.Current.Worksheet.Elements<SheetData>().First();
//     bool headers = true;
//     ReleaseImpactFactor factor = null;
//     var shareStringTablePart = spreadsheetDocument.WorkbookPart.SharedStringTablePart;
//     foreach (Row r in sheetData.Elements<Row>())
//     {
//         if (headers)
//         {
//             headers = false;
//         }
//         else
//         {
//             factor = new ReleaseImpactFactor();
//             IEnumerator<Cell> cells = r.Elements<Cell>().GetEnumerator();

//             //CAS Number
//             cells.MoveNext();
//             factor.casNumber = ImpactFactorCollection.GetCellValue(spreadsheetDocument, cells.Current);

//             //Substance Name
//             cells.MoveNext();
//             factor.SubstanceName = ImpactFactorCollection.GetCellValue(spreadsheetDocument, cells.Current);
//             if (factor.SubstanceName != String.Empty) factors.Add(factor);

//             //Alternate Substance Name
//             cells.MoveNext();
//             factor.AlternativeSubstanceNames = String.Empty;
//             // Sometimes the Alternate Subtance Name cell is missing. Will check the cell
//             // reference to see if it is in column "C".
//             string reference = cells.Current.CellReference.InnerText;
//             if (reference.StartsWith("C"))
//             {
//                 // it is the Alternate Substance name, so it will be added.
//                 factor.AlternativeSubstanceNames = ImpactFactorCollection.GetCellValue(spreadsheetDocument, cells.Current); ;
//                 // and we move to the next cell for Global Warming Potential...
//                 cells.MoveNext();
//             }

//             //Global Warming Air (kg CO2 eq / kg substance)
//             factor.GlobalWarmingPotential = ImpactFactorCollection.GetCellDoubleValue(spreadsheetDocument, cells.Current);

//             //Acidification Air (kg H+ moles eq / kg substance)
//             cells.MoveNext();
//             factor.Acidification = ImpactFactorCollection.GetCellDoubleValue(spreadsheetDocument, cells.Current);

//             //HH Criteria Air (kg PM10 eq / kg substance)
//             cells.MoveNext();
//             factor.HumanHealthCriteria = ImpactFactorCollection.GetCellDoubleValue(spreadsheetDocument, cells.Current);

//             //Eutrophication Air (kg N eq / kg substance)
//             cells.MoveNext();
//             factor.EutrophicationAir = ImpactFactorCollection.GetCellDoubleValue(spreadsheetDocument, cells.Current);

//             //Eutrophication Water (kg N eq / kg substance)
//             cells.MoveNext();
//             factor.EutrophicationWater = ImpactFactorCollection.GetCellDoubleValue(spreadsheetDocument, cells.Current);

//             //Ozone Depletion Air (kg CFC-11 eq / kg substance)
//             cells.MoveNext();
//             factor.OzoneDepletion = ImpactFactorCollection.GetCellDoubleValue(spreadsheetDocument, cells.Current);

//             //Smog Air (kg O3 eq / kg substance)
//             cells.MoveNext();
//             factor.SmogAir = ImpactFactorCollection.GetCellDoubleValue(spreadsheetDocument, cells.Current);

//             //Ecotox. CF [PAF.m3.day.kg-1], Em.airU, freshwater
//             cells.MoveNext();
//             factor.EcotoxCFairUfreshwater = ImpactFactorCollection.GetCellDoubleValue(spreadsheetDocument, cells.Current);

//             //Ecotox. CF [PAF.m3.day.kg-1], Em.airC, freshwater
//             cells.MoveNext();
//             factor.EcotoxCFairCfreshwater = ImpactFactorCollection.GetCellDoubleValue(spreadsheetDocument, cells.Current);

//             //Ecotox. CF [PAF.m3.day.kg-1], Em.fr.waterC, freshwater
//             cells.MoveNext();
//             factor.EcotoxCFfreshWaterCfreshwater = ImpactFactorCollection.GetCellDoubleValue(spreadsheetDocument, cells.Current);

//             //Ecotox. CF [PAF.m3.day.kg-1], Em.sea waterC, freshwater
//             cells.MoveNext();
//             factor.EcotoxCFseaWaterCfreshwater = ImpactFactorCollection.GetCellDoubleValue(spreadsheetDocument, cells.Current);

//             //Ecotox. CF [PAF.m3.day.kg-1], Em.nat.soilC, freshwater
//             cells.MoveNext();
//             factor.EcotoxCFnativeSoilCfreshwater = ImpactFactorCollection.GetCellDoubleValue(spreadsheetDocument, cells.Current);

//             //Ecotox. CF [PAF.m3.day.kg-1], Em.agr.soilC, freshwater
//             cells.MoveNext();
//             factor.EcotoxCFagriculturalSoilCfreshwater = ImpactFactorCollection.GetCellDoubleValue(spreadsheetDocument, cells.Current);

//             //CF Flag Ecotox
//             cells.MoveNext();
//             factor.EcoToxFlag = ImpactFactorCollection.GetCellFlagValue(spreadsheetDocument, cells.Current);

//             //HH Criteria Air (kg PM10 eq / kg substance)
//             cells.MoveNext();
//             factor.HumanHealthUrbanAirCancer = ImpactFactorCollection.GetCellDoubleValue(spreadsheetDocument, cells.Current);

//             //Human health CF  [cases/kgemitted], Emission to urban air, non-canc.
//             cells.MoveNext();
//             factor.HumanHealthUrbanAirNonCancer = ImpactFactorCollection.GetCellDoubleValue(spreadsheetDocument, cells.Current);

//             //Human health CF  [cases/kgemitted], Emission to cont. rural air, cancer
//             cells.MoveNext();
//             factor.HumanHealthRuralAirCancer = ImpactFactorCollection.GetCellDoubleValue(spreadsheetDocument, cells.Current);

//             //Human health CF  [cases/kgemitted], Emission to cont. rural air, non-canc.
//             cells.MoveNext();
//             factor.HumanHealthRuralAirNonCancer = ImpactFactorCollection.GetCellDoubleValue(spreadsheetDocument, cells.Current);

//             //Human health CF  [cases/kgemitted], Emission to cont. freshwater, cancer
//             cells.MoveNext();
//             factor.HumanHealthFreshwaterCancer = ImpactFactorCollection.GetCellDoubleValue(spreadsheetDocument, cells.Current);

//             //Human health CF  [cases/kgemitted], Emission to cont. freshwater, non-canc.
//             cells.MoveNext();
//             factor.HumanHealthFreshwaterNonCancer = ImpactFactorCollection.GetCellDoubleValue(spreadsheetDocument, cells.Current);

//             //Human health CF  [cases/kgemitted], Emission to cont. sea water, cancer
//             cells.MoveNext();
//             factor.HumanHealthSeawaterCancer = ImpactFactorCollection.GetCellDoubleValue(spreadsheetDocument, cells.Current);

//             //Human health CF  [cases/kgemitted], Emission to cont. sea water, non-canc.
//             cells.MoveNext();
//             factor.HumanHealthSeawaterNonCancer = ImpactFactorCollection.GetCellDoubleValue(spreadsheetDocument, cells.Current);

//             //Human health CF  [cases/kgemitted], Emission to cont. natural soil, cancer
//             cells.MoveNext();
//             factor.HumanHealthNativeSoilCancer = ImpactFactorCollection.GetCellDoubleValue(spreadsheetDocument, cells.Current);

//             //Human health CF  [cases/kgemitted], Emission to cont. natural soil, non-canc.
//             cells.MoveNext();
//             factor.HumanHealthNativeSoilNonCancer = ImpactFactorCollection.GetCellDoubleValue(spreadsheetDocument, cells.Current);

//             //Human health CF  [cases/kgemitted], Emission to cont. agric. Soil, cancer
//             cells.MoveNext();
//             factor.HumanHealthAgriculturalSoilCancer = ImpactFactorCollection.GetCellDoubleValue(spreadsheetDocument, cells.Current);

//             //Human health CF  [cases/kgemitted], Emission to cont. agric. Soil, non-canc.
//             cells.MoveNext();
//             factor.HumanHealthAgriculturalSoilNonCancer = ImpactFactorCollection.GetCellDoubleValue(spreadsheetDocument, cells.Current);

//             //CF Flag HH carcinogenic
//             cells.MoveNext();
//             factor.CFHumanHealthCancerFlag = ImpactFactorCollection.GetCellFlagValue(spreadsheetDocument, cells.Current);

//             //CF Flag HH non-carcinogenic
//             cells.MoveNext();
//             factor.CFHumanHealthNonCancerFlag = ImpactFactorCollection.GetCellFlagValue(spreadsheetDocument, cells.Current);
//         }
//     }
//     worksheetParts.MoveNext();
//     worksheetParts.MoveNext();
//     sheetData = worksheetParts.Current.Worksheet.Elements<SheetData>().First();
//     headers = true;
//     String state = String.Empty;
//     String county = String.Empty;
//     System.Collections.Generic.List<string> stateNames = new System.Collections.Generic.List<string>();
//     shareStringTablePart = spreadsheetDocument.WorkbookPart.SharedStringTablePart;
//     foreach (Row r in sheetData.Elements<Row>())
//     {
//         if (headers)
//         {
//             headers = false;
//         }
//         else
//         {
//             IEnumerator<Cell> cells = r.Elements<Cell>().GetEnumerator();

//             //GeogID
//             cells.MoveNext();
//             int GeogID = ImpactFactorCollection.GetCellIntValue(spreadsheetDocument, cells.Current); ;

//             //Location
//             cells.MoveNext();
//             string LocName = ImpactFactorCollection.GetCellValue(spreadsheetDocument, cells.Current); ;

//             //Location2
//             cells.MoveNext();
//             string LocAbb = ImpactFactorCollection.GetCellValue(spreadsheetDocument, cells.Current); ;

//             //EWID
//             cells.MoveNext();
//             int temp = ImpactFactorCollection.GetCellIntValue(spreadsheetDocument, cells.Current); ;
//             Mississippi missEastWest = Mississippi.NotApplicable;
//             if (temp == 57 || GeogID == 57) missEastWest = Mississippi.East;
//             if (temp == 58 || GeogID == 58) missEastWest = Mississippi.West;

//             //RegionID
//             cells.MoveNext();
//             temp = ImpactFactorCollection.GetCellIntValue(spreadsheetDocument, cells.Current); ;
//             GeologicalRegion region = GeologicalRegion.NotApplicable;
//             if (temp == 53 || GeogID == 53) region = GeologicalRegion.NorthEast;
//             if (temp == 54 || GeogID == 54) region = GeologicalRegion.Midwest;
//             if (temp == 55 || GeogID == 55) region = GeologicalRegion.South;
//             if (temp == 56 || GeogID == 56) region = GeologicalRegion.West;

//             //GeoLevelId
//             cells.MoveNext();
//             temp = ImpactFactorCollection.GetCellIntValue(spreadsheetDocument, cells.Current); ;
//             GeographicalLevel level = GeographicalLevel.County;
//             county = LocName;
//             bool isInUS = true;
//             if (temp == 2)
//             {
//                 county = "StateWide";
//                 level = GeographicalLevel.State;
//                 stateNames.Add(LocName);
//             }
//             if (temp == 3)
//             {
//                 county = "None";
//                 state = LocName;
//                 level = GeographicalLevel.Region;
//             }
//             if (temp == 4)
//             {
//                 county = "None";
//                 state = LocName;
//                 level = GeographicalLevel.Region;
//             }
//             if (temp == 5)
//             {
//                 county = "None";
//                 state = LocName;
//                 level = GeographicalLevel.Country;
//             }
//             if (temp == 6)
//             {
//                 county = "None";
//                 state = LocName;
//                 level = GeographicalLevel.Country;
//                 isInUS = false;
//             }


//             //StateId
//             cells.MoveNext();
//             int StateId = ImpactFactorCollection.GetCellIntValue(spreadsheetDocument, cells.Current); ;
//             if (level == GeographicalLevel.State) state = LocName;
//             if (level == GeographicalLevel.County) state = stateNames[StateId - 1].ToString();

//             locations.Add(new Location(GeogID, LocName, county, state, LocAbb, missEastWest, region, level, isInUS, StateId));

//         }
//     }
//     m_States = stateNames.ToArray<string>();
// }

// This code gets a list of tables in an excel spreadsheet

//// Get the data table containg the schema guid. 

//dt = excelConn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, null);
//if (dt == null)
//{ 
//    return null; 
//} 

//String[] excelSheets = new String[dt.Rows.Count]; 
//int i = 0;          
//// Add the sheet name to the string array.         
//foreach (System.Data.DataRow row in dt.Rows)         
//{            
//    excelSheets[i] = row["TABLE_NAME"].ToString();            
//    i++;         
//} 


//static String GetCellValue(SpreadsheetDocument sheet, Cell cell)
//{
//    if (cell.CellValue == null) return String.Empty;
//    String retVal = cell.CellValue.Text;
//    if (cell.DataType != null)
//    {
//        if (cell.DataType == CellValues.SharedString)
//        {
//            int index = int.Parse(retVal);
//            retVal = (sheet.WorkbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(index)).InnerText;
//        }

//    }
//    return retVal;
//}

//static DataQualityFlag GetCellFlagValue(SpreadsheetDocument sheet, Cell cell)
//{
//    DataQualityFlag retVal = DataQualityFlag.NotApplicable;
//    if (cell.CellValue != null)
//    {
//        String text = cell.CellValue.Text;
//        if (cell.DataType != null)
//        {
//            if (cell.DataType == CellValues.SharedString)
//            {
//                int index = int.Parse(text);
//                text = (sheet.WorkbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(index)).InnerText;
//            }
//        }
//        if (text == "Recommended") retVal = DataQualityFlag.Recommended;
//        if (text == "Interim") retVal = DataQualityFlag.Interim;
//    }
//    return retVal;
//}

//static double GetCellDoubleValue(SpreadsheetDocument sheet, Cell cell)
//{
//    if (cell.CellValue == null) return Double.NaN;
//    String retVal = cell.CellValue.Text;
//    if (cell.DataType != null)
//    {
//        if (cell.DataType == CellValues.SharedString)
//        {
//            int index = int.Parse(retVal);
//            retVal = (sheet.WorkbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(index)).InnerText;
//        }

//    }
//    if (retVal == "n/a") return Double.NaN;
//    if (retVal == String.Empty) return Double.NaN;
//    return double.Parse(retVal);
//}

//static int GetCellIntValue(SpreadsheetDocument sheet, Cell cell)
//{
//    if (cell.CellValue == null) return Int32.MaxValue;
//    String retVal = cell.CellValue.Text;
//    if (cell.DataType != null)
//    {
//        if (cell.DataType == CellValues.SharedString)
//        {
//            int index = int.Parse(retVal);
//            retVal = (sheet.WorkbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(index)).InnerText;
//        }

//    }
//    if (retVal == "n/a") return Int32.MaxValue;
//    if (retVal == String.Empty) return Int32.MaxValue;
//    return int.Parse(retVal);
//}

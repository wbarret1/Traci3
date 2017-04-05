using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Traci3
{
    public partial class ViewCharts : Form
    {
        TraciProject m_Project;

        public ViewCharts(TraciProject project)
        {
            m_Project = project;
            InitializeComponent();

            // Tab Page 1
            this.CreateProductComparisonChart();

            // Draw chart on "Multiple Products by Life Cycle Stage" Tab Page
            this.CreateProductChartByLifeCycleStage();

            // Draw chart on "Resources/Releases at the Product Level" Tab Page
            this.resourcesReleasesAtTheProductLevel("Acidification");
        }

        void CreateProductComparisonChart()
        {
            foreach (Product product in m_Project.Products)
            {
                string seriesName = product.FullName;
                chart1.Series.Add(seriesName);
                chart1.Series[seriesName].LegendText = product.Name;

                // Add each releases impact as a data point...
                chart1.Series[seriesName].Points.AddXY("OzoneDepletion", product.OzoneDepletion);
                chart1.Series[seriesName].Points.AddXY("GlobalWarmingPotential", product.GlobalWarmingPotential);
                chart1.Series[seriesName].Points.AddXY("AcidificationAir", product.AcidificationAir);
                chart1.Series[seriesName].Points.AddXY("AcidificationWater", product.AcidificationWater);
                chart1.Series[seriesName].Points.AddXY("Eutrophication", product.EutrophicationWater + product.EutrophicationAir);
                chart1.Series[seriesName].Points.AddXY("Photochemical Smog", product.SmogAir);
                double ecoTox = product.EcotoxCFagriculturalSoilCfreshwater + product.EcotoxCFairCfreshwater;
                ecoTox = ecoTox + product.EcotoxCFairUfreshwater + product.EcotoxCFfreshWaterCfreshwater;
                ecoTox = ecoTox + product.EcotoxCFfreshWaterUfreshwater + product.EcotoxCFnativeSoilCfreshwater;
                ecoTox = ecoTox + product.EcotoxCFseaWaterCfreshwater;
                chart1.Series[seriesName].Points.AddXY("Ecological Toxicity", ecoTox);
                chart1.Series[seriesName].Points.AddXY("Human Health", product.HumanHealthCriteria);
                chart1.Series[seriesName].Points.AddXY("Human Health Cancer", product.HumanHealthCancer);
                chart1.Series[seriesName].Points.AddXY("Human Health Noncancer", product.HumanHealthNonCancer);

                double fossilFuel = 0;
                double landUse = 0;
                double waterUse = 0;
                foreach (Resource resource in product.Resources)
                {
                    if (typeof(NaturalGas) == resource.GetType())
                        fossilFuel = fossilFuel + resource.Quantity * ConversionFactors.Factor(((NaturalGas)resource).UOM);
                    if (typeof(OilResource) == resource.GetType())
                        fossilFuel = fossilFuel + resource.Quantity * ConversionFactors.Factor(((OilResource)resource).UOM);
                    if (typeof(CoalResource) == resource.GetType())
                        fossilFuel = fossilFuel + resource.Quantity * ConversionFactors.Factor(((CoalResource)resource).UOM);
                    if (typeof(LandResource) == resource.GetType())
                        landUse = landUse + resource.Quantity * ConversionFactors.Factor(((LandResource)resource).UOM);
                    if (typeof(WaterResource) == resource.GetType())
                        waterUse = waterUse + resource.Quantity * ConversionFactors.Factor(((WaterResource)resource).UOM);
                }
                chart1.Series[seriesName].Points.AddXY("Fossil Fuel", fossilFuel);
                chart1.Series[seriesName].Points.AddXY("Land Use", landUse);
                chart1.Series[seriesName].Points.AddXY("Water Use", waterUse);

                chart1.ChartAreas["ChartArea1"].AxisX.LabelStyle.IsEndLabelVisible = true;

                // Set interval of X axis to 1 week, with an offset of 1 day
                chart1.ChartAreas["ChartArea1"].AxisX.Interval = 1;

                // Disable axis labels auto fitting of text
                chart1.ChartAreas["ChartArea1"].AxisX.IsLabelAutoFit = false;

                // Set axis labels font
                chart1.ChartAreas["ChartArea1"].AxisX.LabelStyle.Font = new Font("Arial", 10);

                // Set axis labels angle
                chart1.ChartAreas["ChartArea1"].AxisX.LabelStyle.Angle = -90;

                // Disable offset labels style
                chart1.ChartAreas["ChartArea1"].AxisX.LabelStyle.IsStaggered = false;

                // Enable X axis labels
                chart1.ChartAreas["ChartArea1"].AxisX.LabelStyle.Enabled = true;

                // Enable AntiAliasing for either Text and Graphics or just Graphics
                //chart1.AntiAliasing = AntiAliasing.All; // AntiAliasingStyles.Graphics and AntiAliasing.Text
            }
        }

        void CreateProductChartByLifeCycleStage()
        {
            string[] products = new string[m_Project.Products.Count];
            double[] impactValues = new double[m_Project.Products.Count];
            for (int i = 0; i < impactValues.Length; i++) impactValues[i] = 0;
            for (int i = 0; i < m_Project.Products.Count; i++)
            {
                products[i] = m_Project.Products[i].Name;
                foreach (Process process in m_Project.Products[i].Processes)
                {
                    if (rawMaterialsAcquisitionCheckBox.Checked && process.LifeCycleStage == LifeCycleStage.rawMaterialsAcquisition)
                        impactValues[i] = impactValues[i] + this.GetImpactValue(process);
                    if (materialManufactureCheckBox.Checked && process.LifeCycleStage == LifeCycleStage.materialManufacture)
                        impactValues[i] = impactValues[i] + this.GetImpactValue(process);
                    if (productFabricationCheckBox.Checked && process.LifeCycleStage == LifeCycleStage.productFabrication)
                        impactValues[i] = impactValues[i] + this.GetImpactValue(process);
                    if (fillingPackagingDistributionCheckBox.Checked && process.LifeCycleStage == LifeCycleStage.fillingPackagingDistribution)
                        impactValues[i] = impactValues[i] + this.GetImpactValue(process);
                    if (useReuseMaintenanceCheckBox.Checked && process.LifeCycleStage == LifeCycleStage.useReuseMaintenance)
                        impactValues[i] = impactValues[i] + this.GetImpactValue(process);
                    if (recycleWasteManagementCheckBox.Checked && process.LifeCycleStage == LifeCycleStage.recycleWasteManagement)
                        impactValues[i] = impactValues[i] + this.GetImpactValue(process);
                }
            }
            // Populate series data
            chart2.Series["Series1"].Points.DataBindXY(products, impactValues);

            // Set Doughnut chart type
            chart2.Series["Series1"].ChartType = SeriesChartType.Pie;

            // Set labels style
            chart2.Series["Series1"]["PieLabelStyle"] = "Outside";

            // Set Doughnut radius percentage
            //chart2.Series["Series1"]["DoughnutRadius"] = "30";

            // Explode data point with label "Italy"
            //chart2.Series["Default"].Points[4]["Exploded"] = "true";

            // Enable 3D
            //chart2.ChartAreas["Series1"].Area3DStyle.Enable3D = true;

            // Set drawing style
            //chart2.Series["Series1"]["PieDrawingStyle"] = "SoftEdge";

        }

        double GetImpactValue(Process process)
        {
            if (AcidificationAirRadioButton.Checked)
                return process.AcidificationAir;
            if (AcidificationWaterRadioButton.Checked)
                return process.AcidificationWater;
            else if (ecoToxRadioButton.Checked)
            {
                double retVal = process.EcotoxCFagriculturalSoilCfreshwater + process.EcotoxCFairCfreshwater;
                retVal = retVal + process.EcotoxCFairUfreshwater + process.EcotoxCFfreshWaterCfreshwater;
                retVal = retVal + process.EcotoxCFfreshWaterUfreshwater + process.EcotoxCFnativeSoilCfreshwater;
                retVal = retVal + process.EcotoxCFseaWaterCfreshwater;
                return retVal;
            }
            else if (eutrophicationRadioButton.Checked)
                return process.EutrophicationAir + process.EutrophicationWater;
            else if (globalWarmingradioButton.Checked)
                return process.GlobalWarmingPotential;
            else if (humanHealthCancerRadioButton.Checked)
                return process.HumanHealthCancer;
            else if (humanHealthCriteriaRadioButton.Checked)
                return process.HumanHealthCriteria;
            else if (humanHealthNoncCanceRadioButton.Checked)
                return process.HumanHealthNonCancer;
            else if (ozoneDepletionRadioButton.Checked)
                return process.OzoneDepletion;
            else if (photochemicalSmogRadioButton.Checked)
                return process.SmogAir;
            else if (landUseRadioButton.Checked)
                return process.LandUse;
            else if (fossilFuelradioButton.Checked)
                return process.FossilFuelUse;
            else return process.WaterUse;
        }

        private void resourcesReleasesAtTheProductLevel(string impact)
        {
            chart3.Series.Clear();
            string[] releases = m_Project.ReleasesAndResourcesInImpactCategory(impact);
            foreach (String releaseName in releases)
            {
                chart3.Series.Add(releaseName);
                chart3.Series[releaseName].LegendText = releaseName;
                chart3.Series[releaseName].ChartType = SeriesChartType.StackedColumn;
            }
           foreach (Product product in m_Project.Products)
            {
                foreach (string releaseName in product.ReleasesInImpactCategory(impact))
                {
                    //chart3.Series[product.Name]["StackedGroupName"] = product.Name;
                    //// Populate series data
                    // Add each releases impact as a data point...
                    double value = product.GetImpactValueForRelaseAndImpactCategory(releaseName, impact); ;
                    chart3.Series[releaseName].Points.AddXY(product.Name, value);
                }
            }
            //
            //string[] releases = this.GetAllReleases(impact);
            //foreach (string release in releases)
            //{
            //    bool deleteSeries = true;
            //    chart3.Series.Add(release);
            //    chart3.Series[release].LegendText = release;
            //    chart3.Series[release].ChartType = SeriesChartType.StackedBar;
            //    foreach (Product product in m_Project.Products)
            //    {
            //        double value = 0;
            //        foreach (Release rel in product.Releases)
            //        {
            //            //chart3.Series[product.Name]["StackedGroupName"] = product.Name;
            //            //// Populate series data
            //            // Add each releases impact as a data point...
            //            value = value + rel.GetImpactValue(impact);
            //        }
            //        chart3.Series[release].Points.AddXY(product.Name, value);
            //        if (value != 0.0) deleteSeries = false;
            //    }
            //    if (deleteSeries) chart3.Series.Remove(chart3.Series[release]);
            //}
            //chart3.Invalidate();
        }

        string[] GetAllReleases(string impact)
        {
            System.Collections.ArrayList releaseList = new System.Collections.ArrayList();
            foreach (Product product in m_Project.Products)
            {
                foreach (Release release in product.Releases)
                {
                    if (release.GetImpactValue(impact) != 0.0)
                    {
                        bool add = true;
                        for (int i = 0; i < releaseList.Count; i++)
                        {
                            if (releaseList[i].ToString() == release.Name)
                            {
                                add = false;
                            }
                        }
                        if (add)
                        {
                            releaseList.Add(release.Name);
                        }
                    }
                }
            }
            string[] retVal = new string[releaseList.Count];
            for (int i = 0; i < releaseList.Count; i++)
            {
                retVal[i] = releaseList[i].ToString();
            }
            return retVal;
        }

        void GetProductReleases(Product product, string impact, ref string[] releaseNames, ref double[] releaseValues)
        {
            System.Collections.ArrayList releaseList = new System.Collections.ArrayList();
            System.Collections.ArrayList releaseValueList = new System.Collections.ArrayList();
            foreach (Release release in product.Releases)
            {
                bool add = true;
                for (int i = 0; i < releaseList.Count; i++)
                {
                    if (releaseList[i].ToString() == release.Name)
                    {
                        add = false;
                        releaseValueList[i] = double.Parse(releaseValueList[i].ToString()) + release.GetImpactValue(impact);
                    }
                }
                if (add)
                {
                    releaseList.Add(release.Name);
                    releaseValueList.Add(release.GetImpactValue(impact));
                }
            }
            int nonZero = 0;
            double[] releaseValueArray = new double[releaseValueList.Count];
            for (int i = 0; i < releaseValueList.Count; i++)
            {
                releaseValueArray[i] = double.Parse(releaseValueList[i].ToString());
                if (releaseValueArray[i] != 0.0) nonZero++;
            }
            releaseNames = new string[nonZero];
            releaseValues = new double[nonZero];
            int j = 0;
            for (int i = 0; i < releaseList.Count; i++)
            {
                if (releaseValueArray[i] != 0.0)
                {
                    releaseNames[j] = releaseList[i].ToString();
                    releaseValues[j] = releaseValueArray[i];
                    j++;
                }
            }

        }

        private void AcidificationRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            this.CreateProductChartByLifeCycleStage();
        }

        private void ecoToxRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            this.CreateProductChartByLifeCycleStage();
        }

        private void eutrophicationRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            this.CreateProductChartByLifeCycleStage();
        }

        private void fossilFuelradioButton_CheckedChanged(object sender, EventArgs e)
        {
            this.CreateProductChartByLifeCycleStage();
        }

        private void globalWarmingradioButton_CheckedChanged(object sender, EventArgs e)
        {
            this.CreateProductChartByLifeCycleStage();
        }

        private void humanHealthCancerRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            this.CreateProductChartByLifeCycleStage();
        }

        private void humanHealthNoncCanceRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            this.CreateProductChartByLifeCycleStage();
        }

        private void humanHealthCriteriaRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            this.CreateProductChartByLifeCycleStage();
        }

        private void landUseRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            this.CreateProductChartByLifeCycleStage();
        }

        private void ozoneDepletionRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            this.CreateProductChartByLifeCycleStage();
        }

        private void photochemicalSmogRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            this.CreateProductChartByLifeCycleStage();
        }

        private void waterUseRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            this.CreateProductChartByLifeCycleStage();
        }

        private void rawMaterialsAcquisitionCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.CreateProductChartByLifeCycleStage();
        }

        private void materialManufactureCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.CreateProductChartByLifeCycleStage();
        }

        private void productFabricationCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.CreateProductChartByLifeCycleStage();
        }

        private void fillingPackagingDistributionCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.CreateProductChartByLifeCycleStage();
        }

        private void useReuseMaintenanceCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.CreateProductChartByLifeCycleStage();
        }

        private void recycleWasteManagementCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.CreateProductChartByLifeCycleStage();
        }

        private void acidificationRadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            this.resourcesReleasesAtTheProductLevel("Acidification");
        }

        private void ecotoxicityRadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            this.resourcesReleasesAtTheProductLevel("EcoToxicity");
        }

        private void eutrophicationRadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            this.resourcesReleasesAtTheProductLevel("Eutrophication");
        }

        private void fossilFuelRadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            this.resourcesReleasesAtTheProductLevel("FOSSIL FUEL");
        }

        private void globalWarmingRadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            this.resourcesReleasesAtTheProductLevel("GlobalWarmingPotential");
        }

        private void humanHealthCancerRadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            this.resourcesReleasesAtTheProductLevel("HumanHealthCancer");
        }

        private void humanHealthNonCancerRadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            this.resourcesReleasesAtTheProductLevel("HumanHealthNonCancer");
        }

        private void humanHealthCriteriaRadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            this.resourcesReleasesAtTheProductLevel("HumanHealthCriteria");
        }

        private void landUseRadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            this.resourcesReleasesAtTheProductLevel("LAND USE");
        }

        private void ozoneDepletionRadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            this.resourcesReleasesAtTheProductLevel("OzoneDepletion");
        }

        private void photochemicalSmogRadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            this.resourcesReleasesAtTheProductLevel("SmogAir");
        }

        private void waterUseRadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            this.resourcesReleasesAtTheProductLevel("WATER USE");
        }

    }
};
//void CreateProcessCharts()
//{
//    foreach (Product product in m_Project.Products)
//    {
//        foreach (Process process in product.Processes)
//        {

//            // For each Row add a new series
//            string seriesName = process.FullName;
//            chart2.Series.Add(seriesName);
//            chart2.Series[seriesName].LegendText = process.Name;
//            chart2.Series[seriesName].ChartType = SeriesChartType.StackedColumn;
//            chart2.Series[seriesName]["StackedGroupName"] = product.Name;

//            // Add each releases impact as a data point...
//            chart2.Series[seriesName].Points.AddXY("OzoneDepletion", process.OzoneDepletion);
//            chart2.Series[seriesName].Points.AddXY("GlobalWarmingPotential", process.GlobalWarmingPotential);
//            chart2.Series[seriesName].Points.AddXY("Acidification", process.Acidification);
//            chart2.Series[seriesName].Points.AddXY("Eutrophication", process.EutrophicationWater + process.EutrophicationAir);
//            chart2.Series[seriesName].Points.AddXY("Photochemical Smog", process.SmogAir);
//            double ecoTox = process.EcotoxCFagriculturalSoilCfreshwater + process.EcotoxCFairCfreshwater;
//            ecoTox = ecoTox + process.EcotoxCFairUfreshwater + process.EcotoxCFfreshWaterCfreshwater;
//            ecoTox = ecoTox + process.EcotoxCFfreshWaterUfreshwater + process.EcotoxCFnativeSoilCfreshwater;
//            ecoTox = ecoTox + process.EcotoxCFseaWaterCfreshwater;
//            chart2.Series[seriesName].Points.AddXY("Ecological Toxicity", ecoTox);
//            chart2.Series[seriesName].Points.AddXY("Human Health", process.HumanHealthCriteria);
//            chart2.Series[seriesName].Points.AddXY("Human Health Cancer", process.HumanHealthCancer);
//            chart2.Series[seriesName].Points.AddXY("Human Health Noncancer", process.HumanHealthNonCancer);

//            // also, for now, add them to a data table.

//            foreach (Release release in process.Releases)
//            {
//            }
//            double fossilFuel = 0;
//            double landUse = 0;
//            double waterUse = 0;
//            foreach (Resource resource in process.Resources)
//            {
//                if (typeof(NaturalGas) == resource.GetType())
//                    fossilFuel = fossilFuel + resource.Quantity * ConversionFactors.Factor(((NaturalGas)resource).UOM);
//                if (typeof(OilResource) == resource.GetType())
//                    fossilFuel = fossilFuel + resource.Quantity * ConversionFactors.Factor(((OilResource)resource).UOM);
//                if (typeof(CoalResource) == resource.GetType())
//                    fossilFuel = fossilFuel + resource.Quantity * ConversionFactors.Factor(((CoalResource)resource).UOM);
//                if (typeof(LandResource) == resource.GetType())
//                    landUse = landUse + resource.Quantity * ConversionFactors.Factor(((LandResource)resource).UOM);
//                if (typeof(WaterResource) == resource.GetType())
//                    waterUse = waterUse + resource.Quantity * ConversionFactors.Factor(((WaterResource)resource).UOM);
//            }
//            chart2.Series[seriesName].Points.AddXY("Fossil Fuel", fossilFuel);
//            chart2.Series[seriesName].Points.AddXY("Land Use", landUse);
//            chart2.Series[seriesName].Points.AddXY("Water Use", waterUse);
//        }
//    }
//}


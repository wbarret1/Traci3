using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Traci3
{
    /// <summary>
    /// This class provides a GUI form that the user checks boxes for which life cycle stages
    /// desired when exporting life cycle data to an excel spreadsheet.
    /// </summary>
    public partial class ExportInventory : Form
    {
        /// <summary>
        /// Class constructor.
        /// </summary>
        public ExportInventory()
        {
            InitializeComponent();
        }

        /// <summary>
        /// This event hadler responds to the next button being clicked. Clicking the next 
        /// button indicates that the user has selected the desired life cycle stages and can move
        /// on to selecting the fine name for the export.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NextButton_Click(object sender, EventArgs e)
        {
            // close the dialog.
            this.Close();
        }

        /// <summary>
        /// This propoerty provides a boolean array indicating whether a life cycle stage is
        /// to be exported.
        /// </summary>
        /// <value>A boolean array, indicating the following stages, by index: 0 = Raw Materials
        /// Acquisition, 1 = Material Manufacture, 2 = Product Fabrication, 3 = Filling/Packaging/Distribution, 
        /// 4 = Use/Reuse/Maintenance, 5 = Recycle/WasteManagement.</value>
        public bool[] ExportStages
        {
            get
            {
                //create the boolean array.
                bool[] retval = new bool[6];

                // put the value indicating whether the appropriate check box has been checked
                // and the life cycle stage included in the generated excel spreadsheet.
                retval[0] = this.rawMaterialsAcquisitionCheckBox.Checked;
                retval[1] = this.materialManufactureCheckBox.Checked;
                retval[2] = this.productFabricationCheckBox.Checked;
                retval[3] = this.fillingPackagingDistributionheckBox.Checked;
                retval[4] = this.useReuseMaintenanceCheckBox.Checked;
                retval[5] = this.recycleWasteManagementCheckBox.Checked;
                return retval;
            }
        }
    }
}

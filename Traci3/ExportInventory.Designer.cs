namespace Traci3
{
    partial class ExportInventory
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExportInventory));
            this.NextButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.rawMaterialsAcquisitionCheckBox = new System.Windows.Forms.CheckBox();
            this.materialManufactureCheckBox = new System.Windows.Forms.CheckBox();
            this.productFabricationCheckBox = new System.Windows.Forms.CheckBox();
            this.fillingPackagingDistributionheckBox = new System.Windows.Forms.CheckBox();
            this.useReuseMaintenanceCheckBox = new System.Windows.Forms.CheckBox();
            this.recycleWasteManagementCheckBox = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // NextButton
            // 
            this.NextButton.Location = new System.Drawing.Point(193, 278);
            this.NextButton.Name = "NextButton";
            this.NextButton.Size = new System.Drawing.Size(75, 23);
            this.NextButton.TabIndex = 0;
            this.NextButton.Text = "Next ->";
            this.toolTip1.SetToolTip(this.NextButton, "Click button to choose the Excel file to export.");
            this.NextButton.UseVisualStyleBackColor = true;
            this.NextButton.Click += new System.EventHandler(this.NextButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Select Life Cycle Stages:";
            // 
            // rawMaterialsAcquisitionCheckBox
            // 
            this.rawMaterialsAcquisitionCheckBox.AutoSize = true;
            this.rawMaterialsAcquisitionCheckBox.Location = new System.Drawing.Point(57, 61);
            this.rawMaterialsAcquisitionCheckBox.Name = "rawMaterialsAcquisitionCheckBox";
            this.rawMaterialsAcquisitionCheckBox.Size = new System.Drawing.Size(147, 17);
            this.rawMaterialsAcquisitionCheckBox.TabIndex = 2;
            this.rawMaterialsAcquisitionCheckBox.Text = "Raw Materials Acquisition";
            this.toolTip1.SetToolTip(this.rawMaterialsAcquisitionCheckBox, "Click to select or deselect the Raw Materials Acquisition life cycle stage.");
            this.rawMaterialsAcquisitionCheckBox.UseVisualStyleBackColor = true;
            // 
            // materialManufactureCheckBox
            // 
            this.materialManufactureCheckBox.AutoSize = true;
            this.materialManufactureCheckBox.Location = new System.Drawing.Point(57, 98);
            this.materialManufactureCheckBox.Name = "materialManufactureCheckBox";
            this.materialManufactureCheckBox.Size = new System.Drawing.Size(126, 17);
            this.materialManufactureCheckBox.TabIndex = 3;
            this.materialManufactureCheckBox.Text = "Material Manufacture";
            this.toolTip1.SetToolTip(this.materialManufactureCheckBox, "Click to select or deselect the Material Manufacture life cycle stage.");
            this.materialManufactureCheckBox.UseVisualStyleBackColor = true;
            // 
            // productFabricationCheckBox
            // 
            this.productFabricationCheckBox.AutoSize = true;
            this.productFabricationCheckBox.Location = new System.Drawing.Point(56, 135);
            this.productFabricationCheckBox.Name = "productFabricationCheckBox";
            this.productFabricationCheckBox.Size = new System.Drawing.Size(118, 17);
            this.productFabricationCheckBox.TabIndex = 4;
            this.productFabricationCheckBox.Text = "Product Fabrication";
            this.toolTip1.SetToolTip(this.productFabricationCheckBox, "Click to select or deselect the Product Fabrication life cycle stage.");
            this.productFabricationCheckBox.UseVisualStyleBackColor = true;
            // 
            // fillingPackagingDistributionheckBox
            // 
            this.fillingPackagingDistributionheckBox.AutoSize = true;
            this.fillingPackagingDistributionheckBox.Location = new System.Drawing.Point(56, 172);
            this.fillingPackagingDistributionheckBox.Name = "fillingPackagingDistributionheckBox";
            this.fillingPackagingDistributionheckBox.Size = new System.Drawing.Size(188, 17);
            this.fillingPackagingDistributionheckBox.TabIndex = 5;
            this.fillingPackagingDistributionheckBox.Text = "Filling, Packaging, and Distribution";
            this.toolTip1.SetToolTip(this.fillingPackagingDistributionheckBox, "Click to select or deselect the Filling, Packaging, and Distribution life cycle s" +
        "tage.");
            this.fillingPackagingDistributionheckBox.UseVisualStyleBackColor = true;
            // 
            // useReuseMaintenanceCheckBox
            // 
            this.useReuseMaintenanceCheckBox.AutoSize = true;
            this.useReuseMaintenanceCheckBox.Location = new System.Drawing.Point(57, 209);
            this.useReuseMaintenanceCheckBox.Name = "useReuseMaintenanceCheckBox";
            this.useReuseMaintenanceCheckBox.Size = new System.Drawing.Size(171, 17);
            this.useReuseMaintenanceCheckBox.TabIndex = 6;
            this.useReuseMaintenanceCheckBox.Text = "Use, Reuse, and Maintenance";
            this.toolTip1.SetToolTip(this.useReuseMaintenanceCheckBox, "Click to select or deselect the Use, Reuse, and Maintenance life cycle stage.");
            this.useReuseMaintenanceCheckBox.UseVisualStyleBackColor = true;
            // 
            // recycleWasteManagementCheckBox
            // 
            this.recycleWasteManagementCheckBox.AutoSize = true;
            this.recycleWasteManagementCheckBox.Location = new System.Drawing.Point(56, 246);
            this.recycleWasteManagementCheckBox.Name = "recycleWasteManagementCheckBox";
            this.recycleWasteManagementCheckBox.Size = new System.Drawing.Size(182, 17);
            this.recycleWasteManagementCheckBox.TabIndex = 7;
            this.recycleWasteManagementCheckBox.Text = "Recycle and WasteManagement";
            this.toolTip1.SetToolTip(this.recycleWasteManagementCheckBox, "Click to select or deselect the Recycle and WasteManagement life cycle stage.");
            this.recycleWasteManagementCheckBox.UseVisualStyleBackColor = true;
            // 
            // toolTip1
            // 
            this.toolTip1.AutomaticDelay = 2000;
            // 
            // ExportInventory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(305, 331);
            this.Controls.Add(this.recycleWasteManagementCheckBox);
            this.Controls.Add(this.useReuseMaintenanceCheckBox);
            this.Controls.Add(this.fillingPackagingDistributionheckBox);
            this.Controls.Add(this.productFabricationCheckBox);
            this.Controls.Add(this.materialManufactureCheckBox);
            this.Controls.Add(this.rawMaterialsAcquisitionCheckBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.NextButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ExportInventory";
            this.Text = "TRACI - Export Inventory";
            this.toolTip1.SetToolTip(this, "Select desired life cycle stages and click \"Next\" to export to Excel.");
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button NextButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox rawMaterialsAcquisitionCheckBox;
        private System.Windows.Forms.CheckBox materialManufactureCheckBox;
        private System.Windows.Forms.CheckBox productFabricationCheckBox;
        private System.Windows.Forms.CheckBox fillingPackagingDistributionheckBox;
        private System.Windows.Forms.CheckBox useReuseMaintenanceCheckBox;
        private System.Windows.Forms.CheckBox recycleWasteManagementCheckBox;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Traci3
{
    /// <summary>
    /// Main form for the TRACI application.
    /// </summary>
    public partial class MainForm : Form
    {
        // Treenodes added to the Treeview used to show the hierarchy and navigate the life 
        // cycle elements.
        TreeNode m_ProjectNode = new TreeNode("Project");
        TreeNode m_ProductNode = new TreeNode("Products");
        TreeNode m_ProcessesNode = new TreeNode("Processes");
        TreeNode m_ReleasesNode = new TreeNode("Releases");
        TreeNode m_ResourcesNode = new TreeNode("Resources");
        TreeNode m_LocationsNode = new TreeNode("Locations");

        // Name of the current TRACI file.
        string m_FileName = String.Empty;

        // indicates whether any of the lifecycle elements has changes (is dirty).
        bool m_dirty;

        // indicates whether a file is being loaded, or large scale changes are being
        // made to the lifecycle data. This circumvents the event handler that updates the
        // Treeview when changes to the life cycle is made, speeding up the loading or
        // importing process.
        bool m_loading;
        //System.Data.DataSet traci2Data;

        // The current Traci Project.
        Traci3.TraciProject m_traciProject = new Traci3.TraciProject();

        /// <summary>
        /// Class Constructor for the main TRACI project form.
        /// </summary>
        public MainForm()
        {
            // call to InitializeComponent which cerates objects on the form.
            InitializeComponent();

            // Initialize the Project Node on the TreeView. This is done by setting the 
            // current TRACI project in the Treenode tag, selecting it in the PropertyGrid, 
            // and adding it to the TreeView.
            m_ProjectNode.Tag = m_traciProject;
            this.propertyGrid1.SelectedObject = m_ProjectNode.Tag;
            this.treeView1.Nodes.Add(m_ProjectNode);

            // Initialize the Product Node on the TreeView. Also, add an event handler that 
            // detects whether products in the project have been changed.
            //m_ProductNode.Tag = m_traciProject.Products;
            m_traciProject.Products.ListChanged += new System.ComponentModel.ListChangedEventHandler(this.products_ListChanged);
            this.treeView1.Nodes.Add(m_ProductNode);

            // Initialize the Process Node on the TreeView. Also, add an event handler that 
            // detects whether processes in the project have been changed.
            //m_ProcessesNode.Tag = m_traciProject.Processes;
            m_traciProject.Processes.ListChanged += new System.ComponentModel.ListChangedEventHandler(this.products_ListChanged);
            this.treeView1.Nodes.Add(m_ProcessesNode);

            // Initialize the Release Node on the TreeView. Also, add an event handler that 
            // detects whether releases in the project have been changed.
            //m_ReleasesNode.Tag = m_traciProject.Releases;
            m_traciProject.Releases.ListChanged += new System.ComponentModel.ListChangedEventHandler(this.products_ListChanged);
            this.treeView1.Nodes.Add(m_ReleasesNode);

            // Initialize the Resource Node on the TreeView. Also, add an event handler that 
            // detects whether resources in the project have been changed.
            //m_ResourcesNode.Tag = m_traciProject.Resources;
            m_traciProject.Resources.ListChanged += new System.ComponentModel.ListChangedEventHandler(this.products_ListChanged);
            this.treeView1.Nodes.Add(m_ResourcesNode);

            // Initialize the Location Node on the TreeView. The AddLocations method is 
            // called to insert the locations into the Treeview. The need for adding the 
            // locations was to test the static load process, and the locations are not 
            // required in the TreeView. This may be removed in the future. It remains because 
            // this call loads the location data into the static class, speeding future
            // operations at the expense of start up time.
            this.treeView1.Nodes.Add(m_LocationsNode);
            this.AddLocations();

            // the following line of code created a DataTable containing the chemical impact
            // data to compare TRACI v2. This comparison was found to not be needed and the
            // use of the method removed. Code retained for possible future use.
            // this.LoadChemicals();

            // the project has not been modified, so it is not dirty.
            m_dirty = false;
            // nor is it loading.
            m_loading = false;
        }

        // this method loaded the chemical impact factor data into a DataTable. It is currently
        // not used.
        //private void LoadChemicals()
        //{
        //    //traci2Data = new DataSet();
        //    System.IO.FileStream myStream = new System.IO.FileStream("C:\\Traci3\\Traci3\\bin\\Debug\\Traci2 data.xlsx", System.IO.FileMode.Open);
        //    SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(myStream, true);

        //    WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
        //    Sheets sheets = workbookPart.Workbook.Sheets;
        //    System.Collections.ArrayList list1 = new System.Collections.ArrayList();
        //    System.Collections.ArrayList list2 = new System.Collections.ArrayList();
        //    foreach (Sheet sheet in sheets)
        //    {
        //        list1.Add(sheet.Name);
        //        list2.Add(sheet.Id);
        //    }

        //    foreach (WorksheetPart worksheetPart in workbookPart.WorksheetParts)
        //    {            
        //        SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();
        //        bool headers = true;
        //        var shareStringTablePart = spreadsheetDocument.WorkbookPart.SharedStringTablePart;
        //        string idOfPart = workbookPart.GetIdOfPart(worksheetPart);
        //        string tableName = String.Empty;
        //        for (int i = 0; i < list2.Count; i++ )
        //        {
        //            if (list2[i].ToString() == idOfPart) tableName = list1[i].ToString();
        //        }
        //        //DataTable dt = new DataTable(tableName);
        //        //traci2Data.Tables.Add(dt);
        //        foreach (Row r in sheetData.Elements<Row>())
        //        {
        //            if (headers)
        //            {
        //                foreach (Cell cell in r.Elements<Cell>())
        //                {
        //                    //dt.Columns.Add(this.GetCellValue(spreadsheetDocument, cell));
        //                    headers = false;
        //                }
        //            }
        //            else
        //            {
        //                DataRow row = dt.NewRow();
        //                dt.Rows.Add(row);
        //                int i = 0;
        //                foreach (Cell cell in r.Elements<Cell>())
        //                {
        //                    string temp = this.GetCellValue(spreadsheetDocument, cell);
        //                    row[i] = temp;
        //                    i++;
        //                }
        //            }
        //        }
        //    }
        //    myStream.Close();
        //    //this.m_traciProject.Traci2Data = traci2Data;
        //}


        /// <summary>
        /// This method adds the locations into the TreeView. 
        /// </summary>
        /// <remarks>
        /// <para>
        /// Locations in TRACI are cross referenced in a couple of ways. First, TRACi
        /// considers whether the location is inside th US or outside the US. At present,
        /// the only location outside the US is "Non-US". For locations inside the US, they 
        /// are broken down into States and Counties. Currently, each state has its county 
        /// listed as statewide. Each county is tied to a state.
        /// </para>
        /// <para>
        /// The United States is also divided into regions in two ways. First, a location is 
        /// either east or west of the Mississippi River. Second, the country is divided into
        /// four geographical regions, Northeast, Midwest, South and West. Each location in 
        /// the US, (state or county) is assigned as either east or west of teh Mississippi River 
        /// and is placed into either the Northeast, Midwest, South or West regions.
        /// </para>
        /// </remarks>
        private void AddLocations()
        {
            Location[] locations = ImpactFactorCollection.Locations;

            // Create the United States Node and add it to the locaiton nodes.
            TreeNode m_USNode = new TreeNode("United States");
            this.m_LocationsNode.Nodes.Add(m_USNode);

            // Create the outside the United States Node and add it to the locaiton nodes.
            TreeNode m_nonUSNode = new TreeNode("Outside United States");
            this.m_LocationsNode.Nodes.Add(m_nonUSNode);

            // Create the States Node and add it to the locaiton nodes.
            TreeNode m_StateNode = new TreeNode("States");
            m_USNode.Nodes.Add(m_StateNode);

            // Create the Regions Node and add it to the locaiton nodes.
            TreeNode m_RegionNode = new TreeNode("Regions");
            m_USNode.Nodes.Add(m_RegionNode);

            //iterate through each location and put it into nodes indicating where it is
            // inside the US, or outside, a state, a region, or a county...
            foreach (Location place in locations)
            {

                // create a node for the location
                TreeNode newNode = new TreeNode(place.Name);
                newNode.Tag = place;
                // if its not in the US, stick it in the Outside United States node.
                if (!place.IsInUS)
                {
                    m_nonUSNode.Nodes.Add(newNode);
                }

                    // So its in the US, is it a region or a state?
                else
                {
                    // If its a region, add it to the region node
                    if (place.GeoLevelId == GeographicalLevel.Region)
                    {
                        m_RegionNode.Nodes.Add(newNode);
                    }

                    // if its a state, add it to the state node.
                    if (place.GeoLevelId == GeographicalLevel.State)
                    {
                        m_StateNode.Nodes.Add(newNode);
                    }
                    //// if not, its a county. 
                    //else
                    //{
                    //    // find its state
                    //    foreach (TreeNode node in m_StateNode.Nodes)
                    //    {
                    //        // and add it there.
                    //        if (place.StateId == ((location)(node.Tag)).GeogID)
                    //            node.Nodes.Add(m_Node);
                    //    }
                    //}
                }
            }
            // put the states into their appropriate region node...
            foreach (Location place in locations)
            {
                // if its in the US
                if (place.IsInUS)
                {
                    // and is a state
                    if (place.GeoLevelId == GeographicalLevel.State)
                    {
                        // look at each region
                        foreach (TreeNode node in m_RegionNode.Nodes)
                        {
                            // and if the state is in it,
                            if (place.Region == ((Location)node.Tag).Region)
                            {
                                // create a node and add the state to the region.
                                TreeNode newNode = new TreeNode(place.Name);
                                newNode.Tag = place;
                                node.Nodes.Add(newNode);
                            }
                            if (place.EastWestofMississippi == ((Location)node.Tag).EastWestofMississippi)
                            {
                                // create a node and add the state to the region.
                                TreeNode newNode = new TreeNode(place.Name);
                                newNode.Tag = place;
                                node.Nodes.Add(newNode);
                            }
                        }
                    }
                }
            }

            // and put the counties in their state nodes...
            foreach (Location place in locations)
            {
                if (place.IsInUS)
                {
                    if (place.GeoLevelId == GeographicalLevel.County)
                    {
                        // look at each state node
                        foreach (TreeNode state in m_StateNode.Nodes)
                        {
                            // and if it matches the counties state...
                            if (place.StateId == ((Location)(state.Tag)).GeogID)
                            {
                                // add the county to the state...
                                TreeNode m_Node = new TreeNode(place.Name);
                                m_Node.Tag = place;
                                state.Nodes.Add(m_Node);
                            }
                        }

                        // next look at each region...
                        foreach (TreeNode state in m_RegionNode.Nodes)
                        {
                            // and the states in that region...
                            foreach (TreeNode node in state.Nodes)
                            {
                                // and if the county is in that state
                                if (place.StateId == ((Location)(node.Tag)).GeogID)
                                {
                                    // add the county to the state...
                                    TreeNode m_Node = new TreeNode(place.Name);
                                    m_Node.Tag = place;
                                    node.Nodes.Add(m_Node);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Responds the an event fired by the list of products changing...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void products_ListChanged(object sender, ListChangedEventArgs e)
        {
            // the list changed, so the current saved version is no loger valid (is dirty).
            m_dirty = true;

            // if it isn't loading,
            if (!m_loading)
            {
                /// we will refresh the product list in the TreeView.
                this.RefreshProducts();
            }
        }

        /// <summary>
        /// This method updates the products in the TreeView.
        /// </summary>
        private void RefreshProducts()
        {
            // clear the product, process, release and resource nodes...
            m_ProductNode.Nodes.Clear();
            m_ProcessesNode.Nodes.Clear();
            m_ReleasesNode.Nodes.Clear();
            m_ResourcesNode.Nodes.Clear();

            // look at each product...
            foreach (Traci3.Product product in m_traciProject.Products)
            {               
                // create a new node for the product
                TreeNode productNode = new TreeNode(product.Name);
                // tag the node with the product.
                productNode.Tag = product;
                // add it to the product node.
                m_ProductNode.Nodes.Add(productNode);        

                // now for each process in the product...
                foreach (Traci3.Process process in product.Processes)
                {
                    // create two nodes for the process, tage them with the process,
                    // and to add it to the product node and the TreeView's process node...
                    TreeNode processNode = new TreeNode(process.Name);
                    processNode.Tag = process;
                    productNode.Nodes.Add(processNode);
                    TreeNode processNode1 = new TreeNode(product.Name + "." + process.Name);
                    processNode1.Tag = process;
                    m_ProcessesNode.Nodes.Add(processNode1);

                    // create release nodes foreach product node tto add the 
                    // releasess and resources to the prcess nodes.
                    TreeNode releaseNode = new TreeNode("Releases");
                    TreeNode releaseNode1 = new TreeNode("Releases");
                    TreeNode resourceNode = new TreeNode("Resources");
                    TreeNode resourceNode1 = new TreeNode("Resources");
                    processNode.Nodes.Add(releaseNode);
                    processNode1.Nodes.Add(releaseNode1);
                    processNode.Nodes.Add(resourceNode);
                    processNode1.Nodes.Add(resourceNode1);

                    // now go through the releases in each process...
                    foreach (Traci3.Release release in process.Releases)
                    {
                        // set the process as the release's process 
                        release.Process = process;
                                                
                        // create three nodes for the release, tag the nodes with the release
                        // and add the release to the process's node, the process's product's
                        // node and the TreeView's release node. This gives three places where 
                        // the node can be found in the tree.
                        TreeNode currentNode = new TreeNode(release.Name);
                        currentNode.Tag = release;
                        releaseNode.Nodes.Add(currentNode);
                        TreeNode currentNode1 = new TreeNode(process.Name + "." + release.Name);
                        currentNode1.Tag = release;
                        releaseNode1.Nodes.Add(currentNode1);
                        TreeNode currentNode2 = new TreeNode(product.Name + "." + process.Name + "." + release.Name);
                        currentNode2.Tag = release;
                        m_ReleasesNode.Nodes.Add(currentNode2);
                    }

                    // now go through the resources in each process...
                    foreach (Traci3.Resource resource in process.Resources)
                    {
                        // set the process as the resource's process (maybe redundant)
                        resource.Process = process;
                        // create three nodes for the resource, tag the nodes with the resource
                        // and add the resource to the process's node, the process's product's
                        // resource node and the TreeView's resource node. This gives three places where 
                        // the node can be found in the tree.
                        TreeNode currentNode = new TreeNode(resource.Name);
                        TreeNode currentNode1 = new TreeNode(process.Name + "." + resource.Name);
                        TreeNode currentNode2 = new TreeNode(product.Name + "." + process.Name + "." + resource.Name);
                        currentNode.Tag = resource;
                        currentNode1.Tag = resource;
                        currentNode2.Tag = resource;
                        resourceNode.Nodes.Add(currentNode);
                        resourceNode1.Nodes.Add(currentNode1);
                        m_ResourcesNode.Nodes.Add(currentNode2);
                    }
                }
            }
        }

        // this method currently not used.
        //private void RefreshNames()
        //{
        //    foreach (TreeNode productNode in m_ProductNode.Nodes)
        //    {
        //        productNode.Text = ((Traci3.Product)productNode.Tag).Name;
        //        foreach (TreeNode processNode in productNode.Nodes)
        //        {
        //            processNode.Text = ((Traci3.Process)processNode.Tag).Name;
        //            foreach (TreeNode releasesNode in processNode.Nodes[0].Nodes)
        //            {
        //                releasesNode.Text = ((Traci3.Release)releasesNode.Tag).Name;
        //            }
        //            foreach (TreeNode resourceNode in processNode.Nodes[1].Nodes)
        //            {
        //                resourceNode.Text = ((Traci3.Resource)resourceNode.Tag).Name;
        //            }
        //        }
        //    }
        //    foreach (TreeNode processNode in m_ProcessesNode.Nodes)
        //    {
        //        processNode.Text = ((Traci3.Process)processNode.Tag).Product.Name + "." + ((Traci3.Process)processNode.Tag).Name;
        //        foreach (TreeNode releasesNode in processNode.Nodes[0].Nodes)
        //        {
        //            releasesNode.Text = ((Traci3.Release)releasesNode.Tag).Name;
        //        }
        //        foreach (TreeNode resourceNode in processNode.Nodes[1].Nodes)
        //        {
        //            resourceNode.Text = ((Traci3.Resource)resourceNode.Tag).Name;
        //        }
        //    }
        //    foreach (TreeNode releasesNode in m_ReleasesNode.Nodes)
        //    {
        //        releasesNode.Text = ((Traci3.Release)releasesNode.Tag).Process.Product.Name + "." + ((Traci3.Release)releasesNode.Tag).Process.Name + "." + ((Traci3.Release)releasesNode.Tag).Name;
        //    }
        //    foreach (TreeNode resourceNode in m_ResourcesNode.Nodes)
        //    {
        //        resourceNode.Text = ((Traci3.Resource)resourceNode.Tag).Process.Product.Name + "." + ((Traci3.Resource)resourceNode.Tag).Process.Name + "." + ((Traci3.Resource)resourceNode.Tag).Name;
        //    }
        //}

        //Currently not used.
        //private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        //{
        //    this.propertyGrid1.SelectedObject = null;
        //    if (e.Node.Tag != null)
        //        if (!typeof(IBindingList).IsAssignableFrom(e.Node.Tag.GetType()))
        //            this.propertyGrid1.SelectedObject = e.Node.Tag;
        //}

        // 
        /// <summary>
        /// This method puts the currently selected TreeNode's tag as the object displayed 
        /// in the property grid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.propertyGrid1.SelectedObject = e.Node.Tag;
        }

        /// <summary>
        /// Marks the current project as needing saved if a value in the property grid is changed.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            // if a value in the property grid is changed, the project now needs saved (is dirty).
            m_dirty = true;
            //this.propertyGrid1.SelectedObject = this.treeView1.SelectedNode.Tag;
        }

        /// <summary>
        /// Closes the prgram when the "Exit" menu item is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Responds to the event fired when the user clicks the open menu item. 
        /// 
        /// </summary>
        /// <remarks>If the file needs saved (is dirty), the user will be given the option to 
        /// save the file. After the user either declines to save the file or the file is saved,
        /// an exitsing project loaded into the memory and the TreeView.</remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // if the file needs saved (is dirty)...
            if (m_dirty)
            {
                // lets ask if the user wants to save the file...
                var result = MessageBox.Show("Would you like to save this project?", "Traci", MessageBoxButtons.YesNoCancel);

                // if he does...
                if (result == DialogResult.Yes)
                {
                    // save it
                    this.saveToolStripMenuItem_Click(sender, e);
                }
            }

            // and discard the current project.
            m_traciProject = null;

            // loading (so don't update the tree... takes time)
            m_loading = true;

            // the file stream (currently null)
            System.IO.Stream myStream = null;

            // Create and parameterize an OpenFileDialog
            System.Windows.Forms.OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.FileName = "TraciDocument";
            openFileDialog1.DefaultExt = ".traci";
            openFileDialog1.Filter = "TRACI JSon Files (.json)| *.json|TRACI Files (.traci)| *.traci";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            // show the dialog and if a file is selected...
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // not try...catch  Want this to fail hard for the time being!!!
                //try
                //{

                // save the file name to the class
                m_FileName = openFileDialog1.FileName;

                // open the filestream
                if ((myStream = openFileDialog1.OpenFile()) != null)
                {
                    if (System.IO.Path.GetExtension(m_FileName) == ".traci")
                    {
                        // Create a binary serializer.
                        System.Runtime.Serialization.Formatters.Binary.BinaryFormatter serializer = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                        // and deserialize (load) the stream into a TRACI project.
                        m_traciProject = (Traci3.TraciProject)serializer.Deserialize(myStream);

                        // done with the filestream, so close it.
                        myStream.Close();
                    }

                    if (System.IO.Path.GetExtension(m_FileName) == ".json")
                    {
                        // Create a json data contract serializer to write the project to the filestream.
                        System.Runtime.Serialization.Json.DataContractJsonSerializer jsonSerializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(TraciProject));
                        // serialize (aka save) the file to the stream
                        m_traciProject = (Traci3.TraciProject)jsonSerializer.ReadObject(myStream);
                        // close the file
                        myStream.Close();
                    }


                    // tag the project
                    this.m_ProjectNode.Tag = m_traciProject;
                }

                // since we are not done loading, reactivate the RefreshProduct method
                m_loading = false;
                // Refresh the Products in the TreeView.
                this.RefreshProducts();
                // and stick the project into the property grid.
                this.propertyGrid1.SelectedObject = this.m_traciProject;

                // and set the text on the form to include the current file name.
                this.Text = "US Environmental Protection Agency - TRACI - " + System.IO.Path.GetFileName(m_FileName);
                //}
                //catch (Exception ex)
                //{
                //    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                //}
            }
        }

        /// <summary>
        /// Responds to the event fired when the user elects to save the project file. 
        /// </summary>
        /// <remarks>
        /// This method determines whether a file name has been assigned and if one exists,
        /// overwrites the existing file. If there is no current filename, the 
        /// <see cref="saveAsToolStripMenuItem_Click"/> method is called.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // has the filename been set? If not, do a "SaveAs" to get a filename.
            if (m_FileName == String.Empty)
            {
                // Call the Save As method...
                this.saveAsToolStripMenuItem_Click(sender, e);
                // and just retun. Let's assume the user either specified a file name
                // and the file save there or he didn't want to save the file after all, as 
                // there will be no filename set.
                return;
            }
            // not try...catch  Want this to fail hard for the time being!!!
            //try
            //{

            // Create a stream using the filename
            System.IO.Stream myStream = new System.IO.FileStream(m_FileName, System.IO.FileMode.Create);

            // if the stream exists...
            if (myStream != null)
            {
                if (System.IO.Path.GetExtension(m_FileName) == ".json")
                {
                    // Create a json data contract serializer to write the project to the filestream.
                    System.Runtime.Serialization.Json.DataContractJsonSerializer jsonSerializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(TraciProject));
                    // serialize (aka save) the file to the stream
                    jsonSerializer.WriteObject(myStream, m_traciProject);
                    // close the file
                    myStream.Close();
                }
                if (System.IO.Path.GetExtension(m_FileName) == ".xlsx")
                {
                    // Create a binary serializer to write the project to the filestream.
                    System.Runtime.Serialization.Formatters.Binary.BinaryFormatter serializer = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    // serialize (aka save) the file to the stream
                    serializer.Serialize(myStream, m_traciProject);
                    // close the file
                    myStream.Close();
                }
                // and since we just saved it, the file on the disk is the most recent one
                // and is no loger dirty.
                m_dirty = false;
            }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
            //}
        }

        /// <summary>
        /// Responds to the event fired when the user elects to save the project file using a new file name. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Create a stream variable for use below.
            System.IO.Stream myStream = null;

            // Create and parameterize a SaveFileDialog.
            System.Windows.Forms.SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.FileName = "TraciDocument";
            saveFileDialog1.DefaultExt = ".traci";
            saveFileDialog1.Filter = "TRACI JSon Files (.json)| *.json|TRACI Files (.traci)| *.traci";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;

            // Show the Save File Dialog.
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // not try...catch  Want this to fail hard for the time being!!!
                //try
                //{

                // open the file..
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    // now that we have a filename, save it at the class level
                    m_FileName = saveFileDialog1.FileName;
                    // and update the text of the form to reflect the filename.
                    this.Text = "US Environmental Protection Agency - TRACI - " + System.IO.Path.GetFileName(m_FileName);
                    if (System.IO.Path.GetExtension(m_FileName) == ".json")
                    {
                        // Create a json data contract serializer to write the project to the filestream.
                        System.Runtime.Serialization.Json.DataContractJsonSerializer jsonSerializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(TraciProject));
                        // serialize (aka save) the file to the stream
                        jsonSerializer.WriteObject(myStream, m_traciProject);
                        // close the file
                        myStream.Close();
                    }
                    if (System.IO.Path.GetExtension(m_FileName) == ".xlsx")
                    {
                        // Create a binary serializer to write the project to the filestream.
                        System.Runtime.Serialization.Formatters.Binary.BinaryFormatter serializer = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                        // serialize (aka save) the file to the stream
                        serializer.Serialize(myStream, m_traciProject);
                        // close the file
                        myStream.Close();
                    }
                    // and since we just saved it, the file on the disk is the most recent one
                    // and is no loger dirty.
                    m_dirty = false;
                }
                //}
                //catch (Exception ex)
                //{
                //    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                //}
            }
        }

        /// <summary>
        /// Responds to the event fired when the user clicks the menu item to creat a 
        /// new file.
        /// </summary>
        /// <remarks>
        /// If the current file has not been saved (is dirty), the user is asked to save the existing file.
        /// If the user declines to save the project, the project is replaced with a new, empty project file.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // if the file needs saved (is dirty)...
            if (m_dirty)
            {
                // lets ask if the user wants to save the file...
                var result = MessageBox.Show("Would you like to save this project?", "Traci", MessageBoxButtons.YesNoCancel);

                // if he does...
                if (result == DialogResult.Yes)
                {
                    // save it
                    this.saveToolStripMenuItem_Click(sender, e);
                }
            }
            // and set the project to a new project.
            m_traciProject = new Traci3.TraciProject();
        }

        /// <summary>
        /// Responds to the event fired when the user wants to close the file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // if it needs saved...
            if (m_dirty)
            {
                // ask the user if the file should be saved...
                var result = MessageBox.Show("Would you like to save this project?", "Traci", MessageBoxButtons.YesNoCancel);

                // if so...
                if (result == DialogResult.Yes)
                {
                    // save it.
                    this.saveToolStripMenuItem_Click(sender, e);
                }
            }
        }

        // 
        /// <summary>
        /// This method imports a Excel spreadhseet exported from the Original 
        /// TRACI program. 
        /// </summary>
        /// <remarks>
        /// <para>
        /// TRACI2 exports a Excel 1997 formatted spreadsheet file with the following columns: 
        /// A. ProjName, B. ProductName, C. Process, D. State, E. LifeStage, F. Name, G. CASNumber,
        /// H. Media, I. Qty, and JI. UOM.
        /// </para>
        /// <para>
        /// The Excel 1997 was used by Microsoft for Excel versions 1997 through 2003. Microsoft Excel
        /// 2007 and 2010 use Microsoft's Open Office spreadsheet format with an 'xlsx' file 
        /// extension. This version of TRACI will be able to import both 'xls' and 'xlsx' 
        /// format files, using the above column headers. As a result, users wishing to import
        /// files into TRACI can prepare spreadsheet documents with the above column headers in the
        /// appropriate columns. Valid chemical names for releases and locations will be provided in
        /// a separate document for use in creating a import spreadsheet.
        /// </para>
        /// <para>
        /// NOTE: COMMA DELIMITED FILES (CSV) WILL NOT BE SUPPORTED FOR IMPORT INTO TRACTI DUE TO
        /// THE USE OF COMMAS IN THE NAMES OF CHEMICAL COMPOUNDS.
        /// </para>
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void importToolStripTextBox1_Click(object sender, EventArgs e)
        {
            // create on open file dialog box wil the appropriate fiel types.
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            //openFileDialog1.DefaultExt = ".xlsx";
            openFileDialog1.Filter = "Excel 2003 File 2003 (.xls)| *.xls|Excel 2007 or 2010 File (.xlsx)| *.xlsx|All files (*.*)|*.* ";
            openFileDialog1.FilterIndex = 1;

            // If the open file dialog has been successful...
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // we are loading, so don't update the TreeView
                m_loading = true;

                // determine the extension of the file
                String extension = System.IO.Path.GetExtension(openFileDialog1.FileName);

                // if its an EXCEL 1997-2003 file, with a .xls extension
                if (System.IO.Path.GetExtension(openFileDialog1.FileName) == ".xls")
                {
                    // open it as a Excel 1997-2003 document
                    this.ImportExcel2003(openFileDialog1.FileName);
                }

                // just to be safe, check to make suer that the EXCEL 2007/2010 documents
                // have the correct extension (trap a user error)
                if (System.IO.Path.GetExtension(openFileDialog1.FileName) == ".xlsx")
                {
                    // and open the file as an Open Office bassed EXCEL file.
                    this.ImportExcel2007(openFileDialog1.FileName);
                }
            }
            // done loading
            m_loading = false;
            // so update the TreeView.
            this.RefreshProducts();
        }

        /// <summary>
        /// Import the EXCEL 2003 version exported from TRACI 2.
        /// </summary>
        /// <remarks>
        /// <para>
        /// TRACI2 exports a Excel 2003 formatted spreadsheet with the following columns: 
        /// A. ProjName, B. ProductName, C. Process, D. State, E. LifeStage, F. Name, G. CASNumber,
        /// H.  Media,  I. Qty, and J. UOM. Use these headers for any Excel 2003 Spreadsheet to 
        /// import into this version of TRACI. This method uses a SQL query to create the data
        /// table. Please name the spreadsheet "qryExport".
        /// /// </para>
        /// <para>
        /// This import method uses the OLE database manager to import the file. The process 
        /// involves creating a <see cref="System.Data.DataTable"/> using the SQL command
        /// "SELECT * FROM [qryExport]". The resulting DataTable will have the items in
        /// row 1 of the spreadsheet as column headers in the DataTable.
        /// </para>
        /// </remarks>
        /// <param name="filename">The name of the EXCEL 2007 file to be opened.</param>
        void ImportExcel2003(string filename)
        {
            // we are loading products and processes, so don't update the TreeView yet.
            m_loading = true;

            // Create a new DataTable, the database command, and the data adapter.
            DataTable dt = new DataTable();

            // Open the connection to the EXCEL spreadsheet unsing the connection string 
            // in the OLE data connection...
            String excelConnStr = @"Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + filename + "; Extended Properties=Excel 8.0;";
            System.Data.OleDb.OleDbConnection excelConn = new System.Data.OleDb.OleDbConnection(excelConnStr);
            excelConn.Open();

            // Create a OLE database command to select all data from the TRACI export table...
            System.Data.OleDb.OleDbCommand excelCommand = new System.Data.OleDb.OleDbCommand("SELECT * FROM [qryExport]", excelConn);

            // Create a OLE data adapter for the command...
            System.Data.OleDb.OleDbDataAdapter excelDataAdapter = new System.Data.OleDb.OleDbDataAdapter(excelCommand);

            // fill the data table...
            excelDataAdapter.Fill(dt);
            dt.AcceptChanges();

            // and load the DataTable inot the project.
            this.LoadProjectDataFromDataTable(dt);
        }

        /// <summary>
        /// Creates a project from a DataTable.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method is used to process the <see cref="System.Data.DataTable"/> generated 
        /// using the file exported from TRACI2 to Excel 2003. That spreadsheet contains the 
        /// a spreadsheet named "qryExport" having the following columns: 
        /// A. ProjName, B. ProductName, C. Process, D. State, E. LifeStage, F. Name, G. CASNumber,
        /// H. Qty, I. UOM and J. Media. Use these headers and table name for any Excel 2003 
        /// Spreadsheet to import into this version of TRACI. 
        /// </para>
        /// <para>
        /// This import method uses the OLE database manager to import the file. The process 
        /// involves creating a <see cref="System.Data.DataTable"/> using the SQL command
        /// "SELECT * FROM [qryExport]". The resulting DataTable will have the items in
        /// row 1 of the spreadsheet as column headers in the DataTable.
        /// </para>
        /// </remarks>
        /// <param name="filename"></param>
        /// </remarks>
        /// <param name="table"></param>
        void LoadProjectDataFromDataTable(System.Data.DataTable table)
        {
            // iterate through the data rows in the data table...
            foreach (DataRow r in table.Rows)
            {

                //Get the Project Name from the "ProjName" column.
                string temp = r["ProjName"].ToString();
                if (m_traciProject.Name == String.Empty) this.m_traciProject.Name = temp;

                //Get the Product Name from the "ProductName" column
                temp = r["ProductName"].ToString();

                // if the product is not in the project
                if (!m_traciProject.ContainsProduct(temp))
                {
                    m_traciProject.AddProduct(temp);
                }

                // Get and use the product.
                Product product = m_traciProject.GetProduct(temp);

                //Get the Process Name name
                string processName = r["Process"].ToString();

                // check to see if the product already contains the process
                if (!product.ContainsProcess(processName))
                {
                    //Geographical Location
                    temp = r["State"].ToString();
                    Location loc = ImpactFactorCollection.GetLocation(temp);

                    //Get the Life Cycle Stage because we will use it in adding the process...
                    temp = r["LifeStage"].ToString();
                    LifeCycleStage stage = LifeCycleStage.fillingPackagingDistribution;

                    // with the appropraite life cycle stage
                    if (temp == "Product Fabrication") stage = LifeCycleStage.productFabrication;
                    if (temp == "Materials Manufacturing") stage = LifeCycleStage.materialManufacture;
                    if (temp == "Raw Materials Acquisition") stage = LifeCycleStage.rawMaterialsAcquisition;
                    if (temp == "Use/Reuse/Maintenance") stage = LifeCycleStage.useReuseMaintenance;
                    if (temp == "Recycle/Waste Management") stage = LifeCycleStage.recycleWasteManagement;

                    // and add it to the product
                    product.AddProcess(processName, stage, loc);

                }
                // Get and use the process.
                Process process = product.GetProcess(processName);

                //Get the Resource/Release Name
                String resouceName = r["Name"].ToString();

                // create a CAS Number varaible with an empty value
                String casNumber = String.Empty;

                //if there is a CAS number
                if (!DBNull.Value.Equals(r["CASNumber"]))
                    // get it.
                    casNumber = r["CASNumber"].ToString();

                //Get the release media
                String media = r["Media"].ToString();

                // Get the release qunatity
                double quantity = double.Parse(r["Qty"].ToString());

                // Get the unit of measure
                String UOM = r["UOM"].ToString();

                // and add the release or resource.
                process.AddReleaseOrResource(resouceName, casNumber, media, quantity, UOM);
            }
            m_loading = false;
        }

        /// <summary>
        /// Imports an EXCEL 2007 version into TRACI.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method makes use of Microsoft's Open Office SDK to import the inventory data
        /// into TRACI. As a result, this import will work on computers that do not have Microsoft
        /// Excel installed on them. 
        /// </para>
        /// <para>
        /// The spreasheet is processed differently from the method used to import data form
        /// TRACI2 Excel 2003 formatted spreadsheets. The column order remains the same, but 
        /// the column headers are not used to get cell data. This methgod will import an 
        /// Excel 2007 spreadsheet file that contains a single spreadsheet with data organized 
        /// in columns in this order: A. Project Name, B. Product Name, C. Process, D. State, 
        /// E. Life Cycle Stage, F. Name, G. CAS Number, H . Media I. Quantity, and J. Unit 
        /// of Measure. The first row of the spreadsheet is ignored and can be filled with any 
        /// information desired. The remaining rows must contain the data in the prescribed 
        /// order.
        /// </para>
        /// </remarks>
        /// <param name="filename">The name of the EXCEL 2007 file to be opened.</param>
        void ImportExcel2007(string filename)
        {
            // We are loading data into the project data tree.
            m_loading = true;

            // Create the stream and load it into an Open Office Spreadsheet document.
            System.IO.Stream myStream = new System.IO.FileStream(filename, System.IO.FileMode.Open);
            SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(myStream, true);

            // Get the Workbook from the spreadsheet document,
            WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
            // the first or default worksheet from the workbook, 
            WorksheetPart worksheet = workbookPart.WorksheetParts.FirstOrDefault();
            // the sheetdata from the worksheet,
            SheetData sheetData = worksheet.Worksheet.Elements<SheetData>().First();
            // and the shared string table for the sheet data (this is where some cell values are stored).
            var shareStringTablePart = spreadsheetDocument.WorkbookPart.SharedStringTablePart;

            // A boolean value indicating we are on the first row...
            bool firstRow = true;

            // now iterate through the rows of the spreadsheet
            foreach (Row r in sheetData.Elements<Row>())
            {
                // Ignore the first row as it contains headers...
                if (firstRow)
                {
                    // since past the first row, don't ingore any more.
                    firstRow = false;
                }
                else
                {
                    // get the cells in the row...
                    IEnumerator<Cell> cells = r.Elements<Cell>().GetEnumerator();

                    // get the next cell, read its value, and use it as the project name.
                    cells.MoveNext();
                    string temp = this.GetCellValue(spreadsheetDocument, cells.Current);
                    if (m_traciProject.Name == String.Empty) this.m_traciProject.Name = temp;

                    //Get the Product Name from the netx cell
                    cells.MoveNext();
                    temp = this.GetCellValue(spreadsheetDocument, cells.Current);

                    // if the product is not in the project
                    if (!m_traciProject.ContainsProduct(temp))
                    {
                        m_traciProject.AddProduct(temp);
                    }

                    // Get and use the product.
                    Product product = m_traciProject.GetProduct(temp);

                    //Life Cycle Stage
                    cells.MoveNext();
                    String stage = this.GetCellValue(spreadsheetDocument, cells.Current);

                    //Process Name
                    cells.MoveNext();
                    string processName = this.GetCellValue(spreadsheetDocument, cells.Current);
                    // check to see if the product already contains the process
                    if (!product.ContainsProcess(processName))
                    {
                        //Get the Life Cycle Stage because we will use it in adding the process...
                        cells.MoveNext();
                        temp = this.GetCellValue(spreadsheetDocument, cells.Current);
                        Location loc = ImpactFactorCollection.GetLocation(temp);

                        LifeCycleStage lcStage = LifeCycleStage.fillingPackagingDistribution;

                        // with the appropraite life cycle stage
                        if (temp == "Product Fabrication") lcStage = LifeCycleStage.productFabrication;
                        if (temp == "Materials Manufacturing") lcStage = LifeCycleStage.materialManufacture;
                        if (temp == "Raw Materials Acquisition") lcStage = LifeCycleStage.rawMaterialsAcquisition;
                        if (temp == "Use/Reuse/Maintenance") lcStage = LifeCycleStage.useReuseMaintenance;
                        if (temp == "Recycle/Waste Management") lcStage = LifeCycleStage.recycleWasteManagement;

                        // and add it to the product
                        product.AddProcess(processName, lcStage, loc);
                    }
                    else
                    {
                        // if not adding a process, move to the next cell...
                        cells.MoveNext();
                    }

                    // Get and use the process.
                    Process process = product.GetProcess(processName);

                    //Resource/Release
                    cells.MoveNext();
                    String resouceName = this.GetCellValue(spreadsheetDocument, cells.Current);
                    String casNumber = String.Empty;

                    // some lines have an empty value for CAS Number, so check cell reference to
                    // make sure we are in column G when we move to the next cell.
                    cells.MoveNext();
                    string reference = cells.Current.CellReference;
                    if (reference.Remove(1) == "G")
                    {
                        // if in column G, the value is the CAS Number
                        //CAS Number
                        casNumber = this.GetCellValue(spreadsheetDocument, cells.Current);
                        cells.MoveNext();
                    }

                    //Media
                    String media = this.GetCellValue(spreadsheetDocument, cells.Current);

                    //Quantity
                    cells.MoveNext();
                    double quantity = this.GetCellDoubleValue(spreadsheetDocument, cells.Current);

                    //UOM
                    cells.MoveNext();
                    String UOM = this.GetCellValue(spreadsheetDocument, cells.Current);

                    // Add the release or resource to the process.
                    process.AddReleaseOrResource(resouceName, casNumber, media, quantity, UOM);
                }
            }

            // close the stream
            myStream.Close();

            // we are no longer loading
            m_loading = false;

            // so refresh the products
            this.RefreshProducts();
        }

        /// <summary>
        /// This method returns a string value from an Excel 2007 cell.
        /// </summary>
        /// <remarks>
        /// Excel 2007 may store a value as an inline string within the cell value, or may
        /// place the value in the shared string table (another part of the spreadsheet. This
        /// method gets the value in the cell, which is either the cell value or the location
        /// in the shared string table. If it is the value, it is returned directly, otherwise
        /// the method checks to see if the value is a shared string, and gets the vlaue from the
        /// shared string table.
        /// </remarks>
        /// <param name="sheet">The spreadsheet to get the value from.</param>
        /// <param name="cell">The cell to get the value from.</param>
        /// <returns>The value of the cell.</returns>
        private String GetCellValue(SpreadsheetDocument sheet, Cell cell)
        {
            // if the cell has a null value, return an empty string...
            if (cell.CellValue == null) return String.Empty;
            // get the cell value (either the desired value or the shared string reference
            String retVal = cell.CellValue.Text;

            // a little protection... the cell data type may be null...
            if (cell.DataType != null)
            {
                // is the value in the shared string table?
                if (cell.DataType == CellValues.SharedString)
                {
                    // if so, the value in the cell is the shared string table reference
                    int index = int.Parse(retVal);
                    // so get the value from the shared string table.
                    retVal = (sheet.WorkbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(index)).InnerText;
                }
            }

            // return the cell value.
            return retVal;
        }

        /// <summary>
        /// This method returns a double value from an Excel 2007 cell.
        /// </summary>
        /// <remarks>
        /// Excel 2007 may sotre a value as an inline string within the cell value, or may
        /// place the value in the shared string table (another part of the spreadsheet. This
        /// method gets the value in the cell, which is either the cell value or the location
        /// in the shared string table. If it is the value, it is returned directly, otherwise
        /// the method checks to see if the value is a shared string, and gets the vlaue from the
        /// shared string table.
        /// </remarks>
        /// <param name="sheet">The spreadsheet to get the value from.</param>
        /// <param name="cell">The cell to get the value from.</param>
        /// <returns>The value of the cell.</returns>
        private double GetCellDoubleValue(SpreadsheetDocument sheet, Cell cell)
        {
            // if the cell has a null value, return the not-a-number value...
            if (cell.CellValue == null) return double.NaN;
            // get the cell value (either the desired value or the shared string reference
            String retVal = cell.CellValue.Text;

            // a little protection... the cell data type may be null...
            if (cell.DataType != null)
            {
                // is the value in the shared string table?
                if (cell.DataType == CellValues.SharedString)
                {
                    // if so, the value in the cell is the shared string table reference
                    int index = int.Parse(retVal);
                    // so get the value from the shared string table.
                    retVal = (sheet.WorkbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(index)).InnerText;
                }
            }

            // first off, some cells may have the value n/a. If so, return a not-a-number value
            if (retVal == "n/a") return Double.NaN;
            // also, some cells may be empty. If so, return a not-a-number value
            if (retVal == String.Empty) return Double.NaN;
            //lastly, the value is a double, so parse it and return.
            return double.Parse(retVal);
        }

        /// <summary>
        /// Exports data to an EXCEL 2007 file.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method makes use of Microsoft's Open Office SDK to export  data
        /// As a result, this export will work on computers that do not have Microsoft
        /// Excel installed on them. 
        /// </para>
        /// <para>
        /// The inventory is saved in the following column order: A. Project Name, B. Product Name, C. Process, D. State, 
        /// E. Life Cycle Stage, F. Name, G. CAS Number, H. Quantity, I. Unit of Measure and 
        /// J. Media. 
        /// </para>
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exportInventoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // use the ExportInventory dialog form to get desired life cyctel stages
            // so create an instance...
            ExportInventory exporter = new ExportInventory();

            // show it as a dialog...
            exporter.ShowDialog();

            // and get the array with whether to show the stage.
            bool[] export = exporter.ExportStages;

            // Next up, use a save file dialog to get the filename. Create the dialog
            // with the proper parameters;
            System.Windows.Forms.SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.FileName = "TraciInventory";
            saveFileDialog1.DefaultExt = ".xslx";
            saveFileDialog1.Filter = "Excel Files (.xslx)| *.xlsx|All files (*.*)|*.* ";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;

            // lastly, this is a single method that exports the inventory, classification table,
            // and characterization information. The difference is determined by what menu item
            // called this method. So, in any case, we are putting in the inventory data, but not 
            // classification or characterization.
            bool inventory = true;
            bool classification = false;
            bool characterization = false;

            // if its a classification file, add the classification data and call the file
            // a classification file
            if (((System.Windows.Forms.ToolStripItem)sender).Name == "classificationToolStripMenuItem")
            {
                inventory = false;
                classification = true;
                saveFileDialog1.FileName = "TraciClassification";
            }
            // if its a characterization file, add the classification and characterization data 
            // and call the file a characterization file
            if (((System.Windows.Forms.ToolStripItem)sender).Name == "charaterizationToolStripMenuItem")
            {
                inventory = false;
                characterization = true;
                classification = true;
                saveFileDialog1.FileName = "TraciCharaterization";
            }

            // show the dialog and if its result is OK...
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // not try...catch  Want this to fail hard for the time being!!!
                //try
                //{
                // the file name from the save file dialog.
                m_FileName = saveFileDialog1.FileName;

                // create the openxXML spreadsheet document file
                DocumentFormat.OpenXml.Packaging.SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Create(m_FileName, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook);

                // Add a WorkbookPart to the document.
                WorkbookPart workbookpart = spreadsheetDocument.AddWorkbookPart();
                workbookpart.Workbook = new Workbook();

                // Create a new sheet data
                SheetData sheetData = new SheetData();
                // and use it to create a new worksheet
                Worksheet worksheet = new Worksheet(sheetData);

                // Add a WorksheetPart to the WorkbookPart.
                WorksheetPart worksheetPart = workbookpart.AddNewPart<WorksheetPart>();
                // and use the worksheet as the worksheetPart's worksheet.
                worksheetPart.Worksheet = worksheet;

                // Add Sheets to the Workbook.
                Sheets sheets = spreadsheetDocument.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());

                // Apend a new worksheet and associate it with the workbook.
                Sheet sheet = new Sheet()
                {
                    Id = spreadsheetDocument.WorkbookPart.
                        GetIdOfPart(worksheetPart),
                    SheetId = 1,
                    Name = "Inventory"
                };

                // add the sheet to the sheets.
                sheets.Append(sheet);

                // now put the data into the spreadsheet/

                // start with the header row.
                // create a new row
                Row row = new Row();

                // create a cell with reference A1, string data type and add the header "Project Name"
                Cell cell = new Cell()
                {
                    CellReference = "A1",
                    DataType = CellValues.String,
                    CellValue = new CellValue("Project Name")
                };
                // and put the cell into the row
                row.Append(cell);

                // create a cell with reference B1, string data type and add the header "Product Name"
                cell = new Cell()
                {
                    CellReference = "B1",
                    DataType = CellValues.String,
                    CellValue = new CellValue("Product Name")
                };
                // and put the cell into the row
                row.Append(cell);

                // create a cell with reference C1, string data type and add the header "Life Cycle Stage"
                cell = new Cell()
                {
                    CellReference = "C1",
                    DataType = CellValues.String,
                    CellValue = new CellValue("Life Cycle Stage")
                };
                // and put the cell into the row
                row.Append(cell);

                // create a cell with reference D1, string data type and add the header "Process Name"
                cell = new Cell()
                {
                    CellReference = "D1",
                    DataType = CellValues.String,
                    CellValue = new CellValue("Process Name")
                };
                // and put the cell into the row
                row.Append(cell);

                // create a cell with reference E1, string data type and add the header "Geographical Location"
                cell = new Cell()
                {
                    CellReference = "E1",
                    DataType = CellValues.String,
                    CellValue = new CellValue("Geographical Location")
                };
                // and put the cell into the row
                row.Append(cell);

                // create a cell with reference F1, string data type and add the header "Resource/Release"
                cell = new Cell()
                {
                    CellReference = "F1",
                    DataType = CellValues.String,
                    CellValue = new CellValue("Resource/Release")
                };
                // and put the cell into the row
                row.Append(cell);

                // create a cell with reference G1, string data type and add the header "CAS Number"
                cell = new Cell()
                {
                    CellReference = "G1",
                    DataType = CellValues.String,
                    CellValue = new CellValue("CAS Number")
                };
                // and put the cell into the row
                row.Append(cell);

                // create a cell with reference H1, string data type and add the header "Media"
                cell = new Cell()
                {
                    CellReference = "H1",
                    DataType = CellValues.String,
                    CellValue = new CellValue("Media")
                };
                // and put the cell into the row
                row.Append(cell);

                // create a cell with reference I1, string data type and add the header "Quantity"
                cell = new Cell()
                {
                    CellReference = "I1",
                    DataType = CellValues.String,
                    CellValue = new CellValue("Quantity")
                };
                // and put the cell into the row
                row.Append(cell);

                // create a cell with reference J1, string data type and add the header "UOM"
                cell = new Cell()
                {
                    CellReference = "J1",
                    DataType = CellValues.String,
                    CellValue = new CellValue("UOM")
                };
                // and put the cell into the row
                row.Append(cell);

                // if classification data is being saved, add a column header for it
                if (classification)
                {
                    // create a cell with reference K1, string data type and add the header "Impact Type"
                    cell = new Cell()
                    {
                        CellReference = "K1",
                        DataType = CellValues.String,
                        CellValue = new CellValue("Impact Type")
                    };
                    // and put the cell into the row
                    row.Append(cell);
                }

                // if characterization data is being saved, add a column headers for it
                if (characterization)
                {
                    // create a cell with reference L1, string data type and add the header "Characterization Result"
                    cell = new Cell()
                    {
                        CellReference = "L1",
                        DataType = CellValues.String,
                        CellValue = new CellValue("Characterization Result")
                    };
                    // and put the cell into the row
                    row.Append(cell);

                    // create a cell with reference M1, string data type and add the header "Factor Measure"
                    cell = new Cell()
                    {
                        CellReference = "M1",
                        DataType = CellValues.String,
                        CellValue = new CellValue("Factor Measure")
                    };
                    // and put the cell into the row
                    row.Append(cell);
                }
                // now put the row in the sheetdata
                sheetData.Append(row);

                // now put the values into the spreadsheet...
                // start with a row number for the cell reference
                int rowNum = 1;

                // do the releases first...
                foreach (TreeNode node in this.m_ReleasesNode.Nodes)
                {
                    // get the release from the Releses TreeNode.
                    Release release = (Release)(node.Tag);

                    // don't add the row
                    bool addRow = false;
                    // unless its lifecycle stage is to be exported
                    if (export[0] && (release.Process.LifeCycleStage == LifeCycleStage.rawMaterialsAcquisition)) addRow = true;
                    if (export[1] && (release.Process.LifeCycleStage == LifeCycleStage.materialManufacture)) addRow = true;
                    if (export[2] && (release.Process.LifeCycleStage == LifeCycleStage.productFabrication)) addRow = true;
                    if (export[3] && (release.Process.LifeCycleStage == LifeCycleStage.fillingPackagingDistribution)) addRow = true;
                    if (export[4] && (release.Process.LifeCycleStage == LifeCycleStage.useReuseMaintenance)) addRow = true;
                    if (export[5] && (release.Process.LifeCycleStage == LifeCycleStage.recycleWasteManagement)) addRow = true;

                    // if the row is to be added...
                    if (addRow)
                    {
                        // are the characterication and classificaiton data being added? 
                        // this allows inventory data to be added once for each impact category
                        bool addCharClass = true;

                        // for each release, add the information for each impact category of the release.
                        foreach (string impact in release.ImpactCategories)
                        {
                            // go through to add the inventory data or additional impact characterization and 
                            // classification information
                            if (addCharClass)
                            {
                                // increment the row number for the cell reference
                                rowNum++;
                                // create anew row
                                row = new Row();

                                // create a cell with reference A(row number), 
                                // string data type and add the project name as its value
                                cell = new Cell()
                                {
                                    CellReference = "A" + rowNum.ToString(),
                                    DataType = CellValues.String,
                                    CellValue = new CellValue(release.Process.Product.Project.Name)
                                };
                                // and put the cell into the row
                                row.Append(cell);

                                // create a cell with reference B(row number), 
                                // string data type and add the product name as its value
                                cell = new Cell()
                                {
                                    CellReference = "B" + rowNum.ToString(),
                                    DataType = CellValues.String,
                                    CellValue = new CellValue(release.Process.Product.Name)
                                };
                                // and put the cell into the row
                                row.Append(cell);

                                // create a cell with reference C(row number), 
                                // string data type and add the life cycle stage as its value
                                cell = new Cell()
                                {
                                    CellReference = "C" + rowNum.ToString(),
                                    DataType = CellValues.String,
                                    CellValue = new CellValue(release.Process.LifeCycleStage.ToString())
                                };
                                // and put the cell into the row
                                row.Append(cell);

                                // create a cell with reference D(row number), 
                                // string data type and add the process name as its value
                                cell = new Cell()
                                {
                                    CellReference = "D" + rowNum.ToString(),
                                    DataType = CellValues.String,
                                    CellValue = new CellValue(release.Process.Name)
                                };
                                // and put the cell into the row
                                row.Append(cell);

                                // create a cell with reference E(row number), 
                                // string data type and add the location as its value
                                cell = new Cell()
                                {
                                    CellReference = "E" + rowNum.ToString(),
                                    DataType = CellValues.String,
                                    CellValue = new CellValue(release.Process.Location.Name)
                                };
                                // and put the cell into the row
                                row.Append(cell);

                                // create a cell with reference F(row number), 
                                // string data type and add the release's name as its value
                                cell = new Cell()
                                {
                                    CellReference = "F" + rowNum.ToString(),
                                    DataType = CellValues.String,
                                    CellValue = new CellValue(release.Name)
                                };
                                // and put the cell into the row
                                row.Append(cell);

                                // create a cell with reference G(row number), 
                                // string data type and add the releases's CAS Numberas its value
                                cell = new Cell()
                                {
                                    CellReference = "G" + rowNum.ToString(),
                                    DataType = CellValues.String,
                                    CellValue = new CellValue(release.CASNumber)
                                };
                                // and put the cell into the row
                                row.Append(cell);

                                // create a cell with reference H(row number), 
                                // string data type and add the media as its value
                                cell = new Cell()
                                {
                                    CellReference = "H" + rowNum.ToString(),
                                    DataType = CellValues.String,
                                    CellValue = new CellValue(release.Media.ToString())
                                };
                                // and put the cell into the row
                                row.Append(cell);

                                // create a cell with reference I(row number), 
                                // string data type and add the quantity as its value
                                cell = new Cell()
                                {
                                    CellReference = "I" + rowNum.ToString(),
                                    DataType = CellValues.Number,
                                    CellValue = new CellValue(release.Quantity.ToString())
                                };
                                // and put the cell into the row
                                row.Append(cell);

                                // create a cell with reference J(row number), 
                                // string data type and add the unit of measure as its value
                                cell = new Cell()
                                    {
                                        CellReference = "J" + rowNum.ToString(),
                                        DataType = CellValues.String,
                                        CellValue = new CellValue(release.UOM.ToString())
                                    };
                                // and put the cell into the row
                                row.Append(cell);

                                // add classification information if needed...
                                if (classification)
                                {
                                    // create a cell with reference K(row number), 
                                    // string data type and add the project name as its value
                                    cell = new Cell()
                                    {
                                        // create a cell with reference A(row number), 
                                        // string data type and add the impact's name as its value
                                        CellReference = "K" + rowNum.ToString(),
                                        DataType = CellValues.String,
                                        CellValue = new CellValue(impact)
                                    };
                                    // and put the cell into the row
                                    row.Append(cell);
                                }

                                // add characterization information, if needed.
                                if (characterization)
                                {
                                    // create a cell with reference L(row number), 
                                    // string data type and add the impact's value as its value
                                    cell = new Cell()
                                    {
                                        CellReference = "L" + rowNum.ToString(),
                                        DataType = CellValues.Number,
                                        CellValue = new CellValue(release.GetImpactValue(impact).ToString())
                                    };
                                    // and put the cell into the row
                                    row.Append(cell);

                                    // create a cell with reference M(row number), 
                                    // string data type and add the impact's type as its value
                                    cell = new Cell()
                                    {
                                        CellReference = "M" + rowNum.ToString(),
                                        DataType = CellValues.String,
                                        CellValue = new CellValue(release.GetImpactFactor(impact))
                                    };
                                    // and put the cell into the row
                                    row.Append(cell);
                                }
                                // add the row to the sheet's data
                                sheetData.Append(row);

                                // now, if we are doing just inventory, that information is
                                // in the file for this release, so don't add it again.
                                if (inventory) addCharClass = false;
                            }
                        }
                    }
                }
                foreach (TreeNode node in this.m_ResourcesNode.Nodes)
                {
                    // get the resource from the tag of the current resource node in the Treeview.
                    Resource resource = (Resource)(node.Tag);
                    // don't add the row
                    bool addRow = false;
                    // unless its lifecycle stage is to be exported
                    if (export[0] && (resource.Process.LifeCycleStage == LifeCycleStage.rawMaterialsAcquisition)) addRow = true;
                    if (export[1] && (resource.Process.LifeCycleStage == LifeCycleStage.materialManufacture)) addRow = true;
                    if (export[2] && (resource.Process.LifeCycleStage == LifeCycleStage.productFabrication)) addRow = true;
                    if (export[3] && (resource.Process.LifeCycleStage == LifeCycleStage.fillingPackagingDistribution)) addRow = true;
                    if (export[4] && (resource.Process.LifeCycleStage == LifeCycleStage.useReuseMaintenance)) addRow = true;
                    if (export[5] && (resource.Process.LifeCycleStage == LifeCycleStage.recycleWasteManagement)) addRow = true;

                    // if the row is to be added...
                    if (addRow)
                    {
                        // increment the row number for the cell reference
                        rowNum++;
                        // create anew row
                        row = new Row();

                        // create a cell with reference A(row number), 
                        // string data type and add the project name as its value
                        cell = new Cell()
                        {
                            CellReference = "A" + rowNum.ToString(),
                            DataType = CellValues.String,
                            CellValue = new CellValue(resource.Process.Product.Project.Name)
                        };
                        // and put the cell into the row
                        row.Append(cell);

                        // create a cell with reference B(row number), 
                        // string data type and add the product name as its value
                        cell = new Cell()
                        {
                            CellReference = "B" + rowNum.ToString(),
                            DataType = CellValues.String,
                            CellValue = new CellValue(resource.Process.Product.Name)
                        };
                        // and put the cell into the row
                        row.Append(cell);

                        // create a cell with reference C(row number), 
                        // string data type and add the life cycle stage as its value
                        cell = new Cell()
                        {
                            CellReference = "C" + rowNum.ToString(),
                            DataType = CellValues.String,
                            CellValue = new CellValue(resource.Process.LifeCycleStage.ToString())
                        };
                        // and put the cell into the row
                        row.Append(cell);

                        // create a cell with reference D(row number), 
                        // string data type and add the process name as its value
                        cell = new Cell()
                        {
                            CellReference = "D" + rowNum.ToString(),
                            DataType = CellValues.String,
                            CellValue = new CellValue(resource.Process.Name)
                        };
                        // and put the cell into the row
                        row.Append(cell);

                        // create a cell with reference E(row number), 
                        // string data type and add the location as its value
                        cell = new Cell()
                        {
                            CellReference = "E" + rowNum.ToString(),
                            DataType = CellValues.String,
                            CellValue = new CellValue(resource.Process.Location.Name)
                        };
                        // and put the cell into the row
                        row.Append(cell);

                        // create a cell with reference F(row number), 
                        // string data type and add the release's name as its value
                        cell = new Cell()
                        {
                            CellReference = "F" + rowNum.ToString(),
                            DataType = CellValues.String,
                            CellValue = new CellValue(resource.Name)
                        };
                        // and put the cell into the row
                        row.Append(cell);

                        //// create a cell with reference G(row number), 
                        //// string data type and add the releases's CAS Numberas its value
                        //cell = new Cell()
                        //{
                        //    CellReference = "G" + rowNum.ToString(),
                        //    DataType = CellValues.String,
                        //    CellValue = new CellValue(resource.CASNumber)
                        //};
                        //// and put the cell into the row
                        //row.Append(cell);

                        // create a cell with reference H(row number), 
                        // string data type and add the media as its value
                        cell = new Cell()
                        {
                            CellReference = "H" + rowNum.ToString(),
                            DataType = CellValues.String,
                            CellValue = new CellValue(String.Empty)
                        };
                        // and put the cell into the row
                        row.Append(cell);

                        // create a cell with reference I(row number), 
                        // string data type and add the quantity as its value
                        cell = new Cell()
                        {
                            CellReference = "I" + rowNum.ToString(),
                            DataType = CellValues.Number,
                            CellValue = new CellValue(resource.Quantity.ToString())
                        };
                        // and put the cell into the row
                        row.Append(cell);

                        // create a cell with reference J(row number), 
                        // string data type and add the unit of measure as its value
                        cell = new Cell()
                            {
                                CellReference = "J" + rowNum.ToString(),
                                DataType = CellValues.String,
                                CellValue = new CellValue(resource.GetUOM())
                            };
                        // and put the cell into the row
                        row.Append(cell);

                        // add classification information if needed...
                        if (classification)
                        {
                            // create a cell with reference K(row number), 
                            // string data type and add the project name as its value
                            cell = new Cell()
                            {
                                // create a cell with reference A(row number), 
                                // string data type and add the impact's name as its value
                                CellReference = "K" + rowNum.ToString(),
                                DataType = CellValues.String,
                                CellValue = new CellValue(resource.ImpactType)
                            };
                            // and put the cell into the row
                            row.Append(cell);
                        }

                        // add characterization information, if needed.
                        if (characterization)
                        {
                            // create a cell with reference L(row number), 
                            // string data type and add the impact's value as its value
                            cell = new Cell()
                            {
                                CellReference = "L" + rowNum.ToString(),
                                DataType = CellValues.String,
                                CellValue = new CellValue(resource.Characterization.ToString())
                            };
                            // and put the cell into the row
                            row.Append(cell);

                            // create a cell with reference M(row number), 
                            // string data type and add the impact's type as its value
                            cell = new Cell()
                            {
                                CellReference = "M" + rowNum.ToString(),
                                DataType = CellValues.String,
                                CellValue = new CellValue(resource.FactorMeasure)
                            };
                            // and put the cell into the row
                            row.Append(cell);
                        }
                        // add the row to the sheet's data
                        sheetData.Append(row);
                    }
                }

                // Close the document.
                spreadsheetDocument.Close();

                //}
                // catch (Exception ex)
                // {
                //     MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                // }
            }
        }

        /// <summary>
        /// This method displays the dialog that charts the impacts.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void viewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Create and show the ViewCharts form.
            ViewCharts charts = new ViewCharts(m_traciProject);
            charts.ShowDialog();
        }

        /// <summary>
        /// The method reads data from an Excel 2007 spreadsheet into a data table for use in
        /// importing the spreadsheet. Currently not used.
        /// a <see cref="System.Data.DataTable"/>.
        /// </summary>
        /// <param name="filename">The name of the EXCEL 2007 file to be opened.</param>
        void ReadExcel2007toDataTable(string filename)
        {
            m_loading = true;
            // we are loading products and processes, so don't update the TreeView yet.
            m_loading = true;

            // Create a new DataTable, the database command, and the data adapter.
            DataTable dt = new DataTable();

            // Open the connection to the EXCEL spreadsheet unsing the connection string 
            // in the OLE data connection...
            String excelConnStr = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + filename + "; Extended Properties=Excel 8.0;";
            System.Data.OleDb.OleDbConnection excelConn = new System.Data.OleDb.OleDbConnection(excelConnStr);
            excelConn.Open();

            // Create a OLE database command to select all data from the TRACI export table...
            System.Data.OleDb.OleDbCommand excelCommand = new System.Data.OleDb.OleDbCommand("SELECT * FROM [Inventory]", excelConn);

            // Create a OLE data adapter for the command...
            System.Data.OleDb.OleDbDataAdapter excelDataAdapter = new System.Data.OleDb.OleDbDataAdapter(excelCommand);

            // fill the data table...
            excelDataAdapter.Fill(dt);
            dt.AcceptChanges();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutDialog about = new AboutDialog();
            about.ShowDialog();
        }
    };
};


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Traci3
{


    [System.Runtime.InteropServices.ComVisible(false)]
    class ProjectTypeConverter : System.ComponentModel.TypeConverter
    {
        public override bool CanConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Type destinationType)
        {
            if ((typeof(TraciProject)).IsAssignableFrom(destinationType))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        public override Object ConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, Object value, System.Type destinationType)
        {
            if ((typeof(System.String)).IsAssignableFrom(destinationType) && (typeof(TraciProject).IsAssignableFrom(value.GetType())))
            {
                return ((TraciProject)value).Name;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    };

    /// <summary>
    /// The main TRACI project class.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This class is the class that defines a TRACI project. It provides the administrative
    /// details for the project. In addition, it holds the collection of products in the 
    /// life cycle being investigated.
    /// </para>
    /// <para>
    /// The class consists of products used in production of a functional unit of the product
    /// being compared. Processes used to produce each of the products are contained in
    /// <see cref=">Product"/> class. Each <see cref="Process"/> has releases and resources 
    /// associated with it. In addition, the processes, releases and resources can be directly 
    /// access for the product.
    /// </para>
    /// </remarks>
    [Serializable]
    [System.ComponentModel.TypeConverterAttribute(typeof(ProjectTypeConverter))]
    //[System.Runtime.Serialization.DataContract]
    public class TraciProject : ITraciProject, ITraciIdentification
    {

        /// <summary>
        /// Constructor for the <see cref="TraaciProject"/> class.
        /// </summary>
        public TraciProject()
        {
            m_Name = String.Empty;
            //m_Products = new Products(this);
            m_Products.ListChanged += new System.ComponentModel.ListChangedEventHandler(this.products_ListChanged);
        }


        [System.Runtime.Serialization.OnDeserializing]
        private void ResetProjectOnDeserializing(System.Runtime.Serialization.StreamingContext context)
        {
            m_Releases = new Releases();
            m_Resources = new Resources();
        }

        [System.Runtime.Serialization.OnDeserialized]
        private void ResetObjectGraphOnDeserialized(System.Runtime.Serialization.StreamingContext context)
        {
            foreach (Product product in m_Products)
            {
                product.Project = this;
                foreach (Process process in product.Processes)
                {
                    process.Product = product;
                    foreach (Release release in process.Releases)
                    {
                        release.Process = process;
                        m_Releases.Add(release);
                        product.Releases.Add(release);
                    }
                    foreach (Resource resource in process.Resources)
                    {
                        resource.Process = process;
                        m_Resources.Add(resource);
                        product.Resources.Add(resource);
                    }
                }
           }
        }

        /// <summary>
        /// Event handler that responds to a <see cref="Product"/> being added to the <see cref="Products"/> collection.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method assigns the current project to a <see cref="Product"/> when it is added 
        /// to the <see cref="Products"/> collection.
        /// </para>
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void products_ListChanged(object sender, System.ComponentModel.ListChangedEventArgs e)
        {
            // if a product is being added to the product collection...
            if (e.ListChangedType == System.ComponentModel.ListChangedType.ItemAdded)
            {
                // assign this instance to its Project property.
                m_Products[e.NewIndex].Project = this;
            }
        }

        /// <summary>
        /// The name of the project.
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("The name of the current project.")]
        [System.ComponentModel.CategoryAttribute("Project Information")]
        public string Name
        {
            get
            {
                // get accessor returns the name.
                return m_Name;
            }
            set
            {
                /// set accessor sets the name.
                m_Name = value;
            }
        }
        string m_Name;

        /// <summary>
        /// A description of the project.
        /// </summary>
        /// <remarks>
        /// The description should include any assumptions as well as the goal of the study.
        /// </remarks>
        [System.ComponentModel.DescriptionAttribute("The description of the current project.")]
        [System.ComponentModel.CategoryAttribute("Project Information")]
        public string Description
        {
            get
            {
                // get accessor returns the description.
                return m_Description;
            }
            set
            {
                // set accessor to set the description.
                m_Description = value;
            }
        }
        string m_Description;

        /// <summary>
        /// The organization being investigated.
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("The organzation being evaluted by the current project.")]
        [System.ComponentModel.CategoryAttribute("Project Information")]
        public string Organization
        {
            get
            {
                // get accessor for the organization.
                return m_Organization;
            }
            set
            {
                // set the value of the organization.
                m_Organization = value;
            }
        }
        string m_Organization;
        
        /// <summary>
        /// The organizational unit conducting the study.
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("The organzational unit being evaluted by the current project.")]
        [System.ComponentModel.CategoryAttribute("Project Information")]
        public string OrganizationalUnit
        {
            get
            {
                return m_OrganizationalUnit;
            }
            set
            {
                m_OrganizationalUnit = value;
            }
        }
        string m_OrganizationalUnit;

        /// <summary>
        /// The name of the person conducting the study.
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("The primary contact for the current project.")]
        [System.ComponentModel.CategoryAttribute("Project Information")]
        public string Contact
        {
            get
            {
                return m_Contact;
            }
            set
            {
                m_Contact = value;
            }
        }
        string m_Contact;
 
        /// <summary>
        /// Telephone number of the person conducting the study.
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("The phone number of primary contact for the current project.")]
        [System.ComponentModel.CategoryAttribute("Project Information")]
        public string ContactPhone
        {
            get
            {
                return m_ContactPhone;
            }
            set
            {
                m_ContactPhone = value;
            }
        }
        string m_ContactPhone;

        /// <summary>
        /// E-Mail address of the person conducting the study.
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("The e-mail address of primary contact for the current project.")]
        [System.ComponentModel.CategoryAttribute("Project Information")]
        public string ContactEmail
        {
            get
            {
                return m_eMailAddress;
            }
            set
            {
                m_eMailAddress = value;
            }
        }
        string m_eMailAddress;

        /// <summary>
        /// A version identifier for the study.
        /// </summary>
        [System.ComponentModel.DescriptionAttribute("The version of the current project.")]
        [System.ComponentModel.CategoryAttribute("Project Information")]
        public string Version
        {
            get
            {
                return m_Version;
            }
            set
            {
                m_Version = value;
            }
        }
        string m_Version;

        /// <summary>
        /// The scope of the project.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The project scope considers the lide cycle stages being considered in the project. 
        /// The scope is defined in terms of the <see cref="ProjectScope"/> enumeration. The 
        /// project scope has the following values:
        /// <list type="bullet">
        /// <item>
        /// <description>Cradle to grave includes all product stages from raw materials 
        /// acquisition to waste disposal.
        /// </item>
        /// <item>
        /// <description>Cradle to entry gate assesses just the upstream suppliers and 
        /// transportation before the product reaches your company.</description>
        /// </item>
        /// <item>
        /// <description>Entry gate to exit gate assesses the product only during its time at 
        /// your facility.</description>
        /// </item>
        /// <item>
        /// <description>Entry gate to grave assesses the product from the time it is at your 
        /// facility to waste disposal. </description>
        /// </item>
        /// <item>
        /// <description>Exit gate to grave analyzes the product from the time it leaves your 
        /// facility to waste disposal.</description>
        /// </item>
        /// </list>
        /// </para>
        /// </remarks>
        [System.ComponentModel.DescriptionAttribute("The life cycle scope of the current project.")]
        [System.ComponentModel.CategoryAttribute("Project Information")]
        public ProjectScope ProjectScope
        {
            get
            {
                return m_ProjectScope;
            }
            set
            {
                m_ProjectScope = value;
            }
        }
        ProjectScope m_ProjectScope;

        /// <summary>
        /// The functional unit of the project. 
        /// </summary>
        /// <remarks>
        /// <para>
        /// The functional unit is defined as the basis for defining the reference flow for 
        /// all project data. For example, a functional unit of operating one car for a 
        /// distance of 12,000 miles would define a reference flow of all upstream materials 
        /// required to build one car and the fuel to operate it a distance of 12,000 miles.
        /// </para>
        /// </remarks>
        string m_FunctionalUnit;
        [System.ComponentModel.DescriptionAttribute("The functional unit considered in the current project.")]
        [System.ComponentModel.CategoryAttribute("Project Information")]
        public string FunctionalUnit
        {
            get
            {
                return m_FunctionalUnit;
            }
            set
            {
                m_FunctionalUnit = value;
            }
        }

        /// <summary>
        /// Gets the collection of <see cref="Product"/> items in the <see cref="TraciProject"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This property enables you to obtain a reference to the colletion of <see cref="Product"/> 
        /// items in the current <see cref="TraciProject"/>. With this reference, you can add 
        /// items, remove items, and obtain a count of the items in the collection.
        /// </para>
        /// </remarks>
        [System.ComponentModel.DescriptionAttribute("The products being considered in the current project.")]
        [System.ComponentModel.CategoryAttribute("Project Information")]
        public Products Products
        {
            get
            {
                foreach (Product product in m_Products)
                    product.Project = this; 
                return m_Products;
            }
        }
        Products m_Products = new Products();

        /// <summary>
        /// Gets the collection of <see cref="Process"/> items in the <see cref="TraciProject"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This property enables you to obtain a reference to the colletion of <see cref="Process"/> 
        /// items in the current <see cref="TraciProject"/>. With this reference, you can add 
        /// items, remove items, and obtain a count of the items in the collection.
        /// </para>
        /// </remarks>
        [System.ComponentModel.DescriptionAttribute("The processes used in the current project.")]
        [System.ComponentModel.CategoryAttribute("Project Information")]
        [System.ComponentModel.Browsable(false)]
        public Processes Processes
        {
            get
            {
                return m_Processes;
            }
        }
        [NonSerialized]
        Processes m_Processes = new Processes();


        /// <summary>
        /// Gets the collection of <see cref="Release"/> items in the <see cref="TraciProject"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This property enables you to obtain a reference to the colletion of <see cref="Release"/> 
        /// items in the current <see cref="TraciProject"/>. With this reference, you can add 
        /// items, remove items, and obtain a count of the items in the collection.
        /// </para>
        /// </remarks>
        [System.ComponentModel.DescriptionAttribute("The releases in the current project.")]
        [System.ComponentModel.CategoryAttribute("Project Information")]
        [System.ComponentModel.Browsable(false)]
        public Releases Releases
        {
            get
            {
                return m_Releases;
            }
        }
        [NonSerialized]
        Releases m_Releases = new Releases();

        /// <summary>
        /// Gets the collection of <see cref="Resource"/> items in the <see cref="TraciProject"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This property enables you to obtain a reference to the colletion of <see cref="Resource"/> 
        /// items in the current <see cref="TraciProject"/>. With this reference, you can add 
        /// items, remove items, and obtain a count of the items in the collection.
        /// </para>
        /// </remarks>
        [System.ComponentModel.Browsable(false)]
        [System.ComponentModel.DescriptionAttribute("The resources in the current project.")]
        [System.ComponentModel.CategoryAttribute("Project Information")]
        public Resources Resources
        {
            get
            {
                return m_Resources; ;
            }
        }
        [NonSerialized]
        Resources m_Resources = new Resources();

        /// <summary>
        /// Returns a value indicating whether the specified <see cref="Product"/> is in the
        /// current <see cref="TraciProject"/>.
        /// </summary>
        /// <param name="productName">The name of the <see cref="Product"/>.</param>
        /// <returns>true if the specified <see cref="Product"/> is in the 
        /// <see cref="TraciProject"/>; otherwise, false.</returns>
        public bool ContainsProduct(String productName)
        {
            foreach (Product product in m_Products)
            {
                if (product.Name == productName) return true;
            }
            return false;
        }

        /// <summary>
        /// Returns the <see cref="Product"/> having the requested name.
        /// </summary>
        /// <param name="productName">The name of the <see cref="Product"/> desired.</param>
        /// <returns>A reference to the desired <see cref="Product"/>.</returns>
        public Product GetProduct(String productName)
        {
            foreach (Product product in m_Products)
            {
                if (product.Name == productName) return product;
            }
            return null;
        }

        /// <summary>
        /// Adds a new <see cref="Product"/> to the current <see cref="TraciProject"/>
        /// witht eh specified name.
        /// </summary>
        /// <param name="productName">The name of the <see cref="Product"/> to be added.</param>
        public void AddProduct(string productName)
        {
            m_Products.Add(new Product(productName, this));
        }

        /// <summary>
        /// Returns an array of the names of the <see cref="Release"/>and <see cref="Resource"/>
        /// items for the current <see cref="TraciProject"/>.
        /// </summary>
        /// <param name="impactCategory"></param>
        /// <returns></returns>
        public string[] ReleasesAndResourcesInImpactCategory(string impactCategory)
        {
            System.Collections.Generic.List<String> list = new System.Collections.Generic.List<String>();
            foreach (Release release in m_Releases)
            {
                if (release.GetImpactValue(impactCategory) != 0)
                {
                    bool add = true;
                    foreach (string obj in list)
                        if (obj.ToString() == release.Name) add = false;
                    if (add) list.Add(release.Name);
                }
            }
            if (impactCategory == "FOSSIL FUEL") list.Add("FOSSIL FUEL");
            if (impactCategory == "LAND USE") list.Add("LAND USE");
            if (impactCategory == "WATER USE") list.Add("WATER USE");
            return list.ToArray();
        }


        #region ITraciProject Members

        string ITraciProject.GetName()
        {
            return m_Name;
        }

        string ITraciProject.GetDescription()
        {
            return m_Description;
        }

        string ITraciProject.GetFunctionalUnit()
        {
            return m_FunctionalUnit;
        }

        string ITraciProject.GetOrganization()
        {
            return m_Organization;
        }

        string ITraciProject.GetOrganizationalUnit()
        {
            return m_OrganizationalUnit;
        }

        string ITraciProject.GetContact()
        {
            return m_Contact;
        }

        string ITraciProject.GetContactPhone()
        {
            return m_ContactPhone;
        }

        string ITraciProject.GetVersion()
        {
            return m_Version;
        }

        ProjectScope ITraciProject.GetProjectScope()
        {
            return m_ProjectScope;
        }

        string[] ITraciProject.ProductList()
        {
            string[] retval = new string[Products.Count];
            for (int n = 0; n < Products.Count; n++)
            {
                retval[n] = Products[n].Name;
            }
            return retval;
        }

        ITraciProduct ITraciProject.GetProduct(string productName)
        {
            for (int n = 0; n < Products.Count; n++)
            {
                if (productName == Products[n].Name) return Products[n];
            }
            throw new ArgumentException(productName + "was not found in the list of Products");
        }

        #endregion

        #region ITraciIdentification Members

        string ITraciIdentification.GetName()
        {
            return m_Name;
        }

        string ITraciIdentification.GetFullName()
        {
            return m_Name;
        }

        string ITraciIdentification.GetDescription()
        {
            return m_Description;
        }

        #endregion
    }
}

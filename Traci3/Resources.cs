using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Traci3
{
    /// <summary>
    /// This class provides a conversion from the <see cref="Resources"/> collection
    /// class to a <see cref="String"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// When a class containing the <see cref="Resources"/> collection class is inserted 
    /// into a <see cref="PropertyGrid"/> control, this class acts as a converter and 
    /// returns a <see cref="String"/> having the value of "Resources," which appears in
    /// as the value of the Resources property in the the PropertyGrid.
    /// </para>
    /// </remarks>
    [System.Runtime.InteropServices.ComVisible(false)]
    class ResourceCollectionTypeConverter : System.ComponentModel.TypeConverter
    {
        /// <summary>
        /// Returns whether this converter can convert the object to the specified type, using 
        /// the specified context. 
        /// </summary>
        /// <param name="context">An <see cref = "ITypeDescriptorContext"/> that provides a 
        /// format context.</param>
        /// <param name="destinationType">A <see cref="Type"/> that represents the type you want to convert to.</param>
        /// <returns>true if this converter can perform the conversion; otherwise, false.</returns>
        public override bool CanConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Type destinationType)
        {
            // if the required value has the type of a string, 
            if ((typeof(Resources)).IsAssignableFrom(destinationType))
                // this is what we convert, so its true...
                return true;
            // otherwise, does the base class handle the conversion?
            return base.CanConvertTo(context, destinationType);
        }

        /// <summary>
        /// Converts the given value object to the specified type, using the specified 
        /// context and culture information.
        /// </summary>
        /// <remarks>
        /// <para>
        /// In this case, the <see cref="Resource"/> needs to be converted to a <see cref="String"/>
        /// to be placed into the PropertyGrid. This method returns a string having the value
        /// of "Resources".
        /// </para>
        /// </remarks>
        /// <param name="context">An <see cref = "ITypeDescriptorContext"/> that provides a 
        /// format context.</param>
        /// <param name="culture">A CultureInfo. If null is passed, the current culture is 
        /// assumed.</param>
        /// <param name="value">The Object to convert.</param>
        /// <param name="destinationType">The Type to convert the value parameter to. </param>
        /// <returns>An Object that represents the converted value.</returns>
        public override Object ConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, Object value, System.Type destinationType)
        {
            // if the required value has the type of a string, and the value is a Resources class
            if ((typeof(System.String)).IsAssignableFrom(destinationType) && (typeof(Resources).IsAssignableFrom(value.GetType())))
            {
                // return the string value...
                return "Resources";
            }
            // otherwise, call the base class version of this method.
            return base.ConvertTo(context, culture, value, destinationType);
        }
    };

    /// <summary>
    /// Provides a user interface that can edit most types of collections at design time.
    /// </summary>
    class ResourceCollectionEditor : System.ComponentModel.Design.CollectionEditor
    {

        public ResourceCollectionEditor(Type t)
            : base(t)
        {
        }

        /// <summary>
        /// Gets the data types that this collection editor can contain.
        /// </summary>
        /// <remarks>
        /// This method returns an array of Resource types that can be added to the 
        /// Resourses collection.
        /// </remarks>
        /// <returns>An array of data types that this collection can contain.</returns>
        protected override Type[] CreateNewItemTypes()
        {
            Type[] retVal = new Type[4];
            retVal[0] = typeof(NaturalGas);
            retVal[1] = typeof(OilResource);
            retVal[2] = typeof(CoalResource);
            retVal[3] = typeof(LandResourceUnit);
            return retVal;
        }
    };

    /// <summary>
    /// A collection of <see cref="Resource"/> items.
    /// </summary>
    /// <remarks>
    /// <para>
    /// 
    /// </para>
    /// </remarks>
    [Serializable]
    [System.ComponentModel.TypeConverterAttribute(typeof(ResourceCollectionTypeConverter))]
    public class Resources : System.ComponentModel.BindingList<Resource>,
        System.ComponentModel.ICustomTypeDescriptor
    {
        #region ICustomTypeDescriptor Members

        // Implementation of ICustomTypeDescriptor: 

        String System.ComponentModel.ICustomTypeDescriptor.GetClassName()
        {
            return System.ComponentModel.TypeDescriptor.GetClassName(this, true);
        }

        System.ComponentModel.AttributeCollection System.ComponentModel.ICustomTypeDescriptor.GetAttributes()
        {
            return System.ComponentModel.TypeDescriptor.GetAttributes(this, true);
        }

        String System.ComponentModel.ICustomTypeDescriptor.GetComponentName()
        {
            return System.ComponentModel.TypeDescriptor.GetComponentName(this, true);
        }

        System.ComponentModel.TypeConverter System.ComponentModel.ICustomTypeDescriptor.GetConverter()
        {
            return System.ComponentModel.TypeDescriptor.GetConverter(this, true);
        }

        System.ComponentModel.EventDescriptor System.ComponentModel.ICustomTypeDescriptor.GetDefaultEvent()
        {
            return System.ComponentModel.TypeDescriptor.GetDefaultEvent(this, true);
        }

        System.ComponentModel.PropertyDescriptor System.ComponentModel.ICustomTypeDescriptor.GetDefaultProperty()
        {
            return System.ComponentModel.TypeDescriptor.GetDefaultProperty(this, true);
        }

        Object System.ComponentModel.ICustomTypeDescriptor.GetEditor(Type editorBaseType)
        {
            return System.ComponentModel.TypeDescriptor.GetEditor(this, editorBaseType, true);
        }

        System.ComponentModel.EventDescriptorCollection System.ComponentModel.ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
        {
            return System.ComponentModel.TypeDescriptor.GetEvents(this, attributes, true);
        }

        System.ComponentModel.EventDescriptorCollection System.ComponentModel.ICustomTypeDescriptor.GetEvents()
        {
            return System.ComponentModel.TypeDescriptor.GetEvents(this, true);
        }

        Object System.ComponentModel.ICustomTypeDescriptor.GetPropertyOwner(System.ComponentModel.PropertyDescriptor pd)
        {
            return this;
        }

        System.ComponentModel.PropertyDescriptorCollection System.ComponentModel.ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
        {
            return System.ComponentModel.TypeDescriptor.GetProperties(this, true);
        }

        System.ComponentModel.PropertyDescriptorCollection System.ComponentModel.ICustomTypeDescriptor.GetProperties()
        {
            // Create a new collection object PropertyDescriptorCollection
            System.ComponentModel.PropertyDescriptorCollection pds = new System.ComponentModel.PropertyDescriptorCollection(null);
            // Iterate the list of Resources
            for (int i = 0; i < this.Items.Count; i++)
            {
                // For each Resource create a property descriptor 
                // and add it to the PropertyDescriptorCollection instance
                ResourceCollectionPropertyDescriptor pd = new ResourceCollectionPropertyDescriptor(this, i);
                pds.Add(pd);
            }
            return pds;
        }

        #endregion
    }

    /// <summary>
    /// Summary description for CollectionPropertyDescriptor.
    /// </summary>
    [System.Runtime.InteropServices.ComVisibleAttribute(false)]
    class ResourceCollectionPropertyDescriptor : System.ComponentModel.PropertyDescriptor
    {
        private Resources collection;
        private int index;

        public ResourceCollectionPropertyDescriptor(Resources coll, int idx) :
            base("#" + idx.ToString(), null)
        {
            this.collection = coll;
            this.index = idx;
        }

        public override System.ComponentModel.AttributeCollection Attributes
        {
            get
            {
                return new System.ComponentModel.AttributeCollection(null);
            }
        }

        public override bool CanResetValue(Object component)
        {
            return true;
        }

        public override Type ComponentType
        {
            get
            {
                return this.collection.GetType();
            }
        }

        public override String DisplayName
        {
            get
            {
                return this.collection[index].Name;
            }
        }

        public override String Description
        {
            get
            {
                return this.collection[index].Description;
            }
        }

        public override Object GetValue(Object component)
        {
            return this.collection[index];
        }

        public override bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public override String Name
        {
            get
            {
                return String.Concat("#", index.ToString());
            }
        }

        public override Type PropertyType
        {
            get
            {
                return this.collection[index].GetType();
            }
        }

        public override void ResetValue(Object component)
        {

        }

        public override bool ShouldSerializeValue(Object component)
        {
            return true;
        }

        public override void SetValue(Object component, Object value)
        {
            //this.collection[index] = value;
        }
    };
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Traci3
{
    [System.Runtime.InteropServices.ComVisible(false)]
    class ProcessCollectionTypeConverter : System.ComponentModel.TypeConverter
    {
        public override bool CanConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Type destinationType)
        {
            if ((typeof(Processes)).IsAssignableFrom(destinationType))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        public override Object ConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, Object value, System.Type destinationType)
        {
            if ((typeof(System.String)).IsAssignableFrom(destinationType) && (typeof(Processes).IsAssignableFrom(value.GetType())))
            {
                return "Processes";
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    };

    [Serializable]
    [System.ComponentModel.TypeConverterAttribute(typeof(ProcessCollectionTypeConverter))]
    public class Processes : System.ComponentModel.BindingList<Process>,
        System.ComponentModel.ICustomTypeDescriptor
    {
        public String[] Names
        {
            get
            {
                String[] retVal = new String[this.Count];
                for (int i = 0; i < this.Count; i++)
                {
                    retVal[i] = this[i].Name; 
                }
                return retVal;
            }
        }

        public Process GetProcess(String processName)
        {
            foreach (Process process in this)
            {
                if (processName == process.Name) return process;
            }
            return null;
        }

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
            // Iterate the list of Ports
            for (int i = 0; i < this.Items.Count; i++)
            {
                // For each Port create a property descriptor 
                // and add it to the PropertyDescriptorCollection instance
                ProcessCollectionPropertyDescriptor pd = new ProcessCollectionPropertyDescriptor(this, i);
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
    class ProcessCollectionPropertyDescriptor : System.ComponentModel.PropertyDescriptor
    {
        private Processes collection;
        private int index;

        public ProcessCollectionPropertyDescriptor(Processes coll, int idx) :
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
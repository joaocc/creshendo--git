using System;
using System.Text;

namespace Creshendo.Util.Rete
{
    public class TemporalDeffact : Deffact, ITemporalFact
    {
        protected internal long expirationTime = 0;
        protected internal String serviceType = null;
        protected internal String sourceURL = null;
        protected internal int validity;

        public TemporalDeffact(Deftemplate template, Object instance, Slot[] values, long id) : base(template, instance, values, id)
        {
        }

        #region TemporalFact Members

        public virtual long ExpirationTime
        {
            get { return expirationTime; }

            set { expirationTime = value; }
        }

        public virtual String ServiceType
        {
            get { return serviceType; }

            set { serviceType = value; }
        }

        public virtual String Source
        {
            get { return sourceURL; }

            set { sourceURL = value; }
        }

        public virtual int Validity
        {
            get { return validity; }

            set { validity = value; }
        }


        public override String toFactString()
        {
            StringBuilder buf = new StringBuilder();
            buf.Append("f-" + id + " (" + deftemplate.Name);
            if (slots.Length > 0)
            {
                buf.Append(" ");
            }
            for (int idx = 0; idx < slots.Length; idx++)
            {
                buf.Append("(" + slots[idx].Name + " " + ConversionUtils.formatSlot(slots[idx].Value) + ") ");
            }
            // append the temporal attributes
            buf.Append("(" + TemporalFact_Fields.EXPIRATION + " " + expirationTime + ")");
            buf.Append("(" + TemporalFact_Fields.SERVICE_TYPE + " " + serviceType + ")");
            buf.Append("(" + TemporalFact_Fields.SOURCE + " " + sourceURL + ")");
            buf.Append("(" + TemporalFact_Fields.VALIDITY + " " + validity + ")");
            buf.Append(")");
            return buf.ToString();
        }

        #endregion

        /// <summary> the class overrides the method to include the additional
        /// attributes.
        /// </summary>
        public override String toPPString()
        {
            StringBuilder buf = new StringBuilder();
            buf.Append("(" + deftemplate.Name);
            if (slots.Length > 0)
            {
                buf.Append(" ");
            }
            for (int idx = 0; idx < slots.Length; idx++)
            {
                if (slots[idx].Value is BoundParam)
                {
                    BoundParam bp = (BoundParam) slots[idx].Value;
                    buf.Append("(" + slots[idx].Name + " ?" + bp.VariableName + ") ");
                }
                else
                {
                    buf.Append("(" + slots[idx].Name + " " + ConversionUtils.formatSlot(slots[idx].Value) + ") ");
                }
            }
            // append the temporal attributes
            buf.Append("(" + TemporalFact_Fields.EXPIRATION + " " + expirationTime + ")");
            buf.Append("(" + TemporalFact_Fields.SERVICE_TYPE + " " + serviceType + ")");
            buf.Append("(" + TemporalFact_Fields.SOURCE + " " + sourceURL + ")");
            buf.Append("(" + TemporalFact_Fields.VALIDITY + " " + validity + ") ");
            buf.Append(")");
            return buf.ToString();
        }
    }
}
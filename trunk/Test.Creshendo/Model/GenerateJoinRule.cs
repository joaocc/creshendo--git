using System;
using System.Text;

namespace Test.Creshendo.Model
{
    public class GenerateJoinRule
    {
        public static String LINEBREAK = Environment.NewLine;


        public void writeTemplates(StringBuilder buf)
        {
            buf.Append("(deftemplate transaction" + LINEBREAK);
            buf.Append("  (slot accountId (type STRING))" + LINEBREAK);
            buf.Append("  (slot buyPrice (type DOUBLE))" + LINEBREAK);
            buf.Append("  (slot countryCode (type STRING))" + LINEBREAK);
            buf.Append("  (slot currentPrice (type DOUBLE))" + LINEBREAK);
            buf.Append("  (slot cusip (type INTEGER))" + LINEBREAK);
            buf.Append("  (slot exchange (type STRING))" + LINEBREAK);
            buf.Append("  (slot industryGroupID (type INTEGER))" + LINEBREAK);
            buf.Append("  (slot industryID (type INTEGER))" + LINEBREAK);
            buf.Append("  (slot issuer (type STRING))" + LINEBREAK);
            buf.Append("  (slot lastPrice (type DOUBLE))" + LINEBREAK);
            buf.Append("  (slot purchaseDate (type STRING))" + LINEBREAK);
            buf.Append("  (slot sectorID (type INTEGER))" + LINEBREAK);
            buf.Append("  (slot shares (type DOUBLE))" + LINEBREAK);
            buf.Append("  (slot subIndustryID (type INTEGER))" + LINEBREAK);
            buf.Append("  (slot total (type DOUBLE))" + LINEBREAK);
            buf.Append(")" + LINEBREAK);
            buf.Append("(deftemplate account" + LINEBREAK);
            buf.Append("  (slot accountId (type STRING))" + LINEBREAK);
            buf.Append("  (slot cash (type DOUBLE))" + LINEBREAK);
            buf.Append("  (slot fixedIncome (type DOUBLE))" + LINEBREAK);
            buf.Append("  (slot stocks (type DOUBLE))" + LINEBREAK);
            buf.Append("  (slot countryCode (type STRING))" + LINEBREAK);
            buf.Append(")");
        }

        public void writeTransactions(StringBuilder buf)
        {
        }

        /**
         * @param args
         */

        public static void main(String[] args)
        {
        }
    }
}
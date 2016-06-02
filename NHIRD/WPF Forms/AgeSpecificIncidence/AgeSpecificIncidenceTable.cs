using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHIRD
{
    class AgeSpecificIncidenceTable
    {
        string tableName;

        AgeSpecificIncidenceGroup[,] table =
            new AgeSpecificIncidenceGroup[100, 2];

        private AgeSpecificIncidenceTable(string tableName)
        {
            this.tableName = tableName;
        }

        static public AgeSpecificIncidenceTable getNewAgeSpecificIncidenceTable(string tableName)
        {
            return new AgeSpecificIncidenceTable(tableName);
        }

        public void addPatientYear(Gender gender, double startAge, double endAge)
        {
            int genderInt = gender == Gender.Male ? 1 : 0;
            int startAgeInt = Convert.ToInt32(Math.Truncate(startAge));
            int endAgeInt = Convert.ToInt32(Math.Truncate(endAge));
            if (startAge > startAgeInt)
            {
                double startResidual = (startAgeInt + 1) - startAge;
                table[startAgeInt, genderInt].patientYear += startResidual;
                startAgeInt++;
            }
            if (endAgeInt < 100)
            {
                if (endAge > endAgeInt)
                {
                    double endResidual = endAge - endAgeInt;
                    table[endAgeInt, genderInt].patientYear += endResidual;
                }
            }
            else
            {
                endAgeInt = 100;
            }

            for (int i = startAgeInt; i < endAgeInt; i++)
            {
                table[i, genderInt].patientYear++;
            }
        }
        public void addEvent(Gender gender, double eventAge)
        {
            int genderInt = gender == Gender.Male ? 1 : 0;
            int eventAgeInt = Convert.ToInt32(Math.Truncate(eventAge));
            table[eventAgeInt, genderInt].eventCount++;
        }
        public string getResult()
        {
            StringBuilder result = new StringBuilder();
            string title1 = "" + "\tFemale\t\t" + "\tMale\t\t" + "\tTotal\t\t";
            string title2 = "Age Group" + "\tPatient Year\tEvent Count\tAge Specific Incidence"
                                        + "\tPatient Year\tEvent Count\tAge Specific Incidence"
                                        + "\tPatient Year\tEvent Count\tAge Specific Incidence";
            result.AppendLine(title1);
            result.AppendLine(title2);

            for (int i = 0; i < 100; i++)
            {
                result.AppendLine(
                    i + "\t"
                    + table[i, 0].patientYear.Round(1) + "\t"
                    + table[i, 0].eventCount + "\t"
                    + ((double)table[i, 0].eventCount / table[i, 0].patientYear * 100).Round(1) + "%\t"
                    
                    + table[i, 1].patientYear.Round(1) + "\t"
                    + table[i, 1].eventCount + "\t"
                    + ((double)table[i, 1].eventCount / table[i, 1].patientYear * 100).Round(1) + "%\t"
                    
                    + (table[i, 0].patientYear+ table[i, 1].patientYear).Round(1) + "\t"
                    + (table[i, 0].eventCount+ table[i, 1].eventCount) + "\t"
                    + ((double)(table[i, 0].eventCount+ table[i, 1].eventCount) / 
                    (table[i, 0].patientYear+ table[i, 1].patientYear) * 100).Round(1) + "%\t"
                    
                    );
            }

            return result.ToString();
        }

    }

    public enum Gender
    {
        Female, Male
    }

    class AgeSpecificIncidenceGroup
    {
        public double patientYear;
        public int eventCount;
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NHIRD
{
    class AgeSpecificIncidenceViewModel : BasedNotifyPropertyChanged
    {
        public AgeSpecificIncidenceWindow parentWindow;
        public AgeSpecificIncidenceViewModel(AgeSpecificIncidenceWindow parent)
        {
            parentWindow = parent;
        }

        string _patientBasedDataFolderPath;
        public string patientBasedDataFolderPath
        {
            get
            {
                return _patientBasedDataFolderPath;
            }
            set
            {
                _patientBasedDataFolderPath = value;
                GlobalSetting.set("ASI_PBD", value);
                OnPropertyChanged(nameof(patientBasedDataFolderPath));
            }
        }

        string _standarizedIDFolderPath;
        public string standarizedIDFolderPath
        {
            get
            {
                return _standarizedIDFolderPath;
            }

            set
            {
                _standarizedIDFolderPath = value;
                GlobalSetting.set("ASI_SIT", value);
                OnPropertyChanged(nameof(standarizedIDFolderPath));
            }
        }

        string _matchResult;
        public string matchResult
        {
            get
            {
                return _matchResult;
            }
            set
            {
                _matchResult = value;
                OnPropertyChanged(nameof(matchResult));
            }
        }

        List<MatchOfPatiendBasedDataAndStandarizedID> matchOfPatiendBasedDataAndStandarizedID;
        public void generateMatchResult()
        {
            string result = "no match";
            var patientBasedDataFileList = parentWindow.patientBasedDataFolderSelector.FileList;
            var standardIDFileList = parentWindow.standarizedIDFolderSelector.FileList;
            matchOfPatiendBasedDataAndStandarizedID = new List<MatchOfPatiendBasedDataAndStandarizedID>();
            if (patientBasedDataFileList != null && standardIDFileList != null)
            {
                result = "PBD count = " + patientBasedDataFileList.Count + "\nstarndarizeID count = " + standardIDFileList.Count + "\n";
                foreach (var PBDfile in patientBasedDataFileList)
                {
                    var matchedGroupQuery = from q in standardIDFileList
                                            where q.@group == PBDfile.@group && q.hashGroup == PBDfile.hashGroup
                                            select q;
                    if (matchedGroupQuery.Count() == 1)
                    {
                        matchOfPatiendBasedDataAndStandarizedID.Add(
                            new MatchOfPatiendBasedDataAndStandarizedID()
                            {
                                patientBasedData = PBDfile,
                                standarizedID = matchedGroupQuery.First()
                            });
                    }
                    else if (matchedGroupQuery.Count() > 1)
                    {
                        result += PBDfile.name + " has more than 1 match. \n";
                    }
                    else if (matchedGroupQuery.Count() == 0)
                    {
                        result += PBDfile.name + " has no match. \n";
                    }
                }
            }

            matchResult = $"Total {matchOfPatiendBasedDataAndStandarizedID.Count} mached:  \n" + result;
        }

        public void generateAgeSpecificIncidence()
        {
            new AgeSpecificIncidenceCalculator(matchOfPatiendBasedDataAndStandarizedID).Do();
        }

    }
}

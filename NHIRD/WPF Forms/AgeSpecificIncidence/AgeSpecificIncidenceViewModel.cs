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

        string _outputDir;
        public string outputDir
        {
            get
            {
                return _outputDir;
            }

            set
            {
                _outputDir = value;
                GlobalSetting.set("ASI_outputDir", value);
                OnPropertyChanged(nameof(outputDir));
            }
        }

        string _IDIncludeCrieteriaFolderPath;
        public string IDIncludeCrieteriaFolderPath
        {
            get
            {
                return _IDIncludeCrieteriaFolderPath;
            }

            set
            {
                _IDIncludeCrieteriaFolderPath = value;
                GlobalSetting.set("ASI_IDIncludeCrieteriaFolderPath", value);
                OnPropertyChanged(nameof(IDIncludeCrieteriaFolderPath));
            }
        }


        string _dataEndDate;
        public string dataEndDate
        {
            get { return _dataEndDate; }
            set
            {
                _dataEndDate = value;
                GlobalSetting.set("ASI_DataEndDate", value);
                OnPropertyChanged(nameof(dataEndDate));
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
            var IDincludeCriteriaFileList = parentWindow.IDIncludeCrieteriaFolderSelector.FileList;
            bool isIDCriteriaEnable = parentWindow.IDIncludeCrieteriaFolderSelector.IsCriteriaChecked;
            matchOfPatiendBasedDataAndStandarizedID = new List<MatchOfPatiendBasedDataAndStandarizedID>();
            if (patientBasedDataFileList != null && standardIDFileList != null && (!isIDCriteriaEnable || IDincludeCriteriaFileList != null))
            {
                result = "PBD count = " + patientBasedDataFileList.Count + "\nstarndarizeID count = " + standardIDFileList.Count + "\n";
                foreach (var PBDfile in patientBasedDataFileList)
                {
                    var matchedGroupQueryForSID = from q in standardIDFileList
                                                  where q.@group == PBDfile.@group && q.hashGroup == PBDfile.hashGroup
                                                  select q;
                    IEnumerable<File> matchedGroupQueryForIDCriteria = new List<File>();
                    if (isIDCriteriaEnable)
                    {
                        matchedGroupQueryForIDCriteria = from q in IDincludeCriteriaFileList
                                                         where q.@group == PBDfile.@group && q.hashGroup == PBDfile.hashGroup
                                                         select q;
                    }
                    if (matchedGroupQueryForSID.Count() == 1)
                    {
                        var toADD = new MatchOfPatiendBasedDataAndStandarizedID()
                        {
                            patientBasedData = PBDfile,
                            standarizedID = matchedGroupQueryForSID.First()
                        };
                        if (isIDCriteriaEnable)
                        {
                            toADD.IDincludeCriteria = matchedGroupQueryForIDCriteria.First();
                        }
                        matchOfPatiendBasedDataAndStandarizedID.Add(toADD);
                    }
                    else if (matchedGroupQueryForSID.Count() > 1)
                    {
                        result += PBDfile.name + " has more than 1 match. \n";
                    }
                    else if (matchedGroupQueryForSID.Count() == 0)
                    {
                        result += PBDfile.name + " has no match. \n";
                    }
                }
            }

            matchResult = $"Total {matchOfPatiendBasedDataAndStandarizedID.Count} mached:  \n" + result;
            parentWindow.UpdateLayout();
        }

        public void generateAgeSpecificIncidence()
        {
            DateTime dataEnd;
            if (DateTime.TryParse(dataEndDate, out dataEnd))
            {
                new AgeSpecificIncidenceCalculator(matchOfPatiendBasedDataAndStandarizedID).Do(dataEnd, outputDir);
            }
            else
            {
                System.Windows.MessageBox.Show("Invalid date time.");
            }
        }

    }
}

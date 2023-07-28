using ICities;
using UnityEngine;
using ColossalFramework.UI;
using System.IO;
using System;

namespace Fix_pedestrain_garbage
{
    public class Fix_pedestrain_garbage : IUserMod
    {
        public string Name
        {
            get { return "Fix Pedestrian Garbage"; }
        }

        public string Description
        {
            get { return "A verry hackey solution to pedestrian areas not having their garbage collected properly"; }
        }

        //private bool isModActive = false;
        public static bool removeAllGarbage = false;
        public static bool removeFromPedestrianArea = true;
        public static ushort maxGarbageValue = 0;

        public static UICheckBox allCityCheckBox = null;
        public static UICheckBox pedestrainCheckBox = null;
        public static UISlider maxGarbageLevel = null;

        public void OnSettingsUI(UIHelperBase helper)
        {
            try
            {
                string readText = File.ReadAllText("Fix_pedestrain_garbage.txt");
                string[] lines = readText.Split('\n');
                for(int i=0;i<lines.Length; i++)
                {
                    if (lines[i].StartsWith("Enable_city_wide="))
                    {
                        string value = lines[i].Substring(lines[i].IndexOf('=')+1);
                        removeAllGarbage = value.Equals("True");
                    }
                    if (lines[i].StartsWith("Enable_pedestrian="))
                    {
                        string value = lines[i].Substring(lines[i].IndexOf('=')+1);
                        removeFromPedestrianArea = value.Equals("True");
                        
                    }
                    if (lines[i].StartsWith("Max_garbage=")){
                        string value = lines[i].Substring(lines[i].IndexOf('=') + 1);
                        maxGarbageValue = ushort.Parse(value);
                    }
                }
            }
            catch (Exception e)
            {
                saveSettingsState();
            }
            UIHelperBase group1 = helper.AddGroup("Enablization");
            allCityCheckBox = (UICheckBox)group1.AddCheckbox("Remove all garbage City wide", removeAllGarbage, (isChecked) => { 
                removeAllGarbage = isChecked; 
                Debug.Log("set collect all garabge to "+isChecked); 
                allCityCheckBoxUpdated();
                saveSettingsState();
            });
            pedestrainCheckBox = (UICheckBox)group1.AddCheckbox("Remove all grabage in pedestrain areas", removeFromPedestrianArea, (isChecked) => { 
                removeFromPedestrianArea = isChecked; 
                Debug.Log("set collect pedestrain garabge to " + isChecked); 
                pedestrainCheckBoxUpdated();
                saveSettingsState();
            });

            //helper.AddButton("print it", () => { Debug.Log("max garbage: "+CheckGarbageLevel.mostgarbage); });

            UIHelperBase group2 =helper.AddGroup("config");
            maxGarbageLevel = (UISlider)group2.AddSlider("Max building garbage:\n"+ maxGarbageValue, 0, 10000, 1, maxGarbageValue, (value) =>
            {
                Debug.Log("max garbage set to "+ value);
                maxGarbageValue = (ushort)value;
                saveSettingsState();
                maxGarbageLevel.parent.Find<UILabel>("Label").text = "Max building garbage:\n" + maxGarbageValue;
                
            });
            maxGarbageLevel.tooltip = "The ammount of garbage a building needs to have before it is automaticaly removed";
        }

        public void allCityCheckBoxUpdated() {
            if(allCityCheckBox != null && pedestrainCheckBox != null) {
                if (allCityCheckBox.isChecked)
                {
                    pedestrainCheckBox.isChecked = true;
                    removeFromPedestrianArea = true;
                }
            }
        }

        public void pedestrainCheckBoxUpdated()
        {
            if (allCityCheckBox != null && pedestrainCheckBox != null)
            {
                if (allCityCheckBox.isChecked)
                {
                    pedestrainCheckBox.isChecked = true;
                    removeFromPedestrianArea = true;
                }
            }
        }

        void saveSettingsState()
        {
            using (StreamWriter writer = new StreamWriter("Fix_pedestrain_garbage.txt"))
            {
                writer.NewLine = "\n";
                writer.WriteLine("Enable_city_wide="+ removeAllGarbage);
                writer.WriteLine("Enable_pedestrian="+ removeFromPedestrianArea);
                writer.WriteLine("Max_garbage=" + maxGarbageValue);
            }
        }
    }
}
/* This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/. */

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Unshitfuckeryfier
{
    class Program
    {
        public static string UnitRulesXML = "";
        public static string BalanceXML = "";
        public static bool senseThePresenceOfXmlNearby = false;

        // some units, e.g. Bison, Peacocks should not be included in the changes made to balance.xml
        public static List<Unit> ExceptionList = new();
        // some units don't have an age as their preq0 (@_@), so need to also be filtered out
        public static List<Unit> ExceptionList2 = new();

        //Barks and Triremes in EE are hardcoded to use damage modifiers as if they're Modern Age (VII / "age 6" when counting from 0) units. No, I don't fucking know why.
        public static List<Unit> UnitList = new();
        public static List<AgeMultipliers> AgeIsAttackerList = new();
        public static List<AgeMultipliers> AgeIsDefenderList = new();

        // both units may need to have values overriden vs themselves/each other?
        public static List<Unit> BarkTriremeList = new();
        public static List<Unit> BarkOverrideList = new();
        public static List<Unit> TriremeOverrideList = new();

        //Elephants (regardless of age) all do 30% less damage to Barks/Triremes in EE compared to T&P even after the standard fix is applied. No, I don't fucking know why.
        public static List<Elephant> ElephantFixList = new();

        //a separate list generated from balance.xml, to compare with unitrules.xml and note any differences in listed units (except for Exception Units)
        public static List<Unit> BalanceXMLUnitList = new();

        static void Main(string[] args)
        {
            CheckForXMLFiles();

            if (senseThePresenceOfXmlNearby == true)
            {
                //1a) populate ExceptionList from hardcoded
                //1b) populate ElephantList from hardcoded [OBSOLETE]
                //1c) populate AgeIsAttackerList, AgeIsDefenderList, BarkTriremeOverrideList, ElephantFixList from hardcoded

                //2a) populate UnitList from unitrules.xml (UnitName, UnitAge), excluding ExceptionList
                //2b) populate BalanceXMLUnitList from balance.xml
                //2c) compare UnitNames in both lists. if any names are present on one but not both lists, note them to user and ask for confirmation before continuing

                //3a) in balance.xml, change modifiers of all non-exception units vs bark/trireme based on age (AgeIsAttackerList)
                //3b) in balance.xml, change modifiers of bark/trireme vs all non-exception units vs based on age (AgeIsDefenderList)
                //3c) MAYBE: in balance.xml, override modifiers of Bark/Trireme vs themselves/each other (BarkTriremeOverrideList)

                //3d) in balance.xml, change modifiers of elephants to fixed values using ElephantFixList

                try
                {
                    ListExceptionUnits();   // 1a
                    PopulateDamageValues(); // 1c
                    Console.WriteLine("Step 1 completed");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }

                try
                {
                    PopulateUnitList();     // 2a
                    Console.WriteLine("Step 2 completed");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

            else if (senseThePresenceOfXmlNearby == false)
            {
                Console.WriteLine("Can't find one or more of the specified XML files.");
            }

            Console.WriteLine();
            Console.WriteLine("Main function finished. Press any key to continue...");
            Console.ReadKey();
        }

        //1a) populate ExceptionList from hardcoded
        static void ListExceptionUnits()
        {
            ExceptionList.Add(new Unit { UnitName = "Wild_Bird" });
            ExceptionList.Add(new Unit { UnitName = "Flock_Bird" });
            ExceptionList.Add(new Unit { UnitName = "Gull_Bird" });
            ExceptionList.Add(new Unit { UnitName = "Farm_Pig" });
            ExceptionList.Add(new Unit { UnitName = "Farm_Chicken" });
            ExceptionList.Add(new Unit { UnitName = "Herd_Horse" });
            ExceptionList.Add(new Unit { UnitName = "Herd_Sheep" });
            ExceptionList.Add(new Unit { UnitName = "Herd_Bison" });
            ExceptionList.Add(new Unit { UnitName = "Herd_Bear" });
            ExceptionList.Add(new Unit { UnitName = "Herd_Fish" });
            ExceptionList.Add(new Unit { UnitName = "Herd_Whales" });
            ExceptionList.Add(new Unit { UnitName = "Herd_Peacock" });

            //these have non-age preq0s which mess things up if included, but it would look messy in an extracted damage table to not include them at all so they'll be done separately (..eventually)
            ExceptionList2.Add(new Unit { UnitName = "General" });
            ExceptionList2.Add(new Unit { UnitName = "Spy" });
            ExceptionList2.Add(new Unit { UnitName = "The_Despot" });
            ExceptionList2.Add(new Unit { UnitName = "The_Senator" });
            ExceptionList2.Add(new Unit { UnitName = "The_Monarch" });
            ExceptionList2.Add(new Unit { UnitName = "The_President" });
            ExceptionList2.Add(new Unit { UnitName = "The_Comrade" });
            ExceptionList2.Add(new Unit { UnitName = "The_CEO" });
            ExceptionList2.Add(new Unit { UnitName = "Alexander" });
            ExceptionList2.Add(new Unit { UnitName = "Napoleon" });
            ExceptionList2.Add(new Unit { UnitName = "Parmenio" });
            ExceptionList2.Add(new Unit { UnitName = "Antipater" });
            ExceptionList2.Add(new Unit { UnitName = "Ptolemy" });
            ExceptionList2.Add(new Unit { UnitName = "Darius" });
            ExceptionList2.Add(new Unit { UnitName = "Memnon" });
            ExceptionList2.Add(new Unit { UnitName = "Spitamenes" });
            ExceptionList2.Add(new Unit { UnitName = "Chandragupta" });
            ExceptionList2.Add(new Unit { UnitName = "Potus" });
            ExceptionList2.Add(new Unit { UnitName = "Wellington" });
            ExceptionList2.Add(new Unit { UnitName = "Archduke_Charles" });
            ExceptionList2.Add(new Unit { UnitName = "Djezzar" });
            ExceptionList2.Add(new Unit { UnitName = "Advisor Fouché" });
            //ExceptionList2.Add(new Unit { UnitName = "Commander_Schwarzenberg" }); //actually uses Industrial as preq0
            ExceptionList2.Add(new Unit { UnitName = "General_Blucher" });
            ExceptionList2.Add(new Unit { UnitName = "General_Kutosov" });
            //ExceptionList2.Add(new Unit { UnitName = "Commander_Paoli" }); //actually uses Industrial as preq0
            ExceptionList2.Add(new Unit { UnitName = "Boadicea" });
        }

        //1c) populate AgeIsAttackerList, AgeIsDefenderList, BarkTriremeOverrideList, ElephantFixList from hardcoded
        static void PopulateDamageValues()
        {
            //bark to age 59, 59, 63, 67, 83, 88, 100, 100
            //age to bark 100, 115, 120, 150, 161, 170, 170, 150
            //bark to bark 77
            //elephants do 30% less than intended at all ages, so add 43% damage to make it ~100% of original

            AgeIsDefenderList.Add(new AgeMultipliers { Preq0 = "none", AgeNumber = 0, Multiplier = 59 });
            AgeIsDefenderList.Add(new AgeMultipliers { Preq0 = "Classical Age", AgeNumber = 0, Multiplier = 59 });
            AgeIsDefenderList.Add(new AgeMultipliers { Preq0 = "Medieval Age", AgeNumber = 0, Multiplier = 63 });
            AgeIsDefenderList.Add(new AgeMultipliers { Preq0 = "Gunpowder Age", AgeNumber = 0, Multiplier = 67 });
            AgeIsDefenderList.Add(new AgeMultipliers { Preq0 = "Enlightenment Age", AgeNumber = 0, Multiplier = 83 });
            AgeIsDefenderList.Add(new AgeMultipliers { Preq0 = "Industrial Age", AgeNumber = 0, Multiplier = 88 });
            AgeIsDefenderList.Add(new AgeMultipliers { Preq0 = "Modern Age", AgeNumber = 0, Multiplier = 100 });
            AgeIsDefenderList.Add(new AgeMultipliers { Preq0 = "Information Age", AgeNumber = 0, Multiplier = 100 });

            AgeIsAttackerList.Add(new AgeMultipliers { Preq0 = "none", AgeNumber = 0, Multiplier = 100 });
            AgeIsAttackerList.Add(new AgeMultipliers { Preq0 = "Classical Age", AgeNumber = 1, Multiplier = 115 });
            AgeIsAttackerList.Add(new AgeMultipliers { Preq0 = "Medieval Age", AgeNumber = 2, Multiplier = 120 });
            AgeIsAttackerList.Add(new AgeMultipliers { Preq0 = "Gunpowder Age", AgeNumber = 3, Multiplier = 150 });
            AgeIsAttackerList.Add(new AgeMultipliers { Preq0 = "Enlightenment Age", AgeNumber = 4, Multiplier = 161 });
            AgeIsAttackerList.Add(new AgeMultipliers { Preq0 = "Industrial Age", AgeNumber = 5, Multiplier = 170 });
            AgeIsAttackerList.Add(new AgeMultipliers { Preq0 = "Modern Age", AgeNumber = 6, Multiplier = 170 });
            AgeIsAttackerList.Add(new AgeMultipliers { Preq0 = "Information Age", AgeNumber = 7, Multiplier = 150 });

            //the first two will allow it to find each entry, then it uses the ones below it to actually change the modifiers for each unit in the top list
            BarkTriremeList.Add(new Unit { UnitName = "Bark"});
            BarkTriremeList.Add(new Unit { UnitName = "Trireme"});

            BarkOverrideList.Add(new Unit { UnitName = "Bark", OverrideDamage = 77 });
            BarkOverrideList.Add(new Unit { UnitName = "Trireme", OverrideDamage = 77 });
            TriremeOverrideList.Add(new Unit { UnitName = "Bark", OverrideDamage = 77 });
            TriremeOverrideList.Add(new Unit { UnitName = "Trireme", OverrideDamage = 77 });

            //elephants and bugged damage, name a more iconic duo
            ElephantFixList.Add(new Elephant { UnitName = "War_Elephant", Multiplier = 143 });
            ElephantFixList.Add(new Elephant { UnitName = "Mahout", Multiplier = 143 });
            ElephantFixList.Add(new Elephant { UnitName = "Gun_Mahout", Multiplier = 143 });
            ElephantFixList.Add(new Elephant { UnitName = "Culverin_Mahout", Multiplier = 143 });
        }

        //2a) populate UnitList from unitrules.xml (UnitName, UnitAge), excluding ExceptionList
        static void PopulateUnitList()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(UnitRulesXML);

            XmlNodeList nodeList = doc.SelectNodes("ROOT/UNIT");
            foreach (XmlNode node in nodeList)
            {
                //UnitList.Add(new Unit { UnitName = node.ChildNodes[0].InnerText, UnitAge = short.Parse(AgeLookup.Age[(node.ChildNodes[11].InnerText)]});

                foreach (var name in UnitList)
                {
                    Console.WriteLine("test");
                }

                /*foreach (var Unit in UnitList)
                {
                    Console.WriteLine("{0}", Unit);
                }*/

                Console.WriteLine(node.ChildNodes[0].InnerText + " | " + node.ChildNodes[11].InnerText); //this works, so the XML-reading is clearly working
            }
            
            /*foreach (Unit.UnitName _ in UnitList)
            {
                Console.WriteLine(UnitList.ToString());
            }*/


            //remove duplicates from list (because each is only listed once in balance.xml)
        }

        static void ReadUnitrulesXml() // reads the last game name (mostly as a building block for later functions + troubleshooting rather than to use itself)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(UnitRulesXML);

            foreach (XmlElement xmlElement in
                doc.DocumentElement.SelectNodes("UNIT[NAME='Bark]"))
            {
                
            }

            /*XmlNode xmlNode = doc.SelectSingleNode("ROOT/GAMESPY/LAST_GAME_NAME");
            string name = xmlNode.InnerText;

            Console.WriteLine("Last game name: " + name);
            Console.WriteLine("If this is correct, Press ENTER to continue...");
            Console.ReadLine();*/
        }

        static void ReadBalanceXml() // reads the last game name (mostly as a building block for later functions + troubleshooting rather than to use itself)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(BalanceXML);
            XmlNode xmlNode = doc.SelectSingleNode("ROOT/GAMESPY/LAST_GAME_NAME");
            string name = xmlNode.InnerText;

            Console.WriteLine("Last game name: " + name);
            Console.WriteLine("If this is correct, Press ENTER to continue...");
            Console.ReadLine();
        }

        /*static void CheckXml() // checks if #ICON169 is already present in last game name
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(playerProfile);
            XmlNode xmlNode = doc.SelectSingleNode("ROOT/GAMESPY/LAST_GAME_NAME");
            string lastGameName = xmlNode.InnerText;

            if (lastGameName.Contains("#ICON169") == true)
            {
                cbpName = 1;
                Console.WriteLine("Last game name already contains #ICON169. Press ENTER to continue...");
                Console.ReadLine();
                return;
            }
            else
            {
                cbpName = 0;
                Console.WriteLine("#ICON169 not in last game name. Continuing...");
            }

        }

        static void WriteXml() // updates last game name to prefix with #ICON169
        {
            XmlDocument doc = new XmlDocument();

            doc.Load(playerProfile);
            XmlNode xmlNode = doc.SelectSingleNode("ROOT/GAMESPY/LAST_GAME_NAME");
            xmlNode.InnerText = "#ICON169 " + xmlNode.InnerText;
            doc.Save(playerProfile);

            Console.WriteLine("New game name: " + xmlNode.InnerText);
        }*/

        static void CheckForXMLFiles()
        {
            Console.WriteLine("Enter full path for unitrules.xml to use.");
            UnitRulesXML = Console.ReadLine();

            Console.WriteLine("Enter full path for balance.xml to use. This file should already have been run through the object mask workaround utility.");
            BalanceXML = Console.ReadLine();

            // this is surely not an efficient way of using a bool right? but I couldn't find an alternative in 3 minutes so it stays for now
            if (File.Exists(UnitRulesXML) && File.Exists(BalanceXML))
            {
                senseThePresenceOfXmlNearby = true;
            }
            else
            {
                senseThePresenceOfXmlNearby = false;
            }
        }

    }

    class Unit
    {
        public string UnitName { get; set; }
        public short UnitAge { get; set; }
        public int CurrentBalanceDamage { get; set; }
        public int AgeBasedModifier { get; set; }
        public int OverrideDamage { get; set; }//used to override for Bark/Trireme if required
    }

    class AgeMultipliers
    {
        public string Preq0 { get; set; }
        public short AgeNumber { get; set; }
        public int Multiplier { get; set; }
    }

    class Elephant
    {
        public string UnitName { get; set; }
        public int Multiplier { get; set; }
    }

    /*class AgeLookup
    {
        public static string[] Age = new string[8] { "none", "Classical Age", "Medieval Age", "Gunpowder Age", "Enlightenment Age", "Industrial Age", "Modern Age", "Information Age" };
    }*/

    class AgeLookup
    {
        //IDictionary<string, short> Age = new Dictionary<string, short>();
        //Age.Add(new KeyValuePair<string, short>("none", 0));
    }
}

﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Cors;

namespace HarbourGuideServerAndDB {
    public class Request {
        public String Type { get; set; }
        public String URL { get; set; }
        public String Host { get; set; }

        public static Boolean Valid;

        public static String Call;

        public Request(String type, String url, String host) {
            Type = type;
            URL = url;
            Host = host;
        }

        public Boolean getValid() {
            return Valid;
        }

        public String getCall() {
            return Call;
        }

        public static Request GetRequest(String request) {
            if (String.IsNullOrEmpty(request))
                return null;

            String[] dataTokens = null;
            String[] tokens = request.Split(' '); 
            String type = null;
            String url = null;
            String host = null;
            try
            {
                type = tokens[0];
                url = tokens[1];
                host = tokens[4];
                Console.WriteLine(url);
            }
            catch (System.Exception e)
            {
                Console.WriteLine("Error:" + e + "\n\n\n" + tokens[0] + "\n" + tokens[1] + "\n" + tokens[2]);
            }

            if (url.Contains("DATA;")) {
                dataTokens = url.Split(';');
                Console.WriteLine("\nDotaString: " + dataTokens[0] + "," + dataTokens[1] + "," + dataTokens[2] + "," + dataTokens[3] + "," + dataTokens[4] + ",");
                Call = "Data";
            }
            else if (url.Contains("GETSCORE;"))
            {
                HandleGetScore();
                Call = "Getscore";
            }
            else
            {
                Call = "Invalid";
            }

            if (dataTokens != null)
                Valid = HandleData(dataTokens);



            return new Request(type, url, host);
        }

        private static void HandleGetScore()
        {

        }

        private static bool HandleData(String[] dataGiven) {
            String[] dataTokens = dataGiven;

            //Checks if data being passed in is valid
            foreach (string s in dataTokens)
                if (String.IsNullOrEmpty(s))
                    return false;

            foreach(char c in dataTokens[1])
                if (!Char.IsLetter(c))
                    return false;

            foreach (char c in dataTokens[2])
                if (!Char.IsDigit(c))
                    return false;

            foreach (char c in dataTokens[3])
                if (!Char.IsDigit(c))
                    return false;

            foreach (char c in dataTokens[4])
                if (!Char.IsDigit(c))
                    return false;

            foreach (String s in dataTokens)
                Console.Write(s + ", ");

            //Moves all data one index lower, then makes the highest one carry a newline character.
            for (int i = 0; i < 3; i++) {
                dataTokens[i] = dataTokens[i + 1];
            }
            Console.WriteLine("\nCounter Is: " + Program.counter);
            Database.SetScore(dataTokens, ++Program.counter);

            return true;
        }
    }
}

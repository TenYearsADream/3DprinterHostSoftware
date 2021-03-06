﻿/*
   Copyright 2011 repetier repetierdev@gmail.com

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RepetierHost.view;

namespace RepetierHost.model
{
    public class Printjob
    {
        public class PrintTime
        {
            public int line;
            public long time;
        }
        public bool etaModeNormal = true;
        public bool dataComplete = false;
        public int totalLines;
        public int linesSend;
        public bool exclusive = false;
        public int maxLayer = -1;

        /// <summary>
        /// The possible types of jobs that may be running. 
        /// </summary>
        public enum jobMode
        {
            /// <summary>
            /// Not job is currrently running. Old 0
            /// </summary>
            noJob, 

            /// <summary>
            /// Printing job, Old 1
            /// </summary>
            printingJob,

            /// <summary>
            /// Finished the most recent job. Old 2
            /// </summary>
            finishedJob,

            /// <summary>
            /// Aborted the most recent job. Old 3
            /// </summary>
            abortedJob

        }
        public jobMode mode = jobMode.noJob; /// 0 = no job defines, 1 = printing, 2 = finished, 3 = aborted
        public double computedPrintingTime = 0;
        public DateTime jobStarted, jobFinished;
        LinkedList<GCodeCompressed> jobList = new LinkedList<GCodeCompressed>();
        //LinkedList<PrintTime> times = new LinkedList<PrintTime>();
        PrinterConnection connection;
        GCodeAnalyzer ana = null;

        public Printjob(PrinterConnection c)
        {
            connection = c;
        }

        /// <summary>
        /// Begins a new printing job. Resets many of the varriables. 
        /// </summary>
        public void BeginJob()
        {
            connection.firePrinterAction(Trans.T("L_BUILDING_PRINT_JOB...")); //"Building print job...");
            dataComplete = false;
            jobList.Clear();
            //times.Clear();
            totalLines = 0;
            linesSend = 0;
            computedPrintingTime = 0;
            connection.lastlogprogress = -1000;
            maxLayer = -1;
            mode = jobMode.printingJob;
            ana = new GCodeAnalyzer(true);
            connection.analyzer.StartJob();
            
            //Main.main.Invoke(Main.main.mainHelp.UpdateJobButtons);
            UpdateAll updateAll = Main.main.mainUpdaterHelper.UpdateEverythingInMain;
            Main.main.Invoke(updateAll);
        }

        /// <summary>
        /// Ends the current printing job. If another one is in que then it resets some the the varriables. 
        /// </summary>
        public void EndJob()
        {
            // If no more jobs to do. 
            if (jobList.Count == 0)
            {
                mode = 0;
                connection.firePrinterAction(Trans.T("L_IDLE"));
                //Main.main.Invoke(Main.main.UpdateJobButtons);
                //Main.main.Invoke(Main.main.mainHelp.UpdateJobButtons);
                UpdateAll updateAll = Main.main.mainUpdaterHelper.UpdateEverythingInMain;
                Main.main.Invoke(updateAll);

                Main.main.manulControl.Invoke(Main.main.manulControl.SetStatusJobFinished);
                return;
            }
            else // if more jobs to do. 
            {
                dataComplete = true;
                jobStarted = DateTime.Now;
                connection.firePrinterAction(Trans.T("L_PRINTING..."));
            }
        }

        /// <summary>
        /// Stops the current job by injecting M29
        /// </summary>
        public void KillJob()
        {
            //mode = 3;
            mode = jobMode.abortedJob;
            lock (jobList)
            {
                if (dataComplete == false && jobList.Count == 0) return;
                dataComplete = false;
                jobFinished = DateTime.Now;
                jobList.Clear();
                totalLines = linesSend;
            }
            exclusive = false;
            connection.injectManualCommandFirst("M29");
            foreach (GCodeShort code in Main.main.editor.getContentArray(3))
            {
                connection.injectManualCommand(code.text);
            }
            //Main.main.Invoke(Main.main.UpdateJobButtons);
            //Main.main.Invoke(Main.main.mainHelp.UpdateJobButtons);
            UpdateAll updateAll = Main.main.mainUpdaterHelper.UpdateEverythingInMain;
            Main.main.Invoke(updateAll);

            connection.firePrinterAction(Trans.T("L_JOB_KILLED")); //"Job killed");
            DoEndKillActions();
            Main.main.manulControl.Invoke(Main.main.manulControl.SetStatusJobKilled);
        }


        public void DoEndKillActions()
        {
            if (exclusive) // not a normal print job
            {
                exclusive = false;
                return;
            }
            connection.GetInjectLock();
            if (connection.afterJobDisableExtruder)
            {
                for(int i=0;i<Main.connection.numberExtruder;i++) 
                    connection.injectManualCommand("M104 S0 T"+i.ToString());
            }
            if(connection.afterJobDisablePrintbed) 
                connection.injectManualCommand("M140 S0");
            connection.ReturnInjectLock();
            if (connection.afterJobGoDispose)
                connection.doDispose();
            if(connection.afterJobDisableMotors)
                connection.injectManualCommand("M84");
        }
        public void PushData(string code)
        {
            code = code.Replace('\r', '\n');
            string[] lines = code.Split('\n');
            foreach (string line in lines)
            {
                if (line.Length == 0) continue;
                GCode gcode = new GCode();
                gcode.Parse(line);
                if (!gcode.comment)
                {
                    jobList.AddLast(new GCodeCompressed(gcode));
                    totalLines++;
                }
            }
        }
        public void PushGCodeShortArray(List<GCodeShort> codes)
        {
            foreach (GCodeShort line in codes)
            {
                if (line.Length == 0) continue;
                ana.analyzeShort(line);
                GCode gcode = new GCode();
                gcode.Parse(line.text);
                if (!gcode.comment)
                {
                    jobList.AddLast(new GCodeCompressed(gcode));
                    totalLines++;
                }
                if (line.hasLayer)
                    maxLayer = line.layer;
            }
            computedPrintingTime = ana.printingTime;
        }
        /// <summary>
        /// Check, if more data is stored
        /// </summary>
        /// <returns></returns>
        public bool hasData()
        {
            return linesSend < totalLines;
        }

        /// <summary>
        /// Shows the next value (Gcode object )in the jobList linked list, but does not increment or remove the value. 
        /// </summary>
        /// <returns>null if nothing to send, otherwise the next GCode</returns>
        public GCode PeekData()
        {
            if (jobList.Count == 0)
            {
                return null;
            }

            return new GCode(jobList.First.Value);
        }
        public GCode PopData()
        {
            GCode gc = null;
            bool finished = false;
            lock (jobList)
            {
                if (jobList.Count == 0) return null;
                try
                {
                    gc = new GCode(jobList.First.Value);
                    jobList.RemoveFirst();
                    linesSend++;
                    /*PrintTime pt = new PrintTime();
                    pt.line = linesSend;
                    pt.time = DateTime.Now.Ticks;
                    lock (times)
                    {
                        times.AddLast(pt);
                        if (times.Count > 1500)
                            times.RemoveFirst();
                    }*/
                }
                catch 
                {                 
                };

                //finished = jobList.Count == 0 && mode != 3;
                finished = jobList.Count == 0 && mode != jobMode.abortedJob;
            }
            if (finished)
            {
                dataComplete = false;
                mode = Printjob.jobMode.finishedJob;
                jobFinished = DateTime.Now;
                long ticks = (jobFinished.Ticks - jobStarted.Ticks) / 10000;
                long hours = ticks / 3600000;
                ticks -= 3600000 * hours;
                long min = ticks / 60000;
                ticks -= 60000 * min;
                long sec = ticks / 1000;
                //Main.conn.log("Printjob finished at " + jobFinished.ToShortDateString()+" "+jobFinished.ToShortTimeString(),false,3);
                Main.connection.log(Trans.T1("L_PRINTJOB_FINISHED_AT",jobFinished.ToShortDateString() + " " + jobFinished.ToShortTimeString()), false, 3);
                StringBuilder s = new StringBuilder();
                if (hours > 0)
                    s.Append(Trans.T1("L_TIME_H:",hours.ToString())); //"h:");
                if (min > 0)
                    s.Append(Trans.T1("L_TIME_M:",min.ToString()));
                s.Append(Trans.T1("L_TIME_S",sec.ToString()));
                //Main.conn.log("Printing time:"+s.ToString(),false,3);
                //Main.conn.log("Lines send:" + linesSend.ToString(), false, 3);
                //Main.conn.firePrinterAction("Finished in "+s.ToString());
                Main.connection.log(Trans.T1("L_PRINTING_TIME:",s.ToString()), false, 3);
                Main.connection.log(Trans.T1("L_LINES_SEND:X",linesSend.ToString()), false, 3);
                Main.connection.firePrinterAction(Trans.T1("L_FINISHED_IN",s.ToString()));
                DoEndKillActions();
                //.main.Invoke(Main.main.UpdateJobButtons);
                //Main.main.Invoke(Main.main.mainHelp.UpdateJobButtons);
                UpdateAll updateAll = Main.main.mainUpdaterHelper.UpdateEverythingInMain;
                Main.main.Invoke(updateAll);

                Main.main.manulControl.Invoke(Main.main.manulControl.SetStatusJobFinished);
                RepetierHost.view.SoundConfig.PlayPrintFinished(false);
            }
            return gc;
        }
        public float PercentDone {
            get {
              if(totalLines==0) return 100f;
              return 100f*(float)linesSend/(float)totalLines;
            }
        }
        public static String DoubleToTime(double time)
        {
            long ticks = (long)(time*1000);
            long hours = ticks / 3600000;
            ticks -= 3600000 * hours;
            long min = ticks / 60000;
            ticks -= 60000 * min;
            long sec = ticks / 1000;
            StringBuilder s = new StringBuilder();
            if (hours > 0)
                s.Append(Trans.T1("L_TIME_H:", hours.ToString())); //"h:");
            if (min > 0)
                s.Append(Trans.T1("L_TIME_M:", min.ToString()));
            s.Append(Trans.T1("L_TIME_S", sec.ToString()));
            return s.ToString();
        }
        public String ETA {
            get {
                //if (linesSend < 3) return "---";
                try
                {
                    long ticks = 0;
                    /*lock (times)
                    {
                        if (times.Count > 100)
                        {
                            PrintTime t1 = times.First.Value;
                            PrintTime t2 = times.Last.Value;
                            ticks = (t2.time - t1.time) / 10000 * (totalLines - linesSend) / (t2.line - t1.line + 1);
                        }
                        else
                            ticks = (DateTime.Now.Ticks - jobStarted.Ticks) / 10000 * (totalLines - linesSend) / linesSend; // Milliseconds
                    }*/
                    if (etaModeNormal)
                    {
                        ticks = (long)(1000.0 * (computedPrintingTime - Main.connection.analyzer.printingTime) * (1.0 + 0.01 * Main.connection.addPrintingTime) * 100.0 / (float)Main.connection.speedMultiply);
                        long hours = ticks / 3600000;
                        ticks -= 3600000 * hours;
                        long min = ticks / 60000;
                        ticks -= 60000 * min;
                        long sec = ticks / 1000;
                        StringBuilder s = new StringBuilder();
                        if (hours > 0)
                            s.Append(Trans.T1("L_TIME_H:", hours.ToString())); //"h:");
                        if (min > 0)
                            s.Append(Trans.T1("L_TIME_M:", min.ToString()));
                        s.Append(Trans.T1("L_TIME_S", sec.ToString()));
                        return s.ToString();
                    }
                    else
                    {
                        DateTime dt = DateTime.Now;
                        dt = dt.AddSeconds(computedPrintingTime - Main.connection.analyzer.printingTime);
                        //dt.ToLocalTime();
                        return dt.ToLongTimeString();
                    }
                }
                catch
                {
                    return "-"; // Overflow somewhere
                }
            }
        }
    }
}

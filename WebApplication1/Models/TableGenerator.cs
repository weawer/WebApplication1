﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Schedule.Models
{
    public class TableGenerator
    {
        static public DataTable generate(DateTime date, string span /*List<int> groups*/)
        {
            //för tillfäller hårdkodad till vecka måndag -söndag vekan som 2016-01-05 tillhör
            //DateTime date = DateTime.Parse("2016.01.05");
            //string span = "week";
            DataTable dt = new DataTable();
            DateTime toDate;
            if (span.Equals("day"))
            {
                toDate = date;
            } else if (span.ToLower().Equals("week"))
            {
                date = date.AddDays(-(int)date.DayOfWeek + (int)DayOfWeek.Monday);
                toDate = date.AddDays(7);
            } else
            {
                date = date.AddDays(-(int)date.Date.Day + 1);
                toDate = date.AddMonths(1);
                toDate = toDate.AddDays(-1);
            }
            var shiftworkers = ShiftWorker.get(date, toDate/*, groups*/);
            Dictionary<string, int> addedWorkers = new Dictionary<string, int>();
            int counter = 0;
            dt.Columns.Add(new DataColumn("Name"));
            foreach (var s in shiftworkers)
            {
                if (!dt.Columns.Contains(s.date.ToShortDateString()))
                {
                    dt.Columns.Add(new DataColumn(s.date.ToShortDateString()));
                }
                string shiftId = s.shift.shiftId;
                if (s.vacation)
                {
                    shiftId = shiftId + "s";
                }
                if (!addedWorkers.Keys.Contains(s.worker.workerName + " " + s.worker.workerSurName))
                {
                    DataRow row = dt.NewRow();
                    row["Name"] = s.worker.workerName + " " + s.worker.workerSurName;
                    row[s.date.ToShortDateString()] = shiftId;
                    dt.Rows.Add(row);
                    addedWorkers[s.worker.workerName + " " + s.worker.workerSurName] = counter;
                    counter++;

                }
                else
                {
                    dt.Rows[addedWorkers[s.worker.workerName + " " + s.worker.workerSurName]][s.date.ToShortDateString()] = shiftId;
                }
            }
            return dt;
        }
        public static void addDetails(object sender, GridViewRowEventArgs e, GridView Schedule)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int a = e.Row.Cells.Count;
                for (int b = 0; a > b; b++)
                {
                    var header = Schedule.HeaderRow.Cells[b].Text;
                    if (header != "name")
                    {

                        e.Row.Cells[b].Attributes.Add("onClick", "alert('" + e.Row.Cells[0].Text + " " + header + "');");
                    }
                    else
                    {
                        e.Row.Cells[b].Attributes.Add("onClick", "alert('You have clicked :" + e.Row.Cells[0].Text + "');");
                    }


                    e.Row.Cells[b].Attributes["style"] += "cursor:pointer;cursor:hand;";

                    switch (e.Row.Cells[b].Text.ToLower())
                    {
                        case "1":
                            e.Row.Cells[b].BackColor = System.Drawing.Color.Cyan;
                            break;
                        case "2":
                            e.Row.Cells[b].BackColor = System.Drawing.Color.Green;
                            break;
                        case "3":
                            e.Row.Cells[b].BackColor = System.Drawing.Color.Red;
                            break;
                        case "4":
                            e.Row.Cells[b].BackColor = System.Drawing.Color.LightSkyBlue;
                            break;
                        case "5":
                            e.Row.Cells[b].BackColor = System.Drawing.Color.Yellow;
                            break;
                        case "6":
                            e.Row.Cells[b].BackColor = System.Drawing.Color.Pink;
                            break;
                        case "7":
                            e.Row.Cells[b].BackColor = System.Drawing.Color.Purple;
                            break;
                        case "8":
                            e.Row.Cells[b].BackColor = System.Drawing.Color.Coral;
                            break;
                        case "bl":
                            e.Row.Cells[b].BackColor = System.Drawing.Color.White;
                            e.Row.Cells[b].Text = e.Row.Cells[b].Text.ToUpper();
                            break;

                    }
                    if (e.Row.Cells[b].Text.ToLower().Contains("s") && e.Row.Cells[b].Text.Contains("0"))
                    {
                        e.Row.Cells[b].Text = ("S");
                    }
                    if (e.Row.Cells[b].Text.Contains("s") && e.Row.Cells[b].Text != "&nbsp;" && b != 0)
                    {
                        e.Row.Cells[b].BackColor = System.Drawing.Color.Orange;
                        e.Row.Cells[b].Text = e.Row.Cells[b].Text.ToUpper();
                    }
                    if (e.Row.Cells[b].Text.Contains("0") && !e.Row.Cells[b].Text.Contains("s"))
                    {
                        e.Row.Cells[b].Text = "ERROR";
                    }





                }

            }
        }
    }
}
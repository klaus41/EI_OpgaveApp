﻿using EI_OpgaveApp.Database;
using EI_OpgaveApp.Models;
using EI_OpgaveApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EI_OpgaveApp.Synchronizers
{
    public class JobSynchronizer
    {
        List<Job> onlineList = new List<Job>();
        List<Job> jobList = new List<Job>();

        ServiceFacade facade = ServiceFacade.GetInstance;
        MaintenanceDatabase db = App.Database;
        public async void SyncDatabaseWithNAV()
        {
            try
            {
                bool done = false;
                while (!done)
                {
                    jobList = await db.GetJobsAsync();
                    var s = await facade.JobService.GetJobsAsync();
                    foreach (var item in s)
                    {
                        onlineList.Add(item);
                    }
                    done = true;
                }
                foreach (var item in onlineList)
                {
                    try
                    {
                        await db.SaveJobAsync(item);
                    }
                    catch
                    {

                    }
                }
            }
            catch { }
        }
    }
}

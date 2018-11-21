using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quartz;
using Quartz.Impl;
using Quartz.Core;
using Quartz.Listener;
using Quartz.Logging;
using Quartz.Simpl;
using Quartz.Spi;
using Quartz.Util;
using Quartz.Xml;
using System.Threading.Tasks;

namespace DataAccess
{
    public class QuartzHelper
    {
        /// <summary>
        /// 时间间隔执行任务
        /// </summary>
        /// <typeparam name="T">任务类，必须实现IJob接口</typeparam>
        /// <param name="seconds">时间间隔(单位：毫秒)</param>
        public static void ExecuteInterval<T>(int seconds) where T : IJob
        {
            ISchedulerFactory factory = new StdSchedulerFactory();
            IScheduler scheduler = factory.GetScheduler().Result;
            //IJobDetail job = JobBuilder.Create<T>().WithIdentity("job1", "group1").Build();
            IJobDetail job = JobBuilder.Create<T>().Build();

            ITrigger trigger = TriggerBuilder.Create()
                .StartNow()
                .WithSimpleSchedule(x => x.WithIntervalInSeconds(seconds).RepeatForever())
                .Build();

            scheduler.ScheduleJob(job, trigger);

            scheduler.Start();
        }

        /// <summary>
        /// 指定时间执行任务
        /// </summary>
        /// <typeparam name="T">任务类，必须实现IJob接口</typeparam>
        /// <param name="cronExpression">cron表达式，即指定时间点的表达式</param>
        public static void ExecuteByCron<T>(string cronExpression) where T : IJob
        {
            ISchedulerFactory factory = new StdSchedulerFactory();
            Quartz.IScheduler scheduler = factory.GetScheduler().Result;

            IJobDetail job = JobBuilder.Create<T>().Build();

            ICronTrigger trigger = (ICronTrigger)TriggerBuilder.Create()
                .WithCronSchedule(cronExpression)
                .Build();

            scheduler.ScheduleJob(job, trigger);
            scheduler.Start();
        }
    }

    /*
    程序开始计划的代码：

string cronExpression = "0 0 9,16 * * ? ";　　=>这是指每天的9点和16点执行任务
QuartzHelper.ExecuteByCron<MyJob>(cronExpression);　　=>这是调用Cron计划方法

 

简单说一下Cron表达式吧，

"0 0 9,16 * * ? "，

顺序从左到右

0：秒

0：分

9,16：小时，逗号分隔
*/
    #region 任务执行例
    //public class MyJob : IJob
    //{
    //    public void Execute(IJobExecutionContext context)
    //    {
    //        Console.WriteLine("executed..." + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
    //    }
    //} 
    #endregion
}

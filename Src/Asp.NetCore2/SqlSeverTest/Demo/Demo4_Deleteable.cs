﻿using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrmTest
{
    public class Demo4_Deleteable
    {
        public static void Init()
        {
            Console.WriteLine("");
            Console.WriteLine("#### Deleteable Start ####");

            SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
            {
                DbType = DbType.SqlServer,
                ConnectionString = Config.ConnectionString,
                InitKeyType = InitKeyType.Attribute,
                IsAutoCloseConnection = true,
                AopEvents = new AopEvents
                {
                    OnLogExecuting = (sql, p) =>
                    {
                        Console.WriteLine(sql);
                        Console.WriteLine(string.Join(",", p?.Select(it => it.ParameterName + ":" + it.Value)));
                    }
                }
            });
            //by entity
            db.Deleteable<Order>().Where(new Order() { Id = 1111 }).ExecuteCommand();

            //by primary key
            db.Deleteable<Order>().In(1111).ExecuteCommand();

            //by primary key array
            db.Deleteable<Order>().In(new int[] { 1111, 2222 }).ExecuteCommand();

            //by expression
            db.Deleteable<Order>().Where(it => it.Id == 11111).ExecuteCommand();

            //logic delete
            db.CodeFirst.InitTables<LogicDeleteTezt>();
            db.Deleteable<LogicDeleteTezt>().Where(it=>it.Name=="a").IsLogic().ExecuteCommand();

            db.Deleteable<Order>().WhereColumns(db.Queryable<Order>().Take(2).ToList(), it => new { it.Id, it.Name }).ExecuteCommand();
            Console.WriteLine("#### Deleteable End ####");

        }

        public class LogicDeleteTezt 
        {
            public string Name { get; set; }
            [SugarColumn(IsPrimaryKey =true)]
            public int Id { get; set; }
            public bool IsDeleted { get; set; }
        }
    }
}

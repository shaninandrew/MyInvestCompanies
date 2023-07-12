using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query;
using Npgsql;
using NpgsqlTypes;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Xml.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
//using 

namespace MyInvestCompanies.модель_данных
{
    /// <summary>
    /// Струкутра нужной базы данных
    /// </summary>
    public class Database_structure:DbContext
    {
        public DbSet<Company> Companies { set; get; }
        public DbSet<Link> Links { set; get; }
        public DbSet<Owner> Owners { set; get; }
        public DbSet<Document> Documents     { set; get; }
        public DbSet<Invest> Investitions { set; get; }
        public DbSet<Capital> Capitals { set; get; }
        public DbSet<HolderReestr> HolderReestrs { set; get; }
        public DbSet<Deal> Deals { set; get; }
        public DbSet<Acquisition> Acquisitions { set; get; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //Считываем конфигурацию
            var doc = JsonDocument.Parse(System.IO.File.ReadAllText("Properties\\config-db.json"));

            bool at_home =! true;

            string connection_string = "";
            if (!at_home) 
            { 
                connection_string = doc.RootElement.GetProperty("pg").ToString();
                optionsBuilder.UseNpgsql(connection_string);
            }
            else
            { 
                connection_string = doc.RootElement.GetProperty("mssql").ToString();
                optionsBuilder.UseSqlServer(connection_string);
            }

            //настройка подключения
           // optionsBuilder.EnableServiceProviderCaching(false);
            

            base.OnConfiguring(optionsBuilder);
        }


        public Database_structure()
        {
            Database.EnsureCreated();
        }

    }//class


    /// <summary>
    /// Проектная компания
    /// </summary>
    public class Company
    {
        [Key]
        public string Id  { get; set; }
        public string Name { get; set; }
        public DateTime Founded { get; set; }
        public string Inn { get; set; }
        public string KPP { get; set; }
        public string Address { get; set; }
        public string Fact_Address { get; set; }

        public string Description { get; set; }

        public Company()
        {
            Id = Guid.NewGuid().ToString();
            Inn = "000000000";
            Name = "Имя компании";
            KPP = "000000000";
            Address = "г. Ая ул. Ленина д. 00";
            Fact_Address = "г. Ая ул. Ленина д. 00";
            Description = "Описание";
        }

    }

    /// <summary>
    /// Класс персоналий
    /// </summary>
    public class Owner
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Inn { get; set; }
        public string KPP { get; set; }
        public string Address { get; set; }
        /// <summary>
        /// Юр лицо - правда /  Физлицо- ложь
        /// </summary>
        public bool Org { get; set ; }
        public string? Email { get; set; }
        public string PhoneNumber { get; set; }

        public Owner()
        {
            Id      = Guid.NewGuid().ToString();
            Name    = "Иван Иванович";
            Inn     = "0000000";
            KPP = "000000";
            Address = "г. ул.  д. 00";
            PhoneNumber = "+790900112233";
            Email = "name@server.ru";
        }

    }//Owner


    /// <summary>
    /// Документ
    /// </summary>
    public class Document
    {
    // id
        [Key]
        public string Id { get; set; }
        // Номер документа
        public string Num { get; set; }
      
        
        // Дата утверждения
        public DateTime Date { get; set; }
        
        //имя
        public string Name { get; set; }
        
        // путь хранения
        public string Url { get; set; }


        public Document()
        { 
            Id = Guid.NewGuid().ToString();
            Num = "000/2023";
            Name = "документ о ";
            Url = "";
        }
    }

    /// <summary>
    ///  Объем инвестиций, произведенных в проектную компанию с указанием даты,  
    ///  способа и источника инвестиций, а также документов, послуживших основанием для 
    ///  инвестиций и документов подтверждающих совершение инвестиций;
    ///  Документы идут через Link +
    /// </summary>

    public class Invest
    {
        [Key]
        public string Id { get; set; }

        float Sum { set; get; }

        /// <summary>
        /// Дата инвестирования
        /// </summary>
        DateTime Date { get; set; }

        /// <summary>
        /// Способ инвестирования
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// Источник финансирования
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// Жесткая связка с компанией
        /// </summary>
        public string Id_company { get; set; }


        public Invest()
        {
            Id = Guid.NewGuid().ToString();
            Sum = 0;
            Method = "IPO,POS(покупка доли)";
            Source = "деньги инвесторов|деньги государства";
        }

        public Invest (string id_company)
        {
            Id = Guid.NewGuid().ToString();
            Sum = 0;
            Method = "IPO,POS(покупка доли)";
            Source = "деньги инвесторов|деньги государства";
            Id_company = id_company;
        }
    }

    /// <summary>
    /// Информация о размере уставного капитала проектной компании
    /// </summary>
    public class Capital
    {
        [Key]
        public string Id { get; set; }
        public string? Id_company { get; set; }
        public float? Sum { set; get; }
        public DateTime? Date { get; set; }

        public Capital()
        {
            Id = Guid.NewGuid().ToString();
        }
        /// <summary>
        /// Создает строку по компании
        /// </summary>
        /// <param name="id_company"></param>
        public Capital(string id_company)
        {
            Id = Guid.NewGuid().ToString();
            Id_company = id_company;
            Sum = 10000;
            Date = null;
        }
    }
 

    /// <summary>
    /// Глобальная таблица связка между таблицами
    /// </summary>
    public class Link
    {
        [Key]
        public string Id { get; set; }
        public string Parent_Id { get; set; }
        public string Child_Id { get; set; }
        public string? Parent_table { get; set; }
        public string? Child_table { get; set; }

        public Link()
        {
            Id = Guid.NewGuid().ToString();
            Parent_Id = "";
            Child_Id = "";
        }

        public Link(string id_parent, string id_child)
        {
            Id = Guid.NewGuid().ToString();
            Parent_Id = id_parent;
            Child_Id = id_child;    
        }

    }//Link


    /// <summary>
    /// Таблица владельцев компаний - могут быть ю/л, так и ф/л
    /// Информация об участниках проектной компании: ФИО или наименование, размер доли и номинальная стоимость доли, контактные данные;
    /// </summary>
    public class HolderReestr
    {
        [Key]
        public string Id { get; set; }
        /// <summary>
        /// id компании
        /// </summary>
        public string Company_Id { get; set; }
        /// <summary>
        /// id Владельца
        /// </summary>
        public string Owner_Id { get; set; }

        /// <summary>
        /// Доля владельца
        /// </summary>
        public float Share { get; set; }
        /// <summary>
        /// Дата покупки доли
        /// </summary>
        public DateTime Date_buy_share { get; set; }
        /// <summary>
        /// Стоимость доли
        /// </summary>
        public float Cost { get; set; }

        public HolderReestr()
        {
            Id = Guid.NewGuid().ToString();
            Cost = 0;
            
        }

    }//


    /// <summary>
    /// Информация об основных условиях сделки (ОУС) проектной компании 
    /// с указанием реквизитов документа об одобрении ОУС и даты одобрения ОУС;
    ///</summary>

    public class Deal
    {
        [Key]
        public string Id { get; set; }

        public string Id_company { get; set; }

        /// <summary>
        /// Дата одобрение сделки
        /// </summary>
        public DateTime? Date { get; set; }
        
        /// <summary>
        /// Описание сделки
        /// </summary>
        public string Description { get; set; }

        public Deal()
        { 
            Id = Guid.NewGuid().ToString();
            Description = "описание сделки";
        
        }

        public Deal(string id_company)
        {
            Id = Guid.NewGuid().ToString();
            Id_company = id_company;
            Description = "описание сделки";

        }
    }//Deals


    /// <summary>
    /// Основания для приобретения
    /// </summary>
    public   class Acquisition
    {
        [Key]
        public string Id { get; set; }

        public string Id_company { get; set; }

      
        /// <summary>
        /// Описание сделки
        /// </summary>
        public string Description { get; set; }

        public Acquisition()
        {
            Id = Guid.NewGuid().ToString();
            Description = "описание сделки";

        }

        public Acquisition(string id_company)
        {
            Id = Guid.NewGuid().ToString();
            Id_company = id_company;
            Description = "описание сделки";
        }
    }//Acquisition

 }//class

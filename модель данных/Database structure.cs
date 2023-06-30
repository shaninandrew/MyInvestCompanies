using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query;
using Npgsql;
using NpgsqlTypes;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Nodes;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           //Считываем конфигурацию
            var doc = JsonDocument.Parse(System.IO.File.ReadAllText("Properties\\config-db.json"));
            string connection_string = doc.RootElement.GetProperty("Connection string").ToString();
            
            //настройка подключения
            optionsBuilder.UseNpgsql(connection_string);
  
            base.OnConfiguring(optionsBuilder);
        }


        public Database_structure()
        {
            Database.EnsureCreated();
        }

    }

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
        
        public Company()
        {
            Id = Guid.NewGuid().ToString();
            Inn = "000000000";
            Name = "Имя компании";
            KPP = "000000000";
            Address = "г. Ая ул. Ленина д. 00";
            Fact_Address = "г. Ая ул. Ленина д. 00";

        }

    }

    /// <summary>
    /// Класс персоналий
    /// </summary>
    public class Owner
    {
        [Key]
        public string Id { get; set; }
        public string Name { get { return Name; } set { if (value.ToLower().IndexOf("ооо ") > -1) { Org = true; } } }
        public string Inn { get; set; }
        public string KPP { get; set; }
        public string Address { get; set; }
        /// <summary>
        /// Юр лицо - правда /  Физлицо- ложь
        /// </summary>
        public bool Org { get; set ; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public Owner()
        {
            Id      = Guid.NewGuid().ToString();
            Name    = "Иван Иванович";
            Inn     = "0000000";

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

    }//Link


}

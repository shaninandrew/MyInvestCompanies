using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MyInvestCompanies.модель_данных
{
    public class Functions
    {

        /// <summary>
        /// Переключение страниц (чтобы не нагружать клиента)
        /// </summary>
        /// <param name="Company_id"></param>
        /// <returns></returns>
        public EventCallback Add_new_holder(string Company_id)
        {
            Database_structure db = new Database_structure();

            Owner o = new Owner();
            db.Owners.Add(o);

            HolderReestr h = new HolderReestr();
            h.Company_Id = Company_id;
            h.Owner_Id = o.Id;
            db.HolderReestrs.Add(h);

            db.SaveChanges();
            db.Dispose();

            return (new EventCallback());
        }

        /// <summary>
        /// Добавляет существующего владельца
        /// </summary>
        /// <param name="Company_id"></param>
        /// <returns></returns>
        public  EventCallback Add_exist_holder(string Company_id)
        {

            return (new EventCallback());
        }

        /// <summary>
        /// Удаление существующего владельца (связки владельца + компания)
        /// </summary>
        /// <param name="Company_id"></param>
        /// <param name="Selected_Holder"></param>
        /// <returns></returns>
        public  EventCallback Remove_exist_holder(string Company_id, string Selected_Holder)
        {

            Database_structure db = new Database_structure();
            // ..чистим таблицу
            db.HolderReestrs.RemoveRange(db.HolderReestrs.Where<HolderReestr>(h => h.Company_Id == Company_id).Where<HolderReestr>(w => w.Owner_Id == Selected_Holder).ToArray());

            //чистим связи
            db.Links.RemoveRange(db.Links.Where<Link>(l => l.Parent_Id == Company_id).Where<Link>(w => w.Child_Id == Selected_Holder).ToArray());
            db.SaveChanges();
            db.Dispose();
            return (new EventCallback());
        }




        /// <summary>
        /// Удаление компании
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dbx"></param>
        /// <returns></returns>
       public EventCallback Delete_company(string id, Database_structure? dbx)
        {

            Database_structure db = null;

            if (dbx != null)
            { db = dbx; }
            else
            { db = new Database_structure(); }
            try
            {
                Company c = db.Companies.Where<Company>(i => i.Id == id).Single<Company>();
                Console.Write($"Удаляем запись {c.Name} {c.Id}");
                db.Companies.Remove(c);
                db.SaveChanges();
                Console.WriteLine($" успешно...");
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Удаление id= {id} " + ex.Message);
            }

            //чистим память если автономно
            if (dbx == null)
            { db.Dispose(); }


            return (new EventCallback());
        }

        /// <summary>
        /// ДОбавление новыой компании
        /// </summary>
        /// <param name="dbx"></param>
        /// <returns></returns>
       public EventCallback Add_company(Database_structure? dbx)
        {

            Database_structure db = null;

            if (dbx != null)
            { db = dbx; }
            else
            { db = new Database_structure(); }


            Company c = new Company();
            c.Name = "???";
            db.Companies.Add(c);
            db.SaveChanges();

            //чистим память если автономно
            if (dbx == null)
            { db.Dispose(); }

            return (new EventCallback());

        }


        /// <summary>
        /// Установка значений в таблицах
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <param name="e"></param>
        /// <param name="dbx"></param>
        /// <param name="table_name"></param>
        /// <returns></returns>
         public EventCallback Set_value(string name, string id, string value, ChangeEventArgs? e, Database_structure? dbx, string table_name)// (object sender, ChangeEventArgs e)//(string name, string id)
        {
            Database_structure db = null;

            if (dbx != null)
            { db = dbx; }
            else
            { db = new Database_structure(); }

            if (table_name == "Companies")
            {
                Console.WriteLine($"Установка значения {name} id={id}: {value}");

                var t = db.Companies.Where<Company>(i => i.Id == id).Single<Company>();

                Console.WriteLine($"Установка значения - {t.Id} {t.Name}: {t.Description} {t.KPP} {t.Address} {t.Fact_Address} {t.Founded} {t.Inn}");

                try
                {
                    if (name == "Name")
                    { t.Name = e.Value.ToString(); }
                    if (name == "Founded")
                    { t.Founded = DateTime.Parse(e.Value.ToString()).ToUniversalTime(); }
                    if (name == "Address")
                    { t.Address = e.Value.ToString(); }
                    if (name == "Fact_Address")
                    { t.Fact_Address = e.Value.ToString(); }
                    if (name == "Description")
                    { t.Description = e.Value.ToString(); }
                    if (name == "Inn")
                    { t.Inn = e.Value.ToString(); }
                    if (name == "KPP")
                    { t.KPP = e.Value.ToString(); }

                    db.Update<Company>(t);
                    db.SaveChanges();


                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.InnerException.Message);
                }
            } // Comapnies



            if (table_name == "Capitals")
            {
                Console.WriteLine($"Установка значения {name} id={id}: {value}");

                var t = db.Capitals.Where<Capital>(i => i.Id == id).Single<Capital>();

                Console.WriteLine($"Установка значения -> ");

                try
                {

                    if (name == "Date")
                    { t.Date = DateTime.Parse(e.Value.ToString()).ToUniversalTime(); }
                    if (name == "Sum")
                    { t.Sum = float.Parse(e.Value.ToString()); }

                    db.Update<Capital>(t);
                    db.SaveChanges();


                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.InnerException.Message);
                }
            } // Capitals


            if (table_name == "Investitions")
            {
                Console.WriteLine($"Установка значения {name} id={id}: {value}");

                var t = db.Investitions.Where<Invest>(i => i.Id == id).Single<Invest>();

                Console.WriteLine($"Установка значения -> ");

                try
                {

                    if (name == "Method")
                    { t.Method = e.Value.ToString(); }
                    if (name == "Source")
                    { t.Source = e.Value.ToString(); }

                    db.Update<Invest>(t);
                    db.SaveChanges();


                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.InnerException.Message);
                }
            } // Capitals


            if (table_name == "Capitals")
            {
                Console.WriteLine($"Установка значения {name} id={id}: {value}");

                var t = db.Capitals.Where<Capital>(i => i.Id == id).Single<Capital>();

                Console.WriteLine($"Установка значения -> ");

                try
                {

                    if (name == "Date")
                    { t.Date = DateTime.Parse(e.Value.ToString()).ToUniversalTime(); }
                    if (name == "Sum")
                    { t.Sum = float.Parse(e.Value.ToString()); }

                    db.Update<Capital>(t);
                    db.SaveChanges();


                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.InnerException.Message);
                }
            } // Capitals


            if (table_name == "Deals")
            {
                Console.WriteLine($"Установка значения {name} id={id}: {value}");

                var t = db.Deals.Where<Deal>(i => i.Id == id).Single();

                Console.WriteLine($"Установка значения -> ");

                try
                {

                    if (name == "Date")
                    { t.Date = DateTime.Parse(e.Value.ToString()).ToUniversalTime(); }


                    if (name == "Description")
                    { t.Description = e.Value.ToString(); }

                    db.Update<Deal>(t);
                    db.SaveChanges();


                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.InnerException.Message);
                }
            } // Сделки



            if (table_name == "Acquisitions")
            {
                Console.WriteLine($"Установка значения {name} id={id}: {value}");
                var t = db.Acquisitions.Where<Acquisition>(i => i.Id == id).Single();
                                 
                Console.WriteLine($"Установка значения Acquisitions -> ");

                try
                {

                    if (name == "Description")
                    { t.Description = e.Value.ToString(); }

                    db.Update<Acquisition>(t);
                    db.SaveChanges();


                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.InnerException.Message);
                }
            } // Описание


            if (table_name == "Documents")
            {
                Console.WriteLine($"Установка значения {name} id={id}: {value}");
                var t = db.Documents.Where<Document>(i => i.Id == id).Single();
                Console.WriteLine($"Установка значения -> ");

                try
                {
                    if (name == "Date")
                    { t.Date = DateTime.Parse(e.Value.ToString()).ToUniversalTime(); }

                    if (name == "Name")
                    { t.Name = e.Value.ToString(); }

                    if (name == "Num")
                    { t.Num = e.Value.ToString(); }

                    if (name == "Url")
                    { t.Url = e.Value.ToString(); }

                    db.Update<Document>(t);
                    db.SaveChanges();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.InnerException.Message);
                }
            } // Документы



            if (table_name == "HolderReestrs")
            {
                Console.WriteLine($"Установка значения {name} id={id}: {value}");
                var t = db.HolderReestrs.Where<HolderReestr>(i => i.Id == id).Single();
                Console.WriteLine($"Установка значения -> ");

                try
                {
                    /*
                    * HolderReestr h = new HolderReestr();
                   h.Cost
                   h.
                    */


                    if (name == "Date_buy_share")
                    { t.Date_buy_share = DateTime.Parse(e.Value.ToString()).ToUniversalTime(); }


                    if (name == "Cost")
                    { t.Cost = float.Parse(e.Value.ToString()); }

                    if (name == "Share")
                    { t.Share = float.Parse(e.Value.ToString()); }

                    db.Update<HolderReestr>(t);
                    db.SaveChanges();


                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.InnerException.Message);
                }
            } // Реестр владельцев


            if (table_name == "Owners")
            {
                Console.WriteLine($"Установка значения {name} id={id}: {value}");
                var t = db.Owners.Where<Owner>(i => i.Id == id).Single();
                Console.WriteLine($"Установка значения -> ");

                try
                {
                    /*
                     * HolderReestr h = new HolderReestr();
                    h.Cost
                    h.
                    */



                    if (name == "Inn")
                    { t.Inn = e.Value.ToString(); }


                    if (name == "Email")
                    { t.Email = e.Value.ToString(); }


                    if (name == "Org")
                    {
                        t.Org = false;
                        t.Org = (e.Value.ToString() == "1") || (e.Value.ToString() == "TRUE") || (e.Value.ToString() == "True");

                    }

                    if (name == "PhoneNumber")
                    { t.PhoneNumber = e.Value.ToString(); }

                    if (name == "Address")
                    { t.Address = e.Value.ToString(); }

                    if (name == "KPP")
                    { t.KPP = e.Value.ToString(); }

                    if (name == "Name")
                    { t.Name = e.Value.ToString(); }


                    db.Update<Owner>(t);
                    db.SaveChanges();


                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.InnerException.Message);
                }
            } // Реестр владельцев



            //чистим память если автономно
            if (dbx == null)
            { db.Dispose(); }
            EventCallback evc = new EventCallback();
            return (evc);
         }



        /// <summary>
        /// Добавляет сведения об уставном капитале
        /// </summary>
        /// <param name="company_id"></param>
        public async void AddCapital(string company_id)
        {
            Database_structure ds = new Database_structure();

            Capital c_new = new Capital();
            c_new.Id_company = company_id;
            ds.Capitals.Add(c_new);

            //добавление связки
            Link lnk = new Link(company_id, c_new.Id); ;
            lnk.Parent_table = "Companies";
            lnk.Child_table = "Capitals";
            ds.Links.Add(lnk);

            ds.SaveChanges();
            ds.Dispose();
        }


        /// <summary>
        /// УДалить показатели уставного капитала
        /// </summary>
        /// <param name="capital_id"></param>
        public async void Delete_capital(string capital_id)
        {
            Database_structure ds = new Database_structure();
            ds.Capitals.Remove(ds.Capitals.Where(i => i.Id == capital_id).Single());
            ds.Links.RemoveRange(ds.Links.Where<Link>(i => i.Parent_Id == capital_id).ToArray());
            ds.SaveChanges();
            ds.Dispose();
        }

        /// <summary>
        /// Добавляет инвестии в базу
        /// </summary>
        /// <param name="company_id"></param>

        public async void AddInvestitions(string company_id)
        {
            Database_structure ds = new Database_structure();

            Invest i_new = new Invest();
            i_new.Id_company = company_id;
            ds.Investitions.Add(i_new);

            Link lnk = new Link(company_id, i_new.Id); ;
            lnk.Parent_table = "Companies";
            lnk.Child_table = "Investitions";

            ds.Links.Add(lnk);

            ds.SaveChanges();
            ds.Dispose();

        }


        ///   <summary>
        ///   Удаляет инвестицию из базы данных
        /// </summary>
        /// <param name="invest_id"></param>
        
        public async void Delete_Investitions(string invest_id)
        {
            Database_structure ds = new Database_structure();
            ds.Investitions.Remove(ds.Investitions.Where(i => i.Id == invest_id).Single());
            ds.Links.RemoveRange(ds.Links.Where<Link>(i => i.Parent_Id == invest_id).ToArray());


            ds.SaveChanges();
            ds.Dispose();
        }


        /// <summary>
        ///  Добавляет сделку
        /// </summary>
        /// <param name="company_id"></param>
        public async void Add_Deal(string company_id)
        {
            Database_structure ds = new Database_structure();

            Deal i_new = new Deal();
            i_new.Id_company = company_id;

            ds.Deals.Add(i_new);

            Link lnk = new Link(company_id, i_new.Id); ;
            lnk.Parent_table = "Companies";
            lnk.Child_table = "Deals";

            ds.Links.Add(lnk);

            ds.SaveChanges();
            ds.Dispose();
        }
         /// <summary>
         /// Добавить описание сделки
         /// </summary>
         /// <param name="_id"></param>
        public async void Delete_Deal(string _id)
        {
            Database_structure ds = new Database_structure();
            ds.Deals.Remove(ds.Deals.Where(i => i.Id == _id).Single());
            ds.Links.RemoveRange(ds.Links.Where<Link>(i => i.Parent_Id == _id).ToArray());
            ds.SaveChanges();
            ds.Dispose();
        }


        /// <summary>
        /// Добавляет документ привязанные к обоснованию
        /// </summary>
        /// <param name="acq_id"></param>
        public async void Add_Document_for_Acq(string acq_id)
        {
            Database_structure ds = new Database_structure();

            Console.WriteLine($" * Установка нового документа ...для {acq_id}");
            Document i_new = new Document();
            i_new.Name = "Документ обоснование";
            i_new.Num = "111/000-обсн";

            string Child_id = i_new.Id;
            ds.Documents.Add(i_new);

            Link lnk = new Link(acq_id, Child_id); ;
            lnk.Child_table = "Acquisitions";
            lnk.Parent_table = "Companies";
            


            ds.Links.Add(lnk);
            ds.SaveChanges();
            await ds.DisposeAsync();
        }

       /// <summary>
       ///  Удаление обоснований
       /// </summary>
       /// <param name="_id"></param>
        public async void Delete_Acq(string _id)
        {
            Database_structure ds = new Database_structure();
            ds.Acquisitions.Remove(ds.Acquisitions.Where(i => i.Id == _id).Single());
            ds.Links.RemoveRange(ds.Links.Where<Link>(i => i.Parent_Id == _id).ToArray());
            ds.SaveChanges();
            ds.Dispose();
        }


        /// <summary>
        /// Добавляет документ привязанные к сделке
        /// </summary>
        /// <param name="deal_id"></param>
        public async void Add_Document_for_Deal (string deal_id)
        {
            Database_structure ds = new Database_structure();

            Document i_new = new Document();
            string Child_id = i_new.Id;
            ds.Documents.Add(i_new);

            Link lnk = new Link(deal_id, Child_id); ;
            lnk.Parent_table = "Deals";
            ds.Links.Add(lnk);

            ds.SaveChanges();
            await ds.DisposeAsync();
        }

        /// <summary>
        /// Добавляет документ привязанные к инвестиционной сделке 
        /// </summary>
        /// <param name="deal_id"></param>
        public async void Add_Document_for_Invest(string invest_id)
        {
            Database_structure ds = new Database_structure();

            Document i_new = new Document();
            i_new.Name = "Документ об инвестиции";
            i_new.Num = "111/000-инвест";

            string Child_id = i_new.Id;
            ds.Documents.Add(i_new);
            Link lnk = new Link(invest_id, Child_id); ;
            
            lnk.Parent_table = "Investitions";
            ds.Links.Add(lnk);

            ds.SaveChanges();
            await ds.DisposeAsync();
        }


        
         /// <summary>
         /// Добавляет обоснование в таблицу
         /// </summary>
         /// <param name="Company_id"></param>
        public async void Add_Acq(string Company_id)
        {
            Database_structure ds = new Database_structure();

            Acquisition i_new = new Acquisition();
            i_new.Id_company = Company_id;

            string Child_id = i_new.Id;
            ds.Acquisitions.Add(i_new);
            Link lnk = new Link(Company_id, Child_id); ;

            lnk.Parent_table = "Acquisition";
            ds.Links.Add(lnk);

            ds.SaveChanges();
            await ds.DisposeAsync();
        }



        ///  <summary>
        /// Добавляет документ
        /// </summary>
        /// <param name="company_id"></param>
        public async void Add_Document(string company_id)
        {
            Database_structure ds = new Database_structure();

            Document i_new = new Document();

            i_new.Name = "Документ ";
            i_new.Num = "111/000";

            string Child_id = i_new.Id;
            ds.Documents.Add(i_new);

            Link lnk = new Link(company_id, Child_id); ;
            lnk.Parent_table = "Companies";
            ds.Links.Add(lnk);

            ds.SaveChanges();
            ds.Dispose();
        }

        /// <summary>
        /// УДалить документ
        /// </summary>
        /// <param name="_id"></param>
        public async void Delete_Document(string _id)
        {
            try
            {
                Database_structure ds = new Database_structure();
                ds.Documents.Remove(ds.Documents.Where(i => i.Id == _id).Single());
                ds.Links.RemoveRange(ds.Links.Where<Link>(i => i.Parent_Id == _id).ToArray());
                ds.SaveChanges();
                ds.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine( ex.Message);
            }
        }




       


        /// <summary>
        /// Черный ящик для работы с базой данных
        /// </summary>
        public Functions()
        {
                GC.Collect();
        
        }



    } //Functions

}

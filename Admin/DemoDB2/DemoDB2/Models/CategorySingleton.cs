using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DemoDB2.Models;

namespace DemoDB2.Models
{
    public sealed class CategorySingleton
    {
        DBGiaDungEntities db = new DBGiaDungEntities();
        public static CategorySingleton Instance { get; } = new CategorySingleton();
        public List<CATALOG> listCatalog { get; } = new List<CATALOG>();
        private CategorySingleton() { }
        
        public void Init(DBGiaDungEntities db)
        {

            if(listCatalog.Count == 0) 
            {
                var items = db.CATALOGs.ToList();
                foreach (var item in items)
                {
                    listCatalog.Add(item);
                }
            }
        }
        public void Update(DBGiaDungEntities db)
        {
            listCatalog.Clear();
            Init(db);
        }
    }
}
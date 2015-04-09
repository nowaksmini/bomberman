using AutoMapper;
using BomberManModel;
using BomberManModel.Entities;
using BomberManViewModel.DataAccessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BomberManViewModel
{
    public class DataManager
    {
        static bool init = false;
        
        static public BomberManContext DataBaseContext;

        static public void InitContext(BomberManContext Context = null)
        {
            if (Context == null) 
                DataBaseContext = new BomberManContext();
            else 
                DataBaseContext = Context;
            if (init) 
                return; 
            init = true;
            CreateElemts();
            CreateOponents();
           // Mapper.CreateMap<User, UserDAO>().ConvertUsing<UserEntityToUserDAOConverter>();
        }

        static void CreateElemts() 
        {
            var elements = new List<User>{
                               new User{Name = "SYLWIA", Password = "a", ID= 1 }
                           };
            elements.ForEach(s => DataBaseContext.Users.Add(s));
            DataBaseContext.SaveChanges();
        }

        static void CreateOponents() { }

        static void CreateMapers() { }

    }
}

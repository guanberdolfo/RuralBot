using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using RuralBot.Models.Entity;

namespace RuralBot.Services
{
    [Serializable]
    public class RuralServices
    {
        //Obtener Lugares de la DB
        public static List<Lugares> GetLugares()
        {
            List<Lugares> lugaresList;
            using (RuralDB ruralDb = new RuralDB())
            {
                lugaresList = ruralDb.Lugares.ToList();
                return lugaresList;
            }
        }

        public static Lugares GetLugar(int idLugar)
        {
            Lugares lugar;
            using (RuralDB ruralDb = new RuralDB())
            {
                lugar = ruralDb.Lugares.Find(idLugar);
                return lugar;
            }

        }

        //Obtener Casas de la DB
        public static List<Casas> GetCasas()
        {
            List<Casas> casasList;
            using (RuralDB ruralDb = new RuralDB())
            {
                casasList = ruralDb.Casas.ToList();
                return casasList;
            }

        }

        //Obtener Casas del Lugar Seleccionado
        public static List<Casas> GetCasasLugar(int lugarId)
        {
            IQueryable<Casas> casasList;
            using (RuralDB ruralDb = new RuralDB())
            {
                casasList = from c in ruralDb.Casas
                            where c.lugarId.Equals(lugarId)
                            select c;
                return casasList.ToList();
            }
        }

        public static Casas GetCasa(int casaId)
        {
            Casas casa;
            using (RuralDB ruralDb = new RuralDB())
            {
                casa = ruralDb.Casas.Find(casaId);
                return casa;
            }

        }

        public static Users GetUser(string login)
        {
            
            using (RuralDB ruralDb = new RuralDB())
            {
                var users = (from u in ruralDb.Users
                        where u.loginId.Equals(login)
                        select u).SingleOrDefault();

                return users;
            }

        }

        //Obtener Login
        public static bool Login(string user, string password)
        {
            using (RuralDB ruralDb = new RuralDB())
            {
                string pencryt = PasswordEncrypt(password);

                var checkLogin = from u in ruralDb.Users
                                 where u.loginId.Equals(user)
                                 select u;
                var checkPassword = from us in ruralDb.Users
                                    where us.loginPassword.Equals(pencryt)
                                    select us;

                if (!checkLogin.Any() && !checkPassword.Any())
                {
                    return false;
                }
                return true;
            }
        }

        public static void Registro(Users user)
        {
            using (RuralDB ruralDb = new RuralDB())
            {
                user.loginPassword = PasswordEncrypt(user.loginPassword);

                ruralDb.Users.Add(user);
                ruralDb.SaveChanges();
            }
        }

        public static void Alquiler(Ventas venta)
        {
            using (RuralDB ruralDb = new RuralDB())
            {
                ruralDb.Ventas.Add(venta);
                ruralDb.SaveChanges();
            }
        }

        public static string PasswordEncrypt(string password)
        {
            byte[] data = System.Text.Encoding.ASCII.GetBytes(password);
            data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
            return System.Text.Encoding.ASCII.GetString(data);
        }
    }

    //using (var context = new RuralBotDBEntities())
    //{
    //el find devuelve un null cuando no lo encuentra , viene bastante bien para cuando tenemos que hacer el login
    //Esto funciona pasa insertar en cascada, te inserta el usuario guardando la Tabla de Ventas.
    //    UsersTable nuevo = new UsersTable()
    //    {
    //        userFullName = "Hector Garcia Delgado",
    //        userMail = "myemail@gmail.com",
    //        userNationality = "Spain",
    //        userPassword = "1234",
    //        VentasTable = new List<VentasTable>()
    //    };

    //   VentasTable ventas = new VentasTable()
    //   {
    //       UsersTable = nuevo,
    //       CasasTable = context.CasasTable.Find(1),
    //       fechaEntrada = new DateTime(2017,05,28),
    //       fechaSalida = new DateTime(2017,05,30),
    //       inquilinos = 3
    //   };

    //    context.VentasTable.Add(ventas);
    //    context.SaveChanges();
    //}

    //prueba de encrypt password

    //string password = "1234";
    //byte[] data = System.Text.Encoding.ASCII.GetBytes(password);
    //data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
    //String hash = System.Text.Encoding.ASCII.GetString(data);

    //string registro = "1234";
    //byte[] data2 = System.Text.Encoding.ASCII.GetBytes(registro);
    //data2 = new System.Security.Cryptography.SHA256Managed().ComputeHash(data2);
    //String hash2 = System.Text.Encoding.ASCII.GetString(data2);
    //System.Diagnostics.Debug.Write(hash2);

    //if (hash2.Equals(hash))
    //{
    //    System.Diagnostics.Debug.Write("Las contraseñas son correctas,");
    //}
    //else
    //{
    //    System.Diagnostics.Debug.WriteLine("Usted nos ha engañado");
    //}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Models;
using System.Data.Entity;

namespace WebApi.Controllers
{
    public class DepartmentController : ApiController
    {
        /*
         * @description: Metodo para obtener todos los registros de la tabla departments
         * @return : Retorna una lista de todos los departamentos activos
         */ 
        [Route("department/all")]
        public Departments[] GetAll()
        {   
            IQueryable<Departments> list;
            using (var db = new DbEntity())
            {
                // Obtenemos los departamentos con estado activo o igual a 1
                list = db.Departments.Where(d => d.State == 1);
                return list.ToArray();
            }
        }

        /*
         * @description: Metodo para almacenar nuevo registro
         * @return : Retorna una lista de todos los departamentos incluyendo 
         *           el nuevo elemento agregado
         */
        [Route("department/store")]
        public Departments[] PostStore([FromBody] Departments model)
        {
            Departments[] list = null;
            using(var db = new DbEntity())
            {
                //Creamos un nuevo objeto con los datos para almacenar 
                Departments oDepartment = new Departments();
                oDepartment.DepartmentName = model.DepartmentName;
                oDepartment.State = 1;

                //Guardamos el registro mediante el objeto creado
                db.Departments.Add(oDepartment);
                db.SaveChanges();
            }

            //Obtengo y retorno lista de departamentos ya con el registro nuevo agregado
            list = GetAll();
            return list;
        }

        /*
         * @description: Metodo para actualizar un registro
         * @params: Json con los datos para actualizar 
         * @return : Retorna una lista de todos los departamentos incluyendo 
         *           el registro modificado
         */
        [Route("department/update")]
        public Departments[] PutUpadate([FromBody] Departments model)
        {
            using (var db = new DbEntity())
            {
                //Obtenemos el registro y posterior modificamos el mismo
                var oDepartment = db.Departments.Find(model.DepartmentID);
                oDepartment.DepartmentName = model.DepartmentName;

                //Se guarda el registro con los cambios hechos
                db.Entry(oDepartment).State = EntityState.Modified;
                db.SaveChanges();
            }

            //Obtengo la lista con los cambios hechos
            Departments[] list = GetAll();
            return list;
        }

        /*
        * @description: Metodo para obtener registro por ID
        * @params: Integer Value ID 
        * @return : Retorna el registro Departament con el ID del parametro
        */
        [Route("department/getby")]
        public Departments GetById(int id)
        {
            using (var db = new DbEntity())
            {
                //Obtenemos y retornamos el registro mediante el id
                var oDepartment = db.Departments.Find(id);
                return oDepartment;
            }
        }

        /*
        * @description: Metodo para realizar el borrado logico de un registro
        * @params: Integer Value ID
        * @return :  Retorna el registro Departament con el ID del parametro 
        */
        [Route("department/delete")]
        public Departments PutLogic(int id)
        {
            using (var db = new DbEntity())
            {
                //Obtenemos el registro y modificamos su state a inactivo con un cero
                var oDepartment = db.Departments.Find(id);
                oDepartment.State = 0;

                //Guardamos los cambios 
                db.Entry(oDepartment).State = EntityState.Modified;
                db.SaveChanges();

                //Retornamos el registro con el state inactivo
                return oDepartment;
            }
        }


    }
}

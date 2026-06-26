using DAL;
using Servicios;
using System;
using System.Collections.Generic;

namespace BLL
{
    public class PermisoSimpleBLL
    {
        private readonly PermisoSimpleDAL _permisoSimpleDAL;

        public PermisoSimpleBLL()
        {
            _permisoSimpleDAL = new PermisoSimpleDAL();
        }

        //lista todos los permisos simples
        public List<PermisoSimple> ListarTodos()
        {
            return _permisoSimpleDAL.ListarTodos();
        }


        // Obtiene un permiso simple por su ID
        public PermisoSimple ObtenerPorId(int idPermisoSimple)
        {
            return _permisoSimpleDAL.ObtenerPorId(idPermisoSimple);
        }
    }
}

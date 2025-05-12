using System;
using System.Collections.Generic;
using System.Linq;
using TiendaElectronica;

namespace ClnTiendaElectronica
{
    public class MarcaCln
    {
        public static int insertar(Marca marca)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                context.Marca.Add(marca);
                context.SaveChanges();
                return marca.id;
            }
        }

        public static int actualizar(Marca marca)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                var existente = context.Marca.Find(marca.id);
                if (existente != null)
                {
                    existente.nombre = marca.nombre;
                    existente.pais = marca.pais;
                }
                return context.SaveChanges();
            }
        }

        public static int eliminar(int id)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                var marca = context.Marca.Find(id);
                if (marca != null)
                {
                    context.Marca.Remove(marca);
                    return context.SaveChanges();
                }
                return 0;
            }
        }

        public static Marca obtenerUno(int id)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                return context.Marca.FirstOrDefault(x => x.id == id);
            }
        }

        public static List<Marca> listar(string filtro = null)
        {
            using (var context = new TiendaElectronicaEntities())
            {
                return context.Marca
                    .Where(m => string.IsNullOrEmpty(filtro) ||
                                m.nombre.Contains(filtro) ||
                                m.pais.Contains(filtro))
                    .ToList();
            }
        }
    }
}

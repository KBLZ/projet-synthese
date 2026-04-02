<<<<<<< HEAD
﻿using PE_DAL.Oracle.Context;
using System;
=======
>>>>>>> ec2d882f7b2ce5fec3d5b24ddcb42ddbf52a6740
using System.Collections.Generic;
using System.Linq;
using PE_DAL.Oracle;

namespace PE_DAL.Oracle.Repositories
{
    public class DTO_TableauRepository
    {
<<<<<<< HEAD

        private readonly PE_DBContext m_context;

        public DTO_TableauRepository(PE_DBContext context)
        {
            m_context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IEnumerable<T> LireDonnees<T>(string? parametre = null)
        {
            if (typeof(T) == typeof(DTO_Tableau))
            {
                IEnumerable<DTO_Tableau> resultat = m_context.Tableaux.AsEnumerable();

                if (!string.IsNullOrEmpty(parametre))
                {
                    resultat = resultat.Where(t => t.IdTableau.ToString() == parametre || t.TitreTableau == parametre);
                }

                return (IEnumerable<T>)resultat;
            }

            throw new NotSupportedException();
        }

        public void Dispose()
        {
            m_context.Dispose();
=======
        private readonly ProjectDbContext _context;

        public DTO_TableauRepository(ProjectDbContext context)
        {
            _context = context;
        }

        public List<DTO_Tableau> GetAll()
        {
            return _context.Tableaux.ToList();
>>>>>>> ec2d882f7b2ce5fec3d5b24ddcb42ddbf52a6740
        }
    }
}

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
    public class DTO_HistoriqueRepository
    {
<<<<<<< HEAD

        private readonly PE_DBContext m_context;

        public DTO_HistoriqueRepository(PE_DBContext context)
        {
            m_context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IEnumerable<T> LireDonnees<T>(string? parametre = null)
        {
            if (typeof(T) == typeof(DTO_Historique))
            {
                IEnumerable<DTO_Historique> resultat = m_context.Historiques.AsEnumerable();

                if (!string.IsNullOrEmpty(parametre))
                {
                    resultat = resultat.Where(h => h.ChoixPRN == parametre || h.IdUtilisateur == parametre);
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

        public DTO_HistoriqueRepository(ProjectDbContext context)
        {
            _context = context;
        }

        public List<DTO_Historique> GetByUserId(string userId)
        {
            return _context.Historiques
                .Where(h => h.IdUtilisateur == userId)
                .ToList();
>>>>>>> ec2d882f7b2ce5fec3d5b24ddcb42ddbf52a6740
        }
    }
}

using Consultorio.Context;
using Consultorio.Models.Dtos;
using Consultorio.Models.Entities;
using Consultorio.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Consultorio.Repository
{
    public class ConsultaRepository : BaseRepository, IConsultaRepository
    {
        private readonly ConsultorioContext _context;
        public ConsultaRepository(ConsultorioContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Consulta>> GetConsultas(ConsultaParams parametro)
        {
            var consultas = _context.Consultas
                 .Include(x => x.Paciente)
                 .Include(x => x.Profissional)
                 .Include(x => x.Especialidade)
                 .AsQueryable();

            DateTime dataVazia = new DateTime();

            if (parametro.DataInicio != dataVazia)
                consultas = consultas.Where(x => x.DataHorario >= parametro.DataInicio);
            if (parametro.DataFim != dataVazia)
                consultas = consultas.Where(x => x.DataHorario <= parametro.DataFim);

            if (!string.IsNullOrEmpty(parametro.NomeEspecialidade))
            {
                string nomeEspecialidade = parametro.NomeEspecialidade.ToLower().Trim();
                consultas = consultas.Where(x => x.Especialidade.Nome.ToLower().Contains(nomeEspecialidade));
            }

            return await consultas.ToListAsync();
        }

        public async Task<Consulta> GetConsultasById(int id)
        {
            return await _context.Consultas
                 .Include(x => x.Paciente)
                 .Include(x => x.Profissional)
                 .Include(x => x.Especialidade)
                 .Where(x => x.Id == id)
                 .FirstOrDefaultAsync();
        }
    }
}

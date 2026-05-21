using Microsoft.EntityFrameworkCore;
using Barbearia.Models;
namespace Barbearia.Data;

class AppDataContext : DbContext
{

    public AppDataContext(DbContextOptions<AppDataContext>
    options)
    : base(options)
    {
    }
    public DbSet<Servico> Servicos { get; set; }
    public DbSet<Agendamento> Agendamentos { get; set; }
}
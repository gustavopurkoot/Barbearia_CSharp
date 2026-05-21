namespace Barbearia.Models;
class Agendamento
{
    public int Id {set; get; }
    public string clienteNome {set; get; }
    public DateTime dataHora {set; get; }
    public string status {set; get; } = "Pendente";

    public Servico servico {set; get; }
    public int ServicoId {set; get; }

}
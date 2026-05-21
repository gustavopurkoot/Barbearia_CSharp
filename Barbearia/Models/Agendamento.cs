namespace Barbearia.Models;
class Agendamento
{
    public int id {set; get; }
    public string clienteNome {set; get; }
    public string servicoId {set; get; }
    public double dataHora {set; get; }
    public string status {set; get; }
}
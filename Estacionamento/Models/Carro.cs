public class Carro()
{
    public string CarroId { get; set; } = Guid.NewGuid().ToString();
    public string Modelo { get; set; }
    public string Placa { get; set; }
    public DateTime Entrada { get; set; } = DateTime.Now;
    public DateTime? Saida { get; set; }
    public string Status { get; set; } = "Estacionado";
}
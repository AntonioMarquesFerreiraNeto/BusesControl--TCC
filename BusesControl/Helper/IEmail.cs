namespace BusesControl.Helper {
    public interface IEmail {
        bool Enviar(string email, string tema, string msg);
    }
}

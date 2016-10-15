namespace StartIdea.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.Write("Informe a senha: ");
            string senha = System.Console.ReadLine();

            System.Console.Write("Senha Encriptografada: ");
            string cipherText = Encryptor.Encrypt(senha);
            System.Console.WriteLine(cipherText);
            System.Console.Write("Senha Descriptografada: ");
            System.Console.Write(Encryptor.Decrypt("dtR6sgpd/fdb/iAu7rtGhT0uhu/dGc0K164kW/q6uYc="));

            System.Console.ReadKey();
        }
    }
}

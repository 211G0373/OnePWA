using OnePWA.Models.Entities;

namespace OnePWA.Helpers
{
    public class ContraseñaHelper
    {
        public ContraseñaHelper(OnecgdbContext context)
        {
            Context=context;
        }

        public OnecgdbContext Context { get; }

        public static string GenerarContraseña(Users u)
        {
            string password = string.Empty;
            string[] nombre = u.Name.Split(' ');

            foreach (string d in nombre.Take(4))
            {
                foreach (var item in d.Take(1))
                {
                    password += item.ToString();
                }
            }

            int[] rand = new int[5];
            Random r = new Random();

            for (int i = 0; i < 5; i++)
            {
                rand[i] = r.Next(101);
                password+=rand[i].ToString();
            }

            return password.ToString();
        }
    }
}

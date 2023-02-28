namespace Backend.Services {
    public class UserService : IService {
        public void Get(int id) {
            System.Diagnostics.Debug.WriteLine($"Id: {id}");
        }
    }
}
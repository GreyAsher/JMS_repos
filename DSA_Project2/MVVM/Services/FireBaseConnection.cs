using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;

namespace DSA_Project2.MVVM.Services
{
    public class FireBaseConnection
    {
        private readonly FirebaseClient _connection;

        public FireBaseConnection()
        {
            _connection = new FirebaseClient("https://attempt2-5fa3a-default-rtdb.firebaseio.com/ApplicantUser/ApplicantUser.json"); 
        }
        
        
        // kani para sa universal use pampa add ug data sa database. which is ang child
        public async Task<string> AddDataAsync<T>(string parentNode,T dataForTheDB)
        {
            var resultData = await _connection.Child(parentNode).PostAsync(dataForTheDB);

            return resultData.Key;
        }
        // kani kay gina add or update niya ang data based sa key na gi butang.
        public async Task AddOrUpdateInDataBase<T>(string parentNode,string childKey,T data)
        {
             await _connection.Child(parentNode).Child(childKey).PutAsync(data);
        }

        // kany akky gina kuha niya ang data form a specific node.
        public async Task<T> GetDataFromDataBase<T>(string parentNode,string childKey)
        {
            var theData = await _connection.Child(parentNode).Child(childKey).OnceSingleAsync<T>();

            return theData;
        }

        // kani kay kuhaun niya tanan ang data from a node
        public async Task<object> GetAllDataAsync()
        {

        }
    }
}

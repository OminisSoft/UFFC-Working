﻿using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Firebase.Database;

namespace APIs  {
    public static class DatabaseAPI {
        private static DatabaseReference _reference;

        public static string slugify(string str) {
            byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(str);
            str = System.Text.Encoding.ASCII.GetString(bytes);
            str = str.ToLower();

            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            str = Regex.Replace(str, @"\s+", " ").Trim();
            str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
            return Regex.Replace(str, @"\s", "-");
        }
        
        private static DatabaseReference getDatabase() {
            if (_reference != null) {
                return _reference;
            }
            return _reference = FirebaseDatabase.DefaultInstance.RootReference;
        }

        public static void setAsyncData(string path, object value) {
            DatabaseReference customReference = getReferenceFromPath(path);
            customReference.SetValueAsync(value);
        }
        
        public static Task<DataSnapshot> getAsyncData(string path) {
            DatabaseReference customReference = getReferenceFromPath(path);
            Task<DataSnapshot> task = customReference.GetValueAsync();
            return task;
        }
        
        private static DatabaseReference getReferenceFromPath(string path) {
            var splitPath = path.Split('/');
            return splitPath.Aggregate(getDatabase(), (current, child) => current.Child(child));
        }
    }
}
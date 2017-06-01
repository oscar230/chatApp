using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography; //For hashing functions
using System.Diagnostics; //Debug

namespace chatApp
{
    class PasswordHash
    {
        public string HashItteration(string input)
        {
            int itterations = 5000; //Amount of hashing itterations for the password authentication.
            string output = input; //The final hash output.

            Debug.WriteLine("Input: " + input);
            for (int i = 1; i <= itterations; i++)
            {
                output = Hash(output); //Hashes the output and asigns the hashsum as the new output.
                Debug.WriteLine("Hash itteration nr: " + i + " out of " + itterations + " itterations. Hashsum: " + output);
            }

            return output; //Returns the last hashsum after all of the itterations.
        }

        private string Hash(string input)
        {
            //Create sha1 object
            SHA1 sha1 = SHA1.Create();

            //Performs the hash function
            byte[] hashData = sha1.ComputeHash(Encoding.Default.GetBytes(input));

            //Builds a string with StringBuilder
            StringBuilder hash = new StringBuilder();

            //loop for each byte and add it to the StringBuilder hash
            for (int i = 0; i < hashData.Length; i++)
            {
                hash.Append(hashData[i].ToString());
            }

            //This is the final hash
            return hash.ToString();
        }
    }
}

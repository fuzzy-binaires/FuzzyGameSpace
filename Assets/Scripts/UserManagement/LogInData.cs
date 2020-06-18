using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* -- stores all logins, e. g. email address and password --*/
[System.Serializable] public class LogInData
{
        [System.Serializable] public class Credentials {
        public string email;
        public string password;
    }
    public Credentials[] credentials;

}

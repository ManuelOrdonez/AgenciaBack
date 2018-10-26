namespace AgenciaDeEmpleoVirutal.Entities
{
    using Microsoft.WindowsAzure.Storage.Table;
    using System;

    /// <summary>
    /// User Table
    /// </summary>
    public class User : TableEntity
    {
        /// <summary>
        /// Get or Sets User Type
        /// </summary>
        [IgnoreProperty]
        public string UserType
        {
            get => PartitionKey;
            set => PartitionKey=value;
        }

        /// <summary>
        /// Get or Sets User Name
        /// </summary>
        [IgnoreProperty]
        public string UserName
        {
            get => RowKey;
            set => RowKey = value;
        }

        /// <summary>
        /// Get or Sets User Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Get or Sets User Position
        /// </summary>
        public string Position { get; set; }

        /// <summary>
        /// Get or Sets User No Document
        /// </summary>
        public string NoDocument { get; set; }

        /// <summary>
        /// Get or Sets User Password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Get or Sets User Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Get or Sets User Last Name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Get or Sets User Type Document
        /// </summary>
        public string TypeDocument { get; set; }

        /// <summary>
        /// Get or Sets User Genre
        /// </summary>
        public string Genre { get; set; }

        /// <summary>
        /// Get or Sets User Cell Phone 1
        /// </summary>
        public string CellPhone1 { get; set; }

        /// <summary>
        /// Get or Sets User Cell Phone 2
        /// </summary>
        public string CellPhone2 { get; set; }

        /// <summary>
        /// Get or Sets User Addrerss
        /// </summary>
        public string Addrerss { get; set; }

        /// <summary>
        /// Get or Sets User City
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Get or Sets User Departament
        /// </summary>
        public string Departament { get; set; }

        /// <summary>
        /// Get or Sets User State
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Get or Sets User Device Id
        /// </summary>
        public string DeviceId { get; set; }

        /// <summary>
        /// Get or Sets User is Authenticated
        /// </summary>
        public bool Authenticated { get; set; }

        /// <summary>
        /// Get or Sets User Role
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// Get or Sets Company Social Reason
        /// </summary>
        public string SocialReason { get; set; }

        /// <summary>
        /// Get or Sets Company Contact Name
        /// </summary>
        public string ContactName { get; set; }

        /// <summary>
        /// Get or Sets Company Position Contact
        /// </summary>
        public string PositionContact { get; set; }

        /// <summary>
        /// Get or Sets User Education Level
        /// </summary>
        public string EducationLevel { get; set; }

        /// <summary>
        /// Get or Sets User Degree Geted
        /// </summary>
        public string DegreeGeted { get; set; }

        /// <summary>
        /// Get or Sets User Cod Type Document
        /// </summary>
        public string CodTypeDocument { get; set; }

        /// <summary>
        /// Get or Sets User OpenTok Session Id
        /// </summary>
        public string OpenTokSessionId { get; set; }

        /// <summary>
        /// Get or Sets User Count Call Attended
        /// </summary>
        public int CountCallAttended { get; set; }

        /// <summary>
        /// Get or Sets User is Available
        /// </summary>
        public bool Available { get; set; }

        /// <summary>
        /// Get or Sets User Intents to Login
        /// </summary>
        public int IntentsLogin { get; set; }

    }
}
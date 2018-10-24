namespace AgenciaDeEmpleoVirutal.ExternalServices.Referentials
{
    // -----------------------------------------------------------------------
    // <copyright file="ClientWebBase.cs" company="Intergrupo S.A">
    // WebClient Base
    // </copyright>
    // <summary>This file will be communicate with services api rest</summary>
    // -----------------------------------------------------------------------

    using Entities.Referentials;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;   
    using System.Net;


    /// <summary>
    /// Represent client web base class
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    public class ClientWebBase<T> where T : class, new()
    {
        /// <summary>
        /// The media type JSON
        /// </summary>
        protected const string MediaTypeJson = "Application/JSON; charset=utf-8";

        /// <summary>
        /// The access token
        /// </summary>
        private readonly string _accessToken;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientWebBase{T}"/> class.
        /// </summary>
        /// <param name="serviceOptions">The service options.</param>
        /// <param name="serviceName">Name of the service.</param>
        /// <param name="controllerName">Name of the controller.</param>
        public ClientWebBase(IOptions<UserSecretSettings> options, string serviceName, string controllerName)
        {
            if (options == null)
            {
                return;
            }
            string urlBase = (string)options.Value.GetType().GetProperty(serviceName).GetValue(options.Value);
            Url = new Uri($"{urlBase}/{controllerName}");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientWebBase{T}"/> class.
        /// </summary>
        /// <param name="serviceOptions">The service options.</param>
        /// <param name="serviceName">Name of the service.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="token">The token.</param>
        public ClientWebBase(IOptions<UserSecretSettings> options, string serviceName, string controllerName, string token)
        {
            if (options == null)
            {
                return;
            }
            string urlBase = (string)options.Value.GetType().GetProperty(serviceName).GetValue(options.Value);
            Url = new Uri($"{urlBase}/{controllerName}");
            _accessToken = token;
        }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        /// <author>intergrupo\lcorreag</author>
        /// <remarks>
        /// CreationDate: 06/05/2015
        /// ModifiedBy:
        /// ModifiedDate:
        /// </remarks>
        public Uri Url { get; set; }

        /// <summary>
        /// Gets the HTTP put.
        /// </summary>
        /// <value>
        /// The HTTP put.
        /// </value>
        /// <author>intergrupo\lcorreag</author>        
        /// CreationDate: 06/05/2015
        /// ModifiedBy:
        /// ModifiedDate:
        public string HttpPut => "PUT";

        /// <summary>
        /// Gets the HTTP post.
        /// </summary>
        /// <value>
        /// The HTTP post.
        /// </value>
        /// <author>intergrupo\lcorreag</author>
        /// <remarks>
        /// CreationDate: 07/05/2015
        /// ModifiedBy:
        /// ModifiedDate:
        /// </remarks>
        public string HttpPost => "POST";

        /// <summary>
        /// Gets the HTTP get.
        /// </summary>
        /// <value>
        /// The HTTP get.
        /// </value>
        /// <author>intergrupo\lcorreag</author>
        /// <remarks>
        /// CreationDate: 07/05/2015
        /// ModifiedBy:
        /// ModifiedDate:
        /// </remarks>
        public string HttpGet => "GET";

        /// <summary>
        /// Gets the web client.
        /// </summary>
        /// <returns>Returns the status object.</returns>
        public WebClient GetWebClient()
        {
            var webClient = new WebClient();
            webClient.Headers.Add("Content-Type", MediaTypeJson);
            webClient.Headers.Add("Accept-Type", MediaTypeJson);
            return webClient;
        }

        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <returns>Returns the status object.</returns>
        public IList<T> GetList()
        {
            IList<T> entidades;

            using (WebClient context = GetWebClient())
            {
                entidades = JsonConvert.DeserializeObject<List<T>>(context.DownloadString(Url));
            }

            return entidades;
        }

        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Returns the status object.</returns>
        public IList<T> GetList(string id)
        {
            IList<T> entidades;

            using (WebClient context = GetWebClient())
            {
                entidades = JsonConvert.DeserializeObject<List<T>>(context.DownloadString(Url + "/" + id));
            }

            return entidades;
        }

        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <returns>Returns the status object.</returns>
        public virtual T Get()
        {
            T entidad;

            using (WebClient context = GetWebClient())
            {
                entidad = JsonConvert.DeserializeObject<T>(context.DownloadString(Url));
            }

            return entidad;
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public T Get(string id)
        {
            T entidad;
            using (var context = GetWebClient())
            {
                entidad = JsonConvert.DeserializeObject<T>(context.DownloadString($"{Url}/{id}"));
            }

            return entidad;
        }

        /// <summary>
        /// Get information with various parameters.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public virtual T Get(IDictionary<string,string> data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            var param = string.Empty;
            foreach (var item in data)
            {
                param += $"{item.Key}={item.Value}&";
            }

            param = param.Substring(1, param.Length - 1);
            T entidad;
            using (var context = GetWebClient())
            {
                entidad = JsonConvert.DeserializeObject<T>(context.DownloadString($"{Url}?{param}"));
            }

            return entidad;
        }

        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>Returns the status object.</returns>
        public IList<T> GetList(T entity)
        {
            IList<T> entidades;
            string parameters = JsonConvert.SerializeObject(entity);

            using (var context = GetWebClient())
            {
                entidades = JsonConvert.DeserializeObject<List<T>>(context.UploadString(Url, "POST", parameters));
            }

            return entidades;
        }

        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>Returns the status object.</returns>
        public virtual Response<T> GetListResponse(T entity)
        {
            Response<T> response;

            string parameters = JsonConvert.SerializeObject(entity);

            using (var context = GetWebClient())
            {
                response = JsonConvert.DeserializeObject<Response<T>>(context.UploadString(Url, "POST", parameters));
            }

            return response;
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Returns the status object.</returns>
        public T Get(int id)
        {
            T entidad;

            using (WebClient context = GetWebClient())
            {
                entidad = JsonConvert.DeserializeObject<T>(context.DownloadString(Url + "/" + id));
            }

            return entidad;
        }

        /// <summary>
        /// Gets the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>New model.</returns>
        public T Get(T model)
        {
            T entidad;

            string modelString = JsonConvert.SerializeObject(model);

            using (WebClient context = GetWebClient())
            {
                entidad = JsonConvert.DeserializeObject<T>(context.UploadString(Url, "POST", modelString));
            }

            return entidad;
        }

        /// <summary>
        /// Inserts the specified ENTIDAD.
        /// </summary>
        /// <param name="entidad">The ENTIDAD.</param>
        /// <returns>Returns the status object.</returns>
        public T Post(T entidad)
        {
            string parameters = JsonConvert.SerializeObject(entidad);
            T entidadResultado;

            using (WebClient context = GetWebClient())
            {
                entidadResultado = JsonConvert.DeserializeObject<T>(context.UploadString(Url, "POST", parameters));
            }

            return entidadResultado;
        }

        /// <summary>
        /// Updates the specified ENTIDAD.
        /// </summary>
        /// <param name="entidad">The ENTIDAD.</param>
        /// <param name="id">The identifier.</param>
        /// <returns>Returns the status object.</returns>
        public bool Update(T entidad, int id)
        {
            string parameters = JsonConvert.SerializeObject(entidad);

            using (WebClient context = GetWebClient())
            {
                context.UploadString(Url + "/" + id, "PUT", parameters);
            }

            return true;
        }

        /// <summary>
        /// Deletes the specified ENTIDAD.
        /// </summary>
        /// <param name="entidad">The ENTIDAD.</param>
        /// <returns>Returns the status object.</returns>
        public bool Delete(T entidad)
        {
            string parameters = JsonConvert.SerializeObject(entidad);
            using (WebClient context = GetWebClient())
            {
                context.UploadString(Url, "DELETE", parameters);
            }

            return true;
        }

        /// <summary>
        /// Deletes the specified ENTIDAD.
        /// </summary>
        /// <param name="id">The ENTIDAD.</param>
        /// <returns>Returns the status object.</returns>
        public bool Delete(int id)
        {
            using (var context = GetWebClient())
            {
                context.UploadString(Url + "/" + id, "DELETE", string.Empty);
            }

            return true;
        }
    }
}

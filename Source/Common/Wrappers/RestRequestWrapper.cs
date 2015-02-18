// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using RestSharp;
using RestSharp.Serializers;

namespace Wrappers
{
    public class RestRequestWrapper : IRestRequest
    {
        private readonly IRestRequest _restRequest;

        public RestRequestWrapper(string resource, Method method)
        {
            _restRequest = new RestRequest(resource, method);
        }

        public IRestRequest AddFile(string name, string path)
        {
            return _restRequest.AddFile(name, path);
        }

        public IRestRequest AddFile(string name, byte[] bytes, string fileName)
        {
            return _restRequest.AddFile(name, bytes, fileName);
        }

        public IRestRequest AddFile(string name, byte[] bytes, string fileName, string contentType)
        {
            return _restRequest.AddFile(name, bytes, fileName, contentType);
        }

        public IRestRequest AddBody(object obj, string xmlNamespace)
        {
            return _restRequest.AddBody(obj, xmlNamespace);
        }

        public IRestRequest AddBody(object obj)
        {
            return _restRequest.AddBody(obj);
        }

        public IRestRequest AddObject(object obj, params string[] whitelist)
        {
            return _restRequest.AddObject(obj, whitelist);
        }

        public IRestRequest AddObject(object obj)
        {
            return _restRequest.AddObject(obj);
        }

        public IRestRequest AddParameter(Parameter p)
        {
            return _restRequest.AddParameter(p);
        }

        public IRestRequest AddParameter(string name, object value)
        {
            object checkingValue = value ?? "";
            return _restRequest.AddParameter(name, checkingValue);
        }

        public IRestRequest AddParameter(string name, object value, ParameterType type)
        {
            object checkingValue = value ?? "";
            return _restRequest.AddParameter(name, checkingValue, type);
        }

        public IRestRequest AddHeader(string name, string value)
        {
            return _restRequest.AddHeader(name, value);
        }

        public IRestRequest AddCookie(string name, string value)
        {
            return _restRequest.AddCookie(name, value);
        }

        public IRestRequest AddUrlSegment(string name, string value)
        {
            return _restRequest.AddUrlSegment(name, value);
        }

        public void IncreaseNumAttempts()
        {
            _restRequest.IncreaseNumAttempts();
        }

        public bool AlwaysMultipartFormData { get; set; }

        public ISerializer JsonSerializer
        {
            get { return _restRequest.JsonSerializer; }
            set { _restRequest.JsonSerializer = value; }
        }

        public ISerializer XmlSerializer
        {
            get { return _restRequest.XmlSerializer; }
            set { _restRequest.XmlSerializer = value; }
        }

        public Action<Stream> ResponseWriter { get; set; }

        public List<Parameter> Parameters
        {
            get { return _restRequest.Parameters; }
        }

        public List<FileParameter> Files
        {
            get { return _restRequest.Files; }
        }

        public Method Method
        {
            get { return _restRequest.Method; }
            set { _restRequest.Method = value; }
        }

        public string Resource
        {
            get { return _restRequest.Resource; }
            set { _restRequest.Resource = value; }
        }

        public DataFormat RequestFormat
        {
            get { return _restRequest.RequestFormat; }
            set { _restRequest.RequestFormat = value; }
        }

        public string RootElement
        {
            get { return _restRequest.RootElement; }
            set { _restRequest.RootElement = value; }
        }

        public string DateFormat
        {
            get { return _restRequest.DateFormat; }
            set { _restRequest.DateFormat = value; }
        }

        public string XmlNamespace
        {
            get { return _restRequest.XmlNamespace; }
            set { _restRequest.XmlNamespace = value; }
        }

        public ICredentials Credentials
        {
            get { return _restRequest.Credentials; }
            set { _restRequest.Credentials = value; }
        }

        public int Timeout
        {
            get { return _restRequest.Timeout; }
            set { _restRequest.Timeout = value; }
        }

        public int Attempts
        {
            get { return _restRequest.Attempts; }
        }

        public bool UseDefaultCredentials { get; set; }

        public Action<IRestResponse> OnBeforeDeserialization
        {
            get { return _restRequest.OnBeforeDeserialization; }
            set { _restRequest.OnBeforeDeserialization = value; }
        }
    }
}
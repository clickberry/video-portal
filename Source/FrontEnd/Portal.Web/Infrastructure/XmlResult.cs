// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Text;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;

namespace Portal.Web.Infrastructure
{
    public class XmlResult : ContentResult
    {
        public XmlResult(XElement xmlElement)
        {
            Content = xmlElement.ToString();
            ContentEncoding = Encoding.UTF8;
            ContentType = "text/xml";
        }

        public XmlResult(XmlElement xmlElement)
        {
            Content = xmlElement.ToString();
            ContentEncoding = Encoding.UTF8;
            ContentType = "text/xml";
        }

        public XmlResult(string xml)
        {
            Content = xml;
            ContentEncoding = Encoding.UTF8;
            ContentType = "text/xml";
        }
    }
}
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Net;
//using System.IO;
//using System.Web.Script.Serialization;

//namespace Base.MVC
//{
//    public class WebRequest<T> where T : class
//    {
//        public JavaScriptSerializer serializer { get; set; }
//        public string ResponseString { get; set; }
//        public T Response { get; set; }

//        public WebRequest( )
//        {
//            serializer = new JavaScriptSerializer( );
//        }

//        public T Perform(string url, object objectToSend) 
//        {			
//            string toSend = serializer.Serialize( objectToSend );

//            WebRequest webRequest = WebRequest.Create( url );
//            webRequest.Method = "POST";
//            webRequest.ContentType = "application/json";
//            //webRequest.ContentLength = toSend.Length;

//            using (var requestWriter = new StreamWriter( webRequest.GetRequestStream( ) ))
//            {
//                requestWriter.Write( toSend );
//            }

//            WebResponse webResponse = webRequest.GetResponse( );
//            using (var responseReader = new StreamReader( webResponse.GetResponseStream( ) ))
//            {
//                ResponseString = responseReader.ReadToEnd( );
//            }

//            Response = serializer.Deserialize<T>( ResponseString );

//            return Response;
//        }
//    }
//}

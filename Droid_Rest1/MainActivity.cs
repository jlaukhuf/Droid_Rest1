using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Xml.Serialization;
using Android.App;
using Android.Widget;
using Android.OS;
using Newtonsoft.Json;

namespace Droid_Rest1
{
    [Activity(Label = "Consuming REST Services", MainLauncher = true)]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);
            Button readJson = FindViewById<Button>(Resource.Id.readJson);
            Button readXml = FindViewById<Button>(Resource.Id.readXml);
            Button sendData = FindViewById<Button>(Resource.Id.sendData);
            TextView textView = FindViewById<TextView>(Resource.Id.textView);
            //var uri = "http://jsonplaceholder.typicode.com/posts";
            var uri = "http://10.0.2.2:5000/api/list";

            readJson.Click += async delegate
            {
                using (var client = new HttpClient())
                {
                    // send a GET request  
                    //var uri = "http://jsonplaceholder.typicode.com/posts";
                    var result = await client.GetStringAsync(uri);
                    //handling the answer  
                    var posts = JsonConvert.DeserializeObject<List<SimpleList>>(result);

                    // generate the output  
                    var post = posts.First();
                    textView.Text = "First post:\n\n" + post;
                }
            };

            readXml.Click += async delegate
            {
                using (var client = new HttpClient())
                {
                    // send a GET request  
                    //var uri = "http://blog.xamarin.com/feed";
                    var result = await client.GetStreamAsync(uri);

                    //handling the answer  
                    var serializer = new XmlSerializer(typeof(Rss));
                    var feed = (Rss)serializer.Deserialize(result);

                    // generate the output  
                    var item = feed.Channel.Items.First();
                    textView.Text = "First Item:\n\n" + item;
                }
            };

            sendData.Click += async delegate
            {
                using (var client = new HttpClient())
                {
                    // Create a new post  
                    var novoPost = new Post
                    {
                        UserId = 12,
                        Title = "My First Post",
                        Content = "Macoratti .net - Quase tudo para .NET!"
                    };

                    // create the request content and define Json  
                    var json = JsonConvert.SerializeObject(novoPost);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    //  send a POST request  
                    //var uri = "http://jsonplaceholder.typicode.com/posts";
                    var result = await client.PostAsync(uri, content);

                    // on error throw a exception  
                    result.EnsureSuccessStatusCode();

                    // handling the answer  
                    var resultString = await result.Content.ReadAsStringAsync();
                    var post = JsonConvert.DeserializeObject<Post>(resultString);

                    // display the output in TextView  
                    textView.Text = post.ToString();
                }
            };

        }
    }
}
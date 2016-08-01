Imports System.ComponentModel
Imports System.Reactive.Linq
Imports Newtonsoft.Json.Linq
Imports RestSharp

Public Class frmRX

    Dim cl As New RestClient
    Dim lb As IDisposable

    Private Sub frmRX_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        lb.Dispose()
    End Sub

    Private Sub frmRX_Load(sender As Object, e As EventArgs) Handles Me.Load

        cl.BaseUrl = New Uri("https://en.wikipedia.org/w/api.php")
        cl.AddDefaultParameter("prop", "info")
        cl.AddDefaultParameter("format", "json")
        cl.AddDefaultParameter("action", "query")
        cl.AddDefaultParameter("list", "search")

        'text change event as observable
        Dim textChanges = Observable.FromEventPattern(txtSearch, "TextChanged")
        'get text from change
        Dim words = textChanges.Select(Function(tc) DirectCast(tc.Sender, TextBox).Text)

        'limit to text with length of 2 or more and throttle to changes after 500 milliseconds
        Dim wrdSearch = words.Throttle(TimeSpan.FromMilliseconds(500)).
            DistinctUntilChanged.
            Where(Function(w) w.Length >= 2).
                        Select(Function(w)
                                   'generate a request from the word
                                   Dim request As New RestRequest
                                   request.JsonSerializer = New RestSharp.Serializers.JsonSerializer()
                                   request.AddParameter("srsearch", w)
                                   Return request
                               End Function)
        'get response from request
        Dim responses = wrdSearch.Select(Function(r) cl.Get(r))
        ' convert response to JObject and get the search results
        Dim json = responses.Select(Function(r) JObject.Parse(r.Content)("query")("search"))

        lb = json.ObserveOn(Me).Subscribe(Sub(j)
                                              Console.WriteLine(j.ToString)

                                              lstResult.Items.Clear()

                                              lstResult.Items.AddRange(j.Select(Function(t) String.Format(
                                              "{0} ({1:G})", t("title"), Date.Parse(t("timestamp").Value(Of String)))).ToArray)

                                          End Sub
                                          )

    End Sub

End Class

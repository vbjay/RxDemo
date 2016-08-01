Imports Newtonsoft.Json
Imports System.IO
Imports RestSharp.Serializers

Namespace RestSharp.Serializers
    ''' <summary>
    ''' Default JSON serializer for request bodies
    ''' Doesn't currently use the SerializeAs attribute, defers to Newtonsoft's attributes
    ''' </summary>
    Public Class JsonSerializer
        Implements ISerializer
        Private ReadOnly _serializer As Newtonsoft.Json.JsonSerializer

        Private m_ContentType As String

        Private m_DateFormat As String

        Private m_Namespace As String

        Private m_RootElement As String

        ''' <summary>
        ''' Default serializer
        ''' </summary>
        Public Sub New()
            ContentType = "application/json"
            _serializer = New Newtonsoft.Json.JsonSerializer() With {
                .MissingMemberHandling = MissingMemberHandling.Ignore,
                .NullValueHandling = NullValueHandling.Include,
                .DefaultValueHandling = DefaultValueHandling.Include
            }
        End Sub

        ''' <summary>
        ''' Default serializer with overload for allowing custom Json.NET settings
        ''' </summary>
        Public Sub New(serializer As Newtonsoft.Json.JsonSerializer)
            ContentType = "application/json"
            _serializer = serializer
        End Sub

        ''' <summary>
        ''' Unused for JSON Serialization
        ''' </summary>
        Public Property [Namespace]() As String Implements ISerializer.Namespace
            Get
                Return m_Namespace
            End Get
            Set
                m_Namespace = Value
            End Set
        End Property

        ''' <summary>
        ''' Content type for serialized content
        ''' </summary>
        Public Property ContentType() As String Implements ISerializer.ContentType
            Get
                Return m_ContentType
            End Get
            Set
                m_ContentType = Value
            End Set
        End Property

        ''' <summary>
        ''' Unused for JSON Serialization
        ''' </summary>
        Public Property DateFormat() As String Implements ISerializer.DateFormat
            Get
                Return m_DateFormat
            End Get
            Set
                m_DateFormat = Value
            End Set
        End Property

        ''' <summary>
        ''' Unused for JSON Serialization
        ''' </summary>
        Public Property RootElement() As String Implements ISerializer.RootElement
            Get
                Return m_RootElement
            End Get
            Set
                m_RootElement = Value
            End Set
        End Property

        ''' <summary>
        ''' Serialize the object as JSON
        ''' </summary>
        ''' <param name="obj">Object to serialize</param>
        ''' <returns>JSON as String</returns>
        Public Function Serialize(obj As Object) As String Implements ISerializer.Serialize
            Using stringWriter = New StringWriter()
                Using jsonTextWriter = New JsonTextWriter(stringWriter)
                    jsonTextWriter.Formatting = Formatting.Indented
                    jsonTextWriter.QuoteChar = """"c

                    _serializer.Serialize(jsonTextWriter, obj)

                    Dim result = stringWriter.ToString()
                    Return result
                End Using
            End Using
        End Function
    End Class
End Namespace

#Region "License"
'   Copyright 2010 John Sheehan
'
'   Licensed under the Apache License, Version 2.0 (the "License");
'   you may not use this file except in compliance with the License.
'   You may obtain a copy of the License at
'
'     http://www.apache.org/licenses/LICENSE-2.0
'
'   Unless required by applicable law or agreed to in writing, software
'   distributed under the License is distributed on an "AS IS" BASIS,
'   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
'   See the License for the specific language governing permissions and
'   limitations under the License.
#End Region
#Region "Acknowledgements"
' Original JsonSerializer contributed by Daniel Crenna (@dimebrain)
#End Region

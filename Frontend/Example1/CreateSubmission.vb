Imports System.Net.Http
Imports Newtonsoft.Json
Imports System.Diagnostics
Imports Newtonsoft.Json.Serialization

' Event arguments to hold the submission data
Public Class SubmissionEventArgs
    Inherits EventArgs
    Public Property Submission As Submission

    Public Sub New(submission As Submission)
        Me.Submission = submission
    End Sub
End Class

Public Class CreateSubmission
    ' Declare controls
    Private lblName As Label
    Private lblEmail As Label
    Private lblPhoneNumber As Label
    Private lblGithubLink As Label
    Private lblElapsedTime As Label

    Private txtName As TextBox
    Private txtEmail As TextBox
    Private txtPhoneNumber As TextBox
    Private txtGithubLink As TextBox

    Private WithEvents btnToggleStopwatch As Button
    Private WithEvents btnSubmit As Button

    ' Stopwatch variables
    Private stopwatch As Stopwatch
    Private stopwatchPaused As Boolean

    ' Timer control to update elapsed time display
    Private WithEvents Timer1 As New Timer() With {.Interval = 1000}

    ' Event to notify submission creation
    Public Event SubmissionCreated As EventHandler(Of SubmissionEventArgs)

    ' Constructor
    Public Sub New()
        InitializeForm()
    End Sub

    ' Custom method to initialize the form and controls
    Private Sub InitializeForm()
        Me.Text = "Drishti Singh, Slidely Task 2 - Create New Submission"
        Me.Size = New Size(400, 400)
        Me.MinimumSize = New Size(500, 500)

        ' Initialize stopwatch
        stopwatch = New Stopwatch()
        stopwatchPaused = True

        ' Create and configure labels
        lblName = New Label()
        lblName.Text = "Name:"
        lblName.Location = New Point(20, 20)
        lblName.Size = New Size(100, 30)
        Me.Controls.Add(lblName)

        lblEmail = New Label()
        lblEmail.Text = "Email:"
        lblEmail.Location = New Point(20, 60)
        lblEmail.Size = New Size(100, 30)
        Me.Controls.Add(lblEmail)

        lblPhoneNumber = New Label()
        lblPhoneNumber.Text = "Phone Number:"
        lblPhoneNumber.Location = New Point(20, 100)
        lblPhoneNumber.Size = New Size(100, 30)
        Me.Controls.Add(lblPhoneNumber)

        lblGithubLink = New Label()
        lblGithubLink.Text = "GitHub Link:"
        lblGithubLink.Location = New Point(20, 140)
        lblGithubLink.Size = New Size(100, 30)
        Me.Controls.Add(lblGithubLink)

        lblElapsedTime = New Label()
        lblElapsedTime.Text = "00:00:00"
        lblElapsedTime.Location = New Point(20, 180)
        lblElapsedTime.Size = New Size(200, 30)
        Me.Controls.Add(lblElapsedTime)

        ' Create and configure textboxes
        txtName = New TextBox()
        txtName.Location = New Point(130, 20)
        txtName.Size = New Size(200, 30)
        Me.Controls.Add(txtName)

        txtEmail = New TextBox()
        txtEmail.Location = New Point(130, 60)
        txtEmail.Size = New Size(200, 30)
        Me.Controls.Add(txtEmail)

        txtPhoneNumber = New TextBox()
        txtPhoneNumber.Location = New Point(130, 100)
        txtPhoneNumber.Size = New Size(200, 30)
        Me.Controls.Add(txtPhoneNumber)

        txtGithubLink = New TextBox()
        txtGithubLink.Location = New Point(130, 140)
        txtGithubLink.Size = New Size(200, 30)
        Me.Controls.Add(txtGithubLink)

        ' Create and configure buttons
        btnToggleStopwatch = New Button()
        btnToggleStopwatch.Text = "TOGGLE STOPWATCH (CTRL + T)"
        btnToggleStopwatch.Location = New Point(20, 220)
        btnToggleStopwatch.Size = New Size(200, 30)
        Me.Controls.Add(btnToggleStopwatch)

        Dim menuStrip As New MenuStrip()
        Dim shortcutView As New ToolStripMenuItem("Submit")
        shortcutView.ShortcutKeys = Keys.Control Or Keys.S
        AddHandler shortcutView.Click, AddressOf Me.btnSubmit_Click

        menuStrip.Items.Add(shortcutView)

        Me.MainMenuStrip = menuStrip
        Me.Controls.Add(menuStrip)

        btnSubmit = New Button()
        btnSubmit.Text = "Submit (CTRL + S)"
        btnSubmit.Location = New Point(250, 220)
        btnSubmit.Size = New Size(200, 30)
        AddHandler btnSubmit.Click, AddressOf Me.btnSubmit_Click
        Me.Controls.Add(btnSubmit)
    End Sub

    ' Handler for Toggle Stopwatch button
    Private Sub btnToggleStopwatch_Click(sender As Object, e As EventArgs) Handles btnToggleStopwatch.Click
        If stopwatchPaused Then
            ' Start stopwatch
            stopwatch.Start()
            stopwatchPaused = False

            ' Start updating elapsed time display
            Timer1.Start()
        Else
            ' Pause stopwatch
            stopwatch.Stop()
            stopwatchPaused = True

            ' Stop updating elapsed time display
            Timer1.Stop()
        End If
    End Sub

    ' Handler for Timer tick to update elapsed time display
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        lblElapsedTime.Text = $"{stopwatch.Elapsed.ToString("hh\:mm\:ss")}"
    End Sub

    ' Handler for Submit button
    Private Async Sub btnSubmit_Click(sender As Object, e As EventArgs)
        ' Create a new Submission object
        Dim newSubmission As New Submission(
            txtName.Text,
            txtEmail.Text,
            txtPhoneNumber.Text,
            txtGithubLink.Text,
            stopwatch.Elapsed.ToString("hh\:mm\:ss")
        )

        ' Serialize submission object to JSON
        Dim jsonSubmission As String = JsonConvert.SerializeObject(newSubmission, New JsonSerializerSettings With {
        .ContractResolver = New DefaultContractResolver With {
            .NamingStrategy = New CamelCaseNamingStrategy()
        }
        })

        ' Log the JSON request being sent
        Debug.WriteLine($"JSON Request: {jsonSubmission}")

        ' Make HTTP POST request to backend API
        Using client As New HttpClient()
            ' Endpoint URL for /submit API
            Dim apiUrl As String = "http://localhost:3000/submit"

            ' Content to send in POST request
            Dim content As New StringContent(jsonSubmission, System.Text.Encoding.UTF8, "application/json")

            Try
                Dim response As HttpResponseMessage = Await client.PostAsync(apiUrl, content)

                ' Check if request was successful
                If response.IsSuccessStatusCode Then
                    ' Raise event with the submission data

                    RaiseEvent SubmissionCreated(Me, New SubmissionEventArgs(newSubmission))
                    MessageBox.Show("Submitted Successfully")

                    ' Close the form
                    Me.Close()
                Else
                    MessageBox.Show("Failed to submit the form. Please try again.")
                    Me.Close()
                End If
            Catch ex As Exception
                MessageBox.Show($"An error occurred: {ex.Message}")
                Me.Close()
            End Try
        End Using
    End Sub

End Class

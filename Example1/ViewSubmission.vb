Imports System.Net.Http
Imports Newtonsoft.Json

Public Class ViewSubmission
    ' Declare buttons and labels
    Private btnPrevious As Button
    Private btnNext As Button
    Private lblName As Label
    Private lblEmail As Label
    Private lblPhoneNumber As Label
    Private lblGithubLink As Label
    Private lblStopwatchTime As Label

    ' Track current submission index
    Private currentIndex As Integer

    ' Constructor
    Public Sub New()
        Me.InitializeForm()
        currentIndex = 0
        FetchSubmission(currentIndex)
    End Sub

    ' Method to fetch and display submission by index
    Private Async Sub FetchSubmission(index As Integer)
        Try
            Using client As New HttpClient()
                Dim apiUrl As String = $"http://localhost:3000/read?index={index}"
                Dim response As HttpResponseMessage = Await client.GetAsync(apiUrl)

                If response.IsSuccessStatusCode Then
                    Dim json As String = Await response.Content.ReadAsStringAsync()
                    Dim submission As Submission = JsonConvert.DeserializeObject(Of Submission)(json)
                    DisplaySubmission(submission)
                Else
                    MessageBox.Show("Failed to fetch submission from the API.")
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show($"An error occurred while fetching the submission: {ex.Message}")
        End Try
    End Sub

    ' Method to initialize the form
    Private Sub InitializeForm()
        Me.Text = "Drishti Singh, Slidely Task 2 - View Submission"
        Me.Size = New Size(500, 400)
        Me.MinimumSize = New Size(500, 400)

        Dim menuStrip As New MenuStrip()
        Dim shortcutView As New ToolStripMenuItem("Previous")
        shortcutView.ShortcutKeys = Keys.Control Or Keys.P
        AddHandler shortcutView.Click, AddressOf Me.btnPrevious_Click
        menuStrip.Items.Add(shortcutView)

        Dim shortcutNew As New ToolStripMenuItem("Next")
        shortcutNew.ShortcutKeys = Keys.Control Or Keys.N
        AddHandler shortcutNew.Click, AddressOf Me.btnNext_Click
        menuStrip.Items.Add(shortcutNew)

        Me.MainMenuStrip = menuStrip
        Me.Controls.Add(menuStrip)

        lblName = New Label()
        lblName.Location = New Point(20, 30)
        lblName.Size = New Size(350, 30)
        lblName.AutoSize = False
        Me.Controls.Add(lblName)

        lblEmail = New Label()
        lblEmail.Location = New Point(20, 70)
        lblEmail.Size = New Size(350, 30)
        lblEmail.AutoSize = False
        Me.Controls.Add(lblEmail)

        lblPhoneNumber = New Label()
        lblPhoneNumber.Location = New Point(20, 110)
        lblPhoneNumber.Size = New Size(350, 30)
        lblPhoneNumber.AutoSize = False
        Me.Controls.Add(lblPhoneNumber)

        lblGithubLink = New Label()
        lblGithubLink.Location = New Point(20, 150)
        lblGithubLink.Size = New Size(350, 30)
        lblGithubLink.AutoSize = False
        Me.Controls.Add(lblGithubLink)

        lblStopwatchTime = New Label()
        lblStopwatchTime.Location = New Point(20, 190)
        lblStopwatchTime.Size = New Size(350, 30)
        lblStopwatchTime.AutoSize = False
        Me.Controls.Add(lblStopwatchTime)

        btnPrevious = New Button()
        btnPrevious.Text = "Previous  (CTRL + P)"
        btnPrevious.Size = New Size(150, 30)
        btnPrevious.Location = New Point(20, 220)
        AddHandler btnPrevious.Click, AddressOf Me.btnPrevious_Click
        Me.Controls.Add(btnPrevious)

        btnNext = New Button()
        btnNext.Text = "Next (CTRL + N)"
        btnNext.Size = New Size(150, 30)
        btnNext.Location = New Point(200, 220)
        AddHandler btnNext.Click, AddressOf Me.btnNext_Click
        Me.Controls.Add(btnNext)
    End Sub

    ' Method to display the fetched submission in the form
    Private Sub DisplaySubmission(submission As Submission)
        If submission IsNot Nothing Then
            lblName.Text = $"Name: {submission.Name}"
            lblEmail.Text = $"Email: {submission.Email}"
            lblPhoneNumber.Text = $"Phone Number: {submission.PhoneNumber}"
            lblGithubLink.Text = $"GitHub: {submission.GithubLink}"
            lblStopwatchTime.Text = $"Stopwatch Time: {submission.StopwatchTime}"
        End If
    End Sub

    ' Handler for Previous button click
    Private Sub btnPrevious_Click(sender As Object, e As EventArgs)
        If currentIndex > 0 Then
            currentIndex -= 1
            FetchSubmission(currentIndex)
        End If
    End Sub

    ' Handler for Next button click
    Private Sub btnNext_Click(sender As Object, e As EventArgs)
        ' Assuming we have a known length, else you need to handle fetching the total count
        ' in a real-world scenario, you might want to handle this dynamically
        currentIndex += 1
        FetchSubmission(currentIndex)
    End Sub
End Class

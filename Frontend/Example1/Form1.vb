Public Class Form1
    ' Declare buttons
    Private WithEvents btnViewSubmissions As Button
    Private WithEvents btnCreateNewSubmission As Button
    Private submissions As List(Of Submission)

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Set the form title and minimum size
        Me.Text = "Drishti Singh, Slidely Task 2 - Slidely Form App"
        Me.MinimumSize = New Size(300, 200) ' Set a minimum size for the form

        ' Create and configure the MenuStrip
        Dim menuStrip As New MenuStrip()

        ' Create View Submissions shortcut
        Dim shortcutView As New ToolStripMenuItem("View Submissions")
        shortcutView.ShortcutKeys = Keys.Control Or Keys.V
        AddHandler shortcutView.Click, AddressOf Me.btnViewSubmissions_Click
        menuStrip.Items.Add(shortcutView)

        ' Create New Submission shortcut
        Dim shortcutNew As New ToolStripMenuItem("Create New Submission")
        shortcutNew.ShortcutKeys = Keys.Control Or Keys.N
        AddHandler shortcutNew.Click, AddressOf Me.btnCreateNewSubmission_Click
        menuStrip.Items.Add(shortcutNew)

        ' Add the MenuStrip to the form
        Me.MainMenuStrip = menuStrip
        Me.Controls.Add(menuStrip)

        ' Create View Submissions button
        btnViewSubmissions = New Button()
        btnViewSubmissions.Text = "View Submissions"
        btnViewSubmissions.Size = New Size(Me.ClientSize.Width - 40, 50) ' Width dynamically adjusted
        btnViewSubmissions.Location = New Point(20, 40) ' Position adjusted for MenuStrip
        btnViewSubmissions.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        AddHandler btnViewSubmissions.Click, AddressOf Me.btnViewSubmissions_Click
        Me.Controls.Add(btnViewSubmissions)

        ' Create New Submission button
        btnCreateNewSubmission = New Button()
        btnCreateNewSubmission.Text = "Create New Submission"
        btnCreateNewSubmission.Size = New Size(Me.ClientSize.Width - 40, 50) ' Width dynamically adjusted
        btnCreateNewSubmission.Location = New Point(20, 100) ' Position adjusted for MenuStrip
        btnCreateNewSubmission.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        AddHandler btnCreateNewSubmission.Click, AddressOf Me.btnCreateNewSubmission_Click
        Me.Controls.Add(btnCreateNewSubmission)
    End Sub

    Private Sub btnViewSubmissions_Click(sender As Object, e As EventArgs)
        ' Open ViewSubmission form which will fetch the submissions from the API
        Dim viewForm As New ViewSubmission()
        viewForm.ShowDialog()
    End Sub


    Private Sub btnCreateNewSubmission_Click(sender As Object, e As EventArgs) Handles btnCreateNewSubmission.Click
        Dim createForm As New CreateSubmission()
        AddHandler createForm.SubmissionCreated, AddressOf Me.CreateForm_SubmissionCreated
        createForm.ShowDialog()
    End Sub

    Private Sub CreateForm_SubmissionCreated(sender As Object, e As SubmissionEventArgs)
        Dim newSubmission As Submission = e.Submission
        ' Process the new submission as needed (e.g., add to list, update database)
        submissions.Add(newSubmission)
    End Sub


    ' Resize event handler to adjust button sizes dynamically
    Private Sub Form1_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        ' Ensure the buttons are resized only if they are already created
        If btnViewSubmissions IsNot Nothing AndAlso btnCreateNewSubmission IsNot Nothing Then
            btnViewSubmissions.Width = Me.ClientSize.Width - 40
            btnCreateNewSubmission.Width = Me.ClientSize.Width - 40
        End If
    End Sub
End Class


' Define the Submission class to hold the submission data
Public Class Submission
    Public Property Name As String
    Public Property Email As String
    Public Property PhoneNumber As String
    Public Property GithubLink As String
    Public Property StopwatchTime As String

    Public Sub New(name As String, email As String, phoneNumber As String, githubLink As String, stopwatchTime As String)
        Me.Name = name
        Me.Email = email
        Me.PhoneNumber = phoneNumber
        Me.GithubLink = githubLink
        Me.StopwatchTime = stopwatchTime
    End Sub
End Class
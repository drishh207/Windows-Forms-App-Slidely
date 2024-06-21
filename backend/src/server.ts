import express, { Request, Response } from 'express';
import fs from 'fs';

const app = express();
const DB_FILE = "./src/db.json";

app.use(express.json());

// Submit endpoint
app.post('/submit', (req: Request, res: Response) => {
    const { name, email, phoneNumber, githubLink, stopwatchTime } = req.body;

    console.log('Received data:');
    console.log('Name:', name);
    console.log('Email:', email);
    console.log('Phone:', phoneNumber);
    console.log('GitHub Link:', githubLink);
    console.log('Stopwatch Time:', stopwatchTime);

    // Read current submissions from JSON file
    let submissions: any[] = [];
    try {
        const data = fs.readFileSync(DB_FILE, 'utf8');
        submissions = JSON.parse(data);
    } catch (err) {
        console.error('Error reading or parsing db.json:', err);
    }

    // Add new submission
    const newSubmission = {
        name,
        email,
        phoneNumber,
        githubLink,
        stopwatchTime
    };
    submissions.push(newSubmission);

    // Write updated submissions back to JSON file
    try {
        fs.writeFileSync(DB_FILE, JSON.stringify(submissions, null, 2), 'utf8');
        res.json({ success: true });
    } catch (err) {
        console.error('Error writing to db.json:', err);
        res.status(500).json({ error: 'Error saving submission' });
    }
});

// Example ping endpoint
app.get('/ping', (req, res) => {
    res.json({ success: true });
});

// Example read endpoint
app.get('/read', (req: Request, res: Response) => {
    const { index } = req.query;
    let submissions: any[] = [];

    try {
        const data = fs.readFileSync(DB_FILE, 'utf8');
        submissions = JSON.parse(data);
    } catch (err) {
        console.error('Error reading or parsing db.json:', err);
        return res.status(500).json({ error: 'Error reading data' });
    }

    const submissionIndex = Number(index);
    if (!isNaN(submissionIndex) && submissionIndex >= 0 && submissionIndex < submissions.length) {
        res.json(submissions[submissionIndex]);
    } else {
        res.status(404).json({ error: 'Submission not found' });
    }
});


// Start server
const PORT = 3000;
app.listen(PORT, () => {
    console.log(`Server is running on http://localhost:${PORT}`);
});
